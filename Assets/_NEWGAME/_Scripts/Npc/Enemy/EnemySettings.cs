using Sirenix.OdinInspector;
using UnityEngine;

namespace _GAME._Scripts.Npc.Enemy
{
    [CreateAssetMenu(fileName = "Enemy", menuName = "NPC/Enemy")]
    public class EnemySettings : NpcSettings
    {
        [HideLabel]
        public Enemy Enemy;
        public override INpc Npc => Enemy;
    }
}
