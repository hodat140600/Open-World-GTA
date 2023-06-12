using UnityEngine.UI;

namespace _GAME._Scripts._ItemManager
{
    public class EquipmentDisplay : ItemSlot
    {
        public Text slotIdentifier;
        public InputField.OnChangeEvent onChangeIdentifier;
        public UnityEngine.Events.UnityEvent onSelect, onDeselect;
        public bool hasItem;
        public UnityEngine.Events.UnityEvent onSetLockToEquip, onUnlockToEquip;


        public void SetLockToEquip(bool value)
        {
            if (value) onSetLockToEquip.Invoke();
            else onUnlockToEquip.Invoke();
        }
        public void ItemIdentifier(int identifier = 0, bool showIdentifier = false)
        {
            if (!slotIdentifier) return;

            if (showIdentifier)
            {
                if (slotIdentifier)
                    slotIdentifier.text = identifier.ToString();
                onChangeIdentifier.Invoke(identifier.ToString());
            }
            else
            {
                if (slotIdentifier)
                    slotIdentifier.text = string.Empty;
                onChangeIdentifier.Invoke(string.Empty);
            }
        }

        public override void AddItem(Item item)
        {
            base.AddItem(item);
            hasItem = item != null;
        }
    }
}