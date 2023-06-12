using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace _GAME._Scripts._ItemManager
{
    [ClassHeader("Check Item in Inventory", openClose = false)]
    public class CheckItemInInventory : ExtendMonoBehaviour
    {
        protected ItemManager itemManager;
        public bool getInParent = true;
        public List<CheckItemIDEvent> itemIDEvents;

        void Awake()
        {
            if (!itemManager)
            {
                if (getInParent)
                    itemManager = GetComponentInParent<ItemManager>();
                else
                    itemManager = GetComponent<ItemManager>();

                if (itemManager)
                {
                    itemManager.onAddItemID.AddListener(CheckItemExists);
                    itemManager.onRemoveItemID.AddListener(CheckItemExists);
                }
            }
        }

        public void CheckOnTrigger(Collider collider)
        {
            if (collider != null)
            {
                itemManager = collider.gameObject.GetComponent<ItemManager>();

                if (itemManager)
                {
                    for (int i = 0; i < itemIDEvents.Count; i++)
                    {
                        CheckItemIDEvent check = itemIDEvents[i];
                        CheckItemID(check);
                    }
                }
            }
        }

        private void CheckItemExists(int arg1)
        {
            for (int i = 0; i < itemIDEvents.Count; i++)
            {
                CheckItemIDEvent check = itemIDEvents[i];
                CheckItemID(check);
            }
        }

        private void CheckItemID(CheckItemIDEvent check)
        {
            if (check.Check(itemManager))
            {
                check.onContainItem.Invoke();
            }
            else
            {
                check.onNotContainItem.Invoke();
            }
        }

        [System.Serializable]
        public class CheckItemIDEvent
        {
            public string name;
            public List<int> _itemsID;
            public UnityEvent onContainItem, onNotContainItem;

            public bool Check(ItemManager itemManager)
            {
                bool _ContainItem = true;

                for (int i = 0; i < _itemsID.Count; i++)
                {
                    if (!itemManager.ContainItem(_itemsID[i]))
                    {
                        _ContainItem = false;
                        break;
                    }
                }
                return _ContainItem;
            }
        }
    }
}