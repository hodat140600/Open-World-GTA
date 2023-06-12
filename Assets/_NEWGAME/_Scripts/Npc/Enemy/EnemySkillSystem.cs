using Assets._SDK.Skills;
using System.Linq;
using UnityEngine;

namespace _GAME._Scripts.Npc.SkillSystem
{
    public class EnemySkillSystem : NpcSkillSystem
    {
        public Enemy.Enemy Enemy;

        public override INpc npc => Enemy;

        //private void Start()
        //{
        //    if (skillSlots.Count == 0)
        //        InitEnemy(Enemy);
        //}

        public void InitEnemy(Enemy.Enemy Enemy)
        {
            InitAIMeleeSkill((int)Enemy.AIMeleeLevel);
        }

        protected override void InitAIMeleeSkill(int levelIndex = 1)
        {
            var slot = AttachSkillSlot(base.GetSkill<AIMeleeSkill>(), levelIndex);

            skillSlots.Add(slot);
        }
    }
}