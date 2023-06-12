using _GAME._Scripts._Melee;
using Assets._SDK.Skills;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMeleeSkillSlot : AbstractSkillSlot
{
    public AIMeleeSkill AIMeleeSkill;
    public AIMeleeSkillLevel AIMeleeSkillLevel;
    public AIMeleeSkillBehaviour AIMeleeSkillBehaviour;

    public override MonoBehaviour SkillBehavior => AIMeleeSkillBehaviour;

    public AIMeleeSkillSlot(ISkill liveSkill, int levelIndex = 1) : base(liveSkill, levelIndex)
    {

    }

    protected override void AddBehaviorIfNull(GameObject toGameObject)
    {
        if (AIMeleeSkillBehaviour != null) return;
        AIMeleeSkillBehaviour = toGameObject.AddComponent<AIMeleeSkillBehaviour>();
    }

    protected override void LevelUpBehavior()
    {
        AIMeleeSkillLevel = (AIMeleeSkillLevel)SkillLevel;
        AIMeleeSkillBehaviour.LevelUp(AIMeleeSkillLevel);
    }
}
