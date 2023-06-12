using Assets._SDK.Skills;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skill/ShootSkill", fileName = "Shoot")]
public class ShootSkillSetting : AbstractSkillSettings
{
    public ShootSkill shootSkill;
    public override AbstractSkill Skill => shootSkill;
}
