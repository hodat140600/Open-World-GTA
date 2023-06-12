using Assets._SDK.Skills;
using UnityEngine;

[CreateAssetMenu(menuName= "Skill/CrimeDetection", fileName = "CrimeDetection")]
public class CrimeDetectionSkillSetting : AbstractSkillSettings
{
    public CrimeDetectionSkill crimeDetectionSkill;
    public override AbstractSkill Skill => crimeDetectionSkill;
}
