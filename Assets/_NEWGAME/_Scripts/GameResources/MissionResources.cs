using System.Collections.Generic;
using Assets._SDK.Game;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace _GAME._Scripts.Game
{
    public class MissionResources : AbstractGameResources
    {
        private const string MISSION_SETTINGS_FOLDER = "Assets/_NEWGAME/_Settings/Mission";

        public Mission GetMissionByOrder(int order) => sortedSettings[order].Mission;
        
        [SerializeField]
        private List<MissionSettings> sortedSettings;

        public int MissionCount => sortedSettings.Count;

#if UNITY_EDITOR
        private void LoadMissions()
        {
            sortedSettings = new();

            sortedSettings = base.LoadScriptableObject<MissionSettings>(MISSION_SETTINGS_FOLDER);
            sortedSettings.Sort((x, y) => x.Mission.order.CompareTo(y.Mission.order));

            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }
#endif

#if UNITY_EDITOR
        [Sirenix.OdinInspector.Button("Load Resources", ButtonSizes.Medium)]
        public void LoadResources()
        {
            LoadMissions();
            Debug.Log($"Finished. Retrieved {sortedSettings.Count} missions.");
        }
#endif
    }
}