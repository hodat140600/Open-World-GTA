using System;
using System.Collections.Generic;
using System.Linq;
using MyBox;
using UnityEngine;

namespace _GAME._Scripts
{
    [ExecuteInEditMode]
    public class WaypointOnEditor : MonoBehaviour
    {
        [field: SerializeField, MyBox.ReadOnly] public Waypoint ToWaypoint { get; private set; }

        public void InitWaypoint()
        {
            ToWaypoint = new Waypoint(gameObject.name, transform.position, new List<int>());
        }

        // lock position once created
        private void OnValidate()
        {
            transform.position = ToWaypoint.Position;
        }

        public void AddNeighbour(WaypointOnEditor neighbour)
        {
            ToWaypoint.neighbourIds.Add(neighbour.ToWaypoint.Id);
            ToWaypoint.neighbourIds =
                ToWaypoint.neighbourIds
                    .Distinct()
                    .Where(id => ToWaypoint.Id != id)
                    .ToList();
        }

        public void RemoveNeighbour(WaypointOnEditor neighbour)
        {
            ToWaypoint.neighbourIds.Remove(neighbour.ToWaypoint.Id);
        }
    }
}