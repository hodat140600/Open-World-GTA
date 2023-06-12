using Assets._SDK.Skills;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class AIMeleeSkill : AbstractSkill
{
    [SerializeField, PropertyOrder(-1)]
    private List<AIMeleeSkillLevel> _AIMeleeSkillLevel;
    public override List<ISkillLevel> SkillLevels => _AIMeleeSkillLevel?.Select(skillLevel => (ISkillLevel)skillLevel).ToList();

    public override int Id => (nameof(AIMeleeSkill) + Name).GetHashCode();
}
