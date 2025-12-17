using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Compilation;
using EditorToolbarToggle = UnityEditor.UIElements.ToolbarToggle;
using UnityEngine;
using UnityEngine.UIElements;

namespace FlyRabbit.EventCenter.EditorToolkit
{
    public sealed class EventToolkitWindow : EditorWindow
    {
        private const string WindowUxmlPath = "Assets/Scripts/EventCenter/Editor/Toolkit/UI/EventToolkitWindow.uxml";
        private const string WindowUssPath = "Assets/Scripts/EventCenter/Editor/Toolkit/UI/EventToolkitWindow.uss";
        private const string CommonUssPath = "Assets/Scripts/EventCenter/Editor/Toolkit/UI/Styles/Common.uss";
        private const string DefinitionsUxmlPath = "Assets/Scripts/EventCenter/Editor/Toolkit/UI/Pages/DefinitionsPage.uxml";
        private const string ProjectUsageUxmlPath = "Assets/Scripts/EventCenter/Editor/Toolkit/UI/Pages/ProjectUsagePage.uxml";
        private const string SceneUxmlPath = "Assets/Scripts/EventCenter/Editor/Toolkit/UI/Pages/ScenePage.uxml";

        private EditorToolbarToggle m_TabDefinitions;
        private EditorToolbarToggle m_TabProjectUsage;
        private EditorToolbarToggle m_TabScene;
        private Button m_RefreshButton;
        private Label m_StatusLabel;

        private VisualElement m_DefinitionsContainer;
        private VisualElement m_ProjectUsageContainer;
        private VisualElement m_SceneContainer;

        private readonly List<EventDefinition> m_Definitions = new List<EventDefinition>();
        private EventNameSourceService m_EventNameSource;
        private DefinitionsPageController m_DefinitionsPage;
        private ProjectUsagePageController m_ProjectUsagePage;
        private ScenePageController m_ScenePage;
        private bool m_MajorEventsSubscribed;

        [MenuItem("Tools/FlyRabbit/EventCenter Toolkit")]
        public static void Open()
        {
            var window = GetWindow<EventToolkitWindow>();
            window.titleContent = new GUIContent("EventCenter Toolkit");
            window.Show();
        }

        // NOTE: EditorWindow doesn't have a virtual CreateGUI() to override in some Unity versions.
        // UI Toolkit will still invoke this by name.
        public void CreateGUI()
        {
            rootVisualElement.Clear();

            var windowUxml = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(WindowUxmlPath);
            if (windowUxml == null)
            {
                rootVisualElement.Add(new Label($"Missing UXML: {WindowUxmlPath}"));
                return;
            }
            windowUxml.CloneTree(rootVisualElement);

            TryAddStyleSheet(WindowUssPath);
            TryAddStyleSheet(CommonUssPath);

            m_TabDefinitions = rootVisualElement.Q<EditorToolbarToggle>("tabDefinitions");
            m_TabProjectUsage = rootVisualElement.Q<EditorToolbarToggle>("tabProjectUsage");
            m_TabScene = rootVisualElement.Q<EditorToolbarToggle>("tabScene");
            m_RefreshButton = rootVisualElement.Q<Button>("refreshButton");
            m_StatusLabel = rootVisualElement.Q<Label>("statusLabel");

            m_DefinitionsContainer = rootVisualElement.Q<VisualElement>("definitionsContainer");
            m_ProjectUsageContainer = rootVisualElement.Q<VisualElement>("projectUsageContainer");
            m_SceneContainer = rootVisualElement.Q<VisualElement>("sceneContainer");

            var definitionsRoot = MountPage(m_DefinitionsContainer, DefinitionsUxmlPath);
            var projectUsageRoot = MountPage(m_ProjectUsageContainer, ProjectUsageUxmlPath);
            var sceneRoot = MountPage(m_SceneContainer, SceneUxmlPath);

            m_EventNameSource = new EventNameSourceService();
            m_DefinitionsPage = new DefinitionsPageController(m_EventNameSource, RefreshDefinitionsAndNotifyPages, ShowStatus);
            m_ProjectUsagePage = new ProjectUsagePageController(ShowStatus);
            m_ScenePage = new ScenePageController(ShowStatus);

            m_DefinitionsPage.Mount(definitionsRoot);
            m_ProjectUsagePage.Mount(projectUsageRoot);
            m_ScenePage.Mount(sceneRoot);

            WireTabs();

            if (m_RefreshButton != null)
            {
                m_RefreshButton.clicked += RefreshDefinitionsAndNotifyPages;
            }

            RefreshDefinitionsAndNotifyPages();
            m_ScenePage?.OnPlayModeChanged();
        }

        private void OnEnable()
        {
            SubscribeMajorEvents();
            EditorApplication.delayCall += RefreshOnEnable;
        }

        private void OnDisable()
        {
            UnsubscribeMajorEvents();
            m_ScenePage?.OnWindowDisabled();
        }

        private void RefreshOnEnable()
        {
            if (this == null)
            {
                return;
            }

            // After domain reload / playmode transitions, the window instance can exist but UI may not be built yet.
            if (rootVisualElement.childCount == 0)
            {
                CreateGUI(); // CreateGUI will refresh definitions internally.
                return;
            }

            if (m_EventNameSource == null)
            {
                return;
            }

            RefreshDefinitionsAndNotifyPages();
            m_ScenePage?.OnPlayModeChanged();
        }

        private void WireTabs()
        {
            void SetTab(EditorToolbarToggle selected)
            {
                if (m_TabDefinitions == null || m_TabProjectUsage == null || m_TabScene == null)
                {
                    return;
                }

                m_TabDefinitions.SetValueWithoutNotify(selected == m_TabDefinitions);
                m_TabProjectUsage.SetValueWithoutNotify(selected == m_TabProjectUsage);
                m_TabScene.SetValueWithoutNotify(selected == m_TabScene);

                if (m_DefinitionsContainer != null)
                    m_DefinitionsContainer.style.display = selected == m_TabDefinitions ? DisplayStyle.Flex : DisplayStyle.None;
                if (m_ProjectUsageContainer != null)
                    m_ProjectUsageContainer.style.display = selected == m_TabProjectUsage ? DisplayStyle.Flex : DisplayStyle.None;
                if (m_SceneContainer != null)
                    m_SceneContainer.style.display = selected == m_TabScene ? DisplayStyle.Flex : DisplayStyle.None;
            }

            if (m_TabDefinitions != null)
                m_TabDefinitions.RegisterValueChangedCallback(_ => SetTab(m_TabDefinitions));
            if (m_TabProjectUsage != null)
                m_TabProjectUsage.RegisterValueChangedCallback(_ => SetTab(m_TabProjectUsage));
            if (m_TabScene != null)
                m_TabScene.RegisterValueChangedCallback(_ => SetTab(m_TabScene));

            SetTab(m_TabDefinitions);
        }

        private void SubscribeMajorEvents()
        {
            if (m_MajorEventsSubscribed)
            {
                return;
            }

            m_MajorEventsSubscribed = true;
            CompilationPipeline.compilationFinished += OnCompilationFinished;
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        private void UnsubscribeMajorEvents()
        {
            if (!m_MajorEventsSubscribed)
            {
                return;
            }

            m_MajorEventsSubscribed = false;
            CompilationPipeline.compilationFinished -= OnCompilationFinished;
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
        }

        private void OnCompilationFinished(object _)
        {
            EditorApplication.delayCall += () =>
            {
                if (this == null)
                {
                    return;
                }

                // Use guarded refresh to avoid NRE when UI is not yet built.
                RefreshOnEnable();
            };
        }

        private void OnPlayModeStateChanged(PlayModeStateChange change)
        {
            EditorApplication.delayCall += () =>
            {
                if (this == null)
                {
                    return;
                }

                m_ScenePage?.OnPlayModeChanged();

                // Use guarded refresh to avoid NRE when UI is not yet built.
                RefreshOnEnable();
            };
        }

        private VisualElement MountPage(VisualElement container, string uxmlPath)
        {
            if (container == null)
            {
                return null;
            }

            container.Clear();

            var pageUxml = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(uxmlPath);
            if (pageUxml == null)
            {
                container.Add(new Label($"Missing UXML: {uxmlPath}"));
                return container;
            }

            var pageRoot = pageUxml.CloneTree();
            container.Add(pageRoot);
            return pageRoot;
        }

        private void TryAddStyleSheet(string path)
        {
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(path);
            if (styleSheet != null)
            {
                rootVisualElement.styleSheets.Add(styleSheet);
            }
        }

        private void RefreshDefinitionsAndNotifyPages()
        {
            // Guard: this can be called by major events (playmode/compilation) before CreateGUI ran.
            if (m_EventNameSource == null)
            {
                return;
            }

            m_Definitions.Clear();
            var list = m_EventNameSource.ReadAll(out var error);
            if (list != null)
            {
                m_Definitions.AddRange(list);
            }

            m_DefinitionsPage?.SetDefinitions(m_Definitions, error);
            m_ProjectUsagePage?.SetDefinitions(m_Definitions, error);
            m_ScenePage?.SetDefinitions(m_Definitions, error);

            if (!string.IsNullOrWhiteSpace(error))
            {
                ShowStatus(error);
                return;
            }

            ShowStatus(EditorApplication.isCompiling ? "Compiling..." : $"Loaded {m_Definitions.Count} events.");
        }

        private void ShowStatus(string message)
        {
            if (m_StatusLabel == null)
            {
                return;
            }

            m_StatusLabel.text = message ?? string.Empty;
        }
    }
}
