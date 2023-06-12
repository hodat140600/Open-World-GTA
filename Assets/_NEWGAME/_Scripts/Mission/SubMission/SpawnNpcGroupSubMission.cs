using System;
using _GAME._Scripts.Npc.Group;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;

// OUTDATED
namespace _GAME._Scripts
{
    [Serializable, Title("SPAWN NPC GROUP")]
    public class SpawnNpcGroupSubMission : SubMission
    {
        [field: SerializeField, HideLabel]
        public SpawnEnemySetting EnemySpawnSettings { get; private set; }
        
#if UNITY_EDITOR
        [OnValueChanged(nameof(UpdatePositionToThisWaypoint))]
        public string atWaypointName;

        private WaypointEditor waypointEditor;

        private void UpdatePositionToThisWaypoint()
        {
            waypointEditor ??= Object.FindObjectOfType<WaypointEditor>();
            var Waypoint = waypointEditor.MapSettings.Entity.TryGetWaypointByName(atWaypointName);

            AtPosition = Waypoint?.Position ?? Vector3.zero;
        }
#endif
        
        [field: SerializeField, ReadOnly, LabelText("At Position")]
        public Vector3 AtPosition { get; private set; }
        
        public override void Accept(MissionMonitor monitor)
        {
            monitor.ConfigureSpawnNpcAtSubMission(this);
        }

    }
}