using System.Collections;
using UnityEngine;

namespace _GAME._Scripts._ItemManager
{
    [ClassHeader("OpenClose Inventory Trigger", false)]
    public class OpenCloseInventoryTrigger : ExtendMonoBehaviour
    {
        public bool getComponentsInParent = true;
        public Inventory inventory;
        public ItemManager itemManager;

        public UnityEngine.Events.UnityEvent onOpen, onClose;

        protected virtual IEnumerator Start()
        {
            inventory = getComponentsInParent ? GetComponentInParent<Inventory>() : GetComponent<Inventory>();
            if (!inventory)
            {
                yield return new WaitForEndOfFrame();
                itemManager = getComponentsInParent ? GetComponentInParent<ItemManager>() : GetComponent<ItemManager>();
                if (itemManager) inventory = itemManager.inventory;
            }

            if (inventory) inventory.onOpenCloseInventory.AddListener(OpenCloseInventory);
        }

        public void OpenCloseInventory(bool value)
        {
            if (value) onOpen.Invoke();
            else onClose.Invoke();
        }
    }
}

