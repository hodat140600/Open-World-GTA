using System;
using System.Collections;
using System.Collections.Generic;
using Assets._SDK.Game;
using Assets._SDK.Logger;
using GleyTrafficSystem;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ApplyTrafficPanel : GameSingleton<ApplyTrafficPanel>
{
    [SerializeField] private TrafficComponent trafficComponent;

    [SerializeField] private TMP_InputField minDistanceToAdd, maxDistanceToRemove, maxCount;


    // private void Awake()
    // {
    //     if (GameDriver.Instance.FirstTime) return;
    //
    //     maxCount.text            = GameDriver.Instance.MaxCount.ToString();
    //     minDistanceToAdd.text    = GameDriver.Instance.MinD.ToString();
    //     maxDistanceToRemove.text = GameDriver.Instance.MaxD.ToString();
    // }
    //
    // public void Apply()
    // {
    //     //  var vehicleList = Manager.GetVehicleList();
    //     //  
    //     //  vehicleList.Count.LogPrefix("car count before removal");
    //     //  // foreach (VehicleComponent vehicleComponent in vehicleList)
    //     //  // {
    //     //  //     Manager.RemoveVehicle(vehicleComponent.gameObject);
    //     //  // }
    //     // // trafficComponent.
    //     // //  Manager.ClearTrafficOnArea(Vector3.zero, 10000);
    //     //  vehicleList.Count.LogPrefix("car count after removal");
    //     // trafficComponent.minDistanceToAdd = MinD;
    //     // trafficComponent.distanceToRemove = MaxD;
    //     // trafficComponent.nrOfVehicles     = MaxCount;
    //     GameDriver.Instance.MaxCount  = int.Parse(maxCount.text);
    //     GameDriver.Instance.MinD      = float.Parse(minDistanceToAdd.text);
    //     GameDriver.Instance.MaxD      = float.Parse(maxDistanceToRemove.text);
    //     GameDriver.Instance.FirstTime = false;
    //     SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    //     // Manager.Initialize(
    //     //     trafficComponent.player,
    //     //     MaxCount,
    //     //     trafficComponent.vehiclePool,
    //     //     MinD,
    //     //     MaxD,
    //     //     -1,
    //     //     -1);
    // }
}