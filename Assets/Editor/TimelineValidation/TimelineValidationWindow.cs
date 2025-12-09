// Assets/Editor/TimelineValidation/TimelineValidationWindow.cs
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Cheggone.TimelineValidation
{
    public class TimelineValidationWindow : EditorWindow
    {
        private const string UxmlPath = "Assets/Editor/TimelineValidation/UXML/TimelineValidationWindow.uxml";
        private const string UssPath  = "Assets/Editor/TimelineValidation/USS/TimelineValidationWindow.uss";

        private TimelineModel model;

        // UI refs
        private VisualElement ruler;
        private VisualElement tracksContainer;
        private VisualElement libraryContainer;
        private Toggle scaleToggle;
        private FloatField fpsField;
        private Button playBtn;
        private Button addTrackBtn;

        private VisualElement playhead;
        private bool isPlaying;
        private double lastUpdateTime;

        private readonly List<ClipModel> libraryClips = new();

        [MenuItem("Tools/Timeline Validation/Window")]
        public static void Open()
        {
            var w = GetWindow<TimelineValidationWindow>();
            w.titleContent = new GUIContent("Timeline Validation");
            w.minSize = new Vector2(900, 500);
            w.Show();
        }

        private void OnEnable()
        {
            model = new TimelineModel();

            // mock library items (子轨道占位)
            libraryClips.Clear();
            libraryClips.Add(new ClipModel("Anim Clip", 0, 1.2f, "Type=Animation"));
            libraryClips.Add(new ClipModel("SFX Clip", 0, 0.6f, "Type=Audio"));
            libraryClips.Add(new ClipModel("VFX Clip", 0, 0.9f, "Type=VFX"));

            BuildUI();
            EditorApplication.update += OnEditorUpdate;
        }

        private void OnDisable()
        {
            EditorApplication.update -= OnEditorUpdate;
        }

        private void BuildUI()
        {
            rootVisualElement.Clear();

            var tree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(UxmlPath);
            var sheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(UssPath);
            if (tree == null || sheet == null)
            {
                rootVisualElement.Add(new Label("UXML/USS not found. Check paths."));
                return;
            }
            rootVisualElement.styleSheets.Add(sheet);
            tree.CloneTree(rootVisualElement);

            ruler = rootVisualElement.Q<VisualElement>("ruler");
            tracksContainer = rootVisualElement.Q<VisualElement>("tracks-container");
            libraryContainer = rootVisualElement.Q<VisualElement>("library-container");
            scaleToggle = rootVisualElement.Q<Toggle>("scale-toggle");
            fpsField = rootVisualElement.Q<FloatField>("fps-field");
            playBtn = rootVisualElement.Q<Button>("play-btn");
            addTrackBtn = rootVisualElement.Q<Button>("add-track-btn");

            scaleToggle.value = model.useFrameScale;
            fpsField.value = model.fps;

            scaleToggle.RegisterValueChangedCallback(evt =>
            {
                model.useFrameScale = evt.newValue;
                ruler.MarkDirtyRepaint();
                RefreshTracks();
            });
            fpsField.RegisterValueChangedCallback(evt =>
            {
                model.fps = Mathf.Max(1f, evt.newValue);
                ruler.MarkDirtyRepaint();
            });

            playBtn.clicked += TogglePlay;
            addTrackBtn.clicked += () =>
            {
                model.tracks.Add(new TrackModel($"Track Group {model.tracks.Count + 1}"));
                RefreshTracks();
            };

            // 缩放：滚轮
            var timelineBody = rootVisualElement.Q<VisualElement>("timeline-body");
            timelineBody.RegisterCallback<WheelEvent>(OnWheelZoom);

            // 空闲点击 -> 选中 timeline 本身
            timelineBody.RegisterCallback<PointerDownEvent>(evt =>
            {
                if (evt.target == timelineBody || evt.target == tracksContainer || evt.target == ruler)
                {
                    TimelineEvents.SelectTimeline(model);
                }
            });

            SetupRuler();
            SetupPlayhead();
            RefreshLibrary();
            RefreshTracks();

            // 默认通知检查器显示 timeline 信息
            TimelineEvents.SelectTimeline(model);
        }

        private void OnWheelZoom(WheelEvent evt)
        {
            float delta = -evt.delta.y * 0.05f;
            model.zoom = Mathf.Clamp(model.zoom * (1f + delta), 0.2f, 5f);
            ruler.MarkDirtyRepaint();
            RefreshTracks();
            evt.StopPropagation();
        }

        #region Ruler
        private void SetupRuler()
        {
            ruler.generateVisualContent += ctx =>
            {
                var painter = ctx.painter2D;

                float width = ruler.contentRect.width;
                float height = ruler.contentRect.height;

                painter.lineWidth = 1;
                painter.strokeColor = new Color(1,1,1,0.4f);

                float pixelsPerUnit = model.pixelsPerSecond * model.zoom;
                float majorStepSec = model.useFrameScale ? (10f / model.fps) : 1f; // major tick every 1 sec or 10 frames
                float minorStepSec = majorStepSec / 5f;

                int majorCount = Mathf.CeilToInt(width / (pixelsPerUnit * majorStepSec)) + 1;

                for (int i = 0; i < majorCount; i++)
                {
                    float tSec = i * majorStepSec;
                    float x = tSec * pixelsPerUnit;

                    // major line
                    painter.BeginPath();
                    painter.MoveTo(new Vector2(x, height));
                    painter.LineTo(new Vector2(x, height * 0.35f));
                    painter.Stroke();

                    // label
                    string label = model.useFrameScale
                        ? $"{Mathf.RoundToInt(tSec * model.fps)}f"
                        : $"{tSec:0.0}s";

                    ctx.DrawText(label, new Vector2(x + 2, 2), 10, Color.white);

                    // minor ticks
                    if (i < majorCount - 1)
                    {
                        for (int m = 1; m < 5; m++)
                        {
                            float mt = tSec + m * minorStepSec;
                            float mx = mt * pixelsPerUnit;
                            painter.BeginPath();
                            painter.MoveTo(new Vector2(mx, height));
                            painter.LineTo(new Vector2(mx, height * 0.6f));
                            painter.Stroke();
                        }
                    }
                }
            };
        }
        #endregion

        #region Playhead
        private void SetupPlayhead()
        {
            var tracksViewport = rootVisualElement.Q<VisualElement>("tracks-viewport");

            playhead = new VisualElement { name = "playhead" };
            playhead.AddToClassList("playhead");
            tracksViewport.Add(playhead);

            var manipulator = new PlayheadManipulator(this, model);
            playhead.AddManipulator(manipulator);

            UpdatePlayheadVisual();
        }

        public void SetPlayheadTime(float sec)
        {
            model.playheadTime = Mathf.Max(0, sec);
            UpdatePlayheadVisual();
            ruler.MarkDirtyRepaint();
        }

        private void UpdatePlayheadVisual()
        {
            float pixelsPerUnit = model.pixelsPerSecond * model.zoom;
            float x = model.playheadTime * pixelsPerUnit;
            playhead.style.left = x;
        }
        #endregion

        #region Tracks & Clips
        private void RefreshTracks()
        {
            tracksContainer.Clear();

            for (int i = 0; i < model.tracks.Count; i++)
            {
                var track = model.tracks[i];

                var row = new VisualElement();
                row.AddToClassList("track-row");

                // header editable name (timeline头)
                var header = new TextField { value = track.headerName };
                header.AddToClassList("track-header");
                header.RegisterValueChangedCallback(e => track.headerName = e.newValue);
                header.RegisterCallback<PointerDownEvent>(_ => TimelineEvents.SelectTrack(track));
                
                // body (drop zone)
                var body = new VisualElement();
                body.AddToClassList("track-body");
                body.userData = track;

                // allow drops from library
                body.RegisterCallback<DragUpdatedEvent>(OnDragUpdated);
                body.RegisterCallback<DragPerformEvent>(OnDragPerform);

                // add existing clips
                foreach (var clip in track.clips)
                {
                    var clipView = BuildClipView(clip, track);
                    body.Add(clipView);
                }

                row.Add(header);
                row.Add(body);
                tracksContainer.Add(row);
            }
        }

        private VisualElement BuildClipView(ClipModel clip, TrackModel track)
        {
            var ve = new VisualElement();
            ve.AddToClassList("clip");
            ve.name = clip.id;
            ve.userData = clip;

            var label = new Label(clip.name);
            label.AddToClassList("clip-label");
            ve.Add(label);

            // drag
            ve.AddManipulator(new ClipDragManipulator(this, model, track, clip));

            // resize handles
            var leftHandle = new VisualElement();
            leftHandle.AddToClassList("clip-handle-left");
            leftHandle.AddManipulator(new ClipResizeManipulator(this, model, clip, isLeft:true));

            var rightHandle = new VisualElement();
            rightHandle.AddToClassList("clip-handle-right");
            rightHandle.AddManipulator(new ClipResizeManipulator(this, model, clip, isLeft:false));

            ve.Add(leftHandle);
            ve.Add(rightHandle);

            // selection -> inspector shows clip
            ve.RegisterCallback<PointerDownEvent>(evt =>
            {
                TimelineEvents.SelectClip(clip);
                evt.StopPropagation();
            });

            LayoutClip(ve, clip);
            return ve;
        }

        public void LayoutClip(VisualElement clipVe, ClipModel clip)
        {
            float pps = model.pixelsPerSecond * model.zoom;
            clipVe.style.left = clip.start * pps;
            clipVe.style.width = Mathf.Max(10, clip.duration * pps);
        }

        private void OnDragUpdated(DragUpdatedEvent evt)
        {
            if (DragAndDrop.GetGenericData("TimelineLibraryClip") != null)
            {
                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                evt.StopPropagation();
            }
        }

        private void OnDragPerform(DragPerformEvent evt)
        {
            var data = DragAndDrop.GetGenericData("TimelineLibraryClip") as ClipModel;
            if (data == null) return;

            DragAndDrop.AcceptDrag();

            var track = (evt.currentTarget as VisualElement)?.userData as TrackModel;
            if (track == null) return;

            // 新 clip 的 start 自动停靠到当前指针
            var newClip = new ClipModel(data.name, model.playheadTime, data.duration, data.typeHint);
            track.clips.Add(newClip);

            RefreshTracks();
            evt.StopPropagation();
        }
        #endregion

        #region Library
        private void RefreshLibrary()
        {
            libraryContainer.Clear();

            foreach (var c in libraryClips)
            {
                var item = new VisualElement();
                item.AddToClassList("library-item");

                var label = new Label($"{c.name} ({c.typeHint})");
                item.Add(label);

                item.RegisterCallback<PointerDownEvent>(evt =>
                {
                    DragAndDrop.PrepareStartDrag();
                    DragAndDrop.SetGenericData("TimelineLibraryClip", c);
                    DragAndDrop.objectReferences = new Object[] { };
                    DragAndDrop.StartDrag("Timeline Library Clip");
                    evt.StopPropagation();
                });

                libraryContainer.Add(item);
            }
        }
        #endregion

        #region Playback
        private void TogglePlay()
        {
            if (!isPlaying)
            {
                if (model.MaxEndTime <= 0.0001f)
                {
                    EditorUtility.DisplayDialog("Timeline", "Timeline 为空，无法播放。", "OK");
                    return;
                }

                isPlaying = true;
                playBtn.text = "Stop";
                lastUpdateTime = EditorApplication.timeSinceStartup;
            }
            else
            {
                isPlaying = false;
                playBtn.text = "Play";
            }
        }

        private void OnEditorUpdate()
        {
            if (!isPlaying) return;

            double now = EditorApplication.timeSinceStartup;
            float dt = (float)(now - lastUpdateTime);
            lastUpdateTime = now;

            SetPlayheadTime(model.playheadTime + dt);

            if (model.playheadTime >= model.MaxEndTime)
            {
                isPlaying = false;
                playBtn.text = "Play";
            }

            // repaint visuals
            UpdatePlayheadVisual();
        }
        #endregion

        #region Snapping API (for manipulators)
        public float SnapTime(float rawTimeSec, ClipModel draggingClip = null)
        {
            float pps = model.pixelsPerSecond * model.zoom;
            float snapPx = 6f;
            float snapSec = snapPx / pps;

            float best = rawTimeSec;
            float bestDist = snapSec;

            // snap to track start (0)
            if (Mathf.Abs(rawTimeSec - 0f) < bestDist)
            {
                best = 0f; bestDist = Mathf.Abs(rawTimeSec - 0f);
            }

            // snap to playhead
            if (Mathf.Abs(rawTimeSec - model.playheadTime) < bestDist)
            {
                best = model.playheadTime;
                bestDist = Mathf.Abs(rawTimeSec - model.playheadTime);
            }

            // snap to all clip heads/tails
            foreach (var t in model.tracks)
            foreach (var c in t.clips)
            {
                if (draggingClip != null && c.id == draggingClip.id) continue;

                float[] points = { c.start, c.End };
                foreach (var pt in points)
                {
                    float dist = Mathf.Abs(rawTimeSec - pt);
                    if (dist < bestDist)
                    {
                        best = pt;
                        bestDist = dist;
                    }
                }
            }

            return best;
        }
        #endregion
    }
}
