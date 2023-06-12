using System.Collections.Generic;
using _GAME._Scripts.Game;
using _GAME._Scripts.Npc.Group;
using Assets._SDK.Game;
using Sirenix.OdinInspector;
using UnityEngine;
using System.Linq;
using _GAME._Scripts.Npc;

namespace _GAME._Scripts.GameResources
{
    public class NpcGroupResource : AbstractGameResources
    {
        private const string NPC_GROUP_SETTING_DATA_FOLDER = "Assets/_NEWGAME/_Settings/Npc/Group/NPC";
        private const string NETBOX_GROUP_SETTING_DATA_FOLDER = "Assets/_NEWGAME/_Settings/Npc/Group/Mission";
        private const string TUTORIAL_GROUP_SETTING_DATA_FOLDER = "Assets/_NEWGAME/_Settings/Npc/Group/Tutorial";
        public List<NpcGroupSettings> npcGroupSettings;
        public List<NpcGroupSettings> netboxGroupSettings;
        public List<NpcGroupSettings> tutorialGroupSettings;
        public NpcResources NpcResources => GameManager.Instance.Resources.NpcResources;

#if UNITY_EDITOR
        public void LoadNPCGroupData()
        {
            npcGroupSettings = base.LoadScriptableObject<NpcGroupSettings>(NPC_GROUP_SETTING_DATA_FOLDER);
            netboxGroupSettings = base.LoadScriptableObject<NpcGroupSettings>(NETBOX_GROUP_SETTING_DATA_FOLDER);
            tutorialGroupSettings = base.LoadScriptableObject<NpcGroupSettings>(TUTORIAL_GROUP_SETTING_DATA_FOLDER);
        }

        [Sirenix.OdinInspector.Button("Load Resources", ButtonSizes.Medium)]
        public void LoadNPCGroupResources()
        {
            LoadNPCGroupData();
            Debug.Log($"Finished. Retrieved {(npcGroupSettings.Count)} NPCGroup, ");
        }
#endif
    }
}

