using Assets._SDK.Skills;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Skill/AIMeleeSkill", fileName = "AIMeleeSkill")]
public class AIMeleeSkillSettings : AbstractSkillSettings
{
    public AIMeleeSkill AIMeleeSkill;
    public override AbstractSkill Skill => AIMeleeSkill;

#if UNITY_EDITOR
    [Button("SAVE SETTINGS", ButtonSizes.Large)]
    public void SaveSettings()
    {
        AssetDatabase.SaveAssets();
    }
#endif
}
