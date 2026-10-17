// ============================================================
// 创作者：yuanzhangfangwo
// 创作时间：2026-10-17 17:36
// 主要功能：用固定俯仰和水平角跟随玩家，形成 2.5D 战斗大厅视角。
// 主要 API：
// 无额外公开方法，在 LateUpdate 中把摄像机摆到配置指定的位置和朝向。
// ============================================================
using U2610.Client.Characters;
using UnityEngine;

namespace U2610.Client.CameraRig
{
    /// <summary>保持固定 2.5D 机位，并在角色移动后跟随。</summary>
    public class PlayerCamera : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private PlayerMoveConfig moveConfig;

        /// <summary>在角色完成本帧移动后，把摄像机放到固定偏移上。</summary>
        private void LateUpdate()
        {
            Vector3 lookPoint;
            Vector3 offset;
            Quaternion rotation;

            if (target == null || moveConfig == null)
            {
                return;
            }

            rotation = Quaternion.Euler(moveConfig.CameraPitch, moveConfig.CameraYaw, 0f);
            offset = rotation * (Vector3.back * moveConfig.CameraDistance);
            lookPoint = target.position + Vector3.up * moveConfig.CameraLookHeight;
            transform.SetPositionAndRotation(lookPoint + offset, rotation);
        }
    }
}
