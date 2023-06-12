using Assets._SDK.Skills;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiveSkillSlot : AbstractSkillSlot
{
    public LiveSkill liveSkill;
    public LiveSkillLevel liveSkillLevel;
    public LiveSkillBehaviour liveSkillBehaviour;
    public override MonoBehaviour SkillBehavior => liveSkillBehaviour;

    public LiveSkillSlot(ISkill liveSkill, int levelIndex = 1) : base(liveSkill, levelIndex)
    {
        
    }

    protected override void AddBehaviorIfNull(GameObject toGameObject)
    {
        if (liveSkillBehaviour != null) return;
        liveSkillBehaviour = toGameObject.AddComponent<LiveSkillBehaviour>();
    }

    protected override void LevelUpBehavior()
    {
        liveSkillLevel = (LiveSkillLevel)SkillLevel;
        liveSkillBehaviour.LevelUp(liveSkillLevel);
    }
}
