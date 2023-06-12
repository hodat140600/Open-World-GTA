using UnityEngine;
using UnityEngine.EventSystems;

namespace _GAME._Scripts._ItemManager
{
    public class ItemWindowDisplay : MonoBehaviour
    {
        public Inventory inventory;
        public ItemWindow itemWindow;
        public ItemOptionWindow optionWindow;
        [@HideInInspector]
        public ItemSlot currentSelectedSlot;
        [@HideInInspector]
        public int amount;

        public virtual void OnEnable()
        {
            if (inventory == null)
                inventory = GetComponentInParent<Inventory>();

            if (inventory && itemWindow)
            {
                inventory.onDestroyItem.RemoveListener(OnDestroyItem);
                inventory.onDestroyItem.AddListener(OnDestroyItem);
                itemWindow.CreateEquipmentWindow(inventory.items, OnSubmit, OnSelectSlot);
                inventory.OnUpdateInventory -= CheckItemExits;
                inventory.OnUpdateInventory += CheckItemExits;
            }
        }

        public void OnDisable()
        {
            if (inventory)
                inventory.OnUpdateInventory -= CheckItemExits;
        }

        public virtual void OnDestroyItem(Item item, int amount)
        {
            var _slot = itemWindow.slots.Find(slot => slot.item.Equals(item));
            if (_slot != null && (_slot.item == null || _slot.item.amount == 0))
            {

                itemWindow.slots.Remove(_slot);
                Destroy(_slot.gameObject);
            }
        }

        public virtual void OnSubmit(ItemSlot slot)
        {
            currentSelectedSlot = slot;
            if (slot.item)
            {
                var rect = slot.GetComponent<RectTransform>();
                if (optionWindow.CanOpenOptions(slot.item))
                {
                    //optionWindow.transform.position = rect.position;
                    optionWindow.gameObject.SetActive(true);
                    optionWindow.EnableOptions(slot);
                    // currentSelectedSlot = slot;
                }
            }
        }

        public virtual void OnSelectSlot(ItemSlot slot)
        {
            currentSelectedSlot = slot;
        }

        public virtual void DropItem()
        {
            if (amount > 0)
            {
                inventory.OnDropItem(currentSelectedSlot.item, amount);
                if (currentSelectedSlot != null && (currentSelectedSlot.item == null || currentSelectedSlot.item.amount <= 0))
                {
                    if (itemWindow.slots.Contains(currentSelectedSlot))
                        itemWindow.slots.Remove(currentSelectedSlot);
                    Destroy(currentSelectedSlot.gameObject);
                    if (itemWindow.slots.Count > 0)
                        SetSelectable(itemWindow.slots[0].gameObject);
                }
            }
        }

        public virtual void LeaveItem()
        {
            if (amount > 0)
            {
                inventory.OnDestroyItem(currentSelectedSlot.item, amount);
                if (currentSelectedSlot != null && (currentSelectedSlot.item == null || currentSelectedSlot.item.amount <= 0))
                {
                    if (itemWindow.slots.Contains(currentSelectedSlot))
                        itemWindow.slots.Remove(currentSelectedSlot);
                    Destroy(currentSelectedSlot.gameObject);
                    if (itemWindow.slots.Count > 0)
                        SetSelectable(itemWindow.slots[0].gameObject);
                }
            }
        }

        public virtual void UseItem()
        {
            inventory.OnUseItem(currentSelectedSlot.item);
        }

        private void CheckItemExits()
        {
            itemWindow.ReloadItems(inventory.items);
            //var slotsToDestroy =itemWindow.slots.FindAll(slot => slot.item == null || slot.item.amount == 0);
            //for(int i =0;i<slotsToDestroy.Count;i++)
            //{
            //    itemWindow.slots.Remove(slotsToDestroy[i]);
            //    Destroy(slotsToDestroy[i].gameObject);
            //}
        }

        public virtual void SetOldSelectable()
        {
            try
            {
                if (currentSelectedSlot != null)
                    SetSelectable(currentSelectedSlot.gameObject);
                else if (itemWindow.slots.Count > 0 && itemWindow.slots[0] != null)
                {
                    SetSelectable(itemWindow.slots[0].gameObject);
                }
            }
            catch
            {

            }
        }

        public virtual void SetSelectable(GameObject target)
        {
            try
            {
                var pointer = new PointerEventData(EventSystem.current);
                ExecuteEvents.Execute(EventSystem.current.currentSelectedGameObject, pointer, ExecuteEvents.pointerExitHandler);
                EventSystem.current.SetSelectedGameObject(target, new BaseEventData(EventSystem.current));
                ExecuteEvents.Execute(target, pointer, ExecuteEvents.selectHandler);
            }
            catch { }

        }
    }
}
