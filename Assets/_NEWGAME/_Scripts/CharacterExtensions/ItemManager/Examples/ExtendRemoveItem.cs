using UnityEngine;

namespace _GAME._Scripts._ItemManager
{
    [ClassHeader("Remove Item", openClose = false)]
    public class ExtendRemoveItem : ExtendMonoBehaviour
    {
        public RemoveCurrentItem.Type type = RemoveCurrentItem.Type.DestroyItem;

        public bool getItemByName;
        [_Scripts.LeoHideInInspector("getItemByName")]
        public string itemName;
        [_Scripts.LeoHideInInspector("getItemByName", true)]
        public int itemID;

        /// <summary>
        /// Remove item of the target collider
        /// </summary>
        /// <param name="target">target </param>
        public void RemoveItem(Collider target)
        {
            var itemManager = target.GetComponent<ItemManager>();
            RemoveItem(itemManager);
        }

        /// <summary>
        /// Remove item of the target gameObject
        /// </summary>
        /// <param name="target">target </param>
        public void RemoveItem(GameObject target)
        {
            var itemManager = target.GetComponent<ItemManager>();
            RemoveItem(itemManager);
        }

        /// <summary>
        /// Remove item of the target <seealso cref="ItemManager"/> 
        /// </summary>
        /// <param name="target">target</param>
        public void RemoveItem(ItemManager itemManager)
        {
            if (itemManager)
            {
                var item = GetItem(itemManager);

                if (item != null)
                {
                    if (type == RemoveCurrentItem.Type.UnequipItem)
                    {
                        itemManager.UnequipItem(item);
                    }
                    else if (type == RemoveCurrentItem.Type.DestroyItem)
                    {
                        itemManager.DestroyItem(item, 1);
                    }
                    else
                    {
                        itemManager.DropItem(item, 1);
                    }
                }
            }
        }

        Item GetItem(ItemManager itemManager)
        {
            if (getItemByName)
            {
                // Check if you have an item via name (string) in your Inventory
                if (itemManager.ContainItem(itemName))
                {
                    return itemManager.GetItem(itemName);
                }
            }
            else
            {
                // Check if you have an item via ID (integer) in your Inventory
                if (itemManager.ContainItem(itemID))
                {
                    return itemManager.GetItem(itemID);
                }
            }

            return null;
        }
    }
}