using Assets._SDK.Skills;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class ShootSkill : AbstractSkill
{
    [SerializeField, PropertyOrder(-1)]
    private List<ShootSkillLevel> _liveSkillLevel;

    public override List<ISkillLevel> SkillLevels => _liveSkillLevel?.Select(skillLevel => (ISkillLevel)skillLevel).ToList();
    public override int Id => (nameof(ShootSkill) + Name).GetHashCode();
}
