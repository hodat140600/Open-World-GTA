using System;
using System.Collections.Generic;
using System.Linq;
using _GAME._Scripts.Game;
using Assets._SDK.Skills;
using UnityEngine;

namespace _GAME._Scripts.Npc.SkillSystem
{
    public abstract class NpcSkillSystem : AbstractSkillSystem
    {
        public abstract INpc npc { get; }

        protected List<AbstractSkillSlot> skillSlots;
        protected override ISkillSlotFactory SkillSlotFactory { get; set; }

        private void Awake()
        {
            SkillSlotFactory = new SkillSlotFactory();
            skillSlots       = new List<AbstractSkillSlot>();
        }

        protected ISkill GetSkill<T>() where T : AbstractSkill
        {
            var skill = GameManager.Instance.Resources.NpcResources.npcSkillSettings.Where(skill => skill.Skill is T).ToList()[0].Skill;
            return skill;
        }


        protected virtual void InitAIMeleeSkill(int levelIndex = 1)
        {
            var slot = AttachSkillSlot(GetSkill<AIMeleeSkill>(), levelIndex);

            skillSlots.Add(slot);
        }

        public virtual void UpdateSkillSlot<TSkillSlot>() where TSkillSlot : AbstractSkillSlot
        {
            TSkillSlot skillSlot = GetSkillSlot<TSkillSlot>();
            skillSlot.LevelUp(this.gameObject);
        }

        public virtual void GetSkillBehaviour<TSkillSlot, TSkillBehaviour>(out TSkillBehaviour skillBehaviour) where TSkillSlot : AbstractSkillSlot where TSkillBehaviour : MonoBehaviour
        {
            skillBehaviour = (TSkillBehaviour)GetSkillSlot<TSkillSlot>().SkillBehavior;
        }
        public virtual TSkillSlot GetSkillSlot<TSkillSlot>() where TSkillSlot : AbstractSkillSlot
        {
            var skillSlot = (TSkillSlot)skillSlots.FirstOrDefault(skillSlot => skillSlot is TSkillSlot);
            return skillSlot;
        }
    }
}