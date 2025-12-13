# MMF Player 预览/复位体系梳理

本笔记汇总了 Feel 插件中 MMF Player 及其子反馈在编辑器预览时涉及的核心 API 和调用链，便于后续定制和排查“预览后无法复位”类问题。

## 1. Inspector 预览按钮到运行时方法的映射

Unity 编辑器在 UI Toolkit 检视器中直接把底部控制条的按钮绑定到 `MMF_Player` 的运行时方法，这意味着预览与正式播放走同一套逻辑：

```csharp
// Assets/Plugins/Feel/MMFeedbacks/Editor/Core/UIToolkit/MMF_PlayerEditorUITK.cs
Button initializeButton = new Button(() => TargetMmfPlayer.Initialization()) { text = _initializeText };
Button playButton        = new Button(() => TargetMmfPlayer.PlayFeedbacks())   { text = _playText };
Button stopButton        = new Button(() => TargetMmfPlayer.StopFeedbacks())   { text = _stopText };
Button resetButton       = new Button(() => TargetMmfPlayer.ResetFeedbacks())  { text = _resetText };
Button skipButton        = new Button(() => TargetMmfPlayer.SkipToTheEnd())    { text = _skipText };
Button restoreButton     = new Button(() => TargetMmfPlayer.RestoreInitialValues()) { text = _restoreText };
Button changeDirection   = new Button(() => TargetMmfPlayer.ChangeDirection()) { text = _changeDirectionText };
```

- 只有在 **Play Mode** 下这些按钮才允许操作；同时提供了一个 **Keep Play Mode Changes** 按钮切换 `TargetMmfPlayer.KeepPlayModeChanges` 标志，决定是否在退出 Play Mode 后保留运行时改动。

## 2. Player 级生命周期操作

### Initialization
- `Initialization()` 在 Awake/Start/OnEnable 自动或手动调用，逐个初始化反馈，并缓存总时长与循环状态。【F:Assets/Plugins/Feel/MMFeedbacks/MMFeedbacks/Core/MMF_Player/MMF_Player.cs†L48-L180】

### Play / Stop
- `PlayFeedbacks()`/`PlayFeedbacks(position, intensity)` 触发整个序列；内部会先 `ResetFeedbacks()` 确保进入初始状态。
- `StopFeedbacks()` 可选择是否通知每个子反馈的 `Stop()`；同时标记 `IsPlaying = false`，结束预览。【F:Assets/Plugins/Feel/MMFeedbacks/MMFeedbacks/Core/MMF_Player/MMF_Player.cs†L769-L801】

### Reset
- `ResetFeedbacks()` 依次调用每个激活反馈的 `ResetFeedback()`，常用于预览前/后的复位。【F:Assets/Plugins/Feel/MMFeedbacks/MMFeedbacks/Core/MMF_Player/MMF_Player.cs†L807-L821】

### SkipToTheEnd
- `SkipToTheEnd()` 启动 `SkipToTheEndCo()` 协程：标记 `SkippingToTheEnd`，通知所有反馈执行 `SkipToTheEnd()`，等待两帧后强制 `StopFeedbacks()`，用于快速跳到效果尾帧。【F:Assets/Plugins/Feel/MMFeedbacks/MMFeedbacks/Core/MMF_Player/MMF_Player.cs†L919-L925】【F:Assets/Plugins/Feel/MMFeedbacks/MMFeedbacks/Core/MMF_Player/MMF_Player.cs†L743-L762】

### RestoreInitialValues
- 在已经播放过（`PlayCount > 0`）的情况下，倒序调用每个激活反馈的 `RestoreInitialValues()`，再触发对应事件。适合“把目标退回预览前状态”。【F:Assets/Plugins/Feel/MMFeedbacks/MMFeedbacks/Core/MMF_Player/MMF_Player.cs†L882-L901】

### ChangeDirection
- 翻转播放方向（TopToBottom ↔ BottomToTop）并广播事件；影响后续播放和 Pause/Loop 逻辑。【F:Assets/Plugins/Feel/MMFeedbacks/MMFeedbacks/Core/MMF_Player/MMF_Player.cs†L823-L838】

### Keep Play Mode Changes
- `KeepPlayModeChanges` 仅在编辑器使用；开启后，`MMF_PlayerCopy` 会在离开 Play Mode 时缓存该 Player 的 `FeedbacksList`，在回到编辑模式时回写，保留预览中的调整。【F:Assets/Plugins/Feel/MMFeedbacks/MMFeedbacks/Core/MMF_Player/MMF_Player.cs†L20-L46】【F:Assets/Plugins/Feel/MMFeedbacks/Editor/Core/MMF_PlayerCopy.cs†L40-L120】

## 3. Feedback 级行为：Stop / Skip / Restore / Reset

每个 `MMF_Feedback` 提供与 Player 对应的生命周期钩子，确保预览时也能正确终止与复位：

```csharp
// Assets/Plugins/Feel/MMFeedbacks/MMFeedbacks/Core/MMF_Player/MMF_Feedback.cs
public virtual void Stop(Vector3 position, float feedbacksIntensity = 1.0f) { /* 停止协程 & 自定义停止 */ }
public virtual void SkipToTheEnd(Vector3 position, float feedbacksIntensity = 1.0f) { CustomSkipToTheEnd(position, feedbacksIntensity); }
public virtual void RestoreInitialValues() { CustomRestoreInitialValues(); }
public virtual void ResetFeedback() { _playsLeft = Timing.NumberOfRepeats + 1; if (Timing.SetPlayCountToZeroOnReset) ResetPlayCount(); CustomReset(); }
```
【F:Assets/Plugins/Feel/MMFeedbacks/MMFeedbacks/Core/MMF_Player/MMF_Feedback.cs†L903-L955】

这些方法由 Player 按顺序调用，保证：
- `Stop`/`SkipToTheEnd` 终止正在播放的协程或强制跳到目标值；
- `ResetFeedback` 重置计数/内部状态；
- `RestoreInitialValues` 由具体反馈实现“退回初值”的逻辑（位置、缩放、材质等）。

## 4. 外部调用示例：用反馈控制其他 Player

`MMF_PlayerControl` 反馈展示了在一个序列中控制其他 Player 的用法（同样适用于自定义脚本直接调用）：

```csharp
// Assets/Plugins/Feel/MMFeedbacks/MMFeedbacks/Feedbacks/MMF_PlayerControl.cs
switch (Mode)
{
    case Modes.PlayFeedbacks:              foreach (var p in TargetPlayers) { p.PlayFeedbacks(position, feedbacksIntensity); } break;
    case Modes.StopFeedbacks:              foreach (var p in TargetPlayers) { p.StopFeedbacks(); } break;
    case Modes.Initialization:             foreach (var p in TargetPlayers) { p.Initialization(); } break;
    case Modes.ResetFeedbacks:             foreach (var p in TargetPlayers) { p.ResetFeedbacks(); } break;
    case Modes.RestoreInitialValues:       foreach (var p in TargetPlayers) { p.RestoreInitialValues(); } break;
    case Modes.SkipToTheEnd:               foreach (var p in TargetPlayers) { p.SkipToTheEnd(); } break;
    case Modes.ChangeDirection:            foreach (var p in TargetPlayers) { p.ChangeDirection(); } break;
    // ...其他模式同理
}
```
【F:Assets/Plugins/Feel/MMFeedbacks/MMFeedbacks/Feedbacks/MMF_PlayerControl.cs†L10-L180】

可将其视为“预览控制台”的运行时版本：同一套 API 支持面板按钮、其他 Player 内嵌反馈以及自定义工具脚本。

## 5. 预览/复位使用建议

1. **预览前**：调用 `ResetFeedbacks()` 或 `ForceInitialValues()`，确保目标值已回到初态。
2. **预览中断**：使用 `StopFeedbacks()`（必要时勾选“Stop individual feedbacks”逻辑）避免长协程继续执行。
3. **跳帧检查**：`SkipToTheEnd()` 适合查看结果态，但会强制停掉序列；跳转后如需回到初始值，再执行 `RestoreInitialValues()`。
4. **保留试验配置**：在播放模式下调整参数时启用 `Keep Play Mode Changes`，退出 Play Mode 时自动同步改动，避免手动记录。

