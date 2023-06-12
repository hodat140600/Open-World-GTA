using UnityEngine;
using System.Collections;

namespace _GAME._Scripts._ItemManager
{
    [System.Serializable]
    public class EquipProperties
    {
        public Item targetItem;
        public GameObject sender;
        public Transform targetEquipPoint;
    }
}

