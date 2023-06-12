using Assets._SDK.Skills;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeBraveSkillSlot : AbstractSkillSlot
{
    public BeBraveSkill beBraveSkill;
    public BeBraveSkillLevel beBraveSkillLevel;
    public BeBraveSkillBehavior beBraveSkillBehavior;
    public override MonoBehaviour SkillBehavior => beBraveSkillBehavior;

    public BeBraveSkillSlot (ISkill beBraveSkill, int levelIndex=(int)LevelType.Weak ) : base(beBraveSkill,levelIndex)
    {

    }

    protected override void AddBehaviorIfNull(GameObject toGameObject)
    {
        if (beBraveSkillBehavior != null) return;
        beBraveSkillBehavior = toGameObject.AddComponent<BeBraveSkillBehavior>();
    }

    protected override void LevelUpBehavior()
    {
        beBraveSkillLevel = (BeBraveSkillLevel)SkillLevel;
        beBraveSkillBehavior.LevelUp(beBraveSkillLevel);
    }
}
