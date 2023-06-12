using _GAME._Scripts;
using _GAME._Scripts._CharacterController;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace _GAME._Scripts._ItemManager
{
    [ClassHeader("Control Area By Input", "Use to select an EquipArea when you press a Input")]
    public class ControlAreaByInput : ExtendMonoBehaviour
    {
        public List<SlotsSelector> slotsSelectors;
        public EquipArea equipArea;
        public Inventory inventory;

        public delegate void OnSelectSlot(int indexOfSlot);
        public event OnSelectSlot onSelectSlot;

        void Start()
        {
            inventory = GetComponentInParent<Inventory>();

            for (int i = 0; i < slotsSelectors.Count; i++)
            {
                onSelectSlot += slotsSelectors[i].Select;
            }

            onSelectSlot?.Invoke(0);
        }

        void Update()
        {
            if (!inventory || !equipArea || inventory.lockInventoryInput) return;

            for (int i = 0; i < slotsSelectors.Count; i++)
            {
                if (slotsSelectors[i].input.GetButtonDown() && inventory && !inventory.IsLocked() && !inventory.isOpen && inventory.canEquip)
                {
                    if (slotsSelectors[i].indexOfSlot < equipArea.equipSlots.Count && slotsSelectors[i].indexOfSlot >= 0)
                    {
                        equipArea.SetEquipSlot(slotsSelectors[i].indexOfSlot);
                        onSelectSlot?.Invoke(slotsSelectors[i].indexOfSlot);
                    }
                }

                if (slotsSelectors[i].equipDisplay != null && slotsSelectors[i].indexOfSlot < equipArea.equipSlots.Count && slotsSelectors[i].indexOfSlot >= 0)
                {
                    if (equipArea.equipSlots[slotsSelectors[i].indexOfSlot].item != slotsSelectors[i].equipDisplay.item)
                    {
                        slotsSelectors[i].equipDisplay.AddItem(equipArea.equipSlots[slotsSelectors[i].indexOfSlot].item);
                    }
                    else if (equipArea.equipSlots[slotsSelectors[i].indexOfSlot].item == null && slotsSelectors[i].equipDisplay.hasItem)
                    {
                        slotsSelectors[i].equipDisplay.RemoveItem();
                    }
                }
            }
        }

        [Serializable]
        public class SlotsSelector
        {
            public GenericInput input;
            public int indexOfSlot;
            public EquipmentDisplay equipDisplay;
            public bool selected;
            public void Select(int indexOfSlot)
            {
                if (this.indexOfSlot != indexOfSlot && selected)
                {
                    equipDisplay.onDeselect.Invoke();
                    selected = false;
                }
                else if (this.indexOfSlot == indexOfSlot && !selected)
                {
                    equipDisplay.onSelect.Invoke();
                    selected = true;
                }
            }
        }
    }
}