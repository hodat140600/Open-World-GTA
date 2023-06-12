using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace _GAME._Scripts._ItemManager
{
    [ClassHeader("Equip Area", openClose = false)]
    public class EquipArea : ExtendMonoBehaviour
    {
        public delegate void OnPickUpItem(EquipArea area, ItemSlot slot);
        public OnPickUpItem onPickUpItemCallBack;

        public Inventory inventory;
        public ItemWindow itemPicker;
        [Tooltip("Set current equiped slot when submit an slot of this area")]
        public bool setEquipSlotWhenSubmit;
        [Tooltip("Skip empty slots when switching between slots")]
        public bool skipEmptySlots;
        public List<EquipSlot> equipSlots;
        public string equipPointName;

        public Text displayNameText;
        public Text displayTypeText;
        public Text displayAmountText;
        public Text displayDescriptionText;
        public Text displayAttributesText;

        [HelpBox("You can ignore display Attributes using this property")]
        public List<ItemAttributes> ignoreAttributes;
        public UnityEvent onInitPickUpItem, onFinishPickUpItem;
        public InputField.OnChangeEvent onChangeName;
        public InputField.OnChangeEvent onChangeType;
        public InputField.OnChangeEvent onChangeAmount;
        public InputField.OnChangeEvent onChangeDescription;
        public InputField.OnChangeEvent onChangeAttributes;

        public OnChangeEquipmentEvent onEquipItem;
        public OnChangeEquipmentEvent onUnequipItem;
        public OnSelectEquipArea onSelectEquipArea;


        public Toggle.ToggleEvent onSetLockToEquip;
        [@HideInInspector]
        public EquipSlot currentSelectedSlot;
        public EquipSlot lastSelectedSlot;
        [@HideInInspector]
        public int indexOfEquippedItem;
        public Item lastEquipedItem;

        protected bool _isLockedToEquip;
        public bool isLockedToEquip
        {
            get
            {
                return _isLockedToEquip;
            }
            set
            {
                if (_isLockedToEquip != value) onSetLockToEquip.Invoke(value);
                _isLockedToEquip = value;
            }
        }

        public bool ignoreEquipEvents;
        /// <summary>
        /// used to ignore <see cref="onEquipItem"/> event. if true, the inventory will just add the equipment to area but dont will send to Equip the item. you will nedd to call <see cref="EquipCurrentSlot"/> to equip the item in the area.     
        /// </summary>  
        internal bool isInit;

        public void Init()
        {
            if (!isInit) Start();
        }

        void Start()
        {
            if (!isInit)
            {
                isInit = true;
                //indexOfEquipedItem = -1;
                inventory = GetComponentInParent<Inventory>();

                if (equipSlots.Count == 0)
                {
                    var equipSlotsArray = GetComponentsInChildren<EquipSlot>(true);
                    equipSlots = equipSlotsArray.dToList();
                }
                foreach (EquipSlot slot in equipSlots)
                {
                    slot.onSubmitSlotCallBack = OnSubmitSlot;
                    slot.onSelectSlotCallBack = OnSelectSlot;
                    slot.onDeselectSlotCallBack = OnDeselect;
                    onSetLockToEquip.AddListener(slot.SetLockToEquip);
                    if (slot.displayAmountText) slot.displayAmountText.text = "";
                    slot.onChangeAmount.Invoke("");
                }
            }
        }

        /// <summary>
        /// Current Equipped Slot
        /// </summary>
        public EquipSlot currentEquippedSlot
        {
            get
            {
                return equipSlots[indexOfEquippedItem];

            }
        }
        /// <summary>
        /// Item in Current Equipped Slot
        /// </summary>
        public Item currentEquippedItem
        {
            get
            {
                var validEquipSlots = ValidSlots;
                if (validEquipSlots.Count > 0 && indexOfEquippedItem >= 0 && indexOfEquippedItem < validEquipSlots.Count) return validEquipSlots[indexOfEquippedItem].item;

                return null;
            }
        }

        /// <summary>
        /// All valid slot <seealso cref="ItemSlot.isValid"/>
        /// </summary>
        public List<EquipSlot> ValidSlots
        {
            get { return equipSlots.FindAll(slot => slot.isValid && (!skipEmptySlots || slot.item != null)); }
        }

        /// <summary>
        /// Check if Item is in Area
        /// </summary>
        /// <param name="item">item to check</param>
        /// <returns></returns>
        public bool ContainsItem(Item item)
        {
            return ValidSlots.Find(slot => slot.item == item) != null;
        }

        /// <summary>
        /// Event called from Inventory slot UI on Submit
        /// </summary>
        /// <param name="slot"></param>
        public void OnSubmitSlot(ItemSlot slot)
        {
            lastSelectedSlot = currentSelectedSlot;
            if (itemPicker != null)
            {
                currentSelectedSlot = slot as EquipSlot;
                if (setEquipSlotWhenSubmit)
                {
                    SetEquipSlot(equipSlots.IndexOf(currentSelectedSlot));
                }
                itemPicker.gameObject.SetActive(true);
                itemPicker.onCancelSlot.RemoveAllListeners();
                itemPicker.onCancelSlot.AddListener(CancelCurrentSlot);
                itemPicker.CreateEquipmentWindow(inventory.items, currentSelectedSlot.itemType, slot.item, OnPickItem);
                onInitPickUpItem.Invoke();
            }
        }

        /// <summary>
        /// Event called to cancel Submit action
        /// </summary>
        public void CancelCurrentSlot()
        {
            if (currentSelectedSlot == null)
                currentSelectedSlot = lastSelectedSlot;

            if (currentSelectedSlot != null)
                currentSelectedSlot.OnCancel();
            onFinishPickUpItem.Invoke();
        }

        /// <summary>
        /// Unequip Item of the Slot
        /// </summary>
        /// <param name="slot">target slot</param>
        public void UnequipItem(EquipSlot slot)
        {
            if (slot)
            {
                Item item = slot.item;
                if (ValidSlots[indexOfEquippedItem].item == item)
                    lastEquipedItem = item;
                slot.RemoveItem();
                onUnequipItem.Invoke(this, item);
            }
        }

        /// <summary>
        /// Unequip Item if is present in slots 
        /// </summary>
        /// <param name="item"></param>
        public void UnequipItem(Item item)
        {
            var slot = ValidSlots.Find(_slot => _slot.item == item);
            if (slot)
            {
                if (ValidSlots[indexOfEquippedItem].item == item) lastEquipedItem = item;
                slot.RemoveItem();
                onUnequipItem.Invoke(this, item);
            }
        }

        /// <summary>
        /// Unequip <seealso cref="currentEquippedItem"/>
        /// </summary>
        public void UnequipCurrentItem()
        {
            if (currentSelectedSlot && currentSelectedSlot.item)
            {
                var _item = currentSelectedSlot.item;
                if (ValidSlots[indexOfEquippedItem].item == _item) lastEquipedItem = _item;
                currentSelectedSlot.RemoveItem();
                onUnequipItem.Invoke(this, _item);
            }
        }

        /// <summary>
        /// Event called from inventory UI when select an slot
        /// </summary>
        /// <param name="slot">target slot</param>
        public void OnSelectSlot(ItemSlot slot)
        {
            if (equipSlots.Contains(slot as EquipSlot))
                currentSelectedSlot = slot as EquipSlot;
            else currentSelectedSlot = null;

            onSelectEquipArea.Invoke(this);
            CreateFullItemDescription(slot);
        }

        /// <summary>
        /// Event called from inventory UI when unselect an slot
        /// </summary>
        /// <param name="slot">target slot</param>
        public void OnDeselect(ItemSlot slot)
        {
            if (equipSlots.Contains(slot as EquipSlot))
            {
                currentSelectedSlot = null;
            }
        }

        /// <summary>
        /// Create item description
        /// </summary>
        /// <param name="slot">target slot</param>
        protected virtual void CreateFullItemDescription(ItemSlot slot)
        {
            var _name = slot.item ? slot.item.name : "";
            var _type = slot.item ? slot.item.ItemTypeText() : "";
            var _amount = slot.item ? slot.item.amount.ToString() : "";
            var _description = slot.item ? slot.item.description : "";
            var _attributes = slot.item ? slot.item.GetItemAttributesText(ignoreAttributes) : "";

            if (displayNameText) displayNameText.text = _name;
            onChangeName.Invoke(_name);

            if (displayTypeText) displayTypeText.text = _type;
            onChangeType.Invoke(_type);

            if (displayAmountText) displayAmountText.text = _amount;
            onChangeAmount.Invoke(_amount);

            if (displayDescriptionText) displayDescriptionText.text = _description;
            onChangeDescription.Invoke(_description);

            if (displayAttributesText) displayAttributesText.text = _attributes;
            onChangeAttributes.Invoke(_attributes);
        }

        /// <summary>
        /// Event called from inventory UI to open <see cref="ItemWindow"/> when submit slot
        /// </summary>
        /// <param name="slot">target slot</param>
        public void OnPickItem(ItemSlot slot)
        {
            if (!currentSelectedSlot)
                currentSelectedSlot = lastSelectedSlot;

            if (!currentSelectedSlot)
                return;

            if (currentSelectedSlot.item != null && slot.item != currentSelectedSlot.item)
            {
                currentSelectedSlot.item.isInEquipArea = false;
                var item = currentSelectedSlot.item;
                if (item == slot.item) lastEquipedItem = item;
                currentSelectedSlot.RemoveItem();
                onUnequipItem.Invoke(this, item);
            }

            if (slot.item != currentSelectedSlot.item)
            {
                if (onPickUpItemCallBack != null)
                    onPickUpItemCallBack(this, slot);
                currentSelectedSlot.AddItem(slot.item);
                if (!ignoreEquipEvents) onEquipItem.Invoke(this, currentSelectedSlot.item);
            }
            currentSelectedSlot.OnCancel();
            currentSelectedSlot = null;
            lastSelectedSlot = null;
            itemPicker.gameObject.SetActive(false);
            onFinishPickUpItem.Invoke();
        }

        /// <summary>
        /// Equip next slot <seealso cref="currentEquippedItem"/>
        /// </summary>
        public void NextEquipSlot()
        {
            if (equipSlots == null || equipSlots.Count == 0) return;

            lastEquipedItem = currentEquippedItem;
            var validEquipSlots = ValidSlots;
            if (indexOfEquippedItem + 1 < validEquipSlots.Count)
                indexOfEquippedItem++;
            else
                indexOfEquippedItem = 0;

            if (currentEquippedItem != null && !ignoreEquipEvents)
                onEquipItem.Invoke(this, currentEquippedItem);
            onUnequipItem.Invoke(this, lastEquipedItem);
        }

        /// <summary>
        /// Equip previous slot <seealso cref="currentEquippedItem"/>
        /// </summary>
        public void PreviousEquipSlot()
        {
            if (equipSlots == null || equipSlots.Count == 0) return;

            lastEquipedItem = currentEquippedItem;
            var validEquipSlots = ValidSlots;

            if (indexOfEquippedItem - 1 >= 0)
                indexOfEquippedItem--;
            else
                indexOfEquippedItem = validEquipSlots.Count - 1;

            if (currentEquippedItem != null && !ignoreEquipEvents)
                onEquipItem.Invoke(this, currentEquippedItem);

            onUnequipItem.Invoke(this, lastEquipedItem);
        }

        /// <summary>
        /// Equip slot <see cref="currentEquippedItem"/>
        /// </summary>
        /// <param name="indexOfSlot">index of target slot</param>
        public void SetEquipSlot(int indexOfSlot)
        {
            if (equipSlots == null || equipSlots.Count == 0) return;
            if (indexOfSlot < equipSlots.Count && indexOfSlot >= 0)
            {
                lastEquipedItem = currentEquippedItem;
                indexOfEquippedItem = indexOfSlot;
                if (currentEquippedItem != null && !ignoreEquipEvents)
                {
                    onEquipItem.Invoke(this, currentEquippedItem);
                }
                if (currentEquippedItem != lastEquipedItem)
                    onUnequipItem.Invoke(this, lastEquipedItem);
            }
        }


        public void EquipCurrentSlot()
        {
            if (!currentEquippedSlot || currentEquippedSlot.item != null && currentEquippedSlot.item.isEquiped) return;
            if (currentEquippedItem) onEquipItem.Invoke(this, currentEquippedItem);
            else if (lastEquipedItem) onUnequipItem.Invoke(this, lastEquipedItem);
        }
        /// <summary>
        /// Add an item to an slot
        /// </summary>
        /// <param name="slot">target Slot</param>
        /// <param name="item">target Item</param>
        public void AddItemToEquipSlot(ItemSlot slot, Item item, bool autoEquip = false)
        {
            if (slot is EquipSlot && equipSlots.Contains(slot as EquipSlot))
            {
                AddItemToEquipSlot(equipSlots.IndexOf(slot as EquipSlot), item, autoEquip);
            }
        }

        /// <summary>
        /// Add an item to an slot
        /// </summary>
        /// <param name="indexOfSlot">index of target Slot</param>
        /// <param name="item">target Item</param>
        public void AddItemToEquipSlot(int indexOfSlot, Item item, bool autoEquip = false)
        {
            if (indexOfSlot < equipSlots.Count && item != null)
            {
                var slot = equipSlots[indexOfSlot];

                if (slot != null && slot.isValid && slot.itemType.Contains(item.type))
                {
                    var sameSlot = equipSlots.Find(s => s.item == item && s != slot);

                    if (sameSlot != null)
                        RemoveItemOfEquipSlot(equipSlots.IndexOf(sameSlot));

                    if (slot.item != null && slot.item != item)
                    {
                        if (currentEquippedItem == slot.item) lastEquipedItem = slot.item;
                        slot.item.isInEquipArea = false;
                        onUnequipItem.Invoke(this, slot.item);
                    }

                    item.isInEquipArea = true;
                    slot.AddItem(item);
                    if (autoEquip)
                        SetEquipSlot(indexOfSlot);
                    else if (!ignoreEquipEvents)
                        onEquipItem.Invoke(this, item);
                }
            }
        }

        /// <summary>
        /// Remove item of an slot
        /// </summary>
        /// <param name="slot">target Slot</param>
        public void RemoveItemOfEquipSlot(ItemSlot slot)
        {
            if (slot is EquipSlot && equipSlots.Contains(slot as EquipSlot))
            {
                RemoveItemOfEquipSlot(equipSlots.IndexOf(slot as EquipSlot));
            }
        }

        /// <summary>
        /// Remove item of an slot
        /// </summary>
        /// <param name="slot">index of target Slot</param>
        public void RemoveItemOfEquipSlot(int indexOfSlot)
        {
            if (indexOfSlot < equipSlots.Count)
            {
                var slot = equipSlots[indexOfSlot];
                if (slot != null && slot.item != null)
                {
                    var item = slot.item;
                    item.isInEquipArea = false;
                    if (currentEquippedItem == item) lastEquipedItem = currentEquippedItem;
                    slot.RemoveItem();
                    onUnequipItem.Invoke(this, item);
                }
            }
        }

        /// <summary>
        /// Add item to current equiped slot 
        /// </summary>
        /// <param name="item">target item</param>
        public void AddCurrentItem(Item item)
        {
            if (indexOfEquippedItem < equipSlots.Count)
            {
                var slot = equipSlots[indexOfEquippedItem];
                if (slot.item != null && item != slot.item)
                {
                    if (currentEquippedItem == slot.item) lastEquipedItem = slot.item;
                    slot.item.isInEquipArea = false;
                    onUnequipItem.Invoke(this, currentSelectedSlot.item);
                }
                slot.AddItem(item);
                if (!ignoreEquipEvents) onEquipItem.Invoke(this, item);
            }
        }

        /// <summary>
        /// Remove current equiped Item
        /// </summary>
        public void RemoveCurrentItem()
        {
            if (!currentEquippedItem) return;
            lastEquipedItem = currentEquippedItem;
            ValidSlots[indexOfEquippedItem].RemoveItem();
            onUnequipItem.Invoke(this, lastEquipedItem);

        }
    }
}
