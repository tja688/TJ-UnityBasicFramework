<!-- Token Count: ~40,392 -->

# 📄 GitHub - Ma-Hup/ProjectBaseUnity: 将游戏的功能整理分类，集合了常用的单例，降低样板代码，适合mini、小型游戏开发、GameJam，既可作为扩展，也能作为基础框架。 包含 资源管理、UI 管理、事件、持久化、对象池、音乐、输入、常量管理、Mono等常见功能。
**Source:** https://github.com/Ma-Hup/ProjectBaseUnity

[Skip to content](https://github.com/Ma-Hup/ProjectBaseUnity#start-of-content)
  

You signed in with another tab or window. [Reload](https://github.com/Ma-Hup/ProjectBaseUnity)
 to refresh your session. You signed out in another tab or window. [Reload](https://github.com/Ma-Hup/ProjectBaseUnity)
 to refresh your session. You switched accounts on another tab or window. [Reload](https://github.com/Ma-Hup/ProjectBaseUnity)
 to refresh your session. Dismiss alert

[Ma-Hup](https://github.com/Ma-Hup) / **[ProjectBaseUnity](https://github.com/Ma-Hup/ProjectBaseUnity)** Public

*   [Notifications](https://github.com/login?return_to=%2FMa-Hup%2FProjectBaseUnity)
     You must be signed in to change notification settings
*   [Fork 0](https://github.com/login?return_to=%2FMa-Hup%2FProjectBaseUnity)
    
*   [Star 10](https://github.com/login?return_to=%2FMa-Hup%2FProjectBaseUnity)
    

将游戏的功能整理分类，集合了常用的单例，降低样板代码，适合mini、小型游戏开发、GameJam，既可作为扩展，也能作为基础框架。 包含 资源管理、UI 管理、事件、持久化、对象池、音乐、输入、常量管理、Mono等常见功能。

[10 stars](https://github.com/Ma-Hup/ProjectBaseUnity/stargazers)
 [0 forks](https://github.com/Ma-Hup/ProjectBaseUnity/forks)
 [Branches](https://github.com/Ma-Hup/ProjectBaseUnity/branches)
 [Tags](https://github.com/Ma-Hup/ProjectBaseUnity/tags)
 [Activity](https://github.com/Ma-Hup/ProjectBaseUnity/activity)

[Star](https://github.com/login?return_to=%2FMa-Hup%2FProjectBaseUnity)

[Notifications](https://github.com/login?return_to=%2FMa-Hup%2FProjectBaseUnity)
 You must be signed in to change notification settings

Ma-Hup/ProjectBaseUnity
=======================

  main

[Branches](https://github.com/Ma-Hup/ProjectBaseUnity/branches)
[Tags](https://github.com/Ma-Hup/ProjectBaseUnity/tags)

[](https://github.com/Ma-Hup/ProjectBaseUnity/branches)
[](https://github.com/Ma-Hup/ProjectBaseUnity/tags)

Go to file

Code

Open more actions menu

Folders and files
-----------------

| Name |     | Name | Last commit message | Last commit date |
| --- | --- | --- | --- |
| Latest commit<br>-------------<br><br>History<br>-------<br><br>[5 Commits](https://github.com/Ma-Hup/ProjectBaseUnity/commits/main/)<br><br>[](https://github.com/Ma-Hup/ProjectBaseUnity/commits/main/) |     |     |
| [.vscode](https://github.com/Ma-Hup/ProjectBaseUnity/tree/main/.vscode ".vscode") |     | [.vscode](https://github.com/Ma-Hup/ProjectBaseUnity/tree/main/.vscode ".vscode") |     |     |
| [Assets](https://github.com/Ma-Hup/ProjectBaseUnity/tree/main/Assets "Assets") |     | [Assets](https://github.com/Ma-Hup/ProjectBaseUnity/tree/main/Assets "Assets") |     |     |
| [Packages](https://github.com/Ma-Hup/ProjectBaseUnity/tree/main/Packages "Packages") |     | [Packages](https://github.com/Ma-Hup/ProjectBaseUnity/tree/main/Packages "Packages") |     |     |
| [ProjectSettings](https://github.com/Ma-Hup/ProjectBaseUnity/tree/main/ProjectSettings "ProjectSettings") |     | [ProjectSettings](https://github.com/Ma-Hup/ProjectBaseUnity/tree/main/ProjectSettings "ProjectSettings") |     |     |
| [.gitignore](https://github.com/Ma-Hup/ProjectBaseUnity/blob/main/.gitignore ".gitignore") |     | [.gitignore](https://github.com/Ma-Hup/ProjectBaseUnity/blob/main/.gitignore ".gitignore") |     |     |
| [README.md](https://github.com/Ma-Hup/ProjectBaseUnity/blob/main/README.md "README.md") |     | [README.md](https://github.com/Ma-Hup/ProjectBaseUnity/blob/main/README.md "README.md") |     |     |
| [UnityPackage.zip](https://github.com/Ma-Hup/ProjectBaseUnity/blob/main/UnityPackage.zip "UnityPackage.zip") |     | [UnityPackage.zip](https://github.com/Ma-Hup/ProjectBaseUnity/blob/main/UnityPackage.zip "UnityPackage.zip") |     |     |
| View all files |     |     |

Repository files navigation
---------------------------

ProjectBaseUnity
================

[](https://github.com/Ma-Hup/ProjectBaseUnity#projectbaseunity)

将游戏的功能整理分类，集合了常用的单例，降低样板代码，适合mini、小型游戏开发、GameJam，既可作为扩展，也能作为基础框架。 包含 资源管理、UI 管理、事件、持久化、对象池、音乐、输入、常量管理、Mono等常见功能。

ProjectBase 小框架介绍与使用指南
======================

[](https://github.com/Ma-Hup/ProjectBaseUnity#projectbase-%E5%B0%8F%E6%A1%86%E6%9E%B6%E4%BB%8B%E7%BB%8D%E4%B8%8E%E4%BD%BF%E7%94%A8%E6%8C%87%E5%8D%97)

本指南面向首次接触 `ProjectBase` 的开发者，结合示例与图示，帮助快速理解和上手。

* * *

1\. 目录结构
--------

[](https://github.com/Ma-Hup/ProjectBaseUnity#1-%E7%9B%AE%E5%BD%95%E7%BB%93%E6%9E%84)

    Assets/Scripts/ProjectBase/
    ├─ base/                 # 基础单例
    │  ├─ SingletonBase.cs
    │  └─ SingletonMono.cs
    ├─ UI/                   # UI 管理与面板基类
    │  ├─ UIManager.cs
    │  └─ BasePanel.cs
    ├─ Events/               # 事件总线
    │  └─ EventsManager.cs
    ├─ Input/                # 输入统一入口
    │  └─ InputManager.cs
    ├─ Mono/                 # Mono 中枢，提供 Update/协程入口
    │  └─ MonoController.cs
    ├─ ResManager/           # 资源加载（同步/异步/StreamingAssets）
    │  └─ ResManager.cs
    ├─ ObjectPool/           # 对象池
    │  ├─ ObjectPool.cs
    │  └─ PoolData.cs
    ├─ Music/                # BGM/音效管理
    │  └─ MusicManager.cs
    ├─ DataManager/          # 数据存储（PlayerPrefs/Json）
    │  ├─ PPDataManager.cs
    │  ├─ JsonManager.cs
    │  └─ Serialization.cs
    └─ Constants/            # 常量/事件名集中管理
       └─ Constants.cs
    

示例与入口：

*   `Assets/Scripts/PBFramworkSample/`：`PBFPanel.cs`、`PBWinPanelTest1.cs`、`PBWinPanelTest2.cs`
*   `Assets/Scripts/Game/`：`PBFSampleGameStart.cs`

* * *

2\. 架构总览
--------

[](https://github.com/Ma-Hup/ProjectBaseUnity#2-%E6%9E%B6%E6%9E%84%E6%80%BB%E8%A7%88)

核心思想：通过统一的单例管理与事件总线，实现 UI、输入、资源、动画、音频、对象池、数据存储的模块化、低耦合协作。

ASCII 架构图（简化）：

    +--------------------+        +------------------+
    |  MonoController    | <----> |  Update Loop     |
    | (SingletonMono)    |        +------------------+
    +---------+----------+
              |
              v
    +---------+----------+     +------------------+     +------------------+
    |  UIManager         | <--> |  EventsManager  | <--> | InputManager     |
    | (panel层/动画队列) |     | (事件总线)       |     | (输入事件触发)   |
    +---------+----------+     +------------------+     +------------------+
              |
              v
    +-------------------+     +------------------+     +------------------+
    |  ResManager       |     |  ObjectPool      |     | MusicManager     |
    | (资源加载)        |     | (缓存复用)       |     | (BGM/音效)       |
    +-------------------+     +------------------+     +------------------+
              |
              v
    +-------------------+
    | DataManager       |
    | (PlayerPrefs/Json)|
    +-------------------+
    

UI 层级结构（Canvas 子节点）：

    MainCanvas (tag: MainCanvas)
    ├─ bot    # sort order = 0
    ├─ mid    # sort order = 20
    ├─ top    # sort order = 40
    └─ system # sort order = 60
    

* * *

3\. 核心模块与职责
-----------

[](https://github.com/Ma-Hup/ProjectBaseUnity#3-%E6%A0%B8%E5%BF%83%E6%A8%A1%E5%9D%97%E4%B8%8E%E8%81%8C%E8%B4%A3)

*   单例基类
    *   `SingletonBase<T>`：非 `MonoBehaviour` 单例（`base/SingletonBase.cs:5`）
    *   `SingletonMono<T>`：`MonoBehaviour` 单例（自动创建、`DontDestroyOnLoad`）（`base/SingletonMono.cs:5, 28`）
*   UI 管理
    *   `UIManager`：查找/自动加载 `MainCanvas` 与 `EventSystem`、面板层级管理、动画队列、屏幕适配（`UI/UIManager.cs:80, 83-106, 167-235, 325-350`）
    *   `BasePanel`：面板基类，控件收集与按钮/Toggle回调、动画添加（`UI/BasePanel.cs:10, 54-64, 111-118, 80-105`）
*   事件总线
    *   `EventsManager`：类型安全事件订阅/触发/移除（`Events/EventsManager.cs:45, 70, 96`）
*   输入管理
    *   `InputManager`：统一开关与输入事件触发（鼠标左右键、Escape）（`Input/InputManager.cs:13-40`）
*   Mono 中枢
    *   `MonoController`：统一 `Update` 入口与协程平台（`Mono/MonoController.cs:7-33`）
*   资源加载
    *   `ResManager`：`Resources` 同步/异步加载、`StreamingAssets` 文本读取（`ResManager/ResManager.cs:17, 32, 51`）
*   对象池
    *   `ObjectPool` / `PoolData`：按名称组织的对象复用（`ObjectPool/ObjectPool.cs:6, 20-46, 52-66`）
*   音频管理
    *   `MusicManager`：BGM/音效播放、音量与回收（`Music/MusicManager.cs:36-51, 75-96, 101-118, 121-128`）
*   数据存储（推荐使用 Easy Save 3 插件代替PlaerPrefs重新实现，ES3通常情况下，无需考虑序列化问题）
    *   `PPDataManager`：通过反射将复杂对象持久化到 `PlayerPrefs`（支持 `IList`/`IDictionary`）（`DataManager/PPDataManager.cs:19-33, 35-98, 103-126, 129-214`）
    *   `JsonManager`：序列化到 `persistentDataPath` 的 `.json` 文件（`DataManager/JsonManager.cs:15-29, 33-46, 48-55`）
    *   `Serialization.cs`：扩展 `JsonUtility` 支持 `List<T>`/`Dictionary<TKey, TValue>`（`DataManager/Serialization.cs:11-21, 25-57`）
*   常量集中管理
    *   `Constants.cs`：事件名/枚举等统一定义（`Constants/Constants.cs:17-22`）

* * *

4\. 优点与设计取舍
-----------

[](https://github.com/Ma-Hup/ProjectBaseUnity#4-%E4%BC%98%E7%82%B9%E4%B8%8E%E8%AE%BE%E8%AE%A1%E5%8F%96%E8%88%8D)

*   轻量模块化：各模块职责单一，互相低耦合，易于替换/扩展
*   全局统一入口：事件总线、输入抽象、资源加载统一化，减少散落逻辑
*   UI 层级规范：`bot/mid/top/system` 清晰分层，支持返回键关闭栈顶面板（`UIManager.cs:115-133`）
*   BasePanel：面板基类，控件收集与按钮/Toggle回调并且自动绑定、动画添加（`UI/BasePanel.cs:10, 54-64, 111-118, 80-105`）
*   动画队列：面板动画顺序播放、可跳过（`UIManager.cs:323-350`）
*   对象池与资源复用：减少实例化与 GC 压力（`ObjectPool.cs:20-46, 52-66`）
*   数据持久化：`PlayerPrefs` 与 `Json` 双方案，支持复杂结构
*   示例齐全：`PBFSampleGameStart` 与 `PBFramworkSample` 提供开箱即用演示

* * *

5\. 快速上手（5 步）
-------------

[](https://github.com/Ma-Hup/ProjectBaseUnity#5-%E5%BF%AB%E9%80%9F%E4%B8%8A%E6%89%8B5-%E6%AD%A5)

1.  准备 `MainCanvas` 与 `EventSystem`
    *   场景中放置并设置 Tag：`MainCanvas`、`MainEventSystem`
    *   或提供 `Resources/UI/MainCanvas.prefab` 与 `Resources/UI/EventSystem.prefab`（自动加载）
2.  面板预制体
    *   资源默认放在 `Resources/UI/`（可在 `UIManager.cs:122` 自定义），命名为 `YourPanelName`
    *   绑定脚本继承 `BasePanel`，建议添加 `CanvasGroup` 赋值到 `cG` 字段（用于交互锁）
3.  游戏入口
    *   在任意 `MonoBehaviour` 或 `SingletonMono` 中调用 `UIManager.Instance.ShowPanel<T>("PanelName")`
4.  抽象输入与事件（可选）
    *   `InputManager.Instance.SwitchInputCheck(true)` 开启输入检查
    *   使用 `EventsManager` 实现模块通信
5.  资源与音频
    *   用 `ResManager` 统一加载，`MusicManager` 播放 BGM/音效

* * *

6\. 常用范式与示例
-----------

[](https://github.com/Ma-Hup/ProjectBaseUnity#6-%E5%B8%B8%E7%94%A8%E8%8C%83%E5%BC%8F%E4%B8%8E%E7%A4%BA%E4%BE%8B)

*   显示/隐藏面板
    
    ```cs
    // 显示（默认加载 Resources/UI/YourPanel）
    UIManager.Instance.ShowPanel<YourPanel>("YourPanel"); // UI/UIManager.cs:174
    
    // 指定层级和自定义资源路径前缀
    UIManager.Instance.ShowPanel<YourPanel>("YourPanel", UIManager.UIM_layer.Top, null, "CustomUIPrefix/");
    
    // 隐藏/销毁
    UIManager.Instance.HidePanel("YourPanel"); // UI/UIManager.cs:282-292
    
    // 获取面板（可配合 WaitUntil 写瀑布流）
    var pnl = UIManager.Instance.GetPanel<YourPanel>("YourPanel"); // UI/UIManager.cs:315-321
    ```
    
*   在面板中收集控件与按钮回调
    
    ```cs
    public class YourPanel : BasePanel {
        protected override void OnBtnClick(string btnName) {
            if (btnName == "CloseBtn") UIManager.Instance.HidePanel("YourPanel");
        }
        void ToggleExample() {
            var toggle = GetControler<Toggle>("YourToggle");
        }
    } // UI/BasePanel.cs:111-118, 132-145, 152-179
    ```
    
*   事件总线（类型安全）
    
    ```cs
    // 订阅
    EventsManager.Instance.AddEventsListener<int>("EventTest", OnEvent);
    // 触发
    EventsManager.Instance.EventTrigger<int>("EventTest", 42);
    // 移除
    EventsManager.Instance.RemoveListener<int>("EventTest", OnEvent);
    void OnEvent(int i) { Debug.Log(i); }
    // Events/EventsManager.cs:45, 70, 96
    ```
    
*   输入统一入口
    
    ```cs
    InputManager.Instance.SwitchInputCheck(true); // 开启输入
    // 监听 ESC 在 UIManager 构造函数中已处理（关闭栈顶面板）
    // Input/InputManager.cs:13-40
    ```
    
*   UI 动画队列（需要 DOTween）
    
    ```cs
    protected override void ShowMe() {
        var seq = DOTween.Sequence();
        seq.Append(cG.DOFade(1, 0.2f));
        AddUiAnimation(seq, interactable:false); // BasePanel.cs:80-105
    }
    ```
    
*   资源加载
    
    ```cs
    var go = ResManager.Instance.Load<GameObject>("UI/YourPanel");                // 同步
    ResManager.Instance.LoadAsync<AudioClip>("sounds/click", clip => { /*...*/ }); // 异步
    var text = ResManager.Instance.LoadFromStreamingAssets(path);                  // 文本读取
    // ResManager/ResManager.cs:17, 32, 51
    ```
    
*   对象池
    
    ```cs
    ObjectPool.Instance.GetObj("Prefabs/Bullet", obj => {
        // 使用 obj
    }, parent: someTransform, ifAsync: true);
    ObjectPool.Instance.TStoreObj("Bullet", obj); // 归还
    // ObjectPool/ObjectPool.cs:20-46, 52-66
    ```
    
*   音频
    
    ```cs
    MusicManager.Instance.PlayBGM("music/bgm_main");
    MusicManager.Instance.PlaySound("sounds/click");
    MusicManager.Instance.ChangeSoundVolume(0.5f);
    MusicManager.Instance.StopBGM();
    // Music/MusicManager.cs:36-72, 75-96
    ```
    
*   数据存储（复杂对象 → PlayerPrefs）
    
    ```cs
    PPDataManager.Instance.SaveData(playerData, "Player");           // 写入
    var obj = PPDataManager.Instance.LoadData(typeof(PlayerData), "Player"); // 读取
    // DataManager/PPDataManager.cs:19-33, 103-126
    ```
    
*   JSON 存储
    
    ```cs
    JsonManager.Instance.SaveJson(playerData, "player");
    var loaded = JsonManager.Instance.LoadJson<PlayerData>("player");
    JsonManager.Instance.DeletJson("player");
    // DataManager/JsonManager.cs:15-29, 33-46, 48-55
    ```
    
*   屏幕适配辅助
    
    ```cs
    var ratio = UIManager.AspectRatio;    // UI/UIManager.cs:46-49
    var wScale = UIManager.GetWidthScale; // UI/UIManager.cs:53-60
    var hScale = UIManager.GetHeightScale;// UI/UIManager.cs:62-69
    ```
    
*   示例入口（瀑布流展示）
    
    ```cs
    // Game/PBFSampleGameStart.cs:15, 21-35
    UIManager.Instance.ShowPanel<PBFPanel>("PBFPanel", UIManager.UIM_layer.Bottom, null, "ProjecBaseSampleUI/");
    StartCoroutine(UIFalls()); // 依次展示 PBWinPanelTest1 -> PBWinPanelTest2 -> PBFPanel
    ```
    

* * *

7\. 使用建议与注意事项
-------------

[](https://github.com/Ma-Hup/ProjectBaseUnity#7-%E4%BD%BF%E7%94%A8%E5%BB%BA%E8%AE%AE%E4%B8%8E%E6%B3%A8%E6%84%8F%E4%BA%8B%E9%A1%B9)

*   保证存在 `Camera.main`；`UIManager` 会将 Canvas 设为 `ScreenSpaceCamera` 并绑定主摄像机（`UI/UIManager.cs:103-106`）
*   若不在场景中手动放置 `MainCanvas`/`EventSystem`，需在 `Resources/UI/` 下提供同名预制体（`UI/UIManager.cs:83-91`）
*   面板根节点建议为 `RectTransform`，以便 `anchorMin/Max` 与 `sizeDelta` 正常设置（`UI/UIManager.cs:211-220`）
*   复杂面板动画使用 `AddUiAnimation` 管理交互与队列，避免并发冲突（`BasePanel.cs:80-105`）
*   `withoutEscBtn = true` 可使面板不被返回键逻辑管理（`UI/UIManager.cs:135-139`）
*   使用对象池时，归还前确保对象状态复位（位置/缩放/激活等）；框架已在取出时重置 `localScale` 为 `Vector3.one`（`PoolData.cs:33`）
*   数据持久化命名规则：`keyName_Type_FieldType_FieldName`，更改字段名会导致读取失败（`PPDataManager.cs:29-33, 116-122`）
*   事件总线类型需一致；同名事件若类型不匹配会替换为新类型（`EventsManager.cs:45-64`）

* * *

8\. 示例文件导航（关联代码位置）
------------------

[](https://github.com/Ma-Hup/ProjectBaseUnity#8-%E7%A4%BA%E4%BE%8B%E6%96%87%E4%BB%B6%E5%AF%BC%E8%88%AA%E5%85%B3%E8%81%94%E4%BB%A3%E7%A0%81%E4%BD%8D%E7%BD%AE)

*   `PBFSampleGameStart`（入口展示与协程瀑布流）：`Assets/Scripts/Game/PBFSampleGameStart.cs:15, 21-35`
*   `PBFPanel`（示例面板，演示事件订阅与按钮处理）：`Assets/Scripts/PBFramworkSample/PBFPanel.cs:12-19, 26-42, 47-71`
*   `PBWinPanelTest1/2`（简单关闭按钮示例）：`Assets/Scripts/PBFramworkSample/PBWinPanelTest1.cs:7-16`、`PBWinPanelTest2.cs:7-16`

* * *

9\. 扩展建议
--------

[](https://github.com/Ma-Hup/ProjectBaseUnity#9-%E6%89%A9%E5%B1%95%E5%BB%BA%E8%AE%AE)

*   在 `Constants` 中集中维护事件名与关键常量（`Constants/Constants.cs:17-22`）
*   按模块扩展：例如新增 `SceneManager`/`NetworkManager`，均按 `SingletonBase<T>` 或 `SingletonMono<T>` 组织
*   引入更丰富的 UI 动画库时，保持由 `BasePanel` 统一入队管理，避免并发

* * *

10\. FAQ
--------

[](https://github.com/Ma-Hup/ProjectBaseUnity#10-faq)

*   Q：不放 `MainCanvas`/`EventSystem` 在场景里可以吗？
    *   A：可以，`UIManager` 会自动从 `Resources/UI/` 加载同名预制体（`UI/UIManager.cs:83-91`）
*   Q：如何拦截返回键不关闭当前面板？
    *   A：将 `BasePanel.withoutEscBtn = true`（`UI/UIManager.cs:135-139`）
*   Q：如何一次加载多个面板并顺序播放动画？
    *   A：每个面板使用 `AddUiAnimation` 入队，`UIManager` 通过队列顺序播放（`UI/UIManager.cs:323-350`）

11\. 后续建议
---------

[](https://github.com/Ma-Hup/ProjectBaseUnity#11-%E5%90%8E%E7%BB%AD%E5%BB%BA%E8%AE%AE)

*   考虑引入 Easy Save 3 插件，替代 PlayerPrefs 实现数据存储（`DataManager/PPDataManager.cs:19-33, 103-126`）
*   考虑引入依赖注入

旧版文档连接🔗： [https://lcndm0b2t06l.feishu.cn/wiki/DyR2wSuZoiBDjEkvjrTcOnd9nMg?from=from\_copylink](https://lcndm0b2t06l.feishu.cn/wiki/DyR2wSuZoiBDjEkvjrTcOnd9nMg?from=from_copylink)

About
-----

将游戏的功能整理分类，集合了常用的单例，降低样板代码，适合mini、小型游戏开发、GameJam，既可作为扩展，也能作为基础框架。 包含 资源管理、UI 管理、事件、持久化、对象池、音乐、输入、常量管理、Mono等常见功能。

### Resources

[Readme](https://github.com/Ma-Hup/ProjectBaseUnity#readme-ov-file)

### Uh oh!

There was an error while loading. [Please reload this page](https://github.com/Ma-Hup/ProjectBaseUnity)
.

[Activity](https://github.com/Ma-Hup/ProjectBaseUnity/activity)

### Stars

[**10** stars](https://github.com/Ma-Hup/ProjectBaseUnity/stargazers)

### Watchers

[**1** watching](https://github.com/Ma-Hup/ProjectBaseUnity/watchers)

### Forks

[**0** forks](https://github.com/Ma-Hup/ProjectBaseUnity/forks)

[Report repository](https://github.com/contact/report-content?content_url=https%3A%2F%2Fgithub.com%2FMa-Hup%2FProjectBaseUnity&report=Ma-Hup+%28user%29)

[Releases 1](https://github.com/Ma-Hup/ProjectBaseUnity/releases)

------------------------------------------------------------------

[v1.2 Latest\
\
Nov 20, 2025](https://github.com/Ma-Hup/ProjectBaseUnity/releases/tag/v1.2)

[Packages 0](https://github.com/users/Ma-Hup/packages?repo_name=ProjectBaseUnity)

----------------------------------------------------------------------------------

No packages published  

Languages
---------

*   [C# 100.0%](https://github.com/Ma-Hup/ProjectBaseUnity/search?l=c%23)
    

You can’t perform that action at this time.

---

# 📄 Activity · Ma-Hup/ProjectBaseUnity · GitHub
**Source:** https://github.com/Ma-Hup/ProjectBaseUnity/activity

[Skip to content](https://github.com/Ma-Hup/ProjectBaseUnity/activity#start-of-content)
  

You signed in with another tab or window. [Reload](https://github.com/Ma-Hup/ProjectBaseUnity/activity)
 to refresh your session. You signed out in another tab or window. [Reload](https://github.com/Ma-Hup/ProjectBaseUnity/activity)
 to refresh your session. You switched accounts on another tab or window. [Reload](https://github.com/Ma-Hup/ProjectBaseUnity/activity)
 to refresh your session. Dismiss alert

[Ma-Hup](https://github.com/Ma-Hup) / **[ProjectBaseUnity](https://github.com/Ma-Hup/ProjectBaseUnity)** Public

*   [Notifications](https://github.com/login?return_to=%2FMa-Hup%2FProjectBaseUnity)
     You must be signed in to change notification settings
*   [Fork 0](https://github.com/login?return_to=%2FMa-Hup%2FProjectBaseUnity)
    
*   [Star 10](https://github.com/login?return_to=%2FMa-Hup%2FProjectBaseUnity)
    

 All branchesAll activity

All users

All time

[Showing most recent first](https://github.com/Ma-Hup/ProjectBaseUnity/activity?sort=ASC)

Add future improvement suggestions to README




--------------------------------------------------

[![](https://avatars.githubusercontent.com/u/78647352?s=80&v=4)Ma-Hup](https://github.com/Ma-Hup)
pushed 1 commit to [main](https://github.com/Ma-Hup/ProjectBaseUnity/activity?ref=main)
 • 82a2796…f5e712f • 

9 days ago

More activity actions

More activity actions

Revise README for detailed project overview and usage




-----------------------------------------------------------

[![](https://avatars.githubusercontent.com/u/78647352?s=80&v=4)Ma-Hup](https://github.com/Ma-Hup)
pushed 1 commit to [main](https://github.com/Ma-Hup/ProjectBaseUnity/activity?ref=main)
 • 0e42e0a…82a2796 • 

9 days ago

More activity actions

More activity actions

Update README with documentation link




-------------------------------------------

[![](https://avatars.githubusercontent.com/u/78647352?s=80&v=4)Ma-Hup](https://github.com/Ma-Hup)
pushed 1 commit to [main](https://github.com/Ma-Hup/ProjectBaseUnity/activity?ref=main)
 • 55ab851…0e42e0a • 

13 days ago

More activity actions

More activity actions

Init v1.2




---------------

[![](https://avatars.githubusercontent.com/u/78647352?s=80&v=4)Ma-Hup](https://github.com/Ma-Hup)
pushed 1 commit to [main](https://github.com/Ma-Hup/ProjectBaseUnity/activity?ref=main)
 • 8ad0261…55ab851 • 

13 days ago

More activity actions

More activity actions

Initial commit




--------------------

[![](https://avatars.githubusercontent.com/u/78647352?s=80&v=4)Ma-Hup](https://github.com/Ma-Hup)
created [main](https://github.com/Ma-Hup/ProjectBaseUnity/activity?ref=main)
 • 8ad0261 • 

13 days ago

More activity actions

More activity actions

You can’t perform that action at this time.

---

# 📄 Issues · Ma-Hup/ProjectBaseUnity · GitHub
**Source:** https://github.com/Ma-Hup/ProjectBaseUnity/issues

[Skip to content](https://github.com/Ma-Hup/ProjectBaseUnity/issues#start-of-content)
 

You signed in with another tab or window. [Reload](https://github.com/Ma-Hup/ProjectBaseUnity/issues)
 to refresh your session. You signed out in another tab or window. [Reload](https://github.com/Ma-Hup/ProjectBaseUnity/issues)
 to refresh your session. You switched accounts on another tab or window. [Reload](https://github.com/Ma-Hup/ProjectBaseUnity/issues)
 to refresh your session. Dismiss alert

[Ma-Hup](https://github.com/Ma-Hup) / **[ProjectBaseUnity](https://github.com/Ma-Hup/ProjectBaseUnity)** Public

*   [Notifications](https://github.com/login?return_to=%2FMa-Hup%2FProjectBaseUnity)
     You must be signed in to change notification settings
*   [Fork 0](https://github.com/login?return_to=%2FMa-Hup%2FProjectBaseUnity)
    
*   [Star 10](https://github.com/login?return_to=%2FMa-Hup%2FProjectBaseUnity)
    

Issues
======

Search Issues

is:issue state:open

is:issue state:open

Search

[Labels](https://github.com/Ma-Hup/ProjectBaseUnity/labels)
[Milestones](https://github.com/Ma-Hup/ProjectBaseUnity/milestones)
[New issue](https://github.com/login?return_to=https://github.com/Ma-Hup/ProjectBaseUnity/issues)

Search results
--------------

[Open](https://github.com/Ma-Hup/ProjectBaseUnity/issues)
[Closed](https://github.com/Ma-Hup/ProjectBaseUnity/issues)

Author

Labels

Projects

Milestones

Assignees

Sort by Newest, descending

### No results

Try adjusting your search filters.

You can’t perform that action at this time.

---

# 📄 Pull requests · Ma-Hup/ProjectBaseUnity · GitHub
**Source:** https://github.com/Ma-Hup/ProjectBaseUnity/pulls

[Skip to content](https://github.com/Ma-Hup/ProjectBaseUnity/pulls#start-of-content)
  

You signed in with another tab or window. [Reload](https://github.com/Ma-Hup/ProjectBaseUnity/pulls)
 to refresh your session. You signed out in another tab or window. [Reload](https://github.com/Ma-Hup/ProjectBaseUnity/pulls)
 to refresh your session. You switched accounts on another tab or window. [Reload](https://github.com/Ma-Hup/ProjectBaseUnity/pulls)
 to refresh your session. Dismiss alert

[Ma-Hup](https://github.com/Ma-Hup) / **[ProjectBaseUnity](https://github.com/Ma-Hup/ProjectBaseUnity)** Public

*   [Notifications](https://github.com/login?return_to=%2FMa-Hup%2FProjectBaseUnity)
     You must be signed in to change notification settings
*   [Fork 0](https://github.com/login?return_to=%2FMa-Hup%2FProjectBaseUnity)
    
*   [Star 10](https://github.com/login?return_to=%2FMa-Hup%2FProjectBaseUnity)
    

Pull requests: Ma-Hup/ProjectBaseUnity
======================================

[New pull request New](https://github.com/Ma-Hup/ProjectBaseUnity/compare)

[0 Open](https://github.com/Ma-Hup/ProjectBaseUnity/pulls?q=is%3Aopen+is%3Apr)
 [0 Closed](https://github.com/Ma-Hup/ProjectBaseUnity/pulls?q=is%3Apr+is%3Aclosed)

Welcome to pull requests!
-------------------------

Pull requests help you collaborate on code with other people. As pull requests are created, they’ll appear here in a searchable and filterable list. To get started, you should [create a pull request](https://github.com/Ma-Hup/ProjectBaseUnity/compare)
.

**ProTip!** Adding [no:label](https://github.com/Ma-Hup/ProjectBaseUnity/issues?q=is%3Apr+is%3Aopen+no%3Alabel)
 will show everything without a label.

You can’t perform that action at this time.

---

# 📄 Actions · Ma-Hup/ProjectBaseUnity · GitHub
**Source:** https://github.com/Ma-Hup/ProjectBaseUnity/actions

[Skip to content](https://github.com/Ma-Hup/ProjectBaseUnity/actions#start-of-content)
  

You signed in with another tab or window. [Reload](https://github.com/Ma-Hup/ProjectBaseUnity/actions)
 to refresh your session. You signed out in another tab or window. [Reload](https://github.com/Ma-Hup/ProjectBaseUnity/actions)
 to refresh your session. You switched accounts on another tab or window. [Reload](https://github.com/Ma-Hup/ProjectBaseUnity/actions)
 to refresh your session. Dismiss alert

[Ma-Hup](https://github.com/Ma-Hup) / **[ProjectBaseUnity](https://github.com/Ma-Hup/ProjectBaseUnity)** Public

*   [Notifications](https://github.com/login?return_to=%2FMa-Hup%2FProjectBaseUnity)
     You must be signed in to change notification settings
*   [Fork 0](https://github.com/login?return_to=%2FMa-Hup%2FProjectBaseUnity)
    
*   [Star 10](https://github.com/login?return_to=%2FMa-Hup%2FProjectBaseUnity)
    

Automate your workflow from idea to production

GitHub Actions makes it easy to automate all your software workflows, now with world-class CI/CD. Build, test, and deploy your code right from GitHub.

[Learn more about getting started with Actions.](https://docs.github.com/articles/getting-started-with-github-actions)

* * *

![Operating systems and containers](https://github.githubassets.com/assets/actions-linux-and-containers-d898cc488cb3.svg)

#### Linux, macOS, Windows, ARM, and containers

Hosted runners for every major OS make it easy to build and test all your projects. Run directly on a VM or inside a container. Use your own VMs, in the cloud or on-prem, with self-hosted runners.

![Matrix builds](https://github.githubassets.com/assets/actions-matrix-aac8c29bd225.svg)

#### Matrix builds

Save time with matrix workflows that simultaneously test across multiple operating systems and versions of your runtime.

![Any language](https://github.githubassets.com/assets/actions-any-lang-f603eeb8cd45.svg)

#### Any language

GitHub Actions supports Node.js, Python, Java, Ruby, PHP, Go, Rust, .NET, and more. Build, test, and deploy applications in your language of choice.

![Live logs](https://github.githubassets.com/assets/actions-live-logs-532f1c0e442e.svg)

#### Live logs

See your workflow run in realtime with color and emoji. It’s one click to copy a link that highlights a specific line number to share a CI/CD failure.

![Secret store](https://github.githubassets.com/assets/actions-secret-store-4121c7f05a49.svg)

#### Built-in secret store

Automate your software development practices with workflow files embracing the Git flow by codifying it in your repository.

![Multi-container testing](https://github.githubassets.com/assets/actions-multi-container-testing-0951351a6bee.svg)

#### Multi-container testing

Test your web service and its DB in your workflow by simply adding some `docker-compose` to your workflow file.

You can’t perform that action at this time.

---

# 📄 Security Overview · Ma-Hup/ProjectBaseUnity · GitHub
**Source:** https://github.com/Ma-Hup/ProjectBaseUnity/security

[Skip to content](https://github.com/Ma-Hup/ProjectBaseUnity/security#start-of-content)
  

You signed in with another tab or window. [Reload](https://github.com/Ma-Hup/ProjectBaseUnity/security)
 to refresh your session. You signed out in another tab or window. [Reload](https://github.com/Ma-Hup/ProjectBaseUnity/security)
 to refresh your session. You switched accounts on another tab or window. [Reload](https://github.com/Ma-Hup/ProjectBaseUnity/security)
 to refresh your session. Dismiss alert

[Ma-Hup](https://github.com/Ma-Hup) / **[ProjectBaseUnity](https://github.com/Ma-Hup/ProjectBaseUnity)** Public

*   [Notifications](https://github.com/login?return_to=%2FMa-Hup%2FProjectBaseUnity)
     You must be signed in to change notification settings
*   [Fork 0](https://github.com/login?return_to=%2FMa-Hup%2FProjectBaseUnity)
    
*   [Star 10](https://github.com/login?return_to=%2FMa-Hup%2FProjectBaseUnity)
    

Security: Ma-Hup/ProjectBaseUnity
=================================

Security
--------

### No security policy detected

This project has not set up a SECURITY.md file yet.

There aren’t any published security advisories
----------------------------------------------

You can’t perform that action at this time.

---

# 📄 Releases · Ma-Hup/ProjectBaseUnity
**Source:** https://github.com/Ma-Hup/ProjectBaseUnity/releases

[Skip to content](https://github.com/Ma-Hup/ProjectBaseUnity/releases#start-of-content)
  

You signed in with another tab or window. [Reload](https://github.com/Ma-Hup/ProjectBaseUnity/releases)
 to refresh your session. You signed out in another tab or window. [Reload](https://github.com/Ma-Hup/ProjectBaseUnity/releases)
 to refresh your session. You switched accounts on another tab or window. [Reload](https://github.com/Ma-Hup/ProjectBaseUnity/releases)
 to refresh your session. Dismiss alert

[Ma-Hup](https://github.com/Ma-Hup) / **[ProjectBaseUnity](https://github.com/Ma-Hup/ProjectBaseUnity)** Public

*   [Notifications](https://github.com/login?return_to=%2FMa-Hup%2FProjectBaseUnity)
     You must be signed in to change notification settings
*   [Fork 0](https://github.com/login?return_to=%2FMa-Hup%2FProjectBaseUnity)
    
*   [Star 10](https://github.com/login?return_to=%2FMa-Hup%2FProjectBaseUnity)
    

Releases: Ma-Hup/ProjectBaseUnity
=================================

 

Releases · Ma-Hup/ProjectBaseUnity

v1.2
----

20 Nov 10:06

![@Ma-Hup](https://avatars.githubusercontent.com/u/78647352?s=40&v=4) [Ma-Hup](https://github.com/Ma-Hup)

[v1.2](https://github.com/Ma-Hup/ProjectBaseUnity/tree/v1.2)

[`55ab851`](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765)

Compare

Choose a tag to compare
=======================

Sorry, something went wrong.
----------------------------

Filter

Loading 

Sorry, something went wrong.
----------------------------

### Uh oh!

There was an error while loading. [Please reload this page](https://github.com/Ma-Hup/ProjectBaseUnity/releases)
.

No results found
----------------

[View all tags](https://github.com/Ma-Hup/ProjectBaseUnity/tags)

[v1.2](https://github.com/Ma-Hup/ProjectBaseUnity/releases/tag/v1.2)
 [Latest](https://github.com/Ma-Hup/ProjectBaseUnity/releases/latest)

[Latest](https://github.com/Ma-Hup/ProjectBaseUnity/releases/latest)

[UnityPackage.zip](https://github.com/user-attachments/files/23651114/UnityPackage.zip)

Assets 2

*   [Source code (zip)](https://github.com/Ma-Hup/ProjectBaseUnity/archive/refs/tags/v1.2.zip)
    
    2025-11-20T09:56:41Z
    
*   [Source code (tar.gz)](https://github.com/Ma-Hup/ProjectBaseUnity/archive/refs/tags/v1.2.tar.gz)
    
    2025-11-20T09:56:41Z
    

 

All reactions

You can’t perform that action at this time.

---

# 📄 ProjectBaseUnity/.gitignore at main · Ma-Hup/ProjectBaseUnity · GitHub
**Source:** https://github.com/Ma-Hup/ProjectBaseUnity/blob/main/.gitignore

[Skip to content](https://github.com/Ma-Hup/ProjectBaseUnity/blob/main/.gitignore#start-of-content)
  

You signed in with another tab or window. [Reload](https://github.com/Ma-Hup/ProjectBaseUnity/blob/main/.gitignore)
 to refresh your session. You signed out in another tab or window. [Reload](https://github.com/Ma-Hup/ProjectBaseUnity/blob/main/.gitignore)
 to refresh your session. You switched accounts on another tab or window. [Reload](https://github.com/Ma-Hup/ProjectBaseUnity/blob/main/.gitignore)
 to refresh your session. Dismiss alert

[Ma-Hup](https://github.com/Ma-Hup) / **[ProjectBaseUnity](https://github.com/Ma-Hup/ProjectBaseUnity)** Public

*   [Notifications](https://github.com/login?return_to=%2FMa-Hup%2FProjectBaseUnity)
     You must be signed in to change notification settings
*   [Fork 0](https://github.com/login?return_to=%2FMa-Hup%2FProjectBaseUnity)
    
*   [Star 10](https://github.com/login?return_to=%2FMa-Hup%2FProjectBaseUnity)
    

Collapse file tree
------------------

Files
-----

 main

Search this repository

/

.gitignore
==========

Copy path

BlameMore file actions

BlameMore file actions

Latest commit
-------------

History
-------

[History](https://github.com/Ma-Hup/ProjectBaseUnity/commits/main/.gitignore)

[](https://github.com/Ma-Hup/ProjectBaseUnity/commits/main/.gitignore)

99 lines (82 loc) · 2.26 KB

/

.gitignore
==========

Top

File metadata and controls
--------------------------

*   Code
    
*   Blame
    

99 lines (82 loc) · 2.26 KB

[Raw](https://github.com/Ma-Hup/ProjectBaseUnity/raw/refs/heads/main/.gitignore)

Copy raw file

Download raw file

Open symbols panel

Edit and raw actions

\# This .gitignore file should be placed at the root of your Unity project directory # # Get latest from https://github.com/github/gitignore/blob/main/Unity.gitignore # .utmp/ /\[Ll\]ibrary/ /\[Tt\]emp/ /\[Oo\]bj/ /\[Bb\]uild/ /\[Bb\]uilds/ /\[Ll\]ogs/ /\[Uu\]ser\[Ss\]ettings/ \*.log # By default unity supports Blender asset imports, \*.blend1 blender files do not need to be commited to version control. \*.blend1 \*.blend1.meta # MemoryCaptures can get excessive in size. # They also could contain extremely sensitive data /\[Mm\]emoryCaptures/ # Recordings can get excessive in size /\[Rr\]ecordings/ # Uncomment this line if you wish to ignore the asset store tools plugin # /\[Aa\]ssets/AssetStoreTools\* # Autogenerated Jetbrains Rider plugin /\[Aa\]ssets/Plugins/Editor/JetBrains\* # Jetbrains Rider personal-layer settings \*.DotSettings.user # Visual Studio cache directory .vs/ # Gradle cache directory .gradle/ # Autogenerated VS/MD/Consulo solution and project files ExportedObj/ .consulo/ \*.csproj \*.unityproj \*.sln \*.suo \*.tmp \*.user \*.userprefs \*.pidb \*.booproj \*.svd \*.pdb \*.mdb \*.opendb \*.VC.db # Unity3D generated meta files \*.pidb.meta \*.pdb.meta \*.mdb.meta # Unity3D generated file on crash reports sysinfo.txt # Mono auto generated files mono\_crash.\* # Builds \*.apk \*.aab \*.unitypackage \*.unitypackage.meta \*.app # Crashlytics generated file crashlytics-build.properties # TestRunner generated files InitTestScene\*.unity\* # Addressables default ignores, before user customizations /ServerData /\[Aa\]ssets/StreamingAssets/aa\* /\[Aa\]ssets/AddressableAssetsData/link.xml\* /\[Aa\]ssets/Addressables\_Temp\* # By default, Addressables content builds will generate addressables\_content\_state.bin # files in platform-specific subfolders, for example: # /Assets/AddressableAssetsData/OSX/addressables\_content\_state.bin /\[Aa\]ssets/AddressableAssetsData/\*/\*.bin\* # Visual Scripting auto-generated files /\[Aa\]ssets/Unity.VisualScripting.Generated/VisualScripting.Flow/UnitOptions.db /\[Aa\]ssets/Unity.VisualScripting.Generated/VisualScripting.Flow/UnitOptions.db.meta /\[Aa\]ssets/Unity.VisualScripting.Generated/VisualScripting.Core/Property Providers /\[Aa\]ssets/Unity.VisualScripting.Generated/VisualScripting.Core/Property Providers.meta # Auto-generated scenes by play mode tests /\[Aa\]ssets/\[Ii\]nit\[Tt\]est\[Ss\]cene\*.unity\*

1

2

3

4

5

6

7

8

9

10

11

12

13

14

15

16

17

18

19

20

21

22

23

24

25

26

27

28

29

30

31

32

33

34

35

36

37

38

39

40

41

42

43

44

45

46

47

48

49

50

51

52

53

54

55

56

57

58

59

60

61

62

63

64

65

66

67

68

69

70

71

72

73

74

75

76

77

78

79

80

81

82

83

84

85

86

87

88

89

90

91

92

93

94

95

96

97

98

99

You can’t perform that action at this time.

---

# 📄 ProjectBaseUnity/README.md at main · Ma-Hup/ProjectBaseUnity · GitHub
**Source:** https://github.com/Ma-Hup/ProjectBaseUnity/blob/main/README.md

[Skip to content](https://github.com/Ma-Hup/ProjectBaseUnity/blob/main/README.md#start-of-content)
  

You signed in with another tab or window. [Reload](https://github.com/Ma-Hup/ProjectBaseUnity/blob/main/README.md)
 to refresh your session. You signed out in another tab or window. [Reload](https://github.com/Ma-Hup/ProjectBaseUnity/blob/main/README.md)
 to refresh your session. You switched accounts on another tab or window. [Reload](https://github.com/Ma-Hup/ProjectBaseUnity/blob/main/README.md)
 to refresh your session. Dismiss alert

[Ma-Hup](https://github.com/Ma-Hup) / **[ProjectBaseUnity](https://github.com/Ma-Hup/ProjectBaseUnity)** Public

*   [Notifications](https://github.com/login?return_to=%2FMa-Hup%2FProjectBaseUnity)
     You must be signed in to change notification settings
*   [Fork 0](https://github.com/login?return_to=%2FMa-Hup%2FProjectBaseUnity)
    
*   [Star 10](https://github.com/login?return_to=%2FMa-Hup%2FProjectBaseUnity)
    

Collapse file tree
------------------

Files
-----

 main

Search this repository

/

README.md
=========

Copy path

BlameMore file actions

BlameMore file actions

Latest commit
-------------

History
-------

[History](https://github.com/Ma-Hup/ProjectBaseUnity/commits/main/README.md)

[](https://github.com/Ma-Hup/ProjectBaseUnity/commits/main/README.md)

305 lines (254 loc) · 12.8 KB

/

README.md
=========

Top

File metadata and controls
--------------------------

*   Preview
    
*   Code
    
*   Blame
    

305 lines (254 loc) · 12.8 KB

[Raw](https://github.com/Ma-Hup/ProjectBaseUnity/raw/refs/heads/main/README.md)

Copy raw file

Download raw file

Outline

Edit and raw actions

ProjectBaseUnity
================

[](https://github.com/Ma-Hup/ProjectBaseUnity/blob/main/README.md#projectbaseunity)

将游戏的功能整理分类，集合了常用的单例，降低样板代码，适合mini、小型游戏开发、GameJam，既可作为扩展，也能作为基础框架。 包含 资源管理、UI 管理、事件、持久化、对象池、音乐、输入、常量管理、Mono等常见功能。

ProjectBase 小框架介绍与使用指南
======================

[](https://github.com/Ma-Hup/ProjectBaseUnity/blob/main/README.md#projectbase-%E5%B0%8F%E6%A1%86%E6%9E%B6%E4%BB%8B%E7%BB%8D%E4%B8%8E%E4%BD%BF%E7%94%A8%E6%8C%87%E5%8D%97)

本指南面向首次接触 `ProjectBase` 的开发者，结合示例与图示，帮助快速理解和上手。

* * *

1\. 目录结构
--------

[](https://github.com/Ma-Hup/ProjectBaseUnity/blob/main/README.md#1-%E7%9B%AE%E5%BD%95%E7%BB%93%E6%9E%84)

    Assets/Scripts/ProjectBase/
    ├─ base/                 # 基础单例
    │  ├─ SingletonBase.cs
    │  └─ SingletonMono.cs
    ├─ UI/                   # UI 管理与面板基类
    │  ├─ UIManager.cs
    │  └─ BasePanel.cs
    ├─ Events/               # 事件总线
    │  └─ EventsManager.cs
    ├─ Input/                # 输入统一入口
    │  └─ InputManager.cs
    ├─ Mono/                 # Mono 中枢，提供 Update/协程入口
    │  └─ MonoController.cs
    ├─ ResManager/           # 资源加载（同步/异步/StreamingAssets）
    │  └─ ResManager.cs
    ├─ ObjectPool/           # 对象池
    │  ├─ ObjectPool.cs
    │  └─ PoolData.cs
    ├─ Music/                # BGM/音效管理
    │  └─ MusicManager.cs
    ├─ DataManager/          # 数据存储（PlayerPrefs/Json）
    │  ├─ PPDataManager.cs
    │  ├─ JsonManager.cs
    │  └─ Serialization.cs
    └─ Constants/            # 常量/事件名集中管理
       └─ Constants.cs
    

示例与入口：

*   `Assets/Scripts/PBFramworkSample/`：`PBFPanel.cs`、`PBWinPanelTest1.cs`、`PBWinPanelTest2.cs`
*   `Assets/Scripts/Game/`：`PBFSampleGameStart.cs`

* * *

2\. 架构总览
--------

[](https://github.com/Ma-Hup/ProjectBaseUnity/blob/main/README.md#2-%E6%9E%B6%E6%9E%84%E6%80%BB%E8%A7%88)

核心思想：通过统一的单例管理与事件总线，实现 UI、输入、资源、动画、音频、对象池、数据存储的模块化、低耦合协作。

ASCII 架构图（简化）：

    +--------------------+        +------------------+
    |  MonoController    | <----> |  Update Loop     |
    | (SingletonMono)    |        +------------------+
    +---------+----------+
              |
              v
    +---------+----------+     +------------------+     +------------------+
    |  UIManager         | <--> |  EventsManager  | <--> | InputManager     |
    | (panel层/动画队列) |     | (事件总线)       |     | (输入事件触发)   |
    +---------+----------+     +------------------+     +------------------+
              |
              v
    +-------------------+     +------------------+     +------------------+
    |  ResManager       |     |  ObjectPool      |     | MusicManager     |
    | (资源加载)        |     | (缓存复用)       |     | (BGM/音效)       |
    +-------------------+     +------------------+     +------------------+
              |
              v
    +-------------------+
    | DataManager       |
    | (PlayerPrefs/Json)|
    +-------------------+
    

UI 层级结构（Canvas 子节点）：

    MainCanvas (tag: MainCanvas)
    ├─ bot    # sort order = 0
    ├─ mid    # sort order = 20
    ├─ top    # sort order = 40
    └─ system # sort order = 60
    

* * *

3\. 核心模块与职责
-----------

[](https://github.com/Ma-Hup/ProjectBaseUnity/blob/main/README.md#3-%E6%A0%B8%E5%BF%83%E6%A8%A1%E5%9D%97%E4%B8%8E%E8%81%8C%E8%B4%A3)

*   单例基类
    *   `SingletonBase<T>`：非 `MonoBehaviour` 单例（`base/SingletonBase.cs:5`）
    *   `SingletonMono<T>`：`MonoBehaviour` 单例（自动创建、`DontDestroyOnLoad`）（`base/SingletonMono.cs:5, 28`）
*   UI 管理
    *   `UIManager`：查找/自动加载 `MainCanvas` 与 `EventSystem`、面板层级管理、动画队列、屏幕适配（`UI/UIManager.cs:80, 83-106, 167-235, 325-350`）
    *   `BasePanel`：面板基类，控件收集与按钮/Toggle回调、动画添加（`UI/BasePanel.cs:10, 54-64, 111-118, 80-105`）
*   事件总线
    *   `EventsManager`：类型安全事件订阅/触发/移除（`Events/EventsManager.cs:45, 70, 96`）
*   输入管理
    *   `InputManager`：统一开关与输入事件触发（鼠标左右键、Escape）（`Input/InputManager.cs:13-40`）
*   Mono 中枢
    *   `MonoController`：统一 `Update` 入口与协程平台（`Mono/MonoController.cs:7-33`）
*   资源加载
    *   `ResManager`：`Resources` 同步/异步加载、`StreamingAssets` 文本读取（`ResManager/ResManager.cs:17, 32, 51`）
*   对象池
    *   `ObjectPool` / `PoolData`：按名称组织的对象复用（`ObjectPool/ObjectPool.cs:6, 20-46, 52-66`）
*   音频管理
    *   `MusicManager`：BGM/音效播放、音量与回收（`Music/MusicManager.cs:36-51, 75-96, 101-118, 121-128`）
*   数据存储（推荐使用 Easy Save 3 插件代替PlaerPrefs重新实现，ES3通常情况下，无需考虑序列化问题）
    *   `PPDataManager`：通过反射将复杂对象持久化到 `PlayerPrefs`（支持 `IList`/`IDictionary`）（`DataManager/PPDataManager.cs:19-33, 35-98, 103-126, 129-214`）
    *   `JsonManager`：序列化到 `persistentDataPath` 的 `.json` 文件（`DataManager/JsonManager.cs:15-29, 33-46, 48-55`）
    *   `Serialization.cs`：扩展 `JsonUtility` 支持 `List<T>`/`Dictionary<TKey, TValue>`（`DataManager/Serialization.cs:11-21, 25-57`）
*   常量集中管理
    *   `Constants.cs`：事件名/枚举等统一定义（`Constants/Constants.cs:17-22`）

* * *

4\. 优点与设计取舍
-----------

[](https://github.com/Ma-Hup/ProjectBaseUnity/blob/main/README.md#4-%E4%BC%98%E7%82%B9%E4%B8%8E%E8%AE%BE%E8%AE%A1%E5%8F%96%E8%88%8D)

*   轻量模块化：各模块职责单一，互相低耦合，易于替换/扩展
*   全局统一入口：事件总线、输入抽象、资源加载统一化，减少散落逻辑
*   UI 层级规范：`bot/mid/top/system` 清晰分层，支持返回键关闭栈顶面板（`UIManager.cs:115-133`）
*   BasePanel：面板基类，控件收集与按钮/Toggle回调并且自动绑定、动画添加（`UI/BasePanel.cs:10, 54-64, 111-118, 80-105`）
*   动画队列：面板动画顺序播放、可跳过（`UIManager.cs:323-350`）
*   对象池与资源复用：减少实例化与 GC 压力（`ObjectPool.cs:20-46, 52-66`）
*   数据持久化：`PlayerPrefs` 与 `Json` 双方案，支持复杂结构
*   示例齐全：`PBFSampleGameStart` 与 `PBFramworkSample` 提供开箱即用演示

* * *

5\. 快速上手（5 步）
-------------

[](https://github.com/Ma-Hup/ProjectBaseUnity/blob/main/README.md#5-%E5%BF%AB%E9%80%9F%E4%B8%8A%E6%89%8B5-%E6%AD%A5)

1.  准备 `MainCanvas` 与 `EventSystem`
    *   场景中放置并设置 Tag：`MainCanvas`、`MainEventSystem`
    *   或提供 `Resources/UI/MainCanvas.prefab` 与 `Resources/UI/EventSystem.prefab`（自动加载）
2.  面板预制体
    *   资源默认放在 `Resources/UI/`（可在 `UIManager.cs:122` 自定义），命名为 `YourPanelName`
    *   绑定脚本继承 `BasePanel`，建议添加 `CanvasGroup` 赋值到 `cG` 字段（用于交互锁）
3.  游戏入口
    *   在任意 `MonoBehaviour` 或 `SingletonMono` 中调用 `UIManager.Instance.ShowPanel<T>("PanelName")`
4.  抽象输入与事件（可选）
    *   `InputManager.Instance.SwitchInputCheck(true)` 开启输入检查
    *   使用 `EventsManager` 实现模块通信
5.  资源与音频
    *   用 `ResManager` 统一加载，`MusicManager` 播放 BGM/音效

* * *

6\. 常用范式与示例
-----------

[](https://github.com/Ma-Hup/ProjectBaseUnity/blob/main/README.md#6-%E5%B8%B8%E7%94%A8%E8%8C%83%E5%BC%8F%E4%B8%8E%E7%A4%BA%E4%BE%8B)

*   显示/隐藏面板
    
    ```cs
    // 显示（默认加载 Resources/UI/YourPanel）
    UIManager.Instance.ShowPanel<YourPanel>("YourPanel"); // UI/UIManager.cs:174
    
    // 指定层级和自定义资源路径前缀
    UIManager.Instance.ShowPanel<YourPanel>("YourPanel", UIManager.UIM_layer.Top, null, "CustomUIPrefix/");
    
    // 隐藏/销毁
    UIManager.Instance.HidePanel("YourPanel"); // UI/UIManager.cs:282-292
    
    // 获取面板（可配合 WaitUntil 写瀑布流）
    var pnl = UIManager.Instance.GetPanel<YourPanel>("YourPanel"); // UI/UIManager.cs:315-321
    ```
    
*   在面板中收集控件与按钮回调
    
    ```cs
    public class YourPanel : BasePanel {
        protected override void OnBtnClick(string btnName) {
            if (btnName == "CloseBtn") UIManager.Instance.HidePanel("YourPanel");
        }
        void ToggleExample() {
            var toggle = GetControler<Toggle>("YourToggle");
        }
    } // UI/BasePanel.cs:111-118, 132-145, 152-179
    ```
    
*   事件总线（类型安全）
    
    ```cs
    // 订阅
    EventsManager.Instance.AddEventsListener<int>("EventTest", OnEvent);
    // 触发
    EventsManager.Instance.EventTrigger<int>("EventTest", 42);
    // 移除
    EventsManager.Instance.RemoveListener<int>("EventTest", OnEvent);
    void OnEvent(int i) { Debug.Log(i); }
    // Events/EventsManager.cs:45, 70, 96
    ```
    
*   输入统一入口
    
    ```cs
    InputManager.Instance.SwitchInputCheck(true); // 开启输入
    // 监听 ESC 在 UIManager 构造函数中已处理（关闭栈顶面板）
    // Input/InputManager.cs:13-40
    ```
    
*   UI 动画队列（需要 DOTween）
    
    ```cs
    protected override void ShowMe() {
        var seq = DOTween.Sequence();
        seq.Append(cG.DOFade(1, 0.2f));
        AddUiAnimation(seq, interactable:false); // BasePanel.cs:80-105
    }
    ```
    
*   资源加载
    
    ```cs
    var go = ResManager.Instance.Load<GameObject>("UI/YourPanel");                // 同步
    ResManager.Instance.LoadAsync<AudioClip>("sounds/click", clip => { /*...*/ }); // 异步
    var text = ResManager.Instance.LoadFromStreamingAssets(path);                  // 文本读取
    // ResManager/ResManager.cs:17, 32, 51
    ```
    
*   对象池
    
    ```cs
    ObjectPool.Instance.GetObj("Prefabs/Bullet", obj => {
        // 使用 obj
    }, parent: someTransform, ifAsync: true);
    ObjectPool.Instance.TStoreObj("Bullet", obj); // 归还
    // ObjectPool/ObjectPool.cs:20-46, 52-66
    ```
    
*   音频
    
    ```cs
    MusicManager.Instance.PlayBGM("music/bgm_main");
    MusicManager.Instance.PlaySound("sounds/click");
    MusicManager.Instance.ChangeSoundVolume(0.5f);
    MusicManager.Instance.StopBGM();
    // Music/MusicManager.cs:36-72, 75-96
    ```
    
*   数据存储（复杂对象 → PlayerPrefs）
    
    ```cs
    PPDataManager.Instance.SaveData(playerData, "Player");           // 写入
    var obj = PPDataManager.Instance.LoadData(typeof(PlayerData), "Player"); // 读取
    // DataManager/PPDataManager.cs:19-33, 103-126
    ```
    
*   JSON 存储
    
    ```cs
    JsonManager.Instance.SaveJson(playerData, "player");
    var loaded = JsonManager.Instance.LoadJson<PlayerData>("player");
    JsonManager.Instance.DeletJson("player");
    // DataManager/JsonManager.cs:15-29, 33-46, 48-55
    ```
    
*   屏幕适配辅助
    
    ```cs
    var ratio = UIManager.AspectRatio;    // UI/UIManager.cs:46-49
    var wScale = UIManager.GetWidthScale; // UI/UIManager.cs:53-60
    var hScale = UIManager.GetHeightScale;// UI/UIManager.cs:62-69
    ```
    
*   示例入口（瀑布流展示）
    
    ```cs
    // Game/PBFSampleGameStart.cs:15, 21-35
    UIManager.Instance.ShowPanel<PBFPanel>("PBFPanel", UIManager.UIM_layer.Bottom, null, "ProjecBaseSampleUI/");
    StartCoroutine(UIFalls()); // 依次展示 PBWinPanelTest1 -> PBWinPanelTest2 -> PBFPanel
    ```
    

* * *

7\. 使用建议与注意事项
-------------

[](https://github.com/Ma-Hup/ProjectBaseUnity/blob/main/README.md#7-%E4%BD%BF%E7%94%A8%E5%BB%BA%E8%AE%AE%E4%B8%8E%E6%B3%A8%E6%84%8F%E4%BA%8B%E9%A1%B9)

*   保证存在 `Camera.main`；`UIManager` 会将 Canvas 设为 `ScreenSpaceCamera` 并绑定主摄像机（`UI/UIManager.cs:103-106`）
*   若不在场景中手动放置 `MainCanvas`/`EventSystem`，需在 `Resources/UI/` 下提供同名预制体（`UI/UIManager.cs:83-91`）
*   面板根节点建议为 `RectTransform`，以便 `anchorMin/Max` 与 `sizeDelta` 正常设置（`UI/UIManager.cs:211-220`）
*   复杂面板动画使用 `AddUiAnimation` 管理交互与队列，避免并发冲突（`BasePanel.cs:80-105`）
*   `withoutEscBtn = true` 可使面板不被返回键逻辑管理（`UI/UIManager.cs:135-139`）
*   使用对象池时，归还前确保对象状态复位（位置/缩放/激活等）；框架已在取出时重置 `localScale` 为 `Vector3.one`（`PoolData.cs:33`）
*   数据持久化命名规则：`keyName_Type_FieldType_FieldName`，更改字段名会导致读取失败（`PPDataManager.cs:29-33, 116-122`）
*   事件总线类型需一致；同名事件若类型不匹配会替换为新类型（`EventsManager.cs:45-64`）

* * *

8\. 示例文件导航（关联代码位置）
------------------

[](https://github.com/Ma-Hup/ProjectBaseUnity/blob/main/README.md#8-%E7%A4%BA%E4%BE%8B%E6%96%87%E4%BB%B6%E5%AF%BC%E8%88%AA%E5%85%B3%E8%81%94%E4%BB%A3%E7%A0%81%E4%BD%8D%E7%BD%AE)

*   `PBFSampleGameStart`（入口展示与协程瀑布流）：`Assets/Scripts/Game/PBFSampleGameStart.cs:15, 21-35`
*   `PBFPanel`（示例面板，演示事件订阅与按钮处理）：`Assets/Scripts/PBFramworkSample/PBFPanel.cs:12-19, 26-42, 47-71`
*   `PBWinPanelTest1/2`（简单关闭按钮示例）：`Assets/Scripts/PBFramworkSample/PBWinPanelTest1.cs:7-16`、`PBWinPanelTest2.cs:7-16`

* * *

9\. 扩展建议
--------

[](https://github.com/Ma-Hup/ProjectBaseUnity/blob/main/README.md#9-%E6%89%A9%E5%B1%95%E5%BB%BA%E8%AE%AE)

*   在 `Constants` 中集中维护事件名与关键常量（`Constants/Constants.cs:17-22`）
*   按模块扩展：例如新增 `SceneManager`/`NetworkManager`，均按 `SingletonBase<T>` 或 `SingletonMono<T>` 组织
*   引入更丰富的 UI 动画库时，保持由 `BasePanel` 统一入队管理，避免并发

* * *

10\. FAQ
--------

[](https://github.com/Ma-Hup/ProjectBaseUnity/blob/main/README.md#10-faq)

*   Q：不放 `MainCanvas`/`EventSystem` 在场景里可以吗？
    *   A：可以，`UIManager` 会自动从 `Resources/UI/` 加载同名预制体（`UI/UIManager.cs:83-91`）
*   Q：如何拦截返回键不关闭当前面板？
    *   A：将 `BasePanel.withoutEscBtn = true`（`UI/UIManager.cs:135-139`）
*   Q：如何一次加载多个面板并顺序播放动画？
    *   A：每个面板使用 `AddUiAnimation` 入队，`UIManager` 通过队列顺序播放（`UI/UIManager.cs:323-350`）

11\. 后续建议
---------

[](https://github.com/Ma-Hup/ProjectBaseUnity/blob/main/README.md#11-%E5%90%8E%E7%BB%AD%E5%BB%BA%E8%AE%AE)

*   考虑引入 Easy Save 3 插件，替代 PlayerPrefs 实现数据存储（`DataManager/PPDataManager.cs:19-33, 103-126`）
*   考虑引入依赖注入

旧版文档连接🔗： [https://lcndm0b2t06l.feishu.cn/wiki/DyR2wSuZoiBDjEkvjrTcOnd9nMg?from=from\_copylink](https://lcndm0b2t06l.feishu.cn/wiki/DyR2wSuZoiBDjEkvjrTcOnd9nMg?from=from_copylink)

You can’t perform that action at this time.

---

# 📄 Release v1.2 · Ma-Hup/ProjectBaseUnity · GitHub
**Source:** https://github.com/Ma-Hup/ProjectBaseUnity/releases/tag/v1.2

[Skip to content](https://github.com/Ma-Hup/ProjectBaseUnity/releases/tag/v1.2#start-of-content)
  

You signed in with another tab or window. [Reload](https://github.com/Ma-Hup/ProjectBaseUnity/releases/tag/v1.2)
 to refresh your session. You signed out in another tab or window. [Reload](https://github.com/Ma-Hup/ProjectBaseUnity/releases/tag/v1.2)
 to refresh your session. You switched accounts on another tab or window. [Reload](https://github.com/Ma-Hup/ProjectBaseUnity/releases/tag/v1.2)
 to refresh your session. Dismiss alert

[Ma-Hup](https://github.com/Ma-Hup) / **[ProjectBaseUnity](https://github.com/Ma-Hup/ProjectBaseUnity)** Public

*   [Notifications](https://github.com/login?return_to=%2FMa-Hup%2FProjectBaseUnity)
     You must be signed in to change notification settings
*   [Fork 0](https://github.com/login?return_to=%2FMa-Hup%2FProjectBaseUnity)
    
*   [Star 10](https://github.com/login?return_to=%2FMa-Hup%2FProjectBaseUnity)
    

v1.2
====

[Latest](https://github.com/Ma-Hup/ProjectBaseUnity/releases/latest)

[Latest](https://github.com/Ma-Hup/ProjectBaseUnity/releases/latest)

Compare

Choose a tag to compare
=======================

Sorry, something went wrong.
----------------------------

Filter

Loading 

Sorry, something went wrong.
----------------------------

### Uh oh!

There was an error while loading. [Please reload this page](https://github.com/Ma-Hup/ProjectBaseUnity/releases/tag/v1.2)
.

No results found
----------------

[View all tags](https://github.com/Ma-Hup/ProjectBaseUnity/tags)

![@Ma-Hup](https://avatars.githubusercontent.com/u/78647352?s=40&v=4) [Ma-Hup](https://github.com/Ma-Hup)
 released this 20 Nov 10:06

· [3 commits](https://github.com/Ma-Hup/ProjectBaseUnity/compare/v1.2...main)
 to main since this release

[v1.2](https://github.com/Ma-Hup/ProjectBaseUnity/tree/v1.2)

[`55ab851`](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765)

[UnityPackage.zip](https://github.com/user-attachments/files/23651114/UnityPackage.zip)

Assets 2

Loading

### Uh oh!

There was an error while loading. [Please reload this page](https://github.com/Ma-Hup/ProjectBaseUnity/releases/tag/v1.2)
.

 

All reactions

You can’t perform that action at this time.

---

# 📄 Activity · Ma-Hup/ProjectBaseUnity · GitHub
**Source:** https://github.com/Ma-Hup/ProjectBaseUnity/activity?sort=ASC

[Skip to content](https://github.com/Ma-Hup/ProjectBaseUnity/activity?sort=ASC#start-of-content)
  

You signed in with another tab or window. [Reload](https://github.com/Ma-Hup/ProjectBaseUnity/activity?sort=ASC)
 to refresh your session. You signed out in another tab or window. [Reload](https://github.com/Ma-Hup/ProjectBaseUnity/activity?sort=ASC)
 to refresh your session. You switched accounts on another tab or window. [Reload](https://github.com/Ma-Hup/ProjectBaseUnity/activity?sort=ASC)
 to refresh your session. Dismiss alert

[Ma-Hup](https://github.com/Ma-Hup) / **[ProjectBaseUnity](https://github.com/Ma-Hup/ProjectBaseUnity)** Public

*   [Notifications](https://github.com/login?return_to=%2FMa-Hup%2FProjectBaseUnity)
     You must be signed in to change notification settings
*   [Fork 0](https://github.com/login?return_to=%2FMa-Hup%2FProjectBaseUnity)
    
*   [Star 10](https://github.com/login?return_to=%2FMa-Hup%2FProjectBaseUnity)
    

 All branchesAll activity

All users

All time

[Showing oldest first](https://github.com/Ma-Hup/ProjectBaseUnity/activity)

Initial commit




--------------------

[![](https://avatars.githubusercontent.com/u/78647352?s=80&v=4)Ma-Hup](https://github.com/Ma-Hup)
created [main](https://github.com/Ma-Hup/ProjectBaseUnity/activity?ref=main)
 • 8ad0261 • 

13 days ago

More activity actions

More activity actions

Init v1.2




---------------

[![](https://avatars.githubusercontent.com/u/78647352?s=80&v=4)Ma-Hup](https://github.com/Ma-Hup)
pushed 1 commit to [main](https://github.com/Ma-Hup/ProjectBaseUnity/activity?ref=main)
 • 8ad0261…55ab851 • 

13 days ago

More activity actions

More activity actions

Update README with documentation link




-------------------------------------------

[![](https://avatars.githubusercontent.com/u/78647352?s=80&v=4)Ma-Hup](https://github.com/Ma-Hup)
pushed 1 commit to [main](https://github.com/Ma-Hup/ProjectBaseUnity/activity?ref=main)
 • 55ab851…0e42e0a • 

13 days ago

More activity actions

More activity actions

Revise README for detailed project overview and usage




-----------------------------------------------------------

[![](https://avatars.githubusercontent.com/u/78647352?s=80&v=4)Ma-Hup](https://github.com/Ma-Hup)
pushed 1 commit to [main](https://github.com/Ma-Hup/ProjectBaseUnity/activity?ref=main)
 • 0e42e0a…82a2796 • 

9 days ago

More activity actions

More activity actions

Add future improvement suggestions to README




--------------------------------------------------

[![](https://avatars.githubusercontent.com/u/78647352?s=80&v=4)Ma-Hup](https://github.com/Ma-Hup)
pushed 1 commit to [main](https://github.com/Ma-Hup/ProjectBaseUnity/activity?ref=main)
 • 82a2796…f5e712f • 

9 days ago

More activity actions

More activity actions

You can’t perform that action at this time.

---

# 📄 Repository search results · GitHub
**Source:** https://github.com/Ma-Hup/ProjectBaseUnity/search?l=c%23

[Skip to content](https://github.com/Ma-Hup/ProjectBaseUnity/search?l=c%23#start-of-content)
  

You signed in with another tab or window. [Reload](https://github.com/Ma-Hup/ProjectBaseUnity/search?l=c%23)
 to refresh your session. You signed out in another tab or window. [Reload](https://github.com/Ma-Hup/ProjectBaseUnity/search?l=c%23)
 to refresh your session. You switched accounts on another tab or window. [Reload](https://github.com/Ma-Hup/ProjectBaseUnity/search?l=c%23)
 to refresh your session. Dismiss alert

Search Results · repo:Ma-Hup/ProjectBaseUnity language:C#
=========================================================

Filter by
---------

*   *   [Code... (...)results](https://github.com/search?q=repo%3AMa-Hup%2FProjectBaseUnity++language%3AC%23&type=code)
        
    *   [Issues\
        \
        results](https://github.com/search?q=repo%3AMa-Hup%2FProjectBaseUnity++language%3AC%23&type=issues)
        
    *   [Pull requests\
        \
        results](https://github.com/search?q=repo%3AMa-Hup%2FProjectBaseUnity++language%3AC%23&type=pullrequests)
        
    *   [Discussions\
        \
        results](https://github.com/search?q=repo%3AMa-Hup%2FProjectBaseUnity++language%3AC%23&type=discussions)
        
    *   [Commits\
        \
        results](https://github.com/search?q=repo%3AMa-Hup%2FProjectBaseUnity++language%3AC%23&type=commits)
        
    *   [Packages\
        \
        results](https://github.com/search?q=repo%3AMa-Hup%2FProjectBaseUnity++language%3AC%23&type=registrypackages)
        
    *   [Wikis\
        \
        results](https://github.com/search?q=repo%3AMa-Hup%2FProjectBaseUnity++language%3AC%23&type=wikis)
        

*   [Advanced search](https://github.com/search/advanced)
    

You can’t perform that action at this time.

---

# 📄 Activity · Ma-Hup/ProjectBaseUnity · GitHub
**Source:** https://github.com/Ma-Hup/ProjectBaseUnity/activity?ref=main

[Skip to content](https://github.com/Ma-Hup/ProjectBaseUnity/activity?ref=main#start-of-content)
  

You signed in with another tab or window. [Reload](https://github.com/Ma-Hup/ProjectBaseUnity/activity?ref=main)
 to refresh your session. You signed out in another tab or window. [Reload](https://github.com/Ma-Hup/ProjectBaseUnity/activity?ref=main)
 to refresh your session. You switched accounts on another tab or window. [Reload](https://github.com/Ma-Hup/ProjectBaseUnity/activity?ref=main)
 to refresh your session. Dismiss alert

[Ma-Hup](https://github.com/Ma-Hup) / **[ProjectBaseUnity](https://github.com/Ma-Hup/ProjectBaseUnity)** Public

*   [Notifications](https://github.com/login?return_to=%2FMa-Hup%2FProjectBaseUnity)
     You must be signed in to change notification settings
*   [Fork 0](https://github.com/login?return_to=%2FMa-Hup%2FProjectBaseUnity)
    
*   [Star 10](https://github.com/login?return_to=%2FMa-Hup%2FProjectBaseUnity)
    

 mainAll activity

All users

All time

[Showing most recent first](https://github.com/Ma-Hup/ProjectBaseUnity/activity?ref=main&sort=ASC)

Add future improvement suggestions to README




--------------------------------------------------

[![](https://avatars.githubusercontent.com/u/78647352?s=80&v=4)Ma-Hup](https://github.com/Ma-Hup)
pushed 1 commit • 82a2796…f5e712f • 

9 days ago

More activity actions

More activity actions

Revise README for detailed project overview and usage




-----------------------------------------------------------

[![](https://avatars.githubusercontent.com/u/78647352?s=80&v=4)Ma-Hup](https://github.com/Ma-Hup)
pushed 1 commit • 0e42e0a…82a2796 • 

9 days ago

More activity actions

More activity actions

Update README with documentation link




-------------------------------------------

[![](https://avatars.githubusercontent.com/u/78647352?s=80&v=4)Ma-Hup](https://github.com/Ma-Hup)
pushed 1 commit • 55ab851…0e42e0a • 

13 days ago

More activity actions

More activity actions

Init v1.2




---------------

[![](https://avatars.githubusercontent.com/u/78647352?s=80&v=4)Ma-Hup](https://github.com/Ma-Hup)
pushed 1 commit • 8ad0261…55ab851 • 

13 days ago

More activity actions

More activity actions

Initial commit




--------------------

[![](https://avatars.githubusercontent.com/u/78647352?s=80&v=4)Ma-Hup](https://github.com/Ma-Hup)
created this branch • 8ad0261 • 

13 days ago

More activity actions

More activity actions

You can’t perform that action at this time.

---

# 📄 Labels · Ma-Hup/ProjectBaseUnity · GitHub
**Source:** https://github.com/Ma-Hup/ProjectBaseUnity/labels

[Skip to content](https://github.com/Ma-Hup/ProjectBaseUnity/labels#start-of-content)
 

You signed in with another tab or window. [Reload](https://github.com/Ma-Hup/ProjectBaseUnity/labels)
 to refresh your session. You signed out in another tab or window. [Reload](https://github.com/Ma-Hup/ProjectBaseUnity/labels)
 to refresh your session. You switched accounts on another tab or window. [Reload](https://github.com/Ma-Hup/ProjectBaseUnity/labels)
 to refresh your session. Dismiss alert

[Ma-Hup](https://github.com/Ma-Hup) / **[ProjectBaseUnity](https://github.com/Ma-Hup/ProjectBaseUnity)** Public

*   [Notifications](https://github.com/login?return_to=%2FMa-Hup%2FProjectBaseUnity)
     You must be signed in to change notification settings
*   [Fork 0](https://github.com/login?return_to=%2FMa-Hup%2FProjectBaseUnity)
    
*   [Star 10](https://github.com/login?return_to=%2FMa-Hup%2FProjectBaseUnity)
    

Labels
------

Search all labels

Search

Labels
------

### 9 labels

Sort

*   [bug](https://github.com/Ma-Hup/ProjectBaseUnity/issues?q=state%3Aopen%20label%3Abug)
    
    Something isn't working
    
*   [documentation](https://github.com/Ma-Hup/ProjectBaseUnity/issues?q=state%3Aopen%20label%3Adocumentation)
    
    Improvements or additions to documentation
    
*   [duplicate](https://github.com/Ma-Hup/ProjectBaseUnity/issues?q=state%3Aopen%20label%3Aduplicate)
    
    This issue or pull request already exists
    
*   [enhancement](https://github.com/Ma-Hup/ProjectBaseUnity/issues?q=state%3Aopen%20label%3Aenhancement)
    
    New feature or request
    
*   [good first issue](https://github.com/Ma-Hup/ProjectBaseUnity/issues?q=state%3Aopen%20label%3A%22good%20first%20issue%22)
    
    Good for newcomers
    
*   [help wanted](https://github.com/Ma-Hup/ProjectBaseUnity/issues?q=state%3Aopen%20label%3A%22help%20wanted%22)
    
    Extra attention is needed
    
*   [invalid](https://github.com/Ma-Hup/ProjectBaseUnity/issues?q=state%3Aopen%20label%3Ainvalid)
    
    This doesn't seem right
    
*   [question](https://github.com/Ma-Hup/ProjectBaseUnity/issues?q=state%3Aopen%20label%3Aquestion)
    
    Further information is requested
    
*   [wontfix](https://github.com/Ma-Hup/ProjectBaseUnity/issues?q=state%3Aopen%20label%3Awontfix)
    
    This will not be worked on
    

You can’t perform that action at this time.

---

# 📄 Milestones · Ma-Hup/ProjectBaseUnity · GitHub
**Source:** https://github.com/Ma-Hup/ProjectBaseUnity/milestones

[Skip to content](https://github.com/Ma-Hup/ProjectBaseUnity/milestones#start-of-content)
 

You signed in with another tab or window. [Reload](https://github.com/Ma-Hup/ProjectBaseUnity/milestones)
 to refresh your session. You signed out in another tab or window. [Reload](https://github.com/Ma-Hup/ProjectBaseUnity/milestones)
 to refresh your session. You switched accounts on another tab or window. [Reload](https://github.com/Ma-Hup/ProjectBaseUnity/milestones)
 to refresh your session. Dismiss alert

[Ma-Hup](https://github.com/Ma-Hup) / **[ProjectBaseUnity](https://github.com/Ma-Hup/ProjectBaseUnity)** Public

*   [Notifications](https://github.com/login?return_to=%2FMa-Hup%2FProjectBaseUnity)
     You must be signed in to change notification settings
*   [Fork 0](https://github.com/login?return_to=%2FMa-Hup%2FProjectBaseUnity)
    
*   [Star 10](https://github.com/login?return_to=%2FMa-Hup%2FProjectBaseUnity)
    

Milestones
----------

List view
---------

*   [Open\
    \
    0 (0)](https://github.com/Ma-Hup/ProjectBaseUnity/milestones)
    
*   [Closed\
    \
    0 (0)](https://github.com/Ma-Hup/ProjectBaseUnity/milestones?state=closed)
    

Sort

You haven’t created any Milestones.
-----------------------------------

Use Milestones to create collections of Issues and Pull Requests for a particular release or project.

[Create a milestone](https://github.com/Ma-Hup/ProjectBaseUnity/milestones/new)

You can’t perform that action at this time.

---

# 📄 Release v1.2 · Ma-Hup/ProjectBaseUnity · GitHub
**Source:** https://github.com/Ma-Hup/ProjectBaseUnity/releases/latest

[Skip to content](https://github.com/Ma-Hup/ProjectBaseUnity/releases/latest#start-of-content)
  

You signed in with another tab or window. [Reload](https://github.com/Ma-Hup/ProjectBaseUnity/releases/latest)
 to refresh your session. You signed out in another tab or window. [Reload](https://github.com/Ma-Hup/ProjectBaseUnity/releases/latest)
 to refresh your session. You switched accounts on another tab or window. [Reload](https://github.com/Ma-Hup/ProjectBaseUnity/releases/latest)
 to refresh your session. Dismiss alert

[Ma-Hup](https://github.com/Ma-Hup) / **[ProjectBaseUnity](https://github.com/Ma-Hup/ProjectBaseUnity)** Public

*   [Notifications](https://github.com/login?return_to=%2FMa-Hup%2FProjectBaseUnity)
     You must be signed in to change notification settings
*   [Fork 0](https://github.com/login?return_to=%2FMa-Hup%2FProjectBaseUnity)
    
*   [Star 10](https://github.com/login?return_to=%2FMa-Hup%2FProjectBaseUnity)
    

v1.2
====

[Latest](https://github.com/Ma-Hup/ProjectBaseUnity/releases/latest)

[Latest](https://github.com/Ma-Hup/ProjectBaseUnity/releases/latest)

Compare

Choose a tag to compare
=======================

Sorry, something went wrong.
----------------------------

Filter

Loading 

Sorry, something went wrong.
----------------------------

### Uh oh!

There was an error while loading. [Please reload this page](https://github.com/Ma-Hup/ProjectBaseUnity/releases/latest)
.

No results found
----------------

[View all tags](https://github.com/Ma-Hup/ProjectBaseUnity/tags)

![@Ma-Hup](https://avatars.githubusercontent.com/u/78647352?s=40&v=4) [Ma-Hup](https://github.com/Ma-Hup)
 released this 20 Nov 10:06

· [3 commits](https://github.com/Ma-Hup/ProjectBaseUnity/compare/v1.2...main)
 to main since this release

[v1.2](https://github.com/Ma-Hup/ProjectBaseUnity/tree/v1.2)

[`55ab851`](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765)

[UnityPackage.zip](https://github.com/user-attachments/files/23651114/UnityPackage.zip)

Assets 2

*   [Source code (zip)](https://github.com/Ma-Hup/ProjectBaseUnity/archive/refs/tags/v1.2.zip)
    
    2025-11-20T09:56:41Z
    
*   [Source code (tar.gz)](https://github.com/Ma-Hup/ProjectBaseUnity/archive/refs/tags/v1.2.tar.gz)
    
    2025-11-20T09:56:41Z
    

 

All reactions

You can’t perform that action at this time.

---

# 📄 Activity · Ma-Hup/ProjectBaseUnity · GitHub
**Source:** https://github.com/Ma-Hup/ProjectBaseUnity/activity?ref=main&sort=ASC

[Skip to content](https://github.com/Ma-Hup/ProjectBaseUnity/activity?ref=main&sort=ASC#start-of-content)
  

You signed in with another tab or window. [Reload](https://github.com/Ma-Hup/ProjectBaseUnity/activity?ref=main&sort=ASC)
 to refresh your session. You signed out in another tab or window. [Reload](https://github.com/Ma-Hup/ProjectBaseUnity/activity?ref=main&sort=ASC)
 to refresh your session. You switched accounts on another tab or window. [Reload](https://github.com/Ma-Hup/ProjectBaseUnity/activity?ref=main&sort=ASC)
 to refresh your session. Dismiss alert

[Ma-Hup](https://github.com/Ma-Hup) / **[ProjectBaseUnity](https://github.com/Ma-Hup/ProjectBaseUnity)** Public

*   [Notifications](https://github.com/login?return_to=%2FMa-Hup%2FProjectBaseUnity)
     You must be signed in to change notification settings
*   [Fork 0](https://github.com/login?return_to=%2FMa-Hup%2FProjectBaseUnity)
    
*   [Star 10](https://github.com/login?return_to=%2FMa-Hup%2FProjectBaseUnity)
    

 mainAll activity

All users

All time

[Showing oldest first](https://github.com/Ma-Hup/ProjectBaseUnity/activity?ref=main)

Initial commit




--------------------

[![](https://avatars.githubusercontent.com/u/78647352?s=80&v=4)Ma-Hup](https://github.com/Ma-Hup)
created this branch • 8ad0261 • 

13 days ago

More activity actions

More activity actions

Init v1.2




---------------

[![](https://avatars.githubusercontent.com/u/78647352?s=80&v=4)Ma-Hup](https://github.com/Ma-Hup)
pushed 1 commit • 8ad0261…55ab851 • 

13 days ago

More activity actions

More activity actions

Update README with documentation link




-------------------------------------------

[![](https://avatars.githubusercontent.com/u/78647352?s=80&v=4)Ma-Hup](https://github.com/Ma-Hup)
pushed 1 commit • 55ab851…0e42e0a • 

13 days ago

More activity actions

More activity actions

Revise README for detailed project overview and usage




-----------------------------------------------------------

[![](https://avatars.githubusercontent.com/u/78647352?s=80&v=4)Ma-Hup](https://github.com/Ma-Hup)
pushed 1 commit • 0e42e0a…82a2796 • 

9 days ago

More activity actions

More activity actions

Add future improvement suggestions to README




--------------------------------------------------

[![](https://avatars.githubusercontent.com/u/78647352?s=80&v=4)Ma-Hup](https://github.com/Ma-Hup)
pushed 1 commit • 82a2796…f5e712f • 

9 days ago

More activity actions

More activity actions

You can’t perform that action at this time.

---

# 📄 Labels · Ma-Hup/ProjectBaseUnity · GitHub
**Source:** https://github.com/Ma-Hup/ProjectBaseUnity/labels?page=1

[Skip to content](https://github.com/Ma-Hup/ProjectBaseUnity/labels?page=1#start-of-content)
 

You signed in with another tab or window. [Reload](https://github.com/Ma-Hup/ProjectBaseUnity/labels?page=1)
 to refresh your session. You signed out in another tab or window. [Reload](https://github.com/Ma-Hup/ProjectBaseUnity/labels?page=1)
 to refresh your session. You switched accounts on another tab or window. [Reload](https://github.com/Ma-Hup/ProjectBaseUnity/labels?page=1)
 to refresh your session. Dismiss alert

[Ma-Hup](https://github.com/Ma-Hup) / **[ProjectBaseUnity](https://github.com/Ma-Hup/ProjectBaseUnity)** Public

*   [Notifications](https://github.com/login?return_to=%2FMa-Hup%2FProjectBaseUnity)
     You must be signed in to change notification settings
*   [Fork 0](https://github.com/login?return_to=%2FMa-Hup%2FProjectBaseUnity)
    
*   [Star 10](https://github.com/login?return_to=%2FMa-Hup%2FProjectBaseUnity)
    

Labels
------

Search all labels

Search

Labels
------

### 9 labels

Sort

*   [bug](https://github.com/Ma-Hup/ProjectBaseUnity/issues?q=state%3Aopen%20label%3Abug)
    
    Something isn't working
    
*   [documentation](https://github.com/Ma-Hup/ProjectBaseUnity/issues?q=state%3Aopen%20label%3Adocumentation)
    
    Improvements or additions to documentation
    
*   [duplicate](https://github.com/Ma-Hup/ProjectBaseUnity/issues?q=state%3Aopen%20label%3Aduplicate)
    
    This issue or pull request already exists
    
*   [enhancement](https://github.com/Ma-Hup/ProjectBaseUnity/issues?q=state%3Aopen%20label%3Aenhancement)
    
    New feature or request
    
*   [good first issue](https://github.com/Ma-Hup/ProjectBaseUnity/issues?q=state%3Aopen%20label%3A%22good%20first%20issue%22)
    
    Good for newcomers
    
*   [help wanted](https://github.com/Ma-Hup/ProjectBaseUnity/issues?q=state%3Aopen%20label%3A%22help%20wanted%22)
    
    Extra attention is needed
    
*   [invalid](https://github.com/Ma-Hup/ProjectBaseUnity/issues?q=state%3Aopen%20label%3Ainvalid)
    
    This doesn't seem right
    
*   [question](https://github.com/Ma-Hup/ProjectBaseUnity/issues?q=state%3Aopen%20label%3Aquestion)
    
    Further information is requested
    
*   [wontfix](https://github.com/Ma-Hup/ProjectBaseUnity/issues?q=state%3Aopen%20label%3Awontfix)
    
    This will not be worked on
    

You can’t perform that action at this time.

---

# 📄 Milestones · Ma-Hup/ProjectBaseUnity · GitHub
**Source:** https://github.com/Ma-Hup/ProjectBaseUnity/milestones?state=closed

[Skip to content](https://github.com/Ma-Hup/ProjectBaseUnity/milestones?state=closed#start-of-content)
 

You signed in with another tab or window. [Reload](https://github.com/Ma-Hup/ProjectBaseUnity/milestones?state=closed)
 to refresh your session. You signed out in another tab or window. [Reload](https://github.com/Ma-Hup/ProjectBaseUnity/milestones?state=closed)
 to refresh your session. You switched accounts on another tab or window. [Reload](https://github.com/Ma-Hup/ProjectBaseUnity/milestones?state=closed)
 to refresh your session. Dismiss alert

[Ma-Hup](https://github.com/Ma-Hup) / **[ProjectBaseUnity](https://github.com/Ma-Hup/ProjectBaseUnity)** Public

*   [Notifications](https://github.com/login?return_to=%2FMa-Hup%2FProjectBaseUnity)
     You must be signed in to change notification settings
*   [Fork 0](https://github.com/login?return_to=%2FMa-Hup%2FProjectBaseUnity)
    
*   [Star 10](https://github.com/login?return_to=%2FMa-Hup%2FProjectBaseUnity)
    

Milestones
----------

List view
---------

*   [Open\
    \
    0 (0)](https://github.com/Ma-Hup/ProjectBaseUnity/milestones)
    
*   [Closed\
    \
    0 (0)](https://github.com/Ma-Hup/ProjectBaseUnity/milestones?state=closed)
    

Sort

You haven’t created any Milestones.
-----------------------------------

Use Milestones to create collections of Issues and Pull Requests for a particular release or project.

[Create a milestone](https://github.com/Ma-Hup/ProjectBaseUnity/milestones/new)

You can’t perform that action at this time.

---

# 📄 Init v1.2 · Ma-Hup/ProjectBaseUnity@55ab851 · GitHub
**Source:** https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765

[Skip to content](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#start-of-content)
  

You signed in with another tab or window. [Reload](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765)
 to refresh your session. You signed out in another tab or window. [Reload](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765)
 to refresh your session. You switched accounts on another tab or window. [Reload](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765)
 to refresh your session. Dismiss alert

[Ma-Hup](https://github.com/Ma-Hup) / **[ProjectBaseUnity](https://github.com/Ma-Hup/ProjectBaseUnity)** Public

*   [Notifications](https://github.com/login?return_to=%2FMa-Hup%2FProjectBaseUnity)
     You must be signed in to change notification settings
*   [Fork 0](https://github.com/login?return_to=%2FMa-Hup%2FProjectBaseUnity)
    
*   [Star 10](https://github.com/login?return_to=%2FMa-Hup%2FProjectBaseUnity)
    

File tree
---------

Expand file treeCollapse file tree

120 files changed
-----------------

+9601

\-0

lines changed

TopOpen diff view settings

Filter options

*   .vscode
    
    *   [extensions.json](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-c16655a98a3ee89a7636a59c59a72b0e93649e3a1e947327cfc43a1336b4e912)
        
    *   [launch.json](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-bd5430ee7c51dc892a67b3f2829d1f5b6d223f0fd48b82322cfd45baf9f5e945)
        
    *   [settings.json](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-a5de3e5871ffcc383a2294845bd3df25d3eeff6c29ad46e3a396577c413bf357)
        
*   Assets
    
    *   [Plugins.meta](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-5cb9f3b4faf5521fd02332e5fe515346617c26469493201ca129e96c975bf2b6)
        
    *   Plugins
        
        *   [Demigiant.meta](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-d24cca5c37562d8808dafe42b1b52a57d1ac95c078e4e19784db4e6debf982e6)
            
        *   Demigiant
            
            *   [DOTween.meta](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-0d777568fb30f366aa81f4a1fa54884cae6ee20021d1ab1c97151fc16b2e9d6d)
                
            *   DOTween
                
                *   [DOTween.dll.meta](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-0959e7999f8deaa7a48141113df7e0fea306fe0aaf6053f2d94853fd91cb9402)
                    
                *   [DOTween.dll](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-2c2f992ee395725b2fc299edd881879e37538ab837f33ce9084ddb7a5da787b5)
                    
                *   [Editor.meta](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-2f7ea41581bd7041f044aef2b74c8c0cb4866988b0c87f87f5a0b65e293227f3)
                    
                *   Editor
                    
                    *   [DOTweenEditor.dll.meta](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-96a8f320e873d620fb5a6e0c80a014f617f028cfb1793ba17d1a95bf8530cdf7)
                        
                    *   [DOTweenEditor.dll](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-a781516dce1e60b61e12b76e4aba8ee003948f218ef95be0b5b66d6e0c4ba017)
                        
                *   [Modules.meta](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-b1c4acde6a601ae196902209d09f609010e6d900f15ccf3b99efc792f565efb1)
                    
                *   Modules
                    
                    *   [DOTweenModuleAudio.cs.meta](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-79fb71aa469852f854f3939ec7c61c478a89dbc1ab0438e4ee22a8493e0ec46c)
                        
                    *   [DOTweenModuleAudio.cs](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-fba83826dd0e441aacd3373b4514067912765987f7f9e1b1414f14de4826c8d6)
                        
                    *   [DOTweenModulePhysics.cs.meta](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-ffaee898259a50010ad43b7e65a6795d353f18e52197144892c884ef9830112f)
                        
                    *   [DOTweenModulePhysics.cs](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-41b8a28fc04ce12cb0309824608c69514c228f50ced6dc57ecdc516d1d4aedcd)
                        
                    *   [DOTweenModulePhysics2D.cs.meta](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-10c68a87383bdd54917ad2412a06bda353b81207116f4214bd6621ead94ecdcb)
                        
                    *   [DOTweenModulePhysics2D.cs](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-c9cd43b7ded70a21a13b5394a980a3ae239b6d7bd7f09d5dd91567bb3e6054a5)
                        
                    *   [DOTweenModuleSprite.cs.meta](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-e5e4b760cc796680d4d12cb05e9cd9c41e2b5153f3d36fe59bb20111d8390d9b)
                        
                    *   [DOTweenModuleSprite.cs](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-817934785e68e5b9d28d0640db375c074c02b8d039eb81af518d6736795335ec)
                        
                    *   [DOTweenModuleUI.cs.meta](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-e6fa0bcfc7826e17949e09c6c7506404dc941ea6a324b562549ab587f92f87b6)
                        
                    *   [DOTweenModuleUI.cs](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-9b282226ebb7930bdbc29b57b0fdac42c8dcb54c03365a44f6b4d3c59e6bc658)
                        
                    *   [DOTweenModuleUnityVersion.cs.meta](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-9d8e53f021981101c1f4dff9d417d6666644c265c71d741147307997895e6f78)
                        
                    *   [DOTweenModuleUnityVersion.cs](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-e966226b7057809e7778805c1073c7867dc27e28ba939aa2cf8a1b35dca14f68)
                        
                    *   [DOTweenModuleUtils.cs.meta](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-0f3d6a33a683a9891c14f52e5147ccfab83c1cc65336f95f713e72bb2f2d9eb9)
                        
                    *   [DOTweenModuleUtils.cs](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-5e5a5352d3a3903e4eed78d018430cf65d98e944d8822eff72c4ae5fd490c899)
                        
    *   [Resources.meta](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-8f7e026cbca9bebb89dc572e0d78fb3c9aa800efb50ff1082cafcfb6afcef2a0)
        
    *   Resources
        
        *   [ProjecBaseSampleUI.meta](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-0e299a5a9a30216574fc85722e42663e79bcb058c3f2c6037f2e38470afd80b5)
            
        *   ProjecBaseSampleUI
            
            *   [PBFPanel.prefab.meta](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-2e7d09b786ab6f2ac38ca54c814639d9bbf4d931ddd250ec7702aa288d396f3f)
                
            *   [PBFPanel.prefab](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-8980d8b731698192088655d5e6242f2776bedb08ca00ad60d1dc88a9c271b50a)
                
        *   [UI.meta](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-0bd0e89bac600ea8455d79d85ade3bfc20f37269552117a0dc8017c0be04e0a0)
            
        *   UI
            
            *   [EventSystem.prefab.meta](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-dc0ce1ff989363f238ff2099e88f99362077fb402debfd10de0f7c208ab7e821)
                
            *   [EventSystem.prefab](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-79413ca60e219522efc5b298f4c5c81c11c0199811706243eb21a003ef6d6dbc)
                
            *   [MainCanvas.prefab.meta](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-d99ecd85baaff77ab1a8b5e2570793bcde73389bbfbea160b694d8d2be9472c1)
                
            *   [MainCanvas.prefab](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-40c265f10f80ba56458a4e85d2bc133285d2724159e1b775485f584693a1273b)
                
            *   [PBWinPanelTest1.prefab.meta](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-7ed4dc233b56f5fb3a792ccb43b40111ec1b585490d3a1a432fa211c89953586)
                
            *   [PBWinPanelTest1.prefab](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-86cbe5b2c71c033a88ff51f4407cc1a8f9b498f121f5d4cd25eecad49f9d3910)
                
            *   [PBWinPanelTest2.prefab.meta](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-9f0299ea419d5e7c9186c9acee600a390d396cb2d3bf421c8b17382a7d517d39)
                
            *   [PBWinPanelTest2.prefab](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-ad1bdda02fa01261304ebc24d095f08ed8e6253e90a363157c60c6fe1bde943b)
                
    *   [Scenes.meta](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-77bf59dbf8ea17b55ad5e156faf2bfce9f8a6b0d632227648b41ccbf249ebf45)
        
    *   Scenes
        
        *   [ProjectBaseTestScene.unity.meta](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-0a7a57c256d98a9e69cfb1783f0d507849699e09ac95b282a42b746acf81c0ed)
            
        *   [ProjectBaseTestScene.unity](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-52dda16677ba0e4af87ce06e2095eabdf911298354f21926623ad379481eb18f)
            
    *   [Scripts.meta](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-46952f10b5034ee933fd429b1998191875acb1ce5483c3faf44660743021fe4a)
        
    *   Scripts
        
        *   [Game.meta](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-b08b08f51091a45046fb55956f7fabc96dbff892457c67377b93df476d090460)
            
        *   Game
            
            *   [PBFSampleGameStart.cs.meta](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-67a4e1f503cb79d0dfe8a6aa2845fa3291c7fa90d5b631dcee5da39b22d42e86)
                
            *   [PBFSampleGameStart.cs](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-5802d880eb7f4604c8c599ca9351fc240f29bd08577a47386256d862d09b7531)
                
        *   [PBFramworkSample.meta](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-ff661efc247551364c2bfeb3737e160764f0f08b7bc7fbc7abb7284090de6239)
            
        *   PBFramworkSample
            
            *   [PBFPanel.cs.meta](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-dd727b137058e35676eac165238fabaaf4497759194c085f5dc12b8d0f4d2eb1)
                
            *   [PBFPanel.cs](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-2f1e0095d38ab06a4a86dca6a6a869187034fcbc803e95368e88eebde0c08e84)
                
            *   [PBWinPanelTest1.cs.meta](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-7507acb598a0e46fc55e992ad5bdaed635fe00d5ed5758c5a3e973b37d7b98ae)
                
            *   [PBWinPanelTest1.cs](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-6c2c5a05a676ce12066236d296d543c47fd3dfcb88fa38ac11da07be28864961)
                
            *   [PBWinPanelTest2.cs.meta](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-f16e6187949a56b96634ad3ace6b70aa571d2c602ed8bb1ad4f6a08c44f3afb6)
                
            *   [PBWinPanelTest2.cs](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-bef269cc36505d964d7303d902d56e099ae3735833821df5af7d261f65d74aa5)
                
        *   [ProjectBase.meta](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-51429524d9bcce3c92ecaf791a4ad2900e019b4e64087b10b5d7081c68018c09)
            
        *   ProjectBase
            
            *   [Constants.meta](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-3186a7e8f077954b60aedcd3e49812721b11833d09411de7a333465641fbc72f)
                
            *   Constants
                
                *   [Constants.cs.meta](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-b96a7066937660dbac013239b17dbed77a4790a5abcd960f9fb80e868e94bad7)
                    
                *   [Constants.cs](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-a33f6decb7066e33382466d2c177eb543d2c09e46838cd76d08c167d37df74ce)
                    
            *   [DataManager.meta](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-23fd15c71a73154e497a24f6025a8f11ea5da71602b73c34a2346a3eca1033ff)
                
            *   DataManager
                
                *   [JsonManager.cs.meta](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-e067eec7172571d47437a72f2b096d6306ecbdf873dc645235326bcc66568107)
                    
                *   [JsonManager.cs](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-a6d59b13d2c440542035afd3ce54e8e3feca169f6f17b11f936fc66ba79a329a)
                    
                *   [PPDataManager.cs.meta](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-0f03d39935676ba41412ca52c3984928abef62dad49532f5767a1d2dde8bfea1)
                    
                *   [PPDataManager.cs](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-5e4a5591eea887e10801adaa1813b40ba4269cf3657610a904d61dab36648231)
                    
                *   [Serialization.cs.meta](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-df583e42cc9a4266bab3b2415c08cdd1b61315bc6e8ce32073c7c4e80207e8c6)
                    
                *   [Serialization.cs](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-fa0eed0678c9cf64632375c916c3656d0139a1b2b90cf2922ab483fe023a61d3)
                    
            *   [Events.meta](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-1917e65419a1ca6dcc243076aa40fb11bae44451a9443e339f39dd2e81b44927)
                
            *   Events
                
                *   [EventsManager.cs.meta](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-132bb966e45004f8f823551d12e8abf4452f7ef331cc10c7e1e9dff043b6f965)
                    
                *   [EventsManager.cs](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-00b05417081ac016684497709fb3d166cc184b194a7cb1eb354f6199eec04126)
                    
            *   [Input.meta](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-67abb98b4be8ea6a7fe976487952d4c6eb4a3f358ab7e711e0d4657bbfb70756)
                
            *   Input
                
                *   [InputManager.cs.meta](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-f15a72fd7778ffb1fe08717c604ba30f9d32d53ef4e46c02a6bf1132bff11bba)
                    
                *   [InputManager.cs](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-72ba1a7d5087d00f9b3240d92822db13c144c9145d14d50632b5b6bf3e9e6360)
                    
            *   [Mono.meta](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-36c2b903fc2a84ba68798f2f0108f40dfdcbbdaa299a87540365adf697a827ce)
                
            *   Mono
                
                *   [MonoController.cs.meta](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-568a091cb31ab8a2698b6738da569d0afbff184728d5b92346c59ba8d3f4a3b4)
                    
                *   [MonoController.cs](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-fcd978bea954def25637e5fc8ea2c36b65c9c37bab78a998209f6ca9e1eaa6a0)
                    
            *   [Music.meta](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-f673dbf0c18f63c23df1addf0ce339a59395b6a07af863eac7d5f99a308c9652)
                
            *   Music
                
                *   [MusicManager.cs.meta](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-a15aede84b687c6e6a3800a5c91f08f3e8cd9abe8051774bda2643a6bf2831ec)
                    
                *   [MusicManager.cs](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-69491c5545b5f86bde6fd04dc56e1fea0ab92aa5fa7372f87f1ae941b5fc1ba2)
                    
            *   [ObjectPool.meta](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-bb9e350893021510bf3e13974bef2b892900470e059ef1ec7c95de498b325e3a)
                
            *   ObjectPool
                
                *   [ObjectPool.cs.meta](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-900f5b4dd48792489fb22f583fdcd2a84cbad1cca8bfa85e9e15ee7293323695)
                    
                *   [ObjectPool.cs](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-39fa02ec2d11443b8ec4ac63229981607459e6fa2d5f20b384a8657918f88356)
                    
                *   [PoolData.cs.meta](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-1c07ad15423cf11a0dffb0e48a25197f9bfd8e3de92ea5dfe77e285706930660)
                    
                *   [PoolData.cs](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-f11ea27f61097c8930acbca1b90eb05a3d0a0456fc1cfaaf5795393d58e51131)
                    
            *   [ResManager.meta](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-503b0d332588eb55554e788c5a82650205b33b629130cdd5e643500c5ec17545)
                
            *   ResManager
                
                *   [ResManager.cs.meta](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-9747c20423422a636c74098b202f69483b5d41a78cc97f09eba405d259acfbd8)
                    
                *   [ResManager.cs](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-9ee0648e6404b69efaf30f046d53b9f3b8535ba885968fe8cf65d9522613d95a)
                    
            *   [UI.meta](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-58bcfd45db9dd4e459c1e48479bc9132d3ae8ce241e8d9ab4848e50098346c5e)
                
            *   UI
                
                *   [BasePanel.cs.meta](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-cbecf0bd49f905a88c078a62eba585bc2fc4462f742d403b7558df38e22ee78d)
                    
                *   [BasePanel.cs](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-943229a2650c1c9b30102a0887997f9ea2bcf6a5f4f10616e9b360f5bc0a1e38)
                    
                *   [UIManager.cs.meta](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-2e3d9907852d75a3cccc6f850169770db16cfc77779ecc5ab8ebbb16e0f6ebbb)
                    
                *   [UIManager.cs](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-73b26877e6940ecf4953099c04f17f9cf82ecab7424f460aa174d49a8b7810cf)
                    
            *   [base.meta](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-a10a3c4f44f75b7f6c591603984191433ac8958870935355b248843e5030ec89)
                
            *   base
                
                *   [SingletonBase.cs.meta](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-b8611c21d9528c23ea057900dc857b0eeb6359517095d3baacffab732e52c24d)
                    
                *   [SingletonBase.cs](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-1f9611792303c877fd97d963a9fc8ce6af3dc2e455af87e07ae88e07561ae1e0)
                    
                *   [SingletonMono.cs.meta](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-fb693fde427b42943a11cfec357aad6f548871d75ff9cc438f0b268a8324e8fa)
                    
                *   [SingletonMono.cs](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-c64b05a6c16a2b2475c9853428b8f91792ff81f92388b53eee36e26f047b2f77)
                    
*   Packages
    
    *   [manifest.json](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-c1991832e77c2a072aa97683b2b773230f3237d79cf720da96303ba012efc59c)
        
    *   [packages-lock.json](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-8ae68780f55772c74938b9d878faafd03740de2b900b4c8cf2b8da6b3b8a775a)
        
*   ProjectSettings
    
    *   [AudioManager.asset](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-7a79f5a1b54519a59ff2d8eaf091dff5ce32efddd54597e50cee49a6f6cd5b08)
        
    *   [ClusterInputManager.asset](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-85be94710300891c6539292a63e15e6bff825c6b100231707ba473328d28f495)
        
    *   [DynamicsManager.asset](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-be01dfd6d91980b84762e5416e6f87888cbd6c83a858ae7c931b968d01fa2a5a)
        
    *   [EditorBuildSettings.asset](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-8f841c37214b739f14055cdb3e9473fa692bf771affb1c5ec5da406ae76d92dc)
        
    *   [EditorSettings.asset](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-29b79f7389ae00f6791025a4551d29f3148bd4880cb6915a727069cdbb206bce)
        
    *   [GraphicsSettings.asset](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-d6b5b7d6a42d4e04b725021f38ab5029472e26932c63c9ba1a03a3bee95a9a1d)
        
    *   [InputManager.asset](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-a759f3f46fd5fe639648a1b343d0e96051571b806a37c02f15b4138641745744)
        
    *   [MemorySettings.asset](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-d46423a43f8aa6d736c2f21b976e7134c661701f21602ee6bf6068f32d06479c)
        
    *   [NavMeshAreas.asset](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-dc0f9f157672b02c6e09f53d5773a019c5c06e7f2ba394f2081673b028ca5670)
        
    *   [NetworkManager.asset](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-c83c19ebaf39018d4f3d3e18784c309fe7ab282238208632715a0bf866139f33)
        
    *   [PackageManagerSettings.asset](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-6eb7fddee2848ee224b58830f919e391b527ec03b19a4727b653b73b06caeafe)
        
    *   [Physics2DSettings.asset](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-93472a9c31493ef62af7c455731d06f0aa1dd15abc37837c198c4f1f9759439a)
        
    *   [PresetManager.asset](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-549747b36506ba6f130ed84f1e39dc2bc03e22bda4f07071bc0fac4f8ef8e441)
        
    *   [ProjectSettings.asset](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-f1db76edbe0068ccbe4382393c8275c49a9ec78927f9f82b29506a506049adea)
        
    *   [ProjectVersion.txt](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-04bba66d4186c816ac11500635ac14398d7dc60a1b35eee4df63b208851de354)
        
    *   [QualitySettings.asset](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-50530539334718d2c28258d31766bc6df87503ea226ab566233142f33be56b33)
        
    *   [SceneTemplateSettings.json](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-e20fecaad077b9764b35c63b14104a973deee1000d857b1d9b301bd37b83130e)
        
    *   [TagManager.asset](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-e69257521d1bbed0cad6e9152a3d72ed224166b4f2f60e872f8dc80bf1b4cdd2)
        
    *   [TimeManager.asset](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-899a2e45f5ff048cd5e90c5bc2432cbe8157722ecbddb5e4d3ee9fec50b517ca)
        
    *   [UnityConnectSettings.asset](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-da2c1df2e1059ef6d5bc370936cb639892a00bf6a8823fd4ee78adef0335f2ec)
        
    *   [VFXManager.asset](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-7a1bdfbdb0414a5f850b10039fc4cfda60e3801fb46f0337996753a809afad8b)
        
    *   [VersionControlSettings.asset](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-aed2751767de9941c6708699e454879964f9b6ec567b10bf61e9f30897a52749)
        
    *   [XRSettings.asset](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-ffce5bf43e1e87278ea035e147dc7446805d1334d3e3dbecfacd99248e58c0d0)
        
*   [UnityPackage.zip](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-60c7c4e4bee48c4e48d6e021c805761270fe033bffe525b978ba4e34bdd73bd7)
    

Some content is hidden
----------------------

Large Commits have some content hidden by default. Use the searchbox below for content that may be hidden.

Dismiss banner

Expand file treeCollapse file tree

120 files changed
-----------------

+9601

\-0

lines changed

 

Open diff view settings

Collapse file

### [`‎.vscode/extensions.json‎`](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-c16655a98a3ee89a7636a59c59a72b0e93649e3a1e947327cfc43a1336b4e912)

Copy file name to clipboard

+5Lines changed: 5 additions & 0 deletions

| Original file line number | Diff line number | Diff line change |
| --- | --- | --- |
| `   @@ -0,0 +1,5 @@   ` |     |     |     |
|     | `1` | `+  {  ` |
|     | `2` | `+  "recommendations": [  ` |\
|     | `3` | `+  "visualstudiotoolsforunity.vstuc"  ` |\
|     | `4` | `+  ]  ` |
|     | `5` | `+  }  ` |

Collapse file

### [`‎.vscode/launch.json‎`](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-bd5430ee7c51dc892a67b3f2829d1f5b6d223f0fd48b82322cfd45baf9f5e945)

Copy file name to clipboard

+10Lines changed: 10 additions & 0 deletions

| Original file line number | Diff line number | Diff line change |
| --- | --- | --- |
| `   @@ -0,0 +1,10 @@   ` |     |     |     |
|     | `1` | `+  {  ` |
|     | `2` | `+  "version": "0.2.0",  ` |
|     | `3` | `+  "configurations": [  ` |\
|     | `4` | `+  {  ` |\
|     | `5` | `+  "name": "Attach to Unity",  ` |\
|     | `6` | `+  "type": "vstuc",  ` |\
|     | `7` | `+  "request": "attach"  ` |\
|     | `8` | `+  }  ` |\
|     | `9` | `+  ]  ` |
|     | `10` | `+  }  ` |

Collapse file

### [`‎.vscode/settings.json‎`](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-a5de3e5871ffcc383a2294845bd3df25d3eeff6c29ad46e3a396577c413bf357)

Copy file name to clipboard

+60Lines changed: 60 additions & 0 deletions

| Original file line number | Diff line number | Diff line change |
| --- | --- | --- |
| `   @@ -0,0 +1,60 @@   ` |     |     |     |
|     | `1` | `+  {  ` |
|     | `2` | `+  "files.exclude": {  ` |
|     | `3` | `+  "**/.DS_Store": true,  ` |
|     | `4` | `+  "**/.git": true,  ` |
|     | `5` | `+  "**/.vs": true,  ` |
|     | `6` | `+  "**/.gitmodules": true,  ` |
|     | `7` | `+  "**/.vsconfig": true,  ` |
|     | `8` | `+  "**/*.booproj": true,  ` |
|     | `9` | `+  "**/*.pidb": true,  ` |
|     | `10` | `+  "**/*.suo": true,  ` |
|     | `11` | `+  "**/*.user": true,  ` |
|     | `12` | `+  "**/*.userprefs": true,  ` |
|     | `13` | `+  "**/*.unityproj": true,  ` |
|     | `14` | `+  "**/*.dll": true,  ` |
|     | `15` | `+  "**/*.exe": true,  ` |
|     | `16` | `+  "**/*.pdf": true,  ` |
|     | `17` | `+  "**/*.mid": true,  ` |
|     | `18` | `+  "**/*.midi": true,  ` |
|     | `19` | `+  "**/*.wav": true,  ` |
|     | `20` | `+  "**/*.gif": true,  ` |
|     | `21` | `+  "**/*.ico": true,  ` |
|     | `22` | `+  "**/*.jpg": true,  ` |
|     | `23` | `+  "**/*.jpeg": true,  ` |
|     | `24` | `+  "**/*.png": true,  ` |
|     | `25` | `+  "**/*.psd": true,  ` |
|     | `26` | `+  "**/*.tga": true,  ` |
|     | `27` | `+  "**/*.tif": true,  ` |
|     | `28` | `+  "**/*.tiff": true,  ` |
|     | `29` | `+  "**/*.3ds": true,  ` |
|     | `30` | `+  "**/*.3DS": true,  ` |
|     | `31` | `+  "**/*.fbx": true,  ` |
|     | `32` | `+  "**/*.FBX": true,  ` |
|     | `33` | `+  "**/*.lxo": true,  ` |
|     | `34` | `+  "**/*.LXO": true,  ` |
|     | `35` | `+  "**/*.ma": true,  ` |
|     | `36` | `+  "**/*.MA": true,  ` |
|     | `37` | `+  "**/*.obj": true,  ` |
|     | `38` | `+  "**/*.OBJ": true,  ` |
|     | `39` | `+  "**/*.asset": true,  ` |
|     | `40` | `+  "**/*.cubemap": true,  ` |
|     | `41` | `+  "**/*.flare": true,  ` |
|     | `42` | `+  "**/*.mat": true,  ` |
|     | `43` | `+  "**/*.meta": true,  ` |
|     | `44` | `+  "**/*.prefab": true,  ` |
|     | `45` | `+  "**/*.unity": true,  ` |
|     | `46` | `+  "build/": true,  ` |
|     | `47` | `+  "Build/": true,  ` |
|     | `48` | `+  "Library/": true,  ` |
|     | `49` | `+  "library/": true,  ` |
|     | `50` | `+  "obj/": true,  ` |
|     | `51` | `+  "Obj/": true,  ` |
|     | `52` | `+  "Logs/": true,  ` |
|     | `53` | `+  "logs/": true,  ` |
|     | `54` | `+  "ProjectSettings/": true,  ` |
|     | `55` | `+  "UserSettings/": true,  ` |
|     | `56` | `+  "temp/": true,  ` |
|     | `57` | `+  "Temp/": true  ` |
|     | `58` | `+  },  ` |
|     | `59` | `+  "dotnet.defaultSolution": "PBFramwork.sln"  ` |
|     | `60` | `+  }  ` |

Collapse file

### [`‎Assets/Plugins.meta‎`](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-5cb9f3b4faf5521fd02332e5fe515346617c26469493201ca129e96c975bf2b6)

Copy file name to clipboardExpand all lines: Assets/Plugins.meta

+8Lines changed: 8 additions & 0 deletions

Load DiffSome generated files are not rendered by default. Learn more about [customizing how changed files appear on GitHub.](https://docs.github.com/github/administering-a-repository/customizing-how-changed-files-appear-on-github)

Collapse file

### [`‎Assets/Plugins/Demigiant.meta‎`](https://github.com/Ma-Hup/ProjectBaseUnity/commit/55ab85157388a3a1508e6fc491e627a76decb765#diff-d24cca5c37562d8808dafe42b1b52a57d1ac95c078e4e19784db4e6debf982e6)

Copy file name to clipboardExpand all lines: Assets/Plugins/Demigiant.meta

+8Lines changed: 8 additions & 0 deletions

Load DiffSome generated files are not rendered by default. Learn more about [customizing how changed files appear on GitHub.](https://docs.github.com/github/administering-a-repository/customizing-how-changed-files-appear-on-github)

0 commit comments
-----------------

Comments

0 (0)

You can’t perform that action at this time.

14 files remain

---

# 📄 Initial commit · Ma-Hup/ProjectBaseUnity@8ad0261 · GitHub
**Source:** https://github.com/Ma-Hup/ProjectBaseUnity/commit/8ad02618ff4a7541bdb232c116b429adfc4bdae6

[Skip to content](https://github.com/Ma-Hup/ProjectBaseUnity/commit/8ad02618ff4a7541bdb232c116b429adfc4bdae6#start-of-content)
  

You signed in with another tab or window. [Reload](https://github.com/Ma-Hup/ProjectBaseUnity/commit/8ad02618ff4a7541bdb232c116b429adfc4bdae6)
 to refresh your session. You signed out in another tab or window. [Reload](https://github.com/Ma-Hup/ProjectBaseUnity/commit/8ad02618ff4a7541bdb232c116b429adfc4bdae6)
 to refresh your session. You switched accounts on another tab or window. [Reload](https://github.com/Ma-Hup/ProjectBaseUnity/commit/8ad02618ff4a7541bdb232c116b429adfc4bdae6)
 to refresh your session. Dismiss alert

[Ma-Hup](https://github.com/Ma-Hup) / **[ProjectBaseUnity](https://github.com/Ma-Hup/ProjectBaseUnity)** Public

*   [Notifications](https://github.com/login?return_to=%2FMa-Hup%2FProjectBaseUnity)
     You must be signed in to change notification settings
*   [Fork 0](https://github.com/login?return_to=%2FMa-Hup%2FProjectBaseUnity)
    
*   [Star 10](https://github.com/login?return_to=%2FMa-Hup%2FProjectBaseUnity)
    

File tree
---------

Expand file treeCollapse file tree

2 files changed
---------------

+101

\-0

lines changed

TopOpen diff view settings

Filter options

*   [.gitignore](https://github.com/Ma-Hup/ProjectBaseUnity/commit/8ad02618ff4a7541bdb232c116b429adfc4bdae6#diff-bc37d034bad564583790a46f19d807abfe519c5671395fd494d8cce506c42947)
    
*   [README.md](https://github.com/Ma-Hup/ProjectBaseUnity/commit/8ad02618ff4a7541bdb232c116b429adfc4bdae6#diff-b335630551682c19a781afebcf4d07bf978fb1f8ac04c6bf87428ed5106870f5)
    

Expand file treeCollapse file tree

2 files changed
---------------

+101

\-0

lines changed

 

Open diff view settings

Collapse file

### [`‎.gitignore‎`](https://github.com/Ma-Hup/ProjectBaseUnity/commit/8ad02618ff4a7541bdb232c116b429adfc4bdae6#diff-bc37d034bad564583790a46f19d807abfe519c5671395fd494d8cce506c42947)

Copy file name to clipboard

+99Lines changed: 99 additions & 0 deletions

| Original file line number | Diff line number | Diff line change |
| --- | --- | --- |
| `   @@ -0,0 +1,99 @@   ` |     |     |     |
|     | `1` | `+  # This .gitignore file should be placed at the root of your Unity project directory  ` |
|     | `2` | `+  #  ` |
|     | `3` | `+  # Get latest from https://github.com/github/gitignore/blob/main/Unity.gitignore  ` |
|     | `4` | `+  #  ` |
|     | `5` | `+  .utmp/  ` |
|     | `6` | `+  /[Ll]ibrary/  ` |
|     | `7` | `+  /[Tt]emp/  ` |
|     | `8` | `+  /[Oo]bj/  ` |
|     | `9` | `+  /[Bb]uild/  ` |
|     | `10` | `+  /[Bb]uilds/  ` |
|     | `11` | `+  /[Ll]ogs/  ` |
|     | `12` | `+  /[Uu]ser[Ss]ettings/  ` |
|     | `13` | `+  *.log  ` |
|     | `14` | `+  ` |
|     | `15` | `+  # By default unity supports Blender asset imports, *.blend1 blender files do not need to be commited to version control.  ` |
|     | `16` | `+  *.blend1  ` |
|     | `17` | `+  *.blend1.meta  ` |
|     | `18` | `+  ` |
|     | `19` | `+  # MemoryCaptures can get excessive in size.  ` |
|     | `20` | `+  # They also could contain extremely sensitive data  ` |
|     | `21` | `+  /[Mm]emoryCaptures/  ` |
|     | `22` | `+  ` |
|     | `23` | `+  # Recordings can get excessive in size  ` |
|     | `24` | `+  /[Rr]ecordings/  ` |
|     | `25` | `+  ` |
|     | `26` | `+  # Uncomment this line if you wish to ignore the asset store tools plugin  ` |
|     | `27` | `+  # /[Aa]ssets/AssetStoreTools*  ` |
|     | `28` | `+  ` |
|     | `29` | `+  # Autogenerated Jetbrains Rider plugin  ` |
|     | `30` | `+  /[Aa]ssets/Plugins/Editor/JetBrains*  ` |
|     | `31` | `+  # Jetbrains Rider personal-layer settings  ` |
|     | `32` | `+  *.DotSettings.user  ` |
|     | `33` | `+  ` |
|     | `34` | `+  # Visual Studio cache directory  ` |
|     | `35` | `+  .vs/  ` |
|     | `36` | `+  ` |
|     | `37` | `+  # Gradle cache directory  ` |
|     | `38` | `+  .gradle/  ` |
|     | `39` | `+  ` |
|     | `40` | `+  # Autogenerated VS/MD/Consulo solution and project files  ` |
|     | `41` | `+  ExportedObj/  ` |
|     | `42` | `+  .consulo/  ` |
|     | `43` | `+  *.csproj  ` |
|     | `44` | `+  *.unityproj  ` |
|     | `45` | `+  *.sln  ` |
|     | `46` | `+  *.suo  ` |
|     | `47` | `+  *.tmp  ` |
|     | `48` | `+  *.user  ` |
|     | `49` | `+  *.userprefs  ` |
|     | `50` | `+  *.pidb  ` |
|     | `51` | `+  *.booproj  ` |
|     | `52` | `+  *.svd  ` |
|     | `53` | `+  *.pdb  ` |
|     | `54` | `+  *.mdb  ` |
|     | `55` | `+  *.opendb  ` |
|     | `56` | `+  *.VC.db  ` |
|     | `57` | `+  ` |
|     | `58` | `+  # Unity3D generated meta files  ` |
|     | `59` | `+  *.pidb.meta  ` |
|     | `60` | `+  *.pdb.meta  ` |
|     | `61` | `+  *.mdb.meta  ` |
|     | `62` | `+  ` |
|     | `63` | `+  # Unity3D generated file on crash reports  ` |
|     | `64` | `+  sysinfo.txt  ` |
|     | `65` | `+  ` |
|     | `66` | `+  # Mono auto generated files  ` |
|     | `67` | `+  mono_crash.*  ` |
|     | `68` | `+  ` |
|     | `69` | `+  # Builds  ` |
|     | `70` | `+  *.apk  ` |
|     | `71` | `+  *.aab  ` |
|     | `72` | `+  *.unitypackage  ` |
|     | `73` | `+  *.unitypackage.meta  ` |
|     | `74` | `+  *.app  ` |
|     | `75` | `+  ` |
|     | `76` | `+  # Crashlytics generated file  ` |
|     | `77` | `+  crashlytics-build.properties  ` |
|     | `78` | `+  ` |
|     | `79` | `+  # TestRunner generated files  ` |
|     | `80` | `+  InitTestScene*.unity*  ` |
|     | `81` | `+  ` |
|     | `82` | `+  # Addressables default ignores, before user customizations  ` |
|     | `83` | `+  /ServerData  ` |
|     | `84` | `+  /[Aa]ssets/StreamingAssets/aa*  ` |
|     | `85` | `+  /[Aa]ssets/AddressableAssetsData/link.xml*  ` |
|     | `86` | `+  /[Aa]ssets/Addressables_Temp*  ` |
|     | `87` | `+  # By default, Addressables content builds will generate addressables_content_state.bin  ` |
|     | `88` | `+  # files in platform-specific subfolders, for example:  ` |
|     | `89` | `+  # /Assets/AddressableAssetsData/OSX/addressables_content_state.bin  ` |
|     | `90` | `+  /[Aa]ssets/AddressableAssetsData/*/*.bin*  ` |
|     | `91` | `+  ` |
|     | `92` | `+  # Visual Scripting auto-generated files  ` |
|     | `93` | `+  /[Aa]ssets/Unity.VisualScripting.Generated/VisualScripting.Flow/UnitOptions.db  ` |
|     | `94` | `+  /[Aa]ssets/Unity.VisualScripting.Generated/VisualScripting.Flow/UnitOptions.db.meta  ` |
|     | `95` | `+  /[Aa]ssets/Unity.VisualScripting.Generated/VisualScripting.Core/Property Providers  ` |
|     | `96` | `+  /[Aa]ssets/Unity.VisualScripting.Generated/VisualScripting.Core/Property Providers.meta  ` |
|     | `97` | `+  ` |
|     | `98` | `+  # Auto-generated scenes by play mode tests  ` |
|     | `99` | `+  /[Aa]ssets/[Ii]nit[Tt]est[Ss]cene*.unity*  ` |

Collapse file

### [`‎README.md‎`](https://github.com/Ma-Hup/ProjectBaseUnity/commit/8ad02618ff4a7541bdb232c116b429adfc4bdae6#diff-b335630551682c19a781afebcf4d07bf978fb1f8ac04c6bf87428ed5106870f5)

Copy file name to clipboard

+2Lines changed: 2 additions & 0 deletions

*   Display the source diff
*   Display the rich diff

| Original file line number | Diff line number | Diff line change |
| --- | --- | --- |
| `   @@ -0,0 +1,2 @@   ` |     |     |     |
|     | `1` | `+  # ProjectBaseUnity  ` |
|     | `2` | `+  将游戏的功能整理分类，集合了常用的单例，降低样板代码，适合mini、小型游戏开发、GameJam，既可作为扩展，也能作为基础框架。 包含 资源管理、UI 管理、事件、持久化、对象池、音乐、输入、常量管理、Mono等常见功能。  ` |

0 commit comments
-----------------

Comments

0 (0)

You can’t perform that action at this time.

---

