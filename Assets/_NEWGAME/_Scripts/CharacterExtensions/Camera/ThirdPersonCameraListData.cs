using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace _GAME._Scripts
{
    [System.Serializable]
    public class ThirdPersonCameraListData : ScriptableObject
    {
        [SerializeField] public string Name;
        [SerializeField] public List<ThirdPersonCameraState> tpCameraStates;

        public ThirdPersonCameraListData()
        {
            tpCameraStates = new List<ThirdPersonCameraState>();
            tpCameraStates.Add(new ThirdPersonCameraState("Default"));
        }
    }

}