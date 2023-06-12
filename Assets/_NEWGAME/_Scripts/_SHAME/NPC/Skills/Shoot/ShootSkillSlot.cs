using Assets._SDK.Skills;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootSkillSlot : AbstractSkillSlot
{
    public ShootSkill shootSkill;
    public ShootSkillLevel shootSkillLevel;
    public ShootSkillBehavior shootSkillBehavior;
    public override MonoBehaviour SkillBehavior => shootSkillBehavior;

    public ShootSkillSlot(ISkill shootSkill, int levelIndex= 1): base(shootSkill, levelIndex)
    {

    }
    protected override void AddBehaviorIfNull(GameObject toGameObject)
    {
        if (shootSkillBehavior != null) return;
        shootSkillBehavior = toGameObject.AddComponent<ShootSkillBehavior>();
    }

    protected override void LevelUpBehavior()
    {
        shootSkillLevel = (ShootSkillLevel)SkillLevel;
        shootSkillBehavior.LevelUp(shootSkillLevel);
    }
}
