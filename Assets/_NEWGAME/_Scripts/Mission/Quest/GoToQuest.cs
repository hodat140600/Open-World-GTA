using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;

namespace _GAME._Scripts
{
    [Serializable, Title("GO TO")]
    public class GoToQuest : SubMission
    {
#if UNITY_EDITOR
        [OnValueChanged(nameof(UpdatePositionToThisWaypoint))]
        public string waypointName;

        private WaypointEditor waypointEditor;

        private void UpdatePositionToThisWaypoint()
        {
            waypointEditor ??= Object.FindObjectOfType<WaypointEditor>();
            var Waypoint = waypointEditor.MapSettings.Entity.TryGetWaypointByName(waypointName);

            Position = Waypoint?.Position ?? Vector3.zero;
        }
#endif

        [field: SerializeField, ReadOnly]
        public Vector3 Position { get; private set; }

        protected override string DefaultQuestDescription => $"Go to Mission";
        public override void Accept(MissionMonitor monitor)
        {
            monitor.ConfigureGoToSubMission(this);
        }
    }
}