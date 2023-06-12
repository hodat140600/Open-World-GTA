using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace _GAME._Scripts._ItemManager
{
    [System.Serializable]
    public class AmmoListData : ScriptableObject
    {
        [HelpBox("Leave the Count value at 0 if you want to use ammo from the Inventory Attribute", HelpBoxAttribute.MessageType.Info)]
        public List<ItemListData> itemListDatas;
        [@HideInInspector]
        public List<Ammo> ammos = new List<Ammo>();
    }
}

