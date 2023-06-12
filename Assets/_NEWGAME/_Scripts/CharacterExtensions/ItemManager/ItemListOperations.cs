using System.Collections.Generic;
using UnityEngine;
namespace _GAME._Scripts._ItemManager
{
    public static class ItemListOperations
    {
        public static List<Item> GetSameItems(this List<Item> itemList, int id)
        {
            return itemList.FindAll(i => i.id.Equals(id));
        }

        public static List<Item> GetSameItems(this List<Item> itemList, string name)
        {
            return itemList.FindAll(i => i.name.Equals(name));
        }

        public static List<Item> GetSameItems(this List<Item> itemList, params int[] ids)
        {
            return itemList.FindAll(i => System.Array.Exists(ids, id => i.id.Equals(id)));
        }

        public static List<Item> GetSameItems(this List<Item> itemList, params string[] names)
        {
            return itemList.FindAll(i => System.Array.Exists(names, name => i.name.Equals(name)));
        }

        public static bool HasItem(this List<Item> itemList, int id)
        {
            return itemList.Exists(i => i.id.Equals(id));
        }

        public static bool HasItem(this List<Item> itemList, string name)
        {
            return itemList.Exists(i => i.name.Equals(name));
        }

        public static bool HasItems(this List<Item> itemList, params int[] ids)
        {
            bool has = true;
            for (int i = 0; i < ids.Length; i++)
            {
                if (!itemList.HasItem(ids[i]))
                {
                    has = false;
                    break;
                }
            }
            return has;
        }

        public static bool HasItems(this List<Item> itemList, params string[] names)
        {
            bool has = true;
            for (int i = 0; i < names.Length; i++)
            {
                if (!itemList.HasItem(names[i]))
                {
                    has = false;
                    break;
                }
            }
            return has;
        }

        public static int GetItemCount(this List<Item> itemList, int id)
        {
            int count = 0;
            List<Item> sameItems = itemList.GetSameItems(id);
            sameItems.ForEach(delegate (Item item)
            {
                count += item.amount;
            });
            return count;
        }

        public static int GetItemCount(this List<Item> itemList, string name)
        {
            int count = 0;
            List<Item> sameItems = itemList.GetSameItems(name);
            sameItems.ForEach(delegate (Item item)
            {
                count += item.amount;
            });
            return count;
        }

        public static void DestroySameItems(this List<Item> itemList, int id, int amount, System.Action<Item, int> onChangeItemAmount = null)
        {
            List<Item> sameItems = itemList.GetSameItems(id);

            for (int i = 0; i < sameItems.Count; i++)
            {
                var item = sameItems[i];
                if (item.amount > amount)
                {
                    if (onChangeItemAmount != null) onChangeItemAmount.Invoke(item, amount);
                    item.amount -= amount;
                    break;
                }
                else
                {
                    if (onChangeItemAmount != null) onChangeItemAmount.Invoke(item, item.amount);
                    amount -= item.amount;
                    item.amount = 0;
                    itemList.Remove(item);
                    Object.Destroy(item);
                }
            }
        }

        public static void DestroySameItems(this List<Item> itemList, string name, int amount, System.Action<Item, int> onChangeItemAmount = null)
        {
            List<Item> sameItems = itemList.GetSameItems(name);
            for (int i = 0; i < sameItems.Count; i++)
            {
                var item = sameItems[i];
                if (item.amount > amount)
                {
                    if (onChangeItemAmount != null) onChangeItemAmount.Invoke(item, amount);
                    item.amount -= amount;
                    break;
                }
                else
                {
                    if (onChangeItemAmount != null) onChangeItemAmount.Invoke(item, item.amount);
                    amount -= item.amount;
                    item.amount = 0;
                    itemList.Remove(item);
                    Object.Destroy(item);
                }
            }
        }

        public static void DestroySameItems(this List<Item> itemList, int id, System.Action<Item, int> onChangeItemAmount = null)
        {
            List<Item> sameItems = itemList.GetSameItems(id);
            itemList.RemoveAll(i => i.id.Equals(id));

            for (int i = 0; i < sameItems.Count; i++)
            {
                if (onChangeItemAmount != null) onChangeItemAmount.Invoke(sameItems[i], sameItems[i].amount);
                Object.Destroy(sameItems[i]);
            }
        }

        public static void DestroySameItems(this List<Item> itemList, string name, System.Action<Item, int> onChangeItemAmount = null)
        {
            List<Item> sameItems = itemList.GetSameItems(name);
            itemList.RemoveAll(i => i.name.Equals(name));

            for (int i = 0; i < sameItems.Count; i++)
            {
                if (onChangeItemAmount != null) onChangeItemAmount.Invoke(sameItems[i], sameItems[i].amount);
                Object.Destroy(sameItems[i]);
            }
        }

        public static void DestroySameItems(this List<Item> itemList, System.Action<Item, int> onChangeItemAmount = null, params int[] ids)
        {
            List<Item> sameItems = itemList.GetSameItems(ids);
            itemList.RemoveAll(i => System.Array.Exists(ids, id => i.id.Equals(id)));

            for (int i = 0; i < sameItems.Count; i++)
            {
                if (onChangeItemAmount != null) onChangeItemAmount.Invoke(sameItems[i], sameItems[i].amount);
                Object.Destroy(sameItems[i]);
            }
        }

        public static void DestroySameItems(this List<Item> itemList, System.Action<Item, int> onChangeItemAmount = null, params string[] names)
        {
            List<Item> sameItems = itemList.GetSameItems(names);
            itemList.RemoveAll(i => System.Array.Exists(names, name => i.name.Equals(name)));

            for (int i = 0; i < sameItems.Count; i++)
            {
                if (onChangeItemAmount != null) onChangeItemAmount.Invoke(sameItems[i], sameItems[i].amount);
                Object.Destroy(sameItems[i]);
            }
        }

        public static bool ItemIsEquipped(this ItemManager itemManager, int id)
        {
            if (itemManager.inventory) return System.Array.Find(itemManager.inventory.equipAreas, equipArea => equipArea.currentEquippedItem && equipArea.currentEquippedItem.id.Equals(id));
            return false;
        }

        public static bool ItemTypeIsEquipped(this ItemManager itemManager, ItemType type)
        {
            if (itemManager.inventory) return System.Array.Find(itemManager.inventory.equipAreas, equipArea => equipArea.currentEquippedItem && equipArea.currentEquippedItem.type.Equals(type));
            return false;
        }

        public static bool ItemIsEquipped(this ItemManager itemManager, int id, out EquipedItemInfo equipedItemInfo)
        {
            equipedItemInfo = null;
            if (itemManager.inventory)
            {
                var area = System.Array.Find(itemManager.inventory.equipAreas, equipArea => equipArea.currentEquippedItem && equipArea.currentEquippedItem.id.Equals(id));

                if (area)
                {
                    equipedItemInfo = new EquipedItemInfo(area.currentEquippedItem, area);
                    equipedItemInfo.indexOfArea = System.Array.IndexOf(itemManager.inventory.equipAreas, area);
                    equipedItemInfo.indexOfItem = itemManager.items.IndexOf(area.currentEquippedItem);
                }
                return area != null;
            }
            return false;
        }

        public static bool ItemTypeIsEquipped(this ItemManager itemManager, ItemType type, out EquipedItemInfo equipedItemInfo)
        {
            equipedItemInfo = null;
            if (itemManager.inventory)
            {
                var area = System.Array.Find(itemManager.inventory.equipAreas, equipArea => equipArea.currentEquippedItem && equipArea.currentEquippedItem.type.Equals(type));

                if (area)
                {
                    equipedItemInfo = new EquipedItemInfo(area.currentEquippedItem, area);
                    equipedItemInfo.indexOfArea = System.Array.IndexOf(itemManager.inventory.equipAreas, area);
                    equipedItemInfo.indexOfItem = itemManager.items.IndexOf(area.currentEquippedItem);
                }
                return area != null;
            }
            return false;
        }

        public static Item GetEquippedItem(this ItemManager itemManager, int id)
        {
            if (itemManager.inventory)
            {
                var area = System.Array.Find(itemManager.inventory.equipAreas, equipArea => equipArea.currentEquippedItem && equipArea.currentEquippedItem.id.Equals(id));
                return area ? area.currentEquippedItem : null;
            }
            return null;
        }

        public class EquipedItemInfo
        {
            public Item item;
            public int indexOfItem;
            public EquipArea area;
            public int indexOfArea;
            public EquipedItemInfo()
            {

            }
            public EquipedItemInfo(Item item, EquipArea area)
            {
                this.item = item;
                this.area = area;
            }
        }
    }
}