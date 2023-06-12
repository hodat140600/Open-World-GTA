using Assets._SDK.Skills;
using Assets._SDK.Skills.Attributes;
using Assets._SDK.Skills.Enums;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LiveSkillLevel : AbstractSkillLevel
{
    [SerializeField, LabelText("Max Health")]
    public int maxHealth;

    [SerializeField, HideLabel]
    private LevelType levelType;

    public override int Index => (int)levelType;
}
