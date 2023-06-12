using _GAME._Scripts._CharacterController;
using UnityEngine;

namespace _GAME._Scripts._ItemManager
{
    public class EquipItemToArea : MonoBehaviour
    {
        public ItemWindowDisplay itemWindow;
        public AreaToEquip[] areasToEquip;
        public GenericInput cancelInput = new GenericInput("Escape", "B", "B");
        public UnityEngine.Events.UnityEvent onEquip;
        public UnityEngine.Events.UnityEvent onCancel;
        [System.Serializable]
        public class AreaToEquip
        {
            [Tooltip("Target EquipArea to Equip")]
            public EquipArea area;
            [Tooltip("Target EquipSlot of the EquipArea to Equip")]
            public int slotIndex;
            [Tooltip("Optional button to equip on press")]
            public UnityEngine.UI.Button optionalButton;
            [Tooltip("Input to equip on press")]
            public GenericInput input = new GenericInput("Alpha 1", "B", "B");

            internal void CheckInput(Item item, UnityEngine.Events.UnityEvent onEquip)
            {
                if (input.GetButtonDown())
                {
                    Equip(item);
                    onEquip?.Invoke();
                }
            }

            internal void Equip(Item item)
            {
                if (area) area.AddItemToEquipSlot(slotIndex, item);
            }
        }

        protected UnityEngine.Events.UnityAction onEquipAction;

        private void Start()
        {
            onEquipAction = () => { onEquip.Invoke(); };
            for (int i = 0; i < areasToEquip.Length; i++)
            {
                if (areasToEquip[i].optionalButton)
                {
                    AreaToEquip areaToEquip = areasToEquip[i];
                    areasToEquip[i].optionalButton.onClick.AddListener(() =>
                    {

                        Equip(areaToEquip);
                    });
                }
            }
        }

        protected virtual void Update()
        {
            if (itemWindow && itemWindow.currentSelectedSlot && itemWindow.currentSelectedSlot.item)
            {
                for (int i = 0; i < areasToEquip.Length; i++)
                {
                    areasToEquip[i].CheckInput(itemWindow.currentSelectedSlot.item, onEquip);
                }
            }

            if (cancelInput.GetButtonDown())
            {
                onCancel.Invoke();
            }
        }
        protected virtual void Equip(AreaToEquip areaToEquip)
        {
            if (itemWindow && itemWindow.currentSelectedSlot && itemWindow.currentSelectedSlot.item)
            {
                areaToEquip.Equip(itemWindow.currentSelectedSlot.item);
                onEquip.Invoke();
            }
        }

        public virtual void Equip(int index)
        {
            if (index < areasToEquip.Length)
            {
                AreaToEquip areaToEquip = areasToEquip[index];
                Equip(areaToEquip);
            }
        }
    }
}