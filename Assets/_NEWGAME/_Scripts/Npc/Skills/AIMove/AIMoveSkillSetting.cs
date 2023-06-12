using Assets._SDK.Skills;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skill/AIMoveSkillSetting", fileName = "aiMove")]
public class AIMoveSkillSetting : AbstractSkillSettings
{
    public AIMoveSkill AIMoveSkill;
    public override AbstractSkill Skill => AIMoveSkill;
}
