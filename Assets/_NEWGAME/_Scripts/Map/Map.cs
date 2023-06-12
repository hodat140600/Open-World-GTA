using System;
using System.Collections.Generic;
using System.Linq;
using _SDK.Entities;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _GAME._Scripts
{
    [Serializable, HideLabel]
    public class Map : IEntity
    {
        public int Id => (nameof(Map) + Name).GetHashCode();
        public string Name { get; set; }

        [SerializeField]
        public List<Waypoint> Waypoints;

        // public for Waypoint Editor to access to
        private Dictionary<int, Waypoint> IdToWaypoint;

        public void UpdateWaypointDictionary()
        {
            IdToWaypoint = Waypoints.ToDictionary(
                waypoint => waypoint.Id,
                waypoint => waypoint);
        }

        public void AddWaypoint(Waypoint waypoint)
        {
            IdToWaypoint.TryAdd(waypoint.Id, waypoint);
        }

        public List<Waypoint> GetWaypointsByIds(List<int> Ids)
        {
            return Ids.Select(GetWaypointById).ToList();
        }

        public Waypoint GetWaypointById(int Id)
        {
            return IdToWaypoint[Id];
        }

        public Waypoint TryGetWaypointByName(string name)
        {
            int id = (nameof(Waypoint) + name).GetHashCode();
             IdToWaypoint.TryGetValue(id, out Waypoint value);
             return value;
        }

    public bool IsWaypointsNull => Waypoints == null;
    }
}