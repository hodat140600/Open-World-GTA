using System;
using Assets._SDK.Skills;
using Sirenix.OdinInspector;
using UnityEngine;

[EnumToggleButtons]
public enum SpeedType
{
    VerySlow = 0,
    Slow     = 1,
    Normal   = 2,
    Fast     = 3,
    VeryFast = 4,
    None = 5
}

[Serializable]
public class AIMoveSkillLevel : AbstractSkillLevel
{
    [SerializeField]
    public float Speed;

    [field: SerializeField, HideLabel]
    public SpeedType speedType { get; private set; }

    public override int Index => (int)speedType;
}