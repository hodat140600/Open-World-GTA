
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

namespace Assets._SDK.Skills
{
    [EnumToggleButtons]
    public enum LevelType
    {
        NPC    = 0,
        Weak   = 1,
        Medium = 2,
        Strong = 3,
    }
    
    public abstract class AbstractSkillLevel : ISkillLevel
    {
        public abstract int Index { get; }
    }
}