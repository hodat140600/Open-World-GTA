using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace _GAME._Scripts._ItemManager
{
    [ClassHeader("Check If Item Is Equipped", openClose = false)]
    public class CheckItemIsEquipped : ExtendMonoBehaviour
    {
        public ItemManager itemManager;
        public bool getInParent = true;
        [FormerlySerializedAs("itemChecks")]
        public List<CheckItemIDEvent> itemIDEvents;
        public List<CheckItemTypeEvent> itemTypeEvents;

        void Awake()
        {
            if (!itemManager)
            {
                if (getInParent)
                    itemManager = GetComponentInParent<ItemManager>();
                else
                    itemManager = GetComponent<ItemManager>();
                itemManager.onEquipItem.AddListener(CheckIsEquipped);
                itemManager.onUnequipItem.AddListener(CheckIsEquipped);
            }
        }

        private void CheckIsEquipped(EquipArea arg0, Item arg1)
        {
            for (int i = 0; i < itemIDEvents.Count; i++)
            {
                CheckItemIDEvent check = itemIDEvents[i];
                CheckItemID(check);
            }
            for (int i = 0; i < itemTypeEvents.Count; i++)
            {
                CheckItemTypeEvent check = itemTypeEvents[i];
                CheckItemType(check);
            }
        }

        private void CheckItemID(CheckItemIDEvent check)
        {
            bool _isEquipped = check._itemsID.Exists(t => itemManager.ItemIsEquipped(t));

            if (_isEquipped != check.isEquipped)
            {
                check.isEquipped = _isEquipped;
                if (check.isEquipped)
                    check.onIsItemEquipped.Invoke();
                else
                    check.onIsItemUnequipped.Invoke();
            }
        }

        private void CheckItemType(CheckItemTypeEvent check)
        {
            bool _isEquipped = check.itemTypes.Exists(t => itemManager.ItemTypeIsEquipped(t));
            if (_isEquipped != check.isEquipped)
            {
                check.isEquipped = _isEquipped;
                if (check.isEquipped)
                    check.onIsItemEquipped.Invoke();
                else
                    check.onIsItemUnequipped.Invoke();
            }
        }

        [System.Serializable]
        public class CheckItemIDEvent
        {
            public string name;
            public List<int> _itemsID;
            public UnityEvent onIsItemEquipped, onIsItemUnequipped;
            internal bool isEquipped;
        }

        [System.Serializable]
        public class CheckItemTypeEvent
        {
            public string name;
            public List<ItemType> itemTypes;
            public UnityEvent onIsItemEquipped, onIsItemUnequipped;
            internal bool isEquipped;
        }

    }
}