using System;
using Assets._SDK.Skills;
using Sirenix.OdinInspector;
using UnityEngine;


[Serializable]
public class BeBraveSkillLevel : AbstractSkillLevel
{
    [SerializeField, HideLabel]
    private LevelType levelType;

    public override int Index => (int)levelType;
}