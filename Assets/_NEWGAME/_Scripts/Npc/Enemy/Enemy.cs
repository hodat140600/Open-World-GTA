using System;
using _GAME._Scripts.Utilities;
using Assets._SDK.Skills;
using Sirenix.OdinInspector;
using UnityEditor;

namespace _GAME._Scripts.Npc.Enemy
{
    [Serializable]
    public class Enemy : Npc
    {
        [BoxGroup("Skill Levels")]
        public LevelType AIMeleeLevel;


#if UNITY_EDITOR
        [BoxGroup("Skill Levels"), Button(ButtonSizes.Large), GUIColor(0, 1, 0), HorizontalGroup("Skill Levels/bottom", .2f)]
        public void Random()
        {
            Random random = new();
            AIMeleeLevel = random.NextEnum<LevelType>();
            AssetDatabase.SaveAssets();
        }
#endif
    }
}