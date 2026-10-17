// ============================================================
// 创作者：yuanzhangfangwo
// 创作时间：2026-10-17 17:36
// 主要功能：缓存玩家 Animator 参数哈希，避免在运行时反复传字符串。
// 主要 API：
// Speed —— 水平移动速度，驱动 Idle / Walk / Run
// Attack —— 轻攻击触发器
// ============================================================
using UnityEngine;

namespace U2610.Tool
{
    /// <summary>玩家 Animator 参数名的哈希缓存。</summary>
    public static class AnimatorParams
    {
        public static readonly int Speed = Animator.StringToHash("Speed");
        public static readonly int Attack = Animator.StringToHash("Attack");
    }
}
