using UnityEngine;

namespace _GAME._Scripts
{
    public class RouteNavigator
    {
        private readonly Route route;
        private readonly Map   map;

        private int currentIndex;

        public Waypoint CurrentWaypoint => map.GetWaypointById(route.GetWaypointIdByIndex(currentIndex));

        public RouteNavigator(Route route, int starting, Map map)
        {
            this.route   = route;
            this.map     = map;
            currentIndex = starting;
        }

        public void ToNextWaypoint()
        {
            currentIndex = (currentIndex + 1) % route.waypointsIds.Count;
        }
    }
}