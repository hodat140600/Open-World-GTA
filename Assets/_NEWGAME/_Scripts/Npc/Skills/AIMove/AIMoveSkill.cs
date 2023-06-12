using System;
using System.Collections.Generic;
using System.Linq;
using Assets._SDK.Skills;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class AIMoveSkill : AbstractSkill
{
    [SerializeField, PropertyOrder(-1)]
    private List<AIMoveSkillLevel> _skillLevels;
    public override List<ISkillLevel> SkillLevels => _skillLevels?.Select(skillLevel => (ISkillLevel)skillLevel).ToList();
    public override int Id => (nameof(AIMoveSkill) + Name).GetHashCode();
}