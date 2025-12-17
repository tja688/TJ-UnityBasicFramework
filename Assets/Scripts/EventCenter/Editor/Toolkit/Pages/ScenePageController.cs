using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using EditorToolbarSearchField = UnityEditor.UIElements.ToolbarSearchField;
using EditorToolbarToggle = UnityEditor.UIElements.ToolbarToggle;
using UnityEngine;
using UnityEngine.UIElements;

namespace FlyRabbit.EventCenter.EditorToolkit
{
    public sealed class ScenePageController
    {
        private readonly Action<string> m_Status;

        private readonly EventUsageScanner m_Scanner = new EventUsageScanner();
        private readonly Dictionary<string, EventDefinition> m_DefinitionsByName = new Dictionary<string, EventDefinition>(StringComparer.Ordinal);
        private readonly List<EventUsageGroup> m_EditModeGroups = new List<EventUsageGroup>();

        private VisualElement m_Root;
        private VisualElement m_EditModeContainer;
        private VisualElement m_PlayModeContainer;

        private Button m_ScanSceneButton;
        private EditorToolbarSearchField m_SearchField;
        private ScrollView m_EditResultsScroll;
        private Label m_EditFooterLabel;

        private EditorToolbarToggle m_MonitorToggle;
        private Button m_ClearButton;
        private EditorToolbarSearchField m_FilterField;
        private ListView m_HistoryList;
        private Label m_PlayFooterLabel;

        private readonly List<LiveEventRecord> m_AllHistory = new List<LiveEventRecord>();
        private readonly List<LiveEventRecord> m_FilteredHistory = new List<LiveEventRecord>();
        private bool m_IsMonitoring;

        private Action<global::FlyRabbit.EventCenter.EventName, string> m_OnTriggerHandler;

        public ScenePageController(Action<string> status)
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

            m_EditModeContainer = m_Root.Q<VisualElement>("editModeContainer");
            m_PlayModeContainer = m_Root.Q<VisualElement>("playModeContainer");

            m_ScanSceneButton = m_Root.Q<Button>("scanSceneButton");
            m_SearchField = m_Root.Q<EditorToolbarSearchField>("searchField");
            m_EditResultsScroll = m_Root.Q<ScrollView>("editResultsScroll");
            m_EditFooterLabel = m_Root.Q<Label>("editFooterLabel");

            m_MonitorToggle = m_Root.Q<EditorToolbarToggle>("monitorToggle");
            m_ClearButton = m_Root.Q<Button>("clearButton");
            m_FilterField = m_Root.Q<EditorToolbarSearchField>("filterField");
            m_HistoryList = m_Root.Q<ListView>("historyList");
            m_PlayFooterLabel = m_Root.Q<Label>("playFooterLabel");

            m_ScanSceneButton.clicked += ScanOpenScenes;
            m_SearchField.RegisterValueChangedCallback(_ => RenderEditModeResults());

            m_OnTriggerHandler = OnDiagnosticsTrigger;
            ConfigureHistoryListView();

            m_MonitorToggle.SetValueWithoutNotify(false);
            m_MonitorToggle.RegisterValueChangedCallback(evt => SetMonitoring(evt.newValue));
            m_ClearButton.clicked += ClearHistory;
            m_FilterField.RegisterValueChangedCallback(_ => RefreshFilteredHistory());

            UpdateModeVisibility();
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

            if (m_EditModeGroups.Count > 0)
            {
                AnnotateWithDefinitions(m_EditModeGroups);
                RenderEditModeResults();
            }
        }

        public void OnPlayModeChanged()
        {
            UpdateModeVisibility();
        }

        private void UpdateModeVisibility()
        {
            var playing = EditorApplication.isPlaying;
            if (m_EditModeContainer != null)
            {
                m_EditModeContainer.style.display = playing ? DisplayStyle.None : DisplayStyle.Flex;
            }
            if (m_PlayModeContainer != null)
            {
                m_PlayModeContainer.style.display = playing ? DisplayStyle.Flex : DisplayStyle.None;
            }

            if (!playing)
            {
                SetMonitoring(false);
            }
        }

        private void ScanOpenScenes()
        {
            if (EditorApplication.isPlaying)
            {
                EditorUtility.DisplayDialog("EventCenter Toolkit", "Scene subset scan is available in Edit Mode only.", "OK");
                return;
            }
            if (EditorApplication.isCompiling)
            {
                EditorUtility.DisplayDialog("EventCenter Toolkit", "Unity is compiling. Please retry after compilation.", "OK");
                return;
            }

            m_Status?.Invoke("Scanning open scenes (script subset)...");

            var scriptPaths = SceneScriptCollector.CollectOpenSceneScriptAssetPaths();
            var records = m_Scanner.ScanScripts(scriptPaths);
            var groups = m_Scanner.GroupAndMarkMismatches(records);

            m_EditModeGroups.Clear();
            m_EditModeGroups.AddRange(groups);
            AnnotateWithDefinitions(m_EditModeGroups);
            RenderEditModeResults();

            if (m_EditFooterLabel != null)
            {
                m_EditFooterLabel.text = $"{DateTime.Now:G}  Scripts: {scriptPaths.Count}  Events: {m_EditModeGroups.Count}  Usages: {records.Count}";
            }

            m_Status?.Invoke($"Scene subset scan done. Groups: {m_EditModeGroups.Count}");
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

        private void RenderEditModeResults()
        {
            m_EditResultsScroll?.Clear();
            if (m_EditResultsScroll == null)
            {
                return;
            }

            var query = (m_SearchField?.value ?? string.Empty).Trim();
            var hasQuery = query.Length > 0;

            var shown = 0;
            foreach (var group in m_EditModeGroups)
            {
                if (hasQuery && group.EventName.IndexOf(query, StringComparison.OrdinalIgnoreCase) < 0)
                {
                    continue;
                }
                m_EditResultsScroll.Add(BuildGroupFoldout(group));
                shown++;
            }

            if (shown == 0)
            {
                m_EditResultsScroll.Add(new Label("No results. Click 'Scan Open Scenes' to start."));
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

        private void ConfigureHistoryListView()
        {
            if (m_HistoryList == null)
            {
                return;
            }

            m_HistoryList.itemsSource = m_FilteredHistory;
            m_HistoryList.selectionType = SelectionType.Single;
            m_HistoryList.fixedItemHeight = 18;

            m_HistoryList.makeItem = () => new Label();
            m_HistoryList.bindItem = (element, index) =>
            {
                if (element is not Label label)
                {
                    return;
                }

                var record = m_FilteredHistory[index];
                var isObsolete = m_DefinitionsByName.TryGetValue(record.EventName, out var def) && def.IsDeprecated;
                label.text = $"{record.Frame,6}  {record.Time,8:0.000}  {record.EventName}{(isObsolete ? " [Obsolete]" : string.Empty)}  {record.Signature}";
            };
        }

        private void SetMonitoring(bool enabled)
        {
            if (!EditorApplication.isPlaying)
            {
                enabled = false;
            }

            if (m_IsMonitoring == enabled)
            {
                return;
            }
            m_IsMonitoring = enabled;

#if UNITY_EDITOR
            if (enabled)
            {
                global::FlyRabbit.EventCenter.EventDiagnostics.Enabled = true;
                global::FlyRabbit.EventCenter.EventDiagnostics.OnTrigger += m_OnTriggerHandler;
                m_Status?.Invoke("Live monitor enabled.");
            }
            else
            {
                global::FlyRabbit.EventCenter.EventDiagnostics.Enabled = false;
                global::FlyRabbit.EventCenter.EventDiagnostics.OnTrigger -= m_OnTriggerHandler;
                m_Status?.Invoke("Live monitor disabled.");
            }
#endif
        }

        private void ClearHistory()
        {
            m_AllHistory.Clear();
            m_FilteredHistory.Clear();
            m_HistoryList?.Rebuild();
            if (m_PlayFooterLabel != null)
            {
                m_PlayFooterLabel.text = "History cleared.";
            }
        }

        private void RefreshFilteredHistory()
        {
            var filter = (m_FilterField?.value ?? string.Empty).Trim();
            m_FilteredHistory.Clear();
            if (filter.Length == 0)
            {
                m_FilteredHistory.AddRange(m_AllHistory);
            }
            else
            {
                foreach (var record in m_AllHistory)
                {
                    if (record.EventName.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        m_FilteredHistory.Add(record);
                    }
                }
            }
            m_HistoryList?.Rebuild();
            if (m_PlayFooterLabel != null)
            {
                m_PlayFooterLabel.text = $"History: {m_AllHistory.Count}";
            }
        }

        private void OnDiagnosticsTrigger(global::FlyRabbit.EventCenter.EventName eventName, string signature)
        {
            if (!m_IsMonitoring)
            {
                return;
            }

            var record = new LiveEventRecord
            {
                Frame = Time.frameCount,
                Time = Time.realtimeSinceStartup,
                EventName = eventName.ToString(),
                Signature = signature ?? string.Empty,
            };

            m_AllHistory.Add(record);
            if (m_AllHistory.Count > 5000)
            {
                m_AllHistory.RemoveRange(0, m_AllHistory.Count - 5000);
            }

            var filter = (m_FilterField?.value ?? string.Empty).Trim();
            if (filter.Length == 0 || record.EventName.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                m_FilteredHistory.Add(record);
                m_HistoryList?.Rebuild();
            }

            if (m_PlayFooterLabel != null)
            {
                m_PlayFooterLabel.text = $"History: {m_AllHistory.Count}";
            }
        }

        public void OnWindowDisabled()
        {
            SetMonitoring(false);
        }
    }
}
