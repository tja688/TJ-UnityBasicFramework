using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace MyTools
{
    public class CubeSpawnerWindow : EditorWindow
    {
        // 定义UI控件的引用
        private TextField _nameField;
        private Vector3Field _positionField;
        private Toggle _rigidbodyToggle;
        private Button _spawnButton;

        // 设定资源路径常量，方便管理
        // 【重要】如果你修改了文件夹名称，请务必修改这里的路径
        private const string UXML_PATH = "Assets/Editor/CubeSpawner/CubeSpawnerWindow.uxml";
        private const string USS_PATH = "Assets/Editor/CubeSpawner/CubeSpawnerWindow.uss";

        [MenuItem("Tools/我的工具/UI Toolkit 方块生成器")]
        public static void ShowWindow()
        {
            CubeSpawnerWindow wnd = GetWindow<CubeSpawnerWindow>();
            wnd.titleContent = new GUIContent("方块生成器 (Pro)");
            wnd.minSize = new Vector2(320, 300);
        }

        public void CreateGUI()
        {
            // 1. 获取根节点
            VisualElement root = rootVisualElement;

            // 2. 加载并克隆 UXML (结构)
            // LoadAssetAtPath 需要完整路径和文件后缀
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(UXML_PATH);
            if (visualTree == null)
            {
                root.Add(new Label($"错误：未找到 UXML 文件，请检查路径：{UXML_PATH}"));
                return;
            }
            visualTree.CloneTree(root);

            // 3. 加载并应用 USS (样式)
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(USS_PATH);
            if (styleSheet != null)
            {
                root.styleSheets.Add(styleSheet);
            }
            else
            {
                Debug.LogWarning($"[CubeSpawnerWindow] 未找到 USS 样式文件: {USS_PATH}，界面将使用默认样式。");
            }

            // 4. 查询并绑定 UI 元素 (Query & Bind)
            // 使用 Q<类型>("名称") 来查找 UXML 中定义的元素
            _nameField = root.Q<TextField>("input-name");
            _positionField = root.Q<Vector3Field>("input-position");
            _rigidbodyToggle = root.Q<Toggle>("toggle-rigidbody");
            _spawnButton = root.Q<Button>("btn-spawn");

            // 5. 注册事件回调
            if (_spawnButton != null)
            {
                _spawnButton.clicked += OnSpawnButtonClicked;
            }
        }

        /// <summary>
        /// 点击生成按钮后的逻辑处理
        /// </summary>
        private void OnSpawnButtonClicked()
        {
            // 防空检查
            if (_nameField == null || _positionField == null) return;

            // 获取数据
            string objName = _nameField.value;
            Vector3 spawnPos = _positionField.value;
            bool addRb = _rigidbodyToggle.value;

            // 默认名称处理
            if (string.IsNullOrEmpty(objName)) objName = "Cube";

            // 创建物体
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.name = objName;
            cube.transform.position = spawnPos;

            // 添加组件逻辑
            if (addRb)
            {
                cube.AddComponent<Rigidbody>();
            }

            // 注册撤销 (关键步骤)
            Undo.RegisterCreatedObjectUndo(cube, $"Create {objName}");

            // 选中物体
            Selection.activeGameObject = cube;

            // 可选：在Scene视图聚焦该物体
            SceneView.lastActiveSceneView.FrameSelected();

            Debug.Log($"<color=green>成功生成:</color> {objName} at {spawnPos}");
        }
    }
}