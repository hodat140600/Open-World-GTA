using Assets._SDK.Skills;

namespace _GAME._Scripts.Npc.SkillSystem
{
    public class SkillSlotFactory : ISkillSlotFactory
    {
        public AbstractSkillSlot CreateSkillSlotFor(ISkill skill)
        {
            return skill switch
            {
                BeBraveSkill => new BeBraveSkillSlot(skill, skill.SkillLevels[0].Index),
                AIMeleeSkill => new AIMeleeSkillSlot(skill),
                // LiveSkill    => new LiveSkillSlot(skill),
                AIMoveSkill  => new AIMoveSkillSlot(skill),
                ShootSkill   => new ShootSkillSlot(skill),
                // PunchSkill   => new PunchSkillSlot(skill),
                _            => null
            };
        }
    }
}