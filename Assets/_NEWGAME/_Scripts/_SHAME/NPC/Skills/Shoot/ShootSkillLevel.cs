using Assets._SDK.Skills;
using Assets._SDK.Skills.Attributes;
using Assets._SDK.Skills.Enums;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[Serializable]
public class ShootSkillLevel : AbstractSkillLevel
{
    [SerializeField, LabelText("Modifier")]
    public ModifierOperator modifierOperator = ModifierOperator.Override;

    [SerializeField, LabelText("Damage")]
    public PercentAttribute damage;

    [SerializeField, HideLabel]
    private LevelType levelType;

    public override int Index => (int)levelType;
}