using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace _GAME._Scripts.Npc.Group
{
    [CreateAssetMenu(fileName = "NPCGroup", menuName = "NPCGroup")]
    public class NpcGroupSettings : ScriptableObject
    {
#if UNITY_EDITOR
        private static WaypointEditor _editor;

        [OnValueChanged(nameof(UpdateGroup))]
        [SerializeField]
        private List<NpcSettings> npcSettings;

        [Button("SAVE", ButtonSizes.Large), GUIColor(0, .6f, 1)]
        private void UpdateGroup()
        {
            npcGroup.Name     = Path.GetFileNameWithoutExtension(AssetDatabase.GetAssetPath(this));
            npcGroup.npcIds   = npcSettings == null ? new List<int>() : npcSettings.Select(settings => settings.Npc.Id).ToList();
            npcGroup.leaderId = npcGroup.npcIds.FirstOrDefault();
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssetIfDirty(this);
        }
        
        [OnValueChanged(nameof(UpdateStartingWaypointPosition)), PropertyOrder(-1)]
        public string startingWaypointName;

        private void UpdateStartingWaypointPosition()
        {
            _editor ??= FindObjectOfType<WaypointEditor>();
            var Waypoint = _editor.MapSettings.Entity.TryGetWaypointByName(startingWaypointName);

            npcGroup.startingWaypointPosition = Waypoint?.Position ?? Vector3.zero;
        }
#endif

        [SerializeField]
        public NpcGroup npcGroup;
    }
}