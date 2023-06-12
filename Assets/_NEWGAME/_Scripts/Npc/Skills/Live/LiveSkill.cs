using System;
using System.Collections.Generic;
using System.Linq;
using Assets._SDK.Skills;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class LiveSkill : AbstractSkill
{
    [SerializeField, PropertyOrder(-1)]
    private List<LiveSkillLevel> _liveSkillLevel;

    public override List<ISkillLevel> SkillLevels => _liveSkillLevel?.Select(skillLevel => (ISkillLevel)skillLevel).ToList();
    public override int Id => (nameof(LiveSkill) + Name).GetHashCode();
}