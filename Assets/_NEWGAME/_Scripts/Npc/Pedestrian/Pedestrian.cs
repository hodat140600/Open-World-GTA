using System;
using _GAME._Scripts.Game;
using _GAME._Scripts.Utilities;
using Assets._SDK.Skills;
using Sirenix.OdinInspector;
using UnityEditor;

namespace _GAME._Scripts.Npc.Pedestrian
{
    [Serializable]
    public class Pedestrian : Npc
    {
        [BoxGroup("Route Settings")]
        public RouteSettings
            routeSettings;

        [BoxGroup("Route Settings")]
        public int
            startWaypoint;

        [BoxGroup("Skill Levels")]
        public LevelType
            AIMeleeLevel;

#if UNITY_EDITOR
        [BoxGroup("Route Settings"), Button(ButtonSizes.Large), GUIColor(0, 1, 0), HorizontalGroup("Route Settings/bottom", .4f)]
        public void RandomRoute()
        {
            int index = UnityEngine.Random.Range(0, GameManager.Instance.Resources.MapResources.RouteSettings.Count);
            routeSettings = GameManager.Instance.Resources.MapResources.RouteSettings[index];
            startWaypoint = UnityEngine.Random.Range(0, routeSettings.Route.waypointsIds.Count);
            AssetDatabase.SaveAssets();
        }
        [BoxGroup("Skill Levels"), Button(ButtonSizes.Large), GUIColor(0, 1, 0), HorizontalGroup("Skill Levels/bottom", .2f)]
        public void Random()
        {
            Random random = new();
            AIMeleeLevel        = random.NextEnum<LevelType>();
            AssetDatabase.SaveAssets();
        }
#endif
    }
}