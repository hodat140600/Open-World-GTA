using System;
using System.Collections.Generic;

namespace _GAME._Scripts
{
    [Serializable]
    public class Route
    {
        public List<int> waypointsIds;

        public int GetWaypointIdByIndex(int i)
        {
            return waypointsIds[i];
        }
    }
}