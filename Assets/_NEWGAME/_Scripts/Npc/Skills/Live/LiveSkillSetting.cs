using Assets._SDK.Skills;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skill/LiveSkill", fileName = "LiveSkill")]
public class LiveSkillSetting : AbstractSkillSettings
{
    public LiveSkill liveSkill;
    public override AbstractSkill Skill => liveSkill;
}
