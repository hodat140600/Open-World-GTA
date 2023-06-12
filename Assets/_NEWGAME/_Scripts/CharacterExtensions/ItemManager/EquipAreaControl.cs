using UnityEngine;
using System.Collections.Generic;

namespace _GAME._Scripts._ItemManager
{
    public class EquipAreaControl : MonoBehaviour
    {
        [@HideInInspector]
        public List<EquipArea> equipAreas;

        void Start()
        {
            equipAreas = GetComponentsInChildren<EquipArea>().dToList();
            foreach (EquipArea area in equipAreas)
                area.onPickUpItemCallBack = OnPickUpItemCallBack;

            Inventory inventory = GetComponentInParent<Inventory>();
            if (inventory)
                inventory.onOpenCloseInventory.AddListener(OnOpen);
        }

        /// <summary>
        /// Event called when inventory open or close
        /// </summary>
        /// <param name="value">is open</param>
        public virtual void OnOpen(bool value)
        {

        }

        /// <summary>
        /// Event called when an area pick up an item
        /// </summary>
        /// <param name="area">target area</param>
        /// <param name="slot">target slot</param>
        public virtual void OnPickUpItemCallBack(EquipArea area, ItemSlot slot)
        {
            for (int i = 0; i < equipAreas.Count; i++)
            {
                var sameSlots = equipAreas[i].equipSlots.FindAll(slotInArea => slotInArea != slot && slotInArea.item != null && slotInArea.item == slot.item);
                for (int a = 0; a < sameSlots.Count; a++)
                {
                    equipAreas[i].UnequipItem(sameSlots[a]);
                }
            }
            CheckTwoHandItem(area, slot);
        }

        void CheckTwoHandItem(EquipArea area, ItemSlot slot)
        {
            if (slot.item == null) return;
            var opposite = equipAreas.Find(_area => _area != null && area.equipPointName.Equals("LeftArm") && _area.currentEquippedItem != null);
            //var RightEquipmentController = changeEquipmentControllers.Find(equipCtrl => equipCtrl.equipArea != null && equipCtrl.equipArea.equipPointName.Equals("RightArm"));
            if (area.equipPointName.Equals("LeftArm"))
                opposite = equipAreas.Find(_area => _area != null && area.equipPointName.Equals("RightArm") && _area.currentEquippedItem != null);
            else if (!area.equipPointName.Equals("RightArm"))
            {
                return;
            }
            if (opposite != null && (slot.item.twoHandWeapon || opposite.currentEquippedItem.twoHandWeapon))
            {
                opposite.onUnequipItem.Invoke(opposite, slot.item);
                opposite.UnequipItem(slot as EquipSlot);
            }
        }
    }

}
