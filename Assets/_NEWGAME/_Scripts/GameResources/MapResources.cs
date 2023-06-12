using Assets._SDK.Game;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace _GAME._Scripts.Game
{
    public class MapResources : AbstractGameResources
    {
        private const string MAP_SETTINGS_FOLDER = "Assets/_NEWGAME/_Settings/Map";
        private const string ROUTE_SETTINGS_FOLDER = "Assets/_NEWGAME/_Settings/Route";

        [SerializeField] private List<MapSettings> mapSettings;
        [SerializeField] private List<RouteSettings> routeSettings;

        public List<RouteSettings> RouteSettings { get => routeSettings;private set => routeSettings = value; }

        //public Map Map => mapSettings.Entity;

#if UNITY_EDITOR
        private void LoadRouteSettings()
        {
            RouteSettings = LoadScriptableObject<RouteSettings>(ROUTE_SETTINGS_FOLDER);
        }
        private void LoadMapSettings()
        {
            mapSettings = LoadScriptableObject<MapSettings>(MAP_SETTINGS_FOLDER);
        }

        [Button("Load Resources", ButtonSizes.Medium)]
        void LoadMapResources()
        {
            LoadMapSettings();
            LoadRouteSettings();
            Debug.Log($"Finished. Retrieved:" +
                      $"\n    {mapSettings.Count} mapSettings" +
                      $"\n    {RouteSettings.Count} routeSettings");
        }
#endif
    }
}