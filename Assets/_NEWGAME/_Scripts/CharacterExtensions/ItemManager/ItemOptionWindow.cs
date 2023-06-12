using _GAME._Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace _GAME._Scripts._ItemManager
{
    [ClassHeader("Item Options Window")]
    public class ItemOptionWindow : ExtendMonoBehaviour
    {
        public Button useItemButton;
        public Button equipItemButton;
        public Button dropItemButton;
        public Button destroyItemButton;

        public virtual void EnableOptions(ItemSlot slot)
        {
            //if (slot ==null || slot.item==null) return;
            //useItemButton.interactable = itemsCanBeUsed.Contains(slot.item.type);
        }

        protected virtual void ValidateButtons(Item item, out bool result)
        {
            if (item.originalObject && item.originalObject.GetComponent<Equipment>() != null)
            {
                if (equipItemButton)
                    equipItemButton.gameObject.SetActive(true);
                if (useItemButton)
                    useItemButton.gameObject.SetActive(false);
            }
            else
            {
                if (equipItemButton)
                    equipItemButton.gameObject.SetActive(false);
                if (useItemButton)
                    useItemButton.gameObject.SetActive(true);
            }

            if (useItemButton)
                useItemButton.interactable = item.canBeUsed;

            if (dropItemButton)
                dropItemButton.interactable = item.canBeDroped;

            if (destroyItemButton)
                destroyItemButton.interactable = item.canBeDestroyed;

            result = equipItemButton && equipItemButton.gameObject.activeSelf ||
                     useItemButton && useItemButton.interactable ||
                     dropItemButton && dropItemButton.interactable ||
                     destroyItemButton && destroyItemButton.interactable;
        }

        public virtual bool CanOpenOptions(Item item)
        {
            if (item == null) return false;
            var canOpen = false;
            ValidateButtons(item, out canOpen);
            return canOpen;
        }
    }
}

