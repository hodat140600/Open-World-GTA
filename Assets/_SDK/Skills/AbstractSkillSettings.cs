using Assets._SDK.Skills;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets._SDK.Skills
{
    public abstract class AbstractSkillSettings: ScriptableObject
    {
        public abstract AbstractSkill Skill { get; }
    }
}