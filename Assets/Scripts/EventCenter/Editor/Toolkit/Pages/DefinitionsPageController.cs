using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using EditorToolbarMenu = UnityEditor.UIElements.ToolbarMenu;
using EditorToolbarSearchField = UnityEditor.UIElements.ToolbarSearchField;
using UnityEngine;
using UnityEngine.UIElements;

namespace FlyRabbit.EventCenter.EditorToolkit
{
    public sealed class DefinitionsPageController
    {
        private enum FilterMode
        {
            All,
            Active,
            Deprecated
        }

        private readonly EventNameSourceService m_Source;
        private readonly Action m_RequestRefresh;
        private readonly Action<string> m_Status;

        private VisualElement m_Root;
        private EditorToolbarSearchField m_SearchField;
        private EditorToolbarMenu m_FilterMenu;
        private ListView m_EventsList;

        private Label m_SelectedTitle;
        private Label m_DeprecatedBadge;
        private TextField m_SummaryField;
        private VisualElement m_ParamsContainer;
        private IntegerField m_EditParamCountField;
        private VisualElement m_EditParamsFieldsContainer;
        private Button m_SaveMetadataButton;
        private Button m_DeprecateButton;
        private TextField m_DeprecateReasonField;

        private TextField m_NewEventNameField;
        private TextField m_NewSummaryField;
        private IntegerField m_NewParamCountField;
        private VisualElement m_NewParamsContainer;
        private Button m_CreateButton;

        private readonly List<EventDefinition> m_All = new List<EventDefinition>();
        private readonly List<EventDefinition> m_Filtered = new List<EventDefinition>();
        private FilterMode m_FilterMode = FilterMode.All;

        private EventDefinition m_Selected;
        private readonly List<TextField> m_EditParamTypeFields = new List<TextField>();
        private readonly List<TextField> m_EditParamDescFields = new List<TextField>();
        private readonly List<TextField> m_NewParamTypeFields = new List<TextField>();
        private readonly List<TextField> m_NewParamDescFields = new List<TextField>();

        public DefinitionsPageController(EventNameSourceService source, Action requestRefresh, Action<string> status)
        {
            m_Source = source;
            m_RequestRefresh = requestRefresh;
            m_Status = status;
        }

        public void Mount(VisualElement root)
        {
            m_Root = root;
            if (m_Root == null)
            {
                return;
            }

            m_SearchField = m_Root.Q<EditorToolbarSearchField>("searchField");
            m_FilterMenu = m_Root.Q<EditorToolbarMenu>("filterMenu");
            m_EventsList = m_Root.Q<ListView>("eventsList");

            m_SelectedTitle = m_Root.Q<Label>("selectedTitle");
            m_DeprecatedBadge = m_Root.Q<Label>("deprecatedBadge");
            m_SummaryField = m_Root.Q<TextField>("summaryField");
            m_ParamsContainer = m_Root.Q<VisualElement>("paramsContainer");
            m_SaveMetadataButton = m_Root.Q<Button>("saveMetadataButton");
            m_DeprecateButton = m_Root.Q<Button>("deprecateButton");
            m_DeprecateReasonField = m_Root.Q<TextField>("deprecateReasonField");

            m_NewEventNameField = m_Root.Q<TextField>("newEventNameField");
            m_NewSummaryField = m_Root.Q<TextField>("newSummaryField");
            m_NewParamCountField = m_Root.Q<IntegerField>("newParamCountField");
            m_NewParamsContainer = m_Root.Q<VisualElement>("newParamsContainer");
            m_CreateButton = m_Root.Q<Button>("createButton");

            BuildFilterMenu();

            m_ParamsContainer.Clear();
            m_EditParamCountField = new IntegerField("Param Count (0..5)") { value = 0 };
            m_ParamsContainer.Add(m_EditParamCountField);
            m_EditParamsFieldsContainer = new VisualElement();
            m_EditParamsFieldsContainer.style.marginTop = 6;
            m_ParamsContainer.Add(m_EditParamsFieldsContainer);
            BuildParamEditors(m_EditParamsFieldsContainer, m_EditParamTypeFields, m_EditParamDescFields);

            BuildParamEditors(m_NewParamsContainer, m_NewParamTypeFields, m_NewParamDescFields);

            m_NewParamCountField.RegisterValueChangedCallback(_ => UpdateNewParamEditorsVisibility());
            m_EditParamCountField.RegisterValueChangedCallback(_ => UpdateEditParamEditorsVisibility());
            m_SearchField.RegisterValueChangedCallback(_ => ApplyFilter());

            m_SaveMetadataButton.clicked += OnSaveMetadata;
            m_DeprecateButton.clicked += OnDeprecate;
            m_CreateButton.clicked += OnCreate;

            SetupListView();
            UpdateSelected(null);
            UpdateNewParamEditorsVisibility();
        }

        public void SetDefinitions(IReadOnlyList<EventDefinition> definitions, string error)
        {
            m_All.Clear();
            if (definitions != null)
            {
                m_All.AddRange(definitions);
            }

            ApplyFilter();
            if (m_Selected != null)
            {
                var stillExists = m_All.FirstOrDefault(d => d.Name == m_Selected.Name);
                UpdateSelected(stillExists);
            }

            if (!string.IsNullOrWhiteSpace(error))
            {
                m_Status?.Invoke(error);
            }
        }

        private void SetupListView()
        {
            if (m_EventsList == null)
            {
                return;
            }

            m_EventsList.itemsSource = m_Filtered;
            m_EventsList.selectionType = SelectionType.Single;
            m_EventsList.fixedItemHeight = 20;

            m_EventsList.makeItem = () => new Label();
            m_EventsList.bindItem = (element, index) =>
            {
                if (element is not Label label)
                {
                    return;
                }
                var item = m_Filtered[index];
                label.text = item.IsDeprecated ? $"{item.Name}  (Obsolete)" : item.Name;
            };

            m_EventsList.onSelectionChange += selection =>
            {
                var sel = selection?.FirstOrDefault() as EventDefinition;
                UpdateSelected(sel);
            };
        }

        private void BuildFilterMenu()
        {
            if (m_FilterMenu == null)
            {
                return;
            }

            m_FilterMenu.menu.AppendAction("All", _ => SetFilterMode(FilterMode.All));
            m_FilterMenu.menu.AppendAction("Active", _ => SetFilterMode(FilterMode.Active));
            m_FilterMenu.menu.AppendAction("Deprecated", _ => SetFilterMode(FilterMode.Deprecated));
            m_FilterMenu.text = "All";
        }

        private void SetFilterMode(FilterMode mode)
        {
            m_FilterMode = mode;
            m_FilterMenu.text = mode.ToString();
            ApplyFilter();
        }

        private void ApplyFilter()
        {
            m_Filtered.Clear();
            var query = (m_SearchField?.value ?? string.Empty).Trim();

            foreach (var def in m_All)
            {
                if (m_FilterMode == FilterMode.Active && def.IsDeprecated)
                {
                    continue;
                }
                if (m_FilterMode == FilterMode.Deprecated && !def.IsDeprecated)
                {
                    continue;
                }

                if (query.Length > 0 && def.Name.IndexOf(query, StringComparison.OrdinalIgnoreCase) < 0)
                {
                    continue;
                }
                m_Filtered.Add(def);
            }

            m_EventsList?.Rebuild();
        }

        private void UpdateSelected(EventDefinition selected)
        {
            m_Selected = selected;
            var hasSelection = m_Selected != null;

            m_SelectedTitle.text = hasSelection ? $"Selected: {m_Selected.Name}" : "Selected: (none)";
            m_SummaryField.SetEnabled(hasSelection);
            m_ParamsContainer.SetEnabled(hasSelection);
            m_SaveMetadataButton.SetEnabled(hasSelection);
            m_DeprecateButton.SetEnabled(hasSelection);

            m_DeprecateReasonField.style.display = DisplayStyle.None;
            m_DeprecateReasonField.value = string.Empty;
            m_DeprecateButton.text = "Mark Deprecated";

            m_DeprecatedBadge.style.display = hasSelection && m_Selected.IsDeprecated ? DisplayStyle.Flex : DisplayStyle.None;

            if (!hasSelection)
            {
                m_SummaryField.value = string.Empty;
                m_EditParamCountField.SetValueWithoutNotify(0);
                SetParamFields(m_EditParamTypeFields, m_EditParamDescFields, 0, null, clearHidden: true);
                return;
            }

            m_SummaryField.value = m_Selected.Summary ?? string.Empty;
            m_EditParamCountField.SetValueWithoutNotify(m_Selected.Parameters.Count);
            SetParamFields(
                m_EditParamTypeFields,
                m_EditParamDescFields,
                Mathf.Clamp(m_Selected.Parameters.Count, 0, EventNameSourceService.MaxParamCount),
                m_Selected.Parameters,
                clearHidden: true);
        }

        private void OnSaveMetadata()
        {
            if (m_Selected == null)
            {
                return;
            }

            var parameters = ReadParamFields(m_EditParamTypeFields, m_EditParamDescFields);
            if (!m_Source.TryUpdateMemberDoc(m_Selected.Name, m_SummaryField.value, parameters, out var error))
            {
                EditorUtility.DisplayDialog("EventCenter Toolkit", error, "OK");
                return;
            }
            m_Status?.Invoke($"Updated doc: {m_Selected.Name}");
            m_RequestRefresh?.Invoke();
        }

        private void OnDeprecate()
        {
            if (m_Selected == null)
            {
                return;
            }

            if (m_DeprecateReasonField.style.display == DisplayStyle.None)
            {
                m_DeprecateReasonField.style.display = DisplayStyle.Flex;
                m_DeprecateButton.text = "Apply Deprecation";
                m_DeprecateReasonField.Focus();
                return;
            }

            var reason = m_DeprecateReasonField.value;
            if (!m_Source.TryDeprecate(m_Selected.Name, reason, out var error))
            {
                EditorUtility.DisplayDialog("EventCenter Toolkit", error, "OK");
                return;
            }
            m_Status?.Invoke($"Deprecated: {m_Selected.Name}");
            m_RequestRefresh?.Invoke();
        }

        private void OnCreate()
        {
            var name = (m_NewEventNameField.value ?? string.Empty).Trim();
            var summary = m_NewSummaryField.value ?? string.Empty;
            var parameters = ReadParamFields(m_NewParamTypeFields, m_NewParamDescFields);

            if (!m_Source.TryAppendEvent(name, summary, parameters, out var error))
            {
                EditorUtility.DisplayDialog("EventCenter Toolkit", error, "OK");
                return;
            }

            m_NewEventNameField.value = string.Empty;
            m_NewSummaryField.value = string.Empty;
            m_NewParamCountField.value = 0;
            UpdateNewParamEditorsVisibility();
            m_Status?.Invoke($"Created: {name}");
            m_RequestRefresh?.Invoke();
        }

        private void UpdateNewParamEditorsVisibility()
        {
            var count = Mathf.Clamp(m_NewParamCountField.value, 0, EventNameSourceService.MaxParamCount);
            m_NewParamCountField.SetValueWithoutNotify(count);
            SetParamFields(m_NewParamTypeFields, m_NewParamDescFields, count, null, clearHidden: true);
        }

        private void UpdateEditParamEditorsVisibility()
        {
            var count = Mathf.Clamp(m_EditParamCountField.value, 0, EventNameSourceService.MaxParamCount);
            m_EditParamCountField.SetValueWithoutNotify(count);
            SetParamFields(m_EditParamTypeFields, m_EditParamDescFields, count, null, clearHidden: false);
        }

        private static void BuildParamEditors(VisualElement container, List<TextField> typeFields, List<TextField> descFields)
        {
            container.Clear();
            typeFields.Clear();
            descFields.Clear();

            for (var i = 0; i < EventNameSourceService.MaxParamCount; i++)
            {
                var row = new VisualElement();
                row.style.flexDirection = FlexDirection.Row;
                row.style.marginTop = 4;

                var type = new TextField { label = $"Param {i + 1} Type" };
                type.style.flexGrow = 1;
                type.style.marginRight = 8;

                var desc = new TextField { label = $"Param {i + 1} Desc" };
                desc.style.flexGrow = 2;

                row.Add(type);
                row.Add(desc);
                container.Add(row);

                typeFields.Add(type);
                descFields.Add(desc);
            }
        }

        private static void SetParamFields(
            List<TextField> typeFields,
            List<TextField> descFields,
            int visibleCount,
            IReadOnlyList<EventParameterDoc> values,
            bool clearHidden)
        {
            for (var i = 0; i < typeFields.Count; i++)
            {
                var show = i < visibleCount;
                typeFields[i].parent.style.display = show ? DisplayStyle.Flex : DisplayStyle.None;

                if (!show)
                {
                    if (clearHidden)
                    {
                        typeFields[i].value = string.Empty;
                        descFields[i].value = string.Empty;
                    }
                    continue;
                }

                var v = values != null && i < values.Count ? values[i] : null;
                typeFields[i].value = v?.TypeName ?? string.Empty;
                descFields[i].value = v?.Description ?? string.Empty;
            }
        }

        private static List<EventParameterDoc> ReadParamFields(List<TextField> typeFields, List<TextField> descFields)
        {
            var result = new List<EventParameterDoc>();
            for (var i = 0; i < typeFields.Count; i++)
            {
                if (typeFields[i].parent.style.display == DisplayStyle.None)
                {
                    continue;
                }

                result.Add(new EventParameterDoc
                {
                    Index = i + 1,
                    TypeName = (typeFields[i].value ?? string.Empty).Trim(),
                    Description = (descFields[i].value ?? string.Empty).Trim(),
                });
            }
            return result;
        }
    }
}
