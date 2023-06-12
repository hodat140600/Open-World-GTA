using Assets._SDK.Skills;
using System.Linq;
using UnityEngine;

namespace _GAME._Scripts.Npc.SkillSystem
{
    public class PedestrianSkillSystem : NpcSkillSystem
    {
        public Pedestrian.Pedestrian pedestrian;

        public override INpc npc => pedestrian;

        //private void Start()
        //{
        //    if (skillSlots.Count == 0)
        //        InitPedestrian(pedestrian);
        //}

        public void InitPedestrian(Pedestrian.Pedestrian pedestrian)
        {
            InitAIMeleeSkill((int)pedestrian.AIMeleeLevel);
        }
        protected override void InitAIMeleeSkill(int levelIndex = 1)
        {
            var slot = AttachSkillSlot(base.GetSkill<AIMeleeSkill>(), levelIndex);

            skillSlots.Add(slot);
        }
    }
}