using System.Collections.Generic;
using UnityEngine;

namespace _GAME._Scripts._ItemManager
{
    [CreateAssetMenu(menuName = "DatHQ/Inventory/New Item List")]
    public class ItemListData : ScriptableObject
    {
        public List<Item> items = new List<Item>();

        public bool inEdition;

        public bool itemsHidden = true;
    }
}
