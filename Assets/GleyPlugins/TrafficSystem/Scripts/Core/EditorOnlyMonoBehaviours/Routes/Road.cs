﻿using System.Collections.Generic;
using UnityEngine;

namespace GleyTrafficSystem
{
    /// <summary>
    /// Stores road properties
    /// </summary>
    public class Road : MonoBehaviour
    {
        public List<Lane> lanes;
        public Path path;
        public Vector3 positionOffset;
        public Vector3 rotationOffset;
        public float laneWidth = 4;
        public float waypointDistance = 4;
        public int nrOfLanes = 2;
        public int selectedSegmentIndex = -1;
        public bool draw;
        public bool isInsidePrefab;


        public void SetRoadProperties(int globalMaxSpeed)
        {
            draw = true;
            lanes = new List<Lane>();
            for (int i = 0; i < nrOfLanes; i++)
            {
                lanes.Add(new Lane(System.Enum.GetValues(typeof(VehicleTypes)).Length, i % 2 == 0, globalMaxSpeed));
            }
        }


        public Road SetDefaults(int nrOfLanes, float laneWidth, float waypointDistance)
        {
            this.nrOfLanes = nrOfLanes;
            this.laneWidth = laneWidth;
            this.waypointDistance = waypointDistance;
            return this;
        }


        public void UpdateLaneNumber(int maxSpeed)
        {
            if (lanes.Count > nrOfLanes)
            {
                lanes.RemoveRange(nrOfLanes, lanes.Count - nrOfLanes);
            }
            if (lanes.Count < nrOfLanes)
            {
                for (int i = lanes.Count; i < nrOfLanes; i++)
                {
                    lanes.Add(new Lane(System.Enum.GetValues(typeof(VehicleTypes)).Length, i % 2 == 0, maxSpeed));
                }
            }
        }


        public Path CreatePath(Vector3 startPosition, Vector3 endPosition)
        {
            path = new Path(startPosition, endPosition);
            return path;
        }


        public void AddLaneConnector(WaypointSettings inConnector, WaypointSettings outConnector, int index)
        {
            inConnector.name = inConnector.transform.parent.parent.parent.name + "-" + inConnector.transform.parent.name + Constants.inWaypointEnding;
            outConnector.name = outConnector.transform.parent.parent.parent.name + "-" + outConnector.transform.parent.name + Constants.outWaypointEnding;
            lanes[index].laneEdges = new LaneConnectors(inConnector, outConnector);
        }


        public void SwitchDirection(int laneNumber)
        {
            AddLaneConnector(lanes[laneNumber].laneEdges.outConnector, lanes[laneNumber].laneEdges.inConnector, laneNumber);
        }


        public List<VehicleTypes> GetAllowedCars(int laneNumber)
        {
            List<VehicleTypes> result = new List<VehicleTypes>();
            for (int i = 0; i < lanes[laneNumber].allowedCars.Length; i++)
            {
                if (lanes[laneNumber].allowedCars[i] == true)
                {
                    result.Add((VehicleTypes)i);
                }
            }
            return result;
        }


        public int GetNrOfLanes()
        {
            return transform.Find(Constants.lanesHolderName).childCount;
        }
    }
}
