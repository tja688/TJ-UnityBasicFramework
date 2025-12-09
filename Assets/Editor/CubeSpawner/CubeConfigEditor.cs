using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

[CustomEditor(typeof(CubeConfig))]
public class CubeConfigEditor : Editor
{
    public override VisualElement CreateInspectorGUI()
    {
        // 1. 创建根节点
        VisualElement root = new VisualElement();

        // 2. 添加标题栏
        Label header = new Label("🔥🔥 超级详细配置面板 🔥🔥");
        header.style.fontSize = 14;
        header.style.unityFontStyleAndWeight = FontStyle.Bold;
        header.style.color = Color.yellow;
        header.style.backgroundColor = new Color(0.2f, 0.2f, 0.2f);
        
        // 【修复 1】C# 中不支持 paddingAll，需分别设置四个方向
        header.style.paddingTop = 10;
        header.style.paddingBottom = 10;
        header.style.paddingLeft = 10;
        header.style.paddingRight = 10;
        
        header.style.marginBottom = 10;
        root.Add(header);

        // 3. 绑定基本属性
        var nameField = new PropertyField(serializedObject.FindProperty("configName"), "配置名称");
        root.Add(nameField);

        var colorField = new PropertyField(serializedObject.FindProperty("baseColor"), "基础颜色");
        root.Add(colorField);

        // 4. 高级选项容器
        Box advancedBox = new Box();
        
        // 【修复 2】C# 中不支持 borderWidth，需分别设置
        advancedBox.style.borderTopWidth = 1;
        advancedBox.style.borderBottomWidth = 1;
        advancedBox.style.borderLeftWidth = 1;
        advancedBox.style.borderRightWidth = 1;

        // 【修复 3】C# 中不支持 borderColor，需分别设置
        advancedBox.style.borderTopColor = Color.gray;
        advancedBox.style.borderBottomColor = Color.gray;
        advancedBox.style.borderLeftColor = Color.gray;
        advancedBox.style.borderRightColor = Color.gray;

        // 【修复 4】paddingAll 同理
        advancedBox.style.marginTop = 15;
        advancedBox.style.paddingTop = 5;
        advancedBox.style.paddingBottom = 5;
        advancedBox.style.paddingLeft = 5;
        advancedBox.style.paddingRight = 5;
        
        Label subHeader = new Label("物理属性");
        subHeader.style.unityFontStyleAndWeight = FontStyle.Bold;
        advancedBox.Add(subHeader);

        advancedBox.Add(new PropertyField(serializedObject.FindProperty("explosionForce")));
        advancedBox.Add(new PropertyField(serializedObject.FindProperty("isDestructible")));
        
        root.Add(advancedBox);

        // 5. 测试按钮
        Button testBtn = new Button(() => { Debug.Log("在检查器里点击了按钮！"); });
        testBtn.text = "测试配置";
        testBtn.style.marginTop = 10;
        testBtn.style.height = 30;
        root.Add(testBtn);

        return root;
    }
}