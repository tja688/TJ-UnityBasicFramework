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
    private VisualElement _tracksRoot;          // timeline-tracks
    private VisualElement _tracksScroll;        // tracks-scroll
    private VisualElement _pointerLane;         // pointer-lane
    private VisualElement _pointer;             // timeline-pointer
    private VisualElement _ruler;               // timeline-ruler
    private UnityEngine.UIElements.Slider _timeSlider;
    private Label _modeLabel;
    private Label _emptyLabel;
    private ToolbarToggle _frameToggle;

    // ✅ 左侧“名字列”占位 spacer
    private VisualElement _timeLeftSpacer;
    private VisualElement _sliderLeftSpacer;

    private float _pixelsPerSecond = DefaultPixelsPerSecond;
    private float _frameRate = DefaultFrameRate;
    private float _currentDuration = MinimumDuration;

    // ✅ 统一的“时间 0 点”偏移：从 tracksRoot 左边到 clip 区起点
    private float _timelineOffsetX = 0f;

    // ✅ 自动分配颜色：按 TrackKey 稳定分配
    private readonly Dictionary<string, Color> _autoTrackColors = new();

    private MMF_Player _currentPlayer;

    // ✅ debounce：避免 GeometryChanged 时疯狂重建
    private bool _pendingRebuild;

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
        UpdateModeLabel();
        RefreshTimeline();

        // ✅ 窗口尺寸变化/布局变化时，重新计算 spacer & pointerLane 的 left
        rootVisualElement.RegisterCallback<GeometryChangedEvent>(_ => RequestRebuildTimelineLayout());
    }

    private void CacheUi()
    {
        _playerField = rootVisualElement.Q<ObjectField>("mmf-player-field");
        _playerField.objectType = typeof(MMF_Player);

        _tracksRoot = rootVisualElement.Q<VisualElement>("timeline-tracks");
        _tracksScroll = rootVisualElement.Q<VisualElement>("tracks-scroll");
        _pointerLane = rootVisualElement.Q<VisualElement>("pointer-lane");

        _pointer = rootVisualElement.Q<VisualElement>("timeline-pointer");
        _ruler = rootVisualElement.Q<VisualElement>("timeline-ruler");
        _timeSlider = rootVisualElement.Q<UnityEngine.UIElements.Slider>("time-slider");

        _timeLeftSpacer = rootVisualElement.Q<VisualElement>("time-left-spacer");
        _sliderLeftSpacer = rootVisualElement.Q<VisualElement>("slider-left-spacer");

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
        if (_currentPlayer == null) return;
        _currentPlayer.PlayFeedbacks();
    }

    private void OnStopResetClicked()
    {
        if (_currentPlayer == null) return;
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

        _timeSlider.lowValue = 0f;
        _timeSlider.highValue = _frameToggle.value ? totalDuration * _frameRate : totalDuration;

        foreach (var feedback in feedbacks)
        {
            if (feedback == null) continue;

            var trackKey = GetTrackKey(feedback);
            if (!groupedTracks.TryGetValue(trackKey, out var track))
            {
                track = CreateTrack(feedback);
                groupedTracks.Add(trackKey, track);
                _tracksScroll.Add(track);
            }

            AddClip(feedback, track);
        }

        // ✅ 等布局稳定后，计算 clip 起点 offset，并重建 ruler/pointer
        RequestRebuildTimelineLayout();
    }

    private void RequestRebuildTimelineLayout()
    {
        if (_pendingRebuild) return;
        _pendingRebuild = true;

        // ExecuteLater(1)：比 0 更稳，避免刚 CloneTree/刚 Add 子元素时 worldBound 还没更新
        rootVisualElement.schedule.Execute(() =>
        {
            _pendingRebuild = false;

            if (_currentPlayer == null) return;

            UpdateTimelineOffset();
            ApplyTimelineOffsetToLayout();

            BuildRuler(_currentDuration);
            PositionPointer(_timeSlider.value);
        }).ExecuteLater(1);
    }

    private void UpdateTimelineOffset()
    {
        _timelineOffsetX = 0f;

        if (_tracksRoot == null || _tracksScroll == null) return;

        var firstBody = _tracksScroll.Q<VisualElement>(className: "feel-timeline__track-body");
        if (firstBody == null) return;

        // ✅ offset = clip 区起点 - tracksRoot 左边（同一世界坐标系，最稳定）
        _timelineOffsetX = firstBody.worldBound.xMin - _tracksRoot.worldBound.xMin;
        if (_timelineOffsetX < 0f) _timelineOffsetX = 0f;
    }

    private void ApplyTimelineOffsetToLayout()
    {
        if (_timeLeftSpacer != null) _timeLeftSpacer.style.width = _timelineOffsetX;
        if (_sliderLeftSpacer != null) _sliderLeftSpacer.style.width = _timelineOffsetX;

        if (_pointerLane != null)
        {
            // pointerLane 的 left = clip 起点
            _pointerLane.style.left = _timelineOffsetX;
        }
    }

    private float CalculateDuration(IEnumerable<MMF_Feedback> feedbacks)
    {
        float duration = 0f;
        foreach (var feedback in feedbacks)
        {
            if (feedback == null) continue;

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
        if (body == null) return;

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

            // ✅ ruler 已经从 clip 区开始了，所以不需要再加 origin 偏移
            tick.style.left = time * _pixelsPerSecond;

            var tickLabel = new Label(label);
            tickLabel.AddToClassList("feel-timeline__tick-label");
            tick.Add(tickLabel);

            _ruler.Add(tick);
        }
    }

    private string GetTrackKey(MMF_Feedback feedback) => feedback.GetType().Name;

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

    // ✅ 修复“全黑”：DisplayColor 默认经常是黑色(0,0,0,1)
    private Color GetClipColor(MMF_Feedback feedback)
    {
        var display = feedback.DisplayColor;

        if (display.a > 0.001f && display != Color.black)
        {
            display.a = 0.9f;
            return display;
        }

        var key = GetTrackKey(feedback);
        if (!_autoTrackColors.TryGetValue(key, out var c))
        {
            float h = Mathf.Repeat(Mathf.Abs(key.GetHashCode()) * 0.6180339887f, 1f);
            c = Color.HSVToRGB(h, 0.55f, 0.9f);
            c.a = 0.9f;
            _autoTrackColors[key] = c;
        }

        return c;
    }

    private void PositionPointer(float sliderValue)
    {
        if (_currentDuration <= 0f) return;

        float time = _frameToggle.value ? sliderValue / _frameRate : sliderValue;
        float clampedTime = Mathf.Clamp(time, 0f, _currentDuration);
        float pos = clampedTime * _pixelsPerSecond;

        // ✅ pointer 的父容器(pointerLane)就是 clip 区的 0 点
        _pointer.style.left = pos;
        _pointer.style.display = DisplayStyle.Flex;
    }
}
