using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements; // 修复 ColorField 报错的关键引用
using UnityEngine;
using UnityEngine.UIElements;

public class TimelineTestWindow : EditorWindow
{
    // === 核心数据变量 ===
    private float m_ZoomScale = 100f; // 每秒对应的像素宽度 (Pixels Per Second)
    private float m_CurrentTime = 0f; // 当前播放时间 (秒)
    private bool m_IsPlaying = false;
    private double m_LastFrameTime;
    
    // 配置
    private const float MIN_ZOOM = 10f;
    private const float MAX_ZOOM = 500f;
    private const float HEADER_WIDTH = 150f; // 轨道头宽度
    private const float SNAP_THRESHOLD = 10f; // 吸附像素阈值

    // UI 元素引用
    private VisualElement m_RulerContainer;
    private VisualElement m_TrackContainer;
    private VisualElement m_InspectorContent;
    private VisualElement m_PlayheadLine; // 贯穿整个高度的指针线
    private VisualElement m_PlayheadCap;  // 标尺上的指针头
    private ScrollView m_MainScrollView;
    private Toggle m_UnitToggle; // true=Frames, false=Seconds

    // 逻辑数据模型 (简化版)
    private class ClipData
    {
        public string id;
        public string name;
        public float startTime;
        public float duration;
        public int trackIndex;
        public Color color;
    }
    
    private class TrackData
    {
        public string name;
        public List<ClipData> clips = new List<ClipData>();
    }

    private List<TrackData> m_Tracks = new List<TrackData>();
    private ClipData m_SelectedClip = null;

    [MenuItem("Tools/Timeline Verifier")]
    public static void ShowWindow()
    {
        TimelineTestWindow wnd = GetWindow<TimelineTestWindow>();
        wnd.titleContent = new GUIContent("Timeline Verifier");
    }

    public void CreateGUI()
    {
        // 1. 加载 UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/TimelineTest/TimelineTest.uxml");
        if(visualTree == null) 
        {
            rootVisualElement.Add(new Label("请确保 TimelineTest.uxml 在 Assets/Editor/TimelineTest/ 目录下"));
            return;
        }
        visualTree.CloneTree(rootVisualElement);

        // 2. 加载 USS
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/TimelineTest/TimelineTest.uss");
        if(styleSheet != null) rootVisualElement.styleSheets.Add(styleSheet);

        // 3. 获取 UI 引用
        m_RulerContainer = rootVisualElement.Q("RulerContainer");
        m_TrackContainer = rootVisualElement.Q("TrackContainer");
        m_InspectorContent = rootVisualElement.Q("InspectorContent");
        m_MainScrollView = rootVisualElement.Q<ScrollView>("TrackScrollView");
        m_UnitToggle = rootVisualElement.Q<Toggle>("UnitToggle");

        // 4. 初始化功能组件
        SetupToolbar();
        SetupPalette();
        SetupRulerEvents();
        SetupTimelineEvents(); // 缩放与空白点击
        
        // 初始化指针 UI
        CreatePlayheadUI();

        // 5. 初始化一些测试数据 (如果为空)
        if (m_Tracks.Count == 0)
        {
            AddTrack("角色动画轨道");
            AddTrack("特效轨道");
        }
        
        // 首次绘制
        RefreshTracksUI();
        UpdateInspector(null); // 默认选中空闲
    }

    private void OnEnable()
    {
        EditorApplication.update += OnEditorUpdate;
    }

    private void OnDisable()
    {
        EditorApplication.update -= OnEditorUpdate;
    }

    // === 播放逻辑 (需求 8) ===
    private void OnEditorUpdate()
    {
        if (m_IsPlaying)
        {
            double timeNow = EditorApplication.timeSinceStartup;
            float deltaTime = (float)(timeNow - m_LastFrameTime);
            m_LastFrameTime = timeNow;

            m_CurrentTime += deltaTime;

            // 查找最远结束时间
            float maxTime = 0f;
            foreach (var track in m_Tracks)
            {
                foreach (var clip in track.clips)
                {
                    if (clip.startTime + clip.duration > maxTime) maxTime = clip.startTime + clip.duration;
                }
            }

            // 空Timeline警告
            if (maxTime <= 0.01f)
            {
                Debug.LogWarning("Timeline为空或长度为0，停止播放。");
                StopPlayback();
                return;
            }

            // 停止逻辑
            if (m_CurrentTime >= maxTime)
            {
                m_CurrentTime = maxTime;
                StopPlayback();
            }

            UpdatePlayheadPosition();
            m_RulerContainer.MarkDirtyRepaint(); // 重绘标尺文字
        }
    }

    private void StopPlayback()
    {
        m_IsPlaying = false;
        var btn = rootVisualElement.Q<Button>("PlayButton");
        if (btn != null) btn.text = "播放 ▶";
    }

    // === 初始化 UI 组件 ===

    private void CreatePlayheadUI()
    {
        // 标尺上的头
        m_PlayheadCap = new VisualElement();
        m_PlayheadCap.AddToClassList("playhead-cap");
        m_RulerContainer.Add(m_PlayheadCap);

        // 贯穿轨道的线
        m_PlayheadLine = new VisualElement();
        m_PlayheadLine.AddToClassList("playhead");
        m_TrackContainer.Add(m_PlayheadLine);
        
        UpdatePlayheadPosition();
    }

    private void SetupToolbar()
    {
        rootVisualElement.Q<Button>("PlayButton").clicked += () => 
        {
            m_IsPlaying = !m_IsPlaying;
            if (m_IsPlaying)
            {
                m_LastFrameTime = EditorApplication.timeSinceStartup;
                rootVisualElement.Q<Button>("PlayButton").text = "暂停 ||";
            }
            else
            {
                rootVisualElement.Q<Button>("PlayButton").text = "播放 ▶";
            }
        };

        rootVisualElement.Q<Button>("StopButton").clicked += () => 
        {
            StopPlayback();
            m_CurrentTime = 0f;
            UpdatePlayheadPosition();
        };

        rootVisualElement.Q<Button>("AddTrackBtn").clicked += () => 
        {
            AddTrack($"新轨道 {m_Tracks.Count + 1}");
            RefreshTracksUI();
        };

        m_UnitToggle.RegisterValueChangedCallback(evt => 
        {
            DrawRulerLabels(); // 切换单位时重绘标尺
            UpdateInspector(m_SelectedClip); // 刷新检查器显示
        });
    }

    private void SetupPalette()
    {
        // 创建可拖拽的资源 (需求 4, 6)
        var palette = rootVisualElement.Q("PaletteContainer");
        string[] types = new[] { "动画片段", "音效片段", "特效命令" };
        
        foreach (var t in types)
        {
            var item = new Label(t);
            item.AddToClassList("palette-item");
            palette.Add(item);

            // 注册拖拽开始事件
            item.RegisterCallback<MouseDownEvent>(evt => 
            {
                DragAndDrop.PrepareStartDrag();
                DragAndDrop.SetGenericData("ClipType", t);
                DragAndDrop.StartDrag(t);
            });
        }
    }

    // === 交互逻辑：缩放与点击空白 ===

    private void SetupTimelineEvents()
    {
        var area = rootVisualElement.Q("TimelineArea");

        // 需求 2: 鼠标滚轮缩放
        area.RegisterCallback<WheelEvent>(evt => 
        {
            // 默认只要滚轮就缩放
            float delta = -evt.delta.y;
            float zoomFactor = delta > 0 ? 1.1f : 0.9f;
            m_ZoomScale = Mathf.Clamp(m_ZoomScale * zoomFactor, MIN_ZOOM, MAX_ZOOM);
            
            RefreshTracksUI();
            DrawRulerLabels();
            UpdatePlayheadPosition();
            evt.StopPropagation();
        });

        // 需求 3: 点击空白处
        m_MainScrollView.RegisterCallback<MouseDownEvent>(evt => 
        {
            if (evt.target == m_MainScrollView || evt.target == m_TrackContainer)
            {
                m_SelectedClip = null;
                RefreshTracksUI();
                UpdateInspector(null);
            }
        });
        
        // 拖拽放入 (需求 4)
        m_TrackContainer.RegisterCallback<DragUpdatedEvent>(evt => 
        {
            DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
        });

        m_TrackContainer.RegisterCallback<DragPerformEvent>(evt => 
        {
            var data = DragAndDrop.GetGenericData("ClipType") as string;
            if (!string.IsNullOrEmpty(data))
            {
                DragAndDrop.AcceptDrag();
                
                // 计算放入的时间位置
                float mouseX = evt.localMousePosition.x - HEADER_WIDTH;
                float time = Mathf.Max(0, mouseX / m_ZoomScale);
                
                // 计算放入的轨道行
                float mouseY = evt.localMousePosition.y;
                int trackIndex = Mathf.FloorToInt(mouseY / 40f); // 假设行高40

                if (trackIndex >= 0 && trackIndex < m_Tracks.Count)
                {
                    // 需求 5: "子轨道拖入timeline中时会自动把开头停靠指针位置"
                    CreateClip(trackIndex, m_CurrentTime, 2.0f, data);
                }
            }
        });
    }

    // === 交互逻辑：标尺与指针 (需求 5) ===

    private void SetupRulerEvents()
    {
        // 标尺点击与拖拽
        m_RulerContainer.RegisterCallback<MouseDownEvent>(evt => 
        {
            UpdatePlayheadFromMouse(evt.localMousePosition.x);
            m_RulerContainer.CaptureMouse();
        });

        m_RulerContainer.RegisterCallback<MouseMoveEvent>(evt => 
        {
            if (m_RulerContainer.HasMouseCapture())
            {
                UpdatePlayheadFromMouse(evt.localMousePosition.x);
            }
        });

        m_RulerContainer.RegisterCallback<MouseUpEvent>(evt => 
        {
            m_RulerContainer.ReleaseMouse();
        });
        
        // 初始绘制刻度
        m_RulerContainer.RegisterCallback<GeometryChangedEvent>(evt => DrawRulerLabels());
    }

    private void UpdatePlayheadFromMouse(float mouseX)
    {
        float rawX = mouseX - HEADER_WIDTH;
        float time = Mathf.Max(0, rawX / m_ZoomScale);

        // 需求 5: 吸附逻辑 (吸附 Clip 头、尾、Timeline头)
        float snapTime = time;
        float minDiff = float.MaxValue;
        
        // 1. 吸附 Timeline 头 (0)
        if (Mathf.Abs(time - 0) * m_ZoomScale < SNAP_THRESHOLD)
        {
            snapTime = 0;
            minDiff = Mathf.Abs(time - 0);
        }

        // 2. 吸附所有 Clip 的头尾
        foreach (var track in m_Tracks)
        {
            foreach (var clip in track.clips)
            {
                // 头
                float diffStart = Mathf.Abs(time - clip.startTime);
                if (diffStart * m_ZoomScale < SNAP_THRESHOLD && diffStart < minDiff)
                {
                    snapTime = clip.startTime;
                    minDiff = diffStart;
                }
                // 尾
                float diffEnd = Mathf.Abs(time - (clip.startTime + clip.duration));
                if (diffEnd * m_ZoomScale < SNAP_THRESHOLD && diffEnd < minDiff)
                {
                    snapTime = clip.startTime + clip.duration;
                    minDiff = diffEnd;
                }
            }
        }

        m_CurrentTime = snapTime;
        UpdatePlayheadPosition();
        
        // 刷新检查器以显示当前时间
        if (m_SelectedClip == null) UpdateInspector(null);
    }

    private void UpdatePlayheadPosition()
    {
        float xPos = HEADER_WIDTH + (m_CurrentTime * m_ZoomScale);
        
        m_PlayheadCap.style.left = xPos - 7;
        m_PlayheadLine.style.left = xPos;
    }

    private void DrawRulerLabels()
    {
        m_RulerContainer.Clear();
        m_RulerContainer.Add(m_PlayheadCap); // 把指针头加回来

        float width = m_RulerContainer.contentRect.width - HEADER_WIDTH;
        if (width <= 0) return;

        // 根据缩放决定刻度间隔
        float step = 1.0f; // 默认1秒
        if (m_ZoomScale > 100) step = 0.5f;
        if (m_ZoomScale < 30) step = 5.0f;

        int count = Mathf.CeilToInt(width / m_ZoomScale / step) + 1;

        for (int i = 0; i < count; i++)
        {
            float t = i * step;
            float x = HEADER_WIDTH + t * m_ZoomScale;

            var label = new Label();
            label.AddToClassList("ruler-label");
            label.style.left = x;
            
            // 需求 2: 支持帧刻度
            if (m_UnitToggle.value)
            {
                label.text = Mathf.RoundToInt(t * 60).ToString(); // 假设60fps
            }
            else
            {
                label.text = t.ToString("F1") + "s";
            }
            
            m_RulerContainer.Add(label);
        }
    }

    // === 轨道与片段绘制逻辑 ===

    private void AddTrack(string name)
    {
        m_Tracks.Add(new TrackData { name = name });
    }

    private void CreateClip(int trackIndex, float start, float duration, string name)
    {
        var clip = new ClipData
        {
            id = System.Guid.NewGuid().ToString(),
            name = name,
            startTime = start,
            duration = duration,
            trackIndex = trackIndex,
            color = Color.HSVToRGB(UnityEngine.Random.value, 0.6f, 0.8f) // 随机颜色
        };
        m_Tracks[trackIndex].clips.Add(clip);
        RefreshTracksUI();
    }

    private void RefreshTracksUI()
    {
        m_TrackContainer.Clear();
        m_TrackContainer.Add(m_PlayheadLine); // 把指针线加回来

        for (int i = 0; i < m_Tracks.Count; i++)
        {
            var track = m_Tracks[i];
            
            // 轨道行
            var row = new VisualElement();
            row.AddToClassList("track-row");
            
            // 需求 9: 轨道头可编辑名字
            var header = new TextField();
            header.value = track.name;
            header.AddToClassList("track-header");
            header.RegisterValueChangedCallback(evt => track.name = evt.newValue);
            row.Add(header);

            // 轨道内容区
            var content = new VisualElement();
            content.AddToClassList("track-content");
            row.Add(content);

            // 绘制 Clips
            foreach (var clip in track.clips)
            {
                var clipInfo = clip; // 闭包捕获
                var clipEl = new VisualElement();
                clipEl.AddToClassList("clip-element");
                clipEl.style.backgroundColor = clipInfo.color;
                
                // 位置与大小
                clipEl.style.left = clipInfo.startTime * m_ZoomScale;
                clipEl.style.width = clipInfo.duration * m_ZoomScale;
                
                var label = new Label(clipInfo.name);
                clipEl.Add(label);

                if (m_SelectedClip == clip)
                {
                    clipEl.AddToClassList("clip-selected");
                }

                // 需求 7: 点击子轨道更新检查器
                clipEl.RegisterCallback<MouseDownEvent>(evt => 
                {
                    if (evt.button == 0)
                    {
                        m_SelectedClip = clipInfo;
                        RefreshTracksUI(); // 刷新高亮
                        UpdateInspector(clipInfo);
                        evt.StopPropagation();
                    }
                });
                
                // 需求 4: 拖动调整宽度 (右边缘)
                // 修复：移除CursorType报错，暂时移除光标视觉变化，保留拖动逻辑
                /* 如果需要光标变化，需加载自定义Texture。
                   此处仅保留逻辑。
                */

                // 拖拽逻辑
                clipEl.RegisterCallback<MouseDownEvent>(evt => 
                {
                    if (evt.button == 0)
                    {
                        // 判定是否在右边缘 10px 范围内
                        bool isResizing = evt.localMousePosition.x > clipEl.resolvedStyle.width - 10;
                        float startX = evt.mousePosition.x;
                        float initialStartTime = clipInfo.startTime;
                        float initialDuration = clipInfo.duration;

                        clipEl.CaptureMouse();
                        
                        EventCallback<MouseMoveEvent> moveHandler = null;
                        EventCallback<MouseUpEvent> upHandler = null;

                        moveHandler = (moveEvt) => 
                        {
                            float deltaPixels = moveEvt.mousePosition.x - startX;
                            float deltaTime = deltaPixels / m_ZoomScale;

                            if (isResizing)
                            {
                                clipInfo.duration = Mathf.Max(0.1f, initialDuration + deltaTime);
                            }
                            else
                            {
                                clipInfo.startTime = Mathf.Max(0f, initialStartTime + deltaTime);
                            }
                            
                            // 实时更新UI
                            clipEl.style.left = clipInfo.startTime * m_ZoomScale;
                            clipEl.style.width = clipInfo.duration * m_ZoomScale;
                            UpdateInspector(clipInfo); 
                        };

                        upHandler = (upEvt) => 
                        {
                            clipEl.ReleaseMouse();
                            clipEl.UnregisterCallback(moveHandler);
                            clipEl.UnregisterCallback(upHandler);
                            RefreshTracksUI(); 
                        };

                        clipEl.RegisterCallback(moveHandler);
                        clipEl.RegisterCallback(upHandler);
                        evt.StopPropagation();
                    }
                });

                content.Add(clipEl);
            }

            m_TrackContainer.Add(row);
        }
    }

    // === 检查器逻辑 (需求 3, 7) ===
    
    private void UpdateInspector(ClipData clip)
    {
        m_InspectorContent.Clear();

        if (clip == null)
        {
            AddInspectorLabel("当前对象: Timeline 全局配置");
            AddInspectorLabel($"总轨道数: {m_Tracks.Count}");
            AddInspectorLabel($"当前时间: {m_CurrentTime:F2}s");
            AddInspectorLabel($"缩放比例: {m_ZoomScale:F0} px/s");
            
            var helpBox = new HelpBox("这是几种子轨道类型 (功能占位):\n1. 动画轨道\n2. 音频轨道\n3. 事件轨道", HelpBoxMessageType.Info);
            m_InspectorContent.Add(helpBox);
        }
        else
        {
            AddInspectorLabel($"当前对象: 子轨道片段");
            AddInspectorLabel($"名称: {clip.name}");
            
            var startField = new FloatField("开始时间");
            startField.value = clip.startTime;
            startField.RegisterValueChangedCallback(evt => { clip.startTime = evt.newValue; RefreshTracksUI(); });
            m_InspectorContent.Add(startField);

            var durField = new FloatField("持续时间");
            durField.value = clip.duration;
            durField.RegisterValueChangedCallback(evt => { clip.duration = evt.newValue; RefreshTracksUI(); });
            m_InspectorContent.Add(durField);

            var colorField = new ColorField("显示颜色");
            colorField.value = clip.color;
            colorField.RegisterValueChangedCallback(evt => { clip.color = evt.newValue; RefreshTracksUI(); });
            m_InspectorContent.Add(colorField);
            
            var note = new Label("这里是子轨道具体的配置信息占位符...");
            note.style.color = Color.gray;
            m_InspectorContent.Add(note);
        }
    }

    private void AddInspectorLabel(string text)
    {
        var l = new Label(text);
        l.AddToClassList("inspector-label");
        m_InspectorContent.Add(l);
    }
}