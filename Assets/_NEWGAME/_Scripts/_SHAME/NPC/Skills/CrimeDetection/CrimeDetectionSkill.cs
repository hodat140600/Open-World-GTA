using System;
using System.Collections.Generic;
using System.Linq;
using Assets._SDK.Skills;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class CrimeDetectionSkill : AbstractSkill
{
    [SerializeField, PropertyOrder(-1)]
    private List<CrimeDetectionSkillLevel> _skillLevels;

    public override List<ISkillLevel> SkillLevels => _skillLevels?.Select(skillLevel => (ISkillLevel)skillLevel).ToList();
    public override int Id => (nameof(CrimeDetectionSkill) + Name).GetHashCode();
}