using Assets._SDK.Skills;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMoveSkillSlot : AbstractSkillSlot
{
    public AIMoveSkill         AIMoveSkill;
    public AIMoveSkillLevel    AIMoveSkillLevel;
    public AIMoveSkillBehavior AIMoveSkillBehavior;
    
    public AIMoveSkillSlot(ISkill liveSkill, int levelIndex = 1) : base(liveSkill, levelIndex)
    {
    }

    public override MonoBehaviour SkillBehavior => AIMoveSkillBehavior;

    protected override void AddBehaviorIfNull(GameObject toGameObject)
    {
        if (AIMoveSkillBehavior != null) return;
        AIMoveSkillBehavior = toGameObject.AddComponent<AIMoveSkillBehavior>();
    }

    protected override void LevelUpBehavior()
    {
        AIMoveSkillLevel = (AIMoveSkillLevel)SkillLevel;
        AIMoveSkillBehavior.LevelUp(AIMoveSkillLevel);
    }
}