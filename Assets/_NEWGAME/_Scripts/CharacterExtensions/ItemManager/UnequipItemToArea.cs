using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _GAME._Scripts._ItemManager
{
    public class UnequipItemToArea : MonoBehaviour
    {
        [@HideInInspector]
        public List<EquipArea> equipAreas;
        protected EquipArea equipArea;
        protected Inventory inventory;

        void Start()
        {
            equipAreas = GetComponentsInChildren<EquipArea>().dToList();
            foreach (EquipArea area in equipAreas)
            {
                area.onSelectEquipArea.AddListener(OnSelectArea);
            }

            inventory = GetComponentInParent<Inventory>();
        }

        public void OnSelectArea(EquipArea area)
        {
            equipArea = area;
        }

        protected EquipSlot currentSlot
        {
            get { return equipArea ? equipArea.currentSelectedSlot ? equipArea.currentSelectedSlot : equipArea.lastSelectedSlot : null; }
        }

        public void UnequipItem()
        {
            if (equipArea && currentSlot != null && currentSlot.item != null)
            {
                equipArea.RemoveItemOfEquipSlot(currentSlot);
            }
        }
    }
}
