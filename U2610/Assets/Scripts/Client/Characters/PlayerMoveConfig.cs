// ============================================================
// 创作者：yuanzhangfangwo
// 创作时间：2026-10-17 17:36
// 主要功能：保存玩家移动、攻击和 2.5D 摄像机的可调数值。
// 主要 API：
// MoveSpeed —— 水平移动速度
// AttackCooldown —— 两次攻击之间的最短间隔
// CameraPitch —— 摄像机俯仰角
// CameraYaw —— 摄像机水平朝向，同时决定移动的前方向
// ============================================================
using UnityEngine;

namespace U2610.Client.Characters
{
    /// <summary>玩家移动与 2.5D 摄像机的可调配置。</summary>
    [CreateAssetMenu(fileName = "SO_PlayerMove", menuName = "U2610/Player Move")]
    public class PlayerMoveConfig : ScriptableObject
    {
        [SerializeField] private float moveSpeed = 4.5f;
        [SerializeField] private float rotationSpeed = 720f;
        [SerializeField] private float gravity = -20f;
        [SerializeField] private float groundStickSpeed = 2f;
        [SerializeField] private float attackCooldown = 0.55f;
        [SerializeField] private float cameraDistance = 9f;
        [SerializeField] private float cameraPitch = 28f;
        [SerializeField] private float cameraYaw = 18f;
        [SerializeField] private float cameraLookHeight = 1.2f;
        [SerializeField] private float walkSpeedThreshold = 0.15f;

        public float MoveSpeed => moveSpeed;
        public float RotationSpeed => rotationSpeed;
        public float Gravity => gravity;
        public float GroundStickSpeed => groundStickSpeed;
        public float AttackCooldown => attackCooldown;
        public float CameraDistance => cameraDistance;
        public float CameraPitch => cameraPitch;
        public float CameraYaw => cameraYaw;
        public float CameraLookHeight => cameraLookHeight;
        public float WalkSpeedThreshold => walkSpeedThreshold;
    }
}
