using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class FeelTimelineWindow : EditorWindow
{
    private const string UxmlPath = "Assets/Editor/FeelTimeLine/FeelTimelineWindow.uxml";
    private const string UssPath = "Assets/Editor/FeelTimeLine/FeelTimelineWindow.uss";
    private const float DefaultPixelsPerSecond = 120f;
    private const float DefaultFrameRate = 60f;
    private const float MinimumDuration = 1f;

    private ObjectField _playerField;
    private VisualElement _tracksScroll;
    private VisualElement _pointer;
    private VisualElement _ruler;
    private Slider _timeSlider;
    private Label _modeLabel;
    private Label _emptyLabel;
    private ToolbarToggle _frameToggle;

    private float _pixelsPerSecond = DefaultPixelsPerSecond;
    private float _frameRate = DefaultFrameRate;
    private float _currentDuration = MinimumDuration;

    private MMF_Player _currentPlayer;

    [MenuItem("Window/Feel/Timeline Viewer", priority = 2050)]
    public static void ShowWindow()
    {
        var window = GetWindow<FeelTimelineWindow>();
        window.titleContent = new GUIContent("Feel Timeline");
        window.minSize = new Vector2(640, 320);
        window.Show();
    }

    private void OnEnable()
    {
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(UxmlPath);
        rootVisualElement.Clear();
        visualTree.CloneTree(rootVisualElement);

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(UssPath);
        rootVisualElement.styleSheets.Add(styleSheet);

        CacheUi();
        HookEvents();
        RefreshTimeline();
    }

    private void CacheUi()
    {
        _playerField = rootVisualElement.Q<ObjectField>("mmf-player-field");
        _playerField.objectType = typeof(MMF_Player);

        _tracksScroll = rootVisualElement.Q<VisualElement>("tracks-scroll");
        _pointer = rootVisualElement.Q<VisualElement>("timeline-pointer");
        _ruler = rootVisualElement.Q<VisualElement>("timeline-ruler");
        _timeSlider = rootVisualElement.Q<Slider>("time-slider");
        _modeLabel = rootVisualElement.Q<Label>("timeline-mode-label");
        _emptyLabel = rootVisualElement.Q<Label>("timeline-empty");
        _frameToggle = rootVisualElement.Q<ToolbarToggle>("frames-toggle");
    }

    private void HookEvents()
    {
        _playerField.RegisterValueChangedCallback(evt =>
        {
            _currentPlayer = evt.newValue as MMF_Player;
            RefreshTimeline();
        });

        _timeSlider.RegisterValueChangedCallback(evt =>
        {
            PositionPointer(evt.newValue);
        });

        _frameToggle.RegisterValueChangedCallback(evt =>
        {
            UpdateModeLabel();
            RefreshTimeline();
        });

        rootVisualElement.Q<ToolbarButton>("preview-play-button").clicked += OnPreviewClicked;
        rootVisualElement.Q<ToolbarButton>("stop-reset-button").clicked += OnStopResetClicked;
    }

    private void UpdateModeLabel()
    {
        _modeLabel.text = _frameToggle.value ? "Frames" : "Seconds";
    }

    private void OnPreviewClicked()
    {
        if (_currentPlayer == null)
        {
            return;
        }

        _currentPlayer.PlayFeedbacks();
    }

    private void OnStopResetClicked()
    {
        if (_currentPlayer == null)
        {
            return;
        }

        _currentPlayer.StopFeedbacks();
        _currentPlayer.ResetFeedbacks();
    }

    private void RefreshTimeline()
    {
        _tracksScroll.Clear();
        _ruler.Clear();

        if (_currentPlayer == null)
        {
            _emptyLabel.style.display = DisplayStyle.Flex;
            _pointer.style.display = DisplayStyle.None;
            return;
        }

        _emptyLabel.style.display = DisplayStyle.None;

        var feedbacks = _currentPlayer.FeedbacksList ?? new List<MMF_Feedback>();
        var groupedTracks = new Dictionary<string, VisualElement>();

        float totalDuration = Mathf.Max(CalculateDuration(feedbacks), MinimumDuration);
        _currentDuration = totalDuration;
        _timeSlider.highValue = _frameToggle.value ? totalDuration * _frameRate : totalDuration;
        BuildRuler(totalDuration);

        foreach (var feedback in feedbacks)
        {
            if (feedback == null)
            {
                continue;
            }

            var trackKey = GetTrackKey(feedback);
            if (!groupedTracks.TryGetValue(trackKey, out var track))
            {
                track = CreateTrack(feedback);
                groupedTracks.Add(trackKey, track);
                _tracksScroll.Add(track);
            }

            AddClip(feedback, track);
        }

        PositionPointer(_timeSlider.value);
    }

    private float CalculateDuration(IEnumerable<MMF_Feedback> feedbacks)
    {
        float duration = 0f;
        foreach (var feedback in feedbacks)
        {
            if (feedback == null)
            {
                continue;
            }

            float start = feedback.Timing != null ? Mathf.Max(0f, feedback.Timing.InitialDelay) : 0f;
            float clipDuration = feedback.FeedbackDuration > 0f ? feedback.FeedbackDuration : 0.1f;
            duration = Mathf.Max(duration, start + clipDuration);
        }

        return duration;
    }

    private VisualElement CreateTrack(MMF_Feedback feedback)
    {
        var trackRoot = new VisualElement();
        trackRoot.AddToClassList("feel-timeline__track");

        var label = new Label(GetTrackLabel(feedback));
        label.AddToClassList("feel-timeline__track-label");
        trackRoot.Add(label);

        var body = new VisualElement();
        body.AddToClassList("feel-timeline__track-body");
        trackRoot.Add(body);
        return trackRoot;
    }

    private void AddClip(MMF_Feedback feedback, VisualElement track)
    {
        var body = track.Q<VisualElement>(className: "feel-timeline__track-body");
        if (body == null)
        {
            return;
        }

        float start = feedback.Timing != null ? Mathf.Max(0f, feedback.Timing.InitialDelay) : 0f;
        float clipDuration = feedback.FeedbackDuration > 0f ? feedback.FeedbackDuration : 0.25f;
        float startPixels = start * _pixelsPerSecond;
        float widthPixels = Mathf.Max(clipDuration * _pixelsPerSecond, 12f);

        var clip = new VisualElement();
        clip.AddToClassList("feel-timeline__clip");
        clip.style.left = startPixels;
        clip.style.width = widthPixels;
        clip.style.backgroundColor = new StyleColor(GetClipColor(feedback));
        clip.tooltip = feedback.Label;

        clip.Add(new Label(GetClipLabel(feedback)) { pickingMode = PickingMode.Ignore });
        body.Add(clip);
    }

    private void BuildRuler(float totalDuration)
    {
        _ruler.Clear();

        int markerCount = Mathf.Clamp(Mathf.CeilToInt(totalDuration) + 1, 2, 32);
        float step = totalDuration / (markerCount - 1);

        for (int i = 0; i < markerCount; i++)
        {
            float time = step * i;
            string label = _frameToggle.value
                ? $"{Mathf.RoundToInt(time * _frameRate)}f"
                : $"{time:F1}s";

            var tick = new VisualElement();
            tick.AddToClassList("feel-timeline__tick");
            tick.style.left = time * _pixelsPerSecond;

            var tickLabel = new Label(label);
            tickLabel.AddToClassList("feel-timeline__tick-label");
            tick.Add(tickLabel);
            _ruler.Add(tick);
        }
    }

    private string GetTrackKey(MMF_Feedback feedback)
    {
        return feedback.GetType().Name;
    }

    private string GetTrackLabel(MMF_Feedback feedback)
    {
        string typeName = feedback.GetType().Name.Replace("MMF_", "");
        if (!string.IsNullOrEmpty(feedback.Label))
        {
            return $"{typeName} - {feedback.Label}";
        }

        return typeName;
    }

    private string GetClipLabel(MMF_Feedback feedback)
    {
        if (!string.IsNullOrEmpty(feedback.Label))
        {
            return feedback.Label;
        }

        return feedback.GetType().Name;
    }

    private Color GetClipColor(MMF_Feedback feedback)
    {
        Color display = feedback.DisplayColor;
        if (display == default)
        {
            return new Color(0.25f, 0.65f, 0.95f, 0.9f);
        }

        display.a = 0.9f;
        return display;
    }

    private void PositionPointer(float sliderValue)
    {
        if (_currentDuration <= 0f)
        {
            return;
        }

        float time = _frameToggle.value ? sliderValue / _frameRate : sliderValue;
        float clampedTime = Mathf.Clamp(time, 0f, _currentDuration);
        float pos = clampedTime * _pixelsPerSecond;
        _pointer.style.left = pos;
        _pointer.style.display = DisplayStyle.Flex;
    }
}
