// Assets/Editor/TimelineValidation/TimelineValidationInspectorWindow.cs
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Cheggone.TimelineValidation
{
    public class TimelineValidationInspectorWindow : EditorWindow
    {
        private const string UxmlPath = "Assets/Editor/TimelineValidation/UXML/TimelineValidationInspector.uxml";
        private const string UssPath  = "Assets/Editor/TimelineValidation/USS/TimelineValidationInspector.uss";

        private VisualElement root;
        private Label title;
        private VisualElement content;

        [MenuItem("Tools/Timeline Validation/Inspector")]
        public static void Open()
        {
            var w = GetWindow<TimelineValidationInspectorWindow>();
            w.titleContent = new GUIContent("Timeline Inspector");
            w.minSize = new Vector2(320, 400);
            w.Show();
        }

        private void OnEnable()
        {
            BuildUI();

            TimelineEvents.OnTimelineSelected += ShowTimeline;
            TimelineEvents.OnTrackSelected += ShowTrack;
            TimelineEvents.OnClipSelected += ShowClip;
        }

        private void OnDisable()
        {
            TimelineEvents.OnTimelineSelected -= ShowTimeline;
            TimelineEvents.OnTrackSelected -= ShowTrack;
            TimelineEvents.OnClipSelected -= ShowClip;
        }

        private void BuildUI()
        {
            rootVisualElement.Clear();
            var tree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(UxmlPath);
            var sheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(UssPath);

            if (tree == null || sheet == null)
            {
                rootVisualElement.Add(new Label("Inspector UXML/USS not found."));
                return;
            }

            rootVisualElement.styleSheets.Add(sheet);
            tree.CloneTree(rootVisualElement);

            root = rootVisualElement.Q<VisualElement>("inspector-root");
            title = rootVisualElement.Q<Label>("inspector-title");
            content = rootVisualElement.Q<VisualElement>("inspector-content");
        }

        private void ClearContent() => content.Clear();

        private void ShowTimeline(TimelineModel m)
        {
            ClearContent();
            title.text = "Timeline Info";

            content.Add(new Label($"Tracks: {m.tracks.Count}"));
            content.Add(new Label($"Scale: {(m.useFrameScale ? "Frame" : "Time")}"));
            content.Add(new Label($"FPS: {m.fps}"));
            content.Add(new Label($"Max End: {m.MaxEndTime:0.00}s"));
            content.Add(new Label("Sub Tracks (placeholder):"));
            content.Add(new Label("- Animation Track"));
            content.Add(new Label("- Audio Track"));
            content.Add(new Label("- VFX Track"));
        }

        private void ShowTrack(TrackModel t)
        {
            ClearContent();
            title.text = "Track Info";

            content.Add(new Label($"Name: {t.headerName}"));
            content.Add(new Label($"Clips: {t.clips.Count}"));
        }

        private void ShowClip(ClipModel c)
        {
            ClearContent();
            title.text = "Clip Info";

            content.Add(new Label($"Name: {c.name}"));
            content.Add(new Label($"Start: {c.start:0.00}s"));
            content.Add(new Label($"Duration: {c.duration:0.00}s"));
            content.Add(new Label($"End: {c.End:0.00}s"));
            content.Add(new Label($"Config Hint: {c.typeHint}"));
            content.Add(new HelpBox("这里可以放不同子轨道的专属配置面板（占位）", HelpBoxMessageType.Info));
        }
    }
}
