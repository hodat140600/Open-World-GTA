using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets._SDK.Logger;
using Priority_Queue;
using UnityEngine;

namespace _GAME._Scripts
{
    public class PathfinderUtilities
    {
        private static SimplePriorityQueue<Waypoint, float> openSet = new();
        private static Dictionary<Waypoint, float>          gScores = new();
        private static Dictionary<Waypoint, Waypoint>       path    = new();

        public static IEnumerator FindPath(Func<List<int>, List<Waypoint>> IdToWaypoints, Waypoint start, Waypoint end, Action<List<Waypoint>> result)
        {
            openSet.Clear();
            openSet.Enqueue(start, 0);

            gScores.Clear();
            gScores.Add(start, 0);

            path.Clear();
            path.Add(start, null);

            Waypoint current;
            float    fScore;

            List<Waypoint> pathList = new();

            while (openSet.TryDequeue(out current))
            {
                if (current.Name == end.Name)
                {
                    // foreach (KeyValuePair<Waypoint, Waypoint> w in path)
                    // {
                    //     Debug.Log($"KEY: {w.Key.Name}, VALUE: {w.Value?.Name}");
                    // }

                    while (current != null)
                    {
                        pathList.Add(current);
                        current = path[current];
                    }

                    pathList.Reverse();
                    result(pathList);
                    yield break;
                }

                float gCurrent = gScores[current];

                foreach (Waypoint neighbour in IdToWaypoints(current.neighbourIds))
                {
                    // Debug.Log($"{current.Name} => {neighbour.Name}");
                    gScores.TryAdd(neighbour, float.MaxValue);
                    float gNeighbour = gCurrent + Distance(current, neighbour);

                    if (gNeighbour < gScores[neighbour])
                    {
                        gScores[neighbour] = gNeighbour;
                        path[neighbour]    = current;
                        fScore             = gNeighbour + hEval(neighbour, end);
                        openSet.Enqueue(neighbour, fScore);
                    }
                }
            }

            yield return null;
            result(pathList);
        }

        private static float hEval(Waypoint neighbour, Waypoint end)
        {
            return Distance(neighbour, end);
        }

        private static float Distance(Waypoint a, Waypoint b)
        {
            return Vector3.Distance(a.Position, b.Position);
        }

// #if UNITY_EDITOR
//         public static void GetNeighboursFromRoutes(
//             Dictionary<int, Waypoint> IdToWaypoint, List<Route> routes, List<Waypoint> waypoints)
//         {
//             foreach (Waypoint waypoint in waypoints)
//             {
//                 waypoint.RenewNeighbours();
//             }
//
//             foreach (List<Waypoint> waypointsInRoute in routes.Select(route => route.waypoints))
//             {
//                 for (int i = 0; i < waypointsInRoute.Count - 1; i++)
//                 {
//                     var currId = waypointsInRoute[i].Id;
//                     var nextId = waypointsInRoute[i + 1].Id;
//
//                     var currWaypoint = IdToWaypoint[currId];
//                     var nextWaypoint = IdToWaypoint[nextId];
//
//                     currWaypoint.neighbourIds.Add(nextId);
//                     nextWaypoint.neighbourIds.Add(currId);
//                 }
//             }
//
//             foreach (Waypoint waypoint in waypoints)
//             {
//                 waypoint.RemoveDuplicatedNeighbours();
//             }
//         }
// #endif
    }
}