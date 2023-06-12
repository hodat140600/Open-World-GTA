#define PlayerPref
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace _GAME._Scripts._ItemManager
{
    public static class SaveLoadInventory
    {
        public static string InventoryDataFile = Application.dataPath + Path.DirectorySeparatorChar + "InventoryData.json";

        /// <summary>
        /// Create a Json text from Inventory EquipAreas and ItemManager Items
        /// </summary>
        /// <param name="itemManager">Target ItemManaget</param>
        /// <returns>Json file text</returns>
        public static string InventoryToJsonText(ItemManager itemManager)
        {
            if (!itemManager.inventory) return string.Empty;
            InventoryData data = new InventoryData();
            EquipArea[] equipAreas = itemManager.inventory.equipAreas;
            for (int i = 0; i < equipAreas.Length; i++)
            {
                EquipAreaData equipAreaData = new EquipAreaData();
                equipAreaData.indexOfSelectedSlot = equipAreas[i].indexOfEquippedItem;

                for (int e = 0; e < equipAreas[i].equipSlots.Count; e++)
                {
                    SlotData equipAreaSlotData = new SlotData();
                    equipAreaSlotData.hasItem = equipAreas[i].equipSlots[e].item != null;
                    if (equipAreaSlotData.hasItem)
                    {
                        equipAreaSlotData.indexOfItem = itemManager.items.IndexOf(equipAreas[i].equipSlots[e].item);
                    }
                    equipAreaData.slotsData.Add(equipAreaSlotData);
                }
                data.equipAreas.Add(equipAreaData);
            }

            for (int i = 0; i < itemManager.items.Count; i++)
            {
                data.itemReferences.Add(new ItemReference(itemManager.items[i]));
            }
            return JsonUtility.ToJson(data, true);
        }

        /// <summary>
        /// Load json file from <seealso cref="InventoryDataFile"/>
        /// </summary>
        /// <returns>Json file text</returns>
        public static string LoadInventoryJasonText()
        {
#if PlayerPref
            if (PlayerPrefs.GetString(InventoryDataFile) != null) return PlayerPrefs.GetString(InventoryDataFile);
#else
            if (File.Exists(InventoryDataFile)) return File.ReadAllText(InventoryDataFile);
#endif
            return string.Empty;
        }

        /// <summary>
        /// Save inventory items and occupied equipSlots
        /// </summary>
        /// <param name="itemManager"></param>
        public static void SaveInventory(this ItemManager itemManager)
        {
            string json = InventoryToJsonText(itemManager);
            if (!string.IsNullOrEmpty(json))
            {
#if PlayerPref
                PlayerPrefs.SetString(InventoryDataFile, json);
#else
                File.WriteAllText(InventoryDataFile, json);
#endif

                itemManager.onSaveItems.Invoke();
            }
        }

        /// <summary>
        /// Load inventory items and occupied equipSlots
        /// </summary>
        /// <param name="itemManager"></param>
        public static void LoadInventory(this ItemManager itemManager)
        {
            string json = LoadInventoryJasonText();

            if (!string.IsNullOrEmpty(json))
            {
                InventoryData data = new InventoryData();
                JsonUtility.FromJsonOverwrite(json, data);
                itemManager.items = data.GetItems(itemManager.itemListData);
                EquipArea[] equipAreas = itemManager.inventory.equipAreas;

                for (int i = 0; i < equipAreas.Length; i++)
                {
                    if (i < data.equipAreas.Count)
                    {
                        EquipArea area = equipAreas[i];
                        EquipAreaData areaData = data.equipAreas[i];

                        area.indexOfEquippedItem = areaData.indexOfSelectedSlot;

                        for (int e = 0; e < equipAreas[i].equipSlots.Count; e++)
                        {
                            if (e < areaData.slotsData.Count)
                            {
                                SlotData slotData = areaData.slotsData[e];
                                EquipSlot slot = equipAreas[i].equipSlots[e];
                                itemManager.temporarilyIgnoreItemAnimation = true;
                                if (slotData.hasItem)
                                {
                                    area.AddItemToEquipSlot(e, itemManager.items[slotData.indexOfItem]);
                                }
                                else area.RemoveItemOfEquipSlot(e);
                            }
                        }
                    }
                }
            }
            itemManager.inventory.UpdateInventory();
            itemManager.temporarilyIgnoreItemAnimation = false;
            itemManager.onLoadItems.Invoke();
        }

        [System.Serializable]
        class InventoryData
        {
            /// <summary>
            /// List of <see cref="ItemReference"/>
            /// </summary>
            public List<ItemReference> itemReferences = new List<ItemReference>();

            /// <summary>
            /// List of <seealso cref="EquipAreaData"/>
            /// </summary>
            public List<EquipAreaData> equipAreas = new List<EquipAreaData>();

            /// <summary>
            /// Get <seealso cref="Item"/> from <seealso cref="ItemReference"/>
            /// </summary>
            /// <param name="itemListData"></param>
            /// <returns></returns>
            public List<Item> GetItems(ItemListData itemListData)
            {
                List<Item> items = new List<Item>();
                for (int i = 0; i < itemReferences.Count; i++)
                {
                    Item item = itemListData.items.Find(a => a.id.Equals(itemReferences[i].id));
                    item = Object.Instantiate(item);
                    item.amount = itemReferences[i].amount;
                    item.attributes = itemReferences[i].attributes;
                    item.name = item.name.Replace("(Clone)", string.Empty);
                    items.Add(item);
                }
                return items;
            }
        }

        [System.Serializable]
        class EquipAreaData
        {
            /// <summary>
            /// List of <seealso cref="SlotData"/>
            /// </summary>
            public List<SlotData> slotsData = new List<SlotData>();

            public int indexOfSelectedSlot;
        }

        [System.Serializable]
        class SlotData
        {
            public bool hasItem;
            public int indexOfItem;
        }

        [System.Serializable]
        class ItemReference
        {
            [SerializeField] public int amount;
            [SerializeField] public int id;
            [SerializeField] public List<ItemAttribute> attributes;

            public ItemReference(Item item)
            {
                amount = item.amount;
                id = item.id;
                attributes = item.attributes;
            }
        }
    }
}