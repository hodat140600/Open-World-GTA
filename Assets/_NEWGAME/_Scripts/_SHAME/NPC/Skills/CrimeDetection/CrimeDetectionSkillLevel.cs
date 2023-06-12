using Assets._SDK.Skills;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CrimeDetectionSkillLevel : AbstractSkillLevel
{
    [SerializeField, LabelText("Detection Range")]
    public float range;

    [SerializeField, HideLabel]
    private LevelType levelType;

    public override int Index => (int)levelType;
}
