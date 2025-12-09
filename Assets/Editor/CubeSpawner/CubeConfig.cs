using UnityEngine;

// 创建一个可以在 Project 面板右键创建的配置文件
[CreateAssetMenu(fileName = "CubeConfig", menuName = "我的工具/Cube配置数据")]
public class CubeConfig : ScriptableObject
{
    // 这里放那些“很复杂很复杂”的配置
    public string configName = "高级方块配置";
    [Range(0, 10)] public float explosionForce = 5.0f;
    public Color baseColor = Color.red;
    public bool isDestructible = true;
}