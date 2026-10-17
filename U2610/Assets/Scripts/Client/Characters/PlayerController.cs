// ============================================================
// 创作者：yuanzhangfangwo
// 创作时间：2026-10-17 17:36
// 主要功能：读取 Input System，在战斗大厅地面上移动女性角色并播放攻击动画。
// 主要 API：
// PlanarSpeed —— 当前水平速度大小，供调试或其他表现读取
// ============================================================
using U2610.Tool;
using UnityEngine;
using UnityEngine.InputSystem;

namespace U2610.Client.Characters
{
    /// <summary>在固定 2.5D 朝向下移动角色，并按冷却触发攻击动画。</summary>
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerMoveConfig moveConfig;
        [SerializeField] private InputActionAsset inputActions;
        [SerializeField] private Animator animator;

        private CharacterController characterController;
        private InputAction moveAction;
        private InputAction attackAction;
        private float verticalSpeed;
        private float nextAttackTime;
        private bool missingReferenceWarned;

        public float PlanarSpeed { get; private set; }

        /// <summary>缓存控制器、动画器和输入动作。</summary>
        private void Awake()
        {
            InputActionMap playerMap;

            characterController = GetComponent<CharacterController>();
            if (animator == null)
            {
                animator = GetComponentInChildren<Animator>();
            }

            if (inputActions == null || moveConfig == null)
            {
                return;
            }

            playerMap = inputActions.FindActionMap(InputActionNames.PlayerMap, false);
            if (playerMap == null)
            {
                return;
            }

            moveAction = playerMap.FindAction(InputActionNames.Move, false);
            attackAction = playerMap.FindAction(InputActionNames.Attack, false);
        }

        /// <summary>启用移动和攻击输入。</summary>
        private void OnEnable()
        {
            if (moveAction != null)
            {
                moveAction.Enable();
            }

            if (attackAction != null)
            {
                attackAction.Enable();
            }
        }

        /// <summary>关闭移动和攻击输入。</summary>
        private void OnDisable()
        {
            if (moveAction != null)
            {
                moveAction.Disable();
            }

            if (attackAction != null)
            {
                attackAction.Disable();
            }
        }

        /// <summary>按摄像机水平朝向移动，落地时施加粘地速度，并在冷却结束后攻击。</summary>
        private void Update()
        {
            Vector2 moveInput;
            Vector3 wishDirection;
            Vector3 motion;
            Quaternion facing;
            float deltaTime;
            bool attackPressed;

            if (!HasRequiredReferences())
            {
                return;
            }

            deltaTime = Time.deltaTime;
            moveInput = moveAction.ReadValue<Vector2>();
            wishDirection = FlattenToGround(moveInput);
            PlanarSpeed = wishDirection.magnitude * moveConfig.MoveSpeed;

            if (characterController.isGrounded && verticalSpeed < 0f)
            {
                verticalSpeed = -moveConfig.GroundStickSpeed;
            }
            else
            {
                verticalSpeed += moveConfig.Gravity * deltaTime;
            }

            motion = wishDirection * moveConfig.MoveSpeed;
            motion.y = verticalSpeed;
            characterController.Move(motion * deltaTime);

            if (wishDirection.sqrMagnitude > 0f)
            {
                facing = Quaternion.LookRotation(wishDirection, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(
                    transform.rotation,
                    facing,
                    moveConfig.RotationSpeed * deltaTime);
            }

            if (animator != null)
            {
                animator.SetFloat(AnimatorParams.Speed, PlanarSpeed);
            }

            attackPressed = attackAction != null && attackAction.WasPressedThisFrame();
            if (attackPressed && Time.time >= nextAttackTime)
            {
                nextAttackTime = Time.time + moveConfig.AttackCooldown;
                if (animator != null)
                {
                    animator.SetTrigger(AnimatorParams.Attack);
                }
            }
        }

        /// <summary>把摇杆输入转到大厅地面，并限制在单位长度内。</summary>
        /// <param name="moveInput">输入设备上的二维移动值。</param>
        /// <returns>世界空间中的水平方向，长度不超过 1。</returns>
        private Vector3 FlattenToGround(Vector2 moveInput)
        {
            Quaternion yawRotation;
            Vector3 forward;
            Vector3 right;
            Vector3 wishDirection;

            yawRotation = Quaternion.Euler(0f, moveConfig.CameraYaw, 0f);
            forward = yawRotation * Vector3.forward;
            right = yawRotation * Vector3.right;
            wishDirection = right * moveInput.x + forward * moveInput.y;
            if (wishDirection.sqrMagnitude > 1f)
            {
                wishDirection.Normalize();
            }

            return wishDirection;
        }

        /// <summary>确认配置和输入都已指定，缺失时只警告一次。</summary>
        /// <returns>可以继续更新时返回 true。</returns>
        private bool HasRequiredReferences()
        {
            bool ready;

            ready = moveConfig != null && moveAction != null;
            if (!ready && !missingReferenceWarned)
            {
                missingReferenceWarned = true;
                Debug.LogWarning("PlayerController 缺少移动配置或 Move 输入动作。", this);
            }

            return ready;
        }
    }
}
