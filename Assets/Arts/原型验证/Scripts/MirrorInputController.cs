using UnityEngine;
using TarodevController; // 你的 PlayerController 所在命名空间

public class MirrorInputController : MonoBehaviour
{
    [Header("引用")]
    public PlayerController mirrorPlayer; // 拖 Player_Mirror 上的 PlayerController 进来

    [Header("选项")]
    public bool invertHorizontal = true; // 是否反转水平输入

    private void Update()
    {
        // 1. 读取原始输入（和 PlayerController 那边一样）
        float h = Input.GetAxisRaw("Horizontal");
        bool jumpDown = Input.GetButtonDown("Jump");
        bool jumpHeld = Input.GetButton("Jump");

        // 2. 做镜像处理：简单版就是把左右反过来
        if (invertHorizontal) h = -h;

        // 3. 构造一个 FrameInput，塞给镜像玩家
        FrameInput fi;
        fi.Move = new Vector2(h, 0); // 只用 x，y 保持 0 即可
        fi.JumpDown = jumpDown;
        fi.JumpHeld = jumpHeld;

        // 4. 把这个输入喂给镜像玩家
        mirrorPlayer.ExternalInput = fi;
    }
}
