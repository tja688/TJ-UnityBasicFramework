using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using EditorToolbarSearchField = UnityEditor.UIElements.ToolbarSearchField;
using UnityEngine;
using UnityEngine.UIElements;

namespace FlyRabbit.EventCenter.EditorToolkit
{
    public sealed class ProjectUsagePageController
    {
        private readonly Action<string> m_Status;

        private readonly EventUsageScanner m_Scanner = new EventUsageScanner();
        private readonly Dictionary<string, EventDefinition> m_DefinitionsByName = new Dictionary<string, EventDefinition>(StringComparer.Ordinal);
        private readonly List<EventUsageGroup> m_Groups = new List<EventUsageGroup>();

        private VisualElement m_Root;
        private Button m_ScanButton;
        private EditorToolbarSearchField m_SearchField;
        private ScrollView m_ResultsScroll;
        private Label m_FooterLabel;

        public ProjectUsagePageController(Action<string> status)
        {
            m_Status = status;
        }

        public void Mount(VisualElement root)
        {
            m_Root = root;
            if (m_Root == null)
            {
                return;
            }

            m_ScanButton = m_Root.Q<Button>("scanProjectButton");
            m_SearchField = m_Root.Q<EditorToolbarSearchField>("searchField");
            m_ResultsScroll = m_Root.Q<ScrollView>("resultsScroll");
            m_FooterLabel = m_Root.Q<Label>("footerLabel");

            m_ScanButton.clicked += ScanProject;
            m_SearchField.RegisterValueChangedCallback(_ => Render());
        }

        public void SetDefinitions(IReadOnlyList<EventDefinition> definitions, string error)
        {
            m_DefinitionsByName.Clear();
            if (definitions != null)
            {
                foreach (var def in definitions)
                {
                    if (def?.Name == null)
                    {
                        continue;
                    }
                    m_DefinitionsByName[def.Name] = def;
                }
            }

            if (m_Groups.Count > 0)
            {
                AnnotateWithDefinitions(m_Groups);
                Render();
            }
        }

        private void ScanProject()
        {
            if (EditorApplication.isCompiling)
            {
                EditorUtility.DisplayDialog("EventCenter Toolkit", "Unity is compiling. Please retry after compilation.", "OK");
                return;
            }

            m_Status?.Invoke("Scanning project scripts...");

            var paths = new List<string>();
            var guids = AssetDatabase.FindAssets("t:Script", new[] { "Assets" });
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                if (string.IsNullOrWhiteSpace(path) || path.Contains("/Editor/"))
                {
                    continue;
                }
                paths.Add(path);
            }

            var records = m_Scanner.ScanScripts(paths);
            var groups = m_Scanner.GroupAndMarkMismatches(records);

            m_Groups.Clear();
            m_Groups.AddRange(groups);
            AnnotateWithDefinitions(m_Groups);
            Render();

            m_FooterLabel.text = $"{DateTime.Now:G}  Scripts: {paths.Count}  Events: {m_Groups.Count}  Usages: {records.Count}";
            m_Status?.Invoke($"Project scan done. Groups: {m_Groups.Count}");
        }

        private void AnnotateWithDefinitions(List<EventUsageGroup> groups)
        {
            foreach (var group in groups)
            {
                group.MissingDefinition = !m_DefinitionsByName.TryGetValue(group.EventName, out var def);
                group.IsDeprecated = def != null && def.IsDeprecated;
                group.DeprecatedMessage = def?.DeprecatedMessage;
                group.DefinitionSignature = def == null ? null : NormalizeSignature(def.Signature);

                var observed = NormalizeSignature(group.ObservedSignature ?? string.Empty);
                group.ObservedSignature = observed;
                group.HasDefinitionMismatch = def != null && !string.Equals(observed, group.DefinitionSignature ?? string.Empty, StringComparison.Ordinal);
            }
        }

        private void Render()
        {
            m_ResultsScroll?.Clear();
            if (m_ResultsScroll == null)
            {
                return;
            }

            var query = (m_SearchField?.value ?? string.Empty).Trim();
            var hasQuery = query.Length > 0;

            var shown = 0;
            foreach (var group in m_Groups)
            {
                if (hasQuery && group.EventName.IndexOf(query, StringComparison.OrdinalIgnoreCase) < 0)
                {
                    continue;
                }
                m_ResultsScroll.Add(BuildGroupFoldout(group));
                shown++;
            }

            if (shown == 0)
            {
                m_ResultsScroll.Add(new Label("No results. Click 'Scan Project' to start."));
            }
        }

        private VisualElement BuildGroupFoldout(EventUsageGroup group)
        {
            var foldout = new Foldout();
            foldout.value = false;

            var tags = string.Empty;
            if (group.MissingDefinition)
            {
                tags += " [MissingDefinition]";
            }
            if (group.HasObservedMismatch || group.HasDefinitionMismatch)
            {
                tags += " [SignatureMismatch]";
            }
            if (group.IsDeprecated)
            {
                tags += " [Obsolete]";
            }

            var sig = group.ObservedSignature ?? string.Empty;
            foldout.text = $"{group.EventName}  Sig: {sig}{tags}";

            AddUsageBlock(foldout, "TriggerEvent", group.Triggers);
            AddUsageBlock(foldout, "AddListener", group.Adds);
            AddUsageBlock(foldout, "RemoveListener", group.Removes);

            return foldout;
        }

        private void AddUsageBlock(VisualElement parent, string title, List<EventUsageRecord> records)
        {
            if (records.Count == 0)
            {
                return;
            }

            var header = new Label(title);
            header.style.unityFontStyleAndWeight = FontStyle.Bold;
            header.style.marginTop = 6;
            parent.Add(header);

            foreach (var record in records)
            {
                parent.Add(BuildUsageRow(record));
            }
        }

        private VisualElement BuildUsageRow(EventUsageRecord record)
        {
            var row = new VisualElement();
            row.style.flexDirection = FlexDirection.Row;
            row.style.marginTop = 3;
            row.style.alignItems = Align.Center;

            var file = Path.GetFileName(record.AssetPath);
            var left = new Label($"{file} (L{record.Line})");
            left.style.minWidth = 220;
            row.Add(left);

            var sig = new Label(record.Signature ?? string.Empty);
            sig.style.flexGrow = 1;
            if (record.IsMismatch)
            {
                sig.style.color = new StyleColor(new Color(1f, 0.35f, 0.35f));
            }
            row.Add(sig);

            var ping = new Button(() => Ping(record.AssetPath)) { text = "Ping" };
            ping.style.marginLeft = 8;
            row.Add(ping);

            var open = new Button(() => Open(record.AssetPath, record.Line)) { text = "Open" };
            open.style.marginLeft = 6;
            row.Add(open);

            return row;
        }

        private static void Ping(string assetPath)
        {
            var script = AssetDatabase.LoadAssetAtPath<MonoScript>(assetPath);
            if (script == null)
            {
                return;
            }
            Selection.activeObject = script;
            EditorGUIUtility.PingObject(script);
        }

        private static void Open(string assetPath, int line)
        {
            var script = AssetDatabase.LoadAssetAtPath<MonoScript>(assetPath);
            if (script == null)
            {
                return;
            }
            AssetDatabase.OpenAsset(script, line);
        }

        private static string NormalizeSignature(string signature)
        {
            return string.IsNullOrWhiteSpace(signature) ? string.Empty : signature.Replace(" ", string.Empty).Replace("\t", string.Empty);
        }
    }
}
