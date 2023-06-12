using System;
using _GAME._Scripts.Utilities;
using _GAME._Scripts.Npc;
using Assets._SDK.Skills;
using Sirenix.OdinInspector;

[Serializable]
public class Police : Npc
{
    [BoxGroup("Skill Levels")]
    public SpeedType AIMoveLevel;
    
    [BoxGroup("Skill Levels")]
    public LevelType BeBraveLevel,
        CrimeDetectionLevel,
        LiveLevel,
        PunchLevel,
        ShootLevel;

#if UNITY_EDITOR
    [BoxGroup("Skill Levels"), Button(ButtonSizes.Large), GUIColor(0, 1, 0), HorizontalGroup("Skill Levels/bottom", .2f)]
    public void Randomize()
    {
        Random random = new();
        AIMoveLevel         = random.NextEnum<SpeedType>(1);
        BeBraveLevel        = random.NextEnum<LevelType>();
        CrimeDetectionLevel = random.NextEnum<LevelType>();
        LiveLevel           = random.NextEnum<LevelType>();
        PunchLevel          = random.NextEnum<LevelType>();
        ShootLevel          = random.NextEnum<LevelType>();
    }
#endif
}