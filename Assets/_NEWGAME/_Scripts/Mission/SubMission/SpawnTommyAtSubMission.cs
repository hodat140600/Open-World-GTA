using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;

namespace _GAME._Scripts
{
    [Serializable, Title("SPAWN TOMMY AT")]
    public class SpawnTommyAtSubMission : SubMission
    {
#if UNITY_EDITOR
        [OnValueChanged(nameof(UpdatePositionToThisWaypoint))]
        public string waypointName;
        
        private void UpdatePositionToThisWaypoint()
        {
            WaypointEditor waypointEditor = Object.FindObjectOfType<WaypointEditor>();
            if(waypointEditor == null)
            {
                Position = Vector3.zero;
                return;
            }
            
            Waypoint waypoint = waypointEditor.MapSettings.Entity.TryGetWaypointByName(waypointName);

            Position = waypoint?.Position ?? Vector3.zero;
        }
#endif

        [field: SerializeField, ReadOnly]
        public Vector3 Position { get; private set; }

        public override void Accept(MissionMonitor monitor)
        {
            monitor.ConfigureSpawnTommyAtSubMission(this);
        }
    }
}