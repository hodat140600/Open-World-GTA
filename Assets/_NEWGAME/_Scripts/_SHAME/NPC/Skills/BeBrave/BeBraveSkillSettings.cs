using Assets._SDK.Skills;
using System;
using UnityEngine;

namespace _GAME._Scripts.NPC.Skill_Settings
{
    [CreateAssetMenu(menuName = "Skill/BeBrave", fileName = "BeBrave")]
    public class BeBraveSkillSettings : AbstractSkillSettings
    {
        public BeBraveSkill beBraveSkill;
        public override AbstractSkill Skill => beBraveSkill;
    }
}
