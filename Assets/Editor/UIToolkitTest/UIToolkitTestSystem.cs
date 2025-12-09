using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIToolkitTest
{
    // ==========================================
    // 0. 基础数据 (保持不变)
    // ==========================================
    public class NameCheckerSO : ScriptableObject { }
    public class PositionCheckerSO : ScriptableObject { }

    // ==========================================
    // 1. 发送方窗口：专属检查器 (Special Inspector)
    // ==========================================
    public class SpecialInspectorWindow : EditorWindow
    {
        // 保持引用，防止 GC
        private NameCheckerSO _soA;
        private PositionCheckerSO _soB;

        [MenuItem("Tools/1. 打开专属检查器 (发送方)")]
        public static void ShowWindow()
        {
            GetWindow<SpecialInspectorWindow>("专属检查器").minSize = new Vector2(200, 300);
        }

        private void OnEnable()
        {
            // 在内存中创建两个测试对象
            if (!_soA) { _soA = CreateInstance<NameCheckerSO>(); _soA.name = "名称检查器 (SO_A)"; }
            if (!_soB) { _soB = CreateInstance<PositionCheckerSO>(); _soB.name = "位置追踪器 (SO_B)"; }
        }

        public void CreateGUI()
        {
            // 加载你的专属 UXML/USS
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/UIToolkitTest/SpecialInspector.uxml");
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/UIToolkitTest/SpecialInspector.uss");
            
            // 简单防呆，防止资源还没创建报错
            if (!visualTree || !styleSheet) { rootVisualElement.Add(new Label("请检查 UXML/USS 路径")); return; }

            var root = visualTree.CloneTree();
            root.styleSheets.Add(styleSheet);
            rootVisualElement.Add(root);

            var container = root.Q("ItemContainer");

            // --- 动态生成两个可拖拽的“按钮” ---
            CreateDraggableItem(container, _soA, Color.cyan);
            CreateDraggableItem(container, _soB, Color.green);
        }

        private void CreateDraggableItem(VisualElement parent, ScriptableObject so, Color color)
        {
            var item = new VisualElement();
            item.AddToClassList("draggable-item");
            item.style.borderLeftColor = color; // 覆盖 USS 颜色

            var label = new Label(so.name);
            label.AddToClassList("item-label");
            item.Add(label);

            var hint = new Label("(按住拖拽)");
            hint.style.fontSize = 10;
            hint.style.color = Color.gray;
            item.Add(hint);

            // === 核心：发起拖拽逻辑 ===
            item.RegisterCallback<MouseDownEvent>(evt =>
            {
                if (evt.button == 0) // 左键
                {
                    // 1. 准备拖拽
                    DragAndDrop.PrepareStartDrag();
                    
                    // 2. 填充数据 (这里是关键！放入 objectReferences)
                    DragAndDrop.objectReferences = new UnityEngine.Object[] { so };
                    
                    // 3. 开始拖拽
                    DragAndDrop.StartDrag("Dragging " + so.name);
                    
                    // 视觉反馈
                    Debug.Log($"[发送方] 开始拖拽: {so.name}");
                }
            });

            parent.Add(item);
        }
    }

    // ==========================================
    // 2. 接收方窗口：主测试面板 (Main Receiver)
    // ==========================================
    public class MainTestWindow : EditorWindow
    {
        [MenuItem("Tools/2. 打开主测试面板 (接收方)")]
        public static void ShowWindow()
        {
            GetWindow<MainTestWindow>("主测试面板").minSize = new Vector2(300, 200);
        }

        public void CreateGUI()
        {
            var root = rootVisualElement;
            root.style.backgroundColor = new Color(0.15f, 0.15f, 0.15f);
            root.style.paddingLeft = 20; root.style.paddingRight = 20; root.style.paddingTop = 20;

            root.Add(new Label("把 '专属检查器' 里的东西拖进来") { 
                style = { fontSize = 18, marginBottom = 20, color = Color.white, unityFontStyleAndWeight = FontStyle.Bold } 
            });

            // 创建插槽
            var slot = new VisualElement();
            slot.style.height = 100;
            slot.style.borderTopWidth = 2; slot.style.borderBottomWidth = 2;
            slot.style.borderLeftWidth = 2; slot.style.borderRightWidth = 2;
            slot.style.borderTopColor = Color.gray; slot.style.borderBottomColor = Color.gray;
            slot.style.borderLeftColor = Color.gray; slot.style.borderRightColor = Color.gray;
            slot.style.justifyContent = Justify.Center;
            slot.style.alignItems = Align.Center;
            slot.style.backgroundColor = new Color(0.2f, 0.2f, 0.2f);

            var slotLabel = new Label("插槽 (空)");
            slotLabel.style.fontSize = 20;
            slotLabel.style.color = Color.gray;
            slot.Add(slotLabel);

            // === 核心：接收拖拽逻辑 ===
            
            // 1. 拖拽更新 (每帧检测)
            slot.RegisterCallback<DragUpdatedEvent>(evt =>
            {
                // 检测是否包含 ScriptableObject 类型的对象
                if (DragAndDrop.objectReferences.Length > 0 && DragAndDrop.objectReferences[0] is ScriptableObject)
                {
                    DragAndDrop.visualMode = DragAndDropVisualMode.Link; // 鼠标变图标
                    slot.style.backgroundColor = new Color(0.3f, 0.3f, 0.3f); // 高亮
                }
            });

            // 2. 拖拽离开 (恢复原状)
            slot.RegisterCallback<DragLeaveEvent>(evt =>
            {
                slot.style.backgroundColor = new Color(0.2f, 0.2f, 0.2f);
            });

            // 3. 拖拽放置 (执行逻辑)
            slot.RegisterCallback<DragPerformEvent>(evt =>
            {
                DragAndDrop.AcceptDrag();
                
                var droppedObj = DragAndDrop.objectReferences[0];
                
                // 成功逻辑
                Debug.Log($"[接收方] 收到: {droppedObj.name}");
                
                // UI 更新
                slotLabel.text = $"已接收:\n{droppedObj.name}";
                slotLabel.style.color = Color.yellow;
                slotLabel.style.unityTextAlign = TextAnchor.MiddleCenter;
                slot.style.borderTopColor = Color.yellow; slot.style.borderBottomColor = Color.yellow;
                slot.style.borderLeftColor = Color.yellow; slot.style.borderRightColor = Color.yellow;
            });

            root.Add(slot);
        }
    }
}