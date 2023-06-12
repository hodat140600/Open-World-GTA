using _GAME._Scripts._CharacterController._AI;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class MappingDataToWaypointSystem : MonoBehaviour
{
    public _GAME._Scripts.MapSettings mapSettings;

    public WaypointArea pathArea;


#if UNITY_EDITOR
    [Button("MappingMapSettings", ButtonSizes.Large)]
    void MappingDataMapSettings()
    {
        if (!pathArea) return;
        CheckNodes(ref pathArea.waypoints);
        foreach(var waypoint in mapSettings.Entity.Waypoints)
        {
            CreateNode(pathArea, waypoint.Position, waypoint.Id);
            EditorUtility.SetDirty(pathArea);
        }
    }
    void CheckNodes(ref List<Waypoint> waypoints)
    {
        var wP = ((WaypointArea)pathArea).transform.GetComponentsInChildren<Waypoint>();

        if (waypoints.Count != wP.Length)
            waypoints = wP.ToList();
        foreach (Waypoint waypoint in waypoints)
        {
            var vP = waypoint.transform.GetComponentsInChildren<_GAME._Scripts.Point>();
            var _vp = vP.ToList().FindAll(p => p.transform != waypoint.transform);
            if (waypoint.subPoints.Count != _vp.Count)
                waypoint.subPoints = _vp;
        }
    }
    void CreateNode(WaypointArea wayArea, Vector3 position, int id)
    {
        var nodeObj = new GameObject("node");
        var node = nodeObj.AddComponent<Waypoint>();
        node.subPoints = new List<_GAME._Scripts.Point>();
        node.SetIdMappingData(id);
        nodeObj.transform.position = position;
        nodeObj.transform.parent = wayArea.transform;
        wayArea.waypoints.Add(node);
        //currentNode = node;
        //indexOfWaypoint = wayArea.waypoints.IndexOf(currentNode);
    }
#endif
}
