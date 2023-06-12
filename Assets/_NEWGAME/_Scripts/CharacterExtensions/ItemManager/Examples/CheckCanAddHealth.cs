using System.Collections.Generic;

namespace _GAME._Scripts._ItemManager
{
    using _GAME._Scripts;
    using _GAME._Scripts._CharacterController;
    [ClassHeader("Check if can Add Health", "Simple Example to verify if the health item can be used based on the character's health is full or not.", openClose = false)]
    public class CheckCanAddHealth : ExtendMonoBehaviour
    {
        public ItemManager itemManager;
        public ThirdPersonController tpController;
        public bool getInParent = true;
        internal bool canUse;
        internal bool firstRun;

        private void Start()
        {
            // first we need to access our itemManager from the Controller
            if (itemManager == null)
            {
                //check 'getInParent' if this script is attached to a children of the itemManager
                if (getInParent)
                    itemManager = GetComponentInParent<ItemManager>();
                else
                    itemManager = GetComponent<ItemManager>();
            }

            // now we access the Controller itself to know the currentHealth later
            if (tpController == null)
            {
                if (getInParent)
                    tpController = GetComponentInParent<ThirdPersonController>();
                else
                    tpController = GetComponent<ThirdPersonController>();
            }

            // if a itemManager is founded, we use this event to call our CanUseItem method 
            if (itemManager)
            {
                itemManager.canUseItemDelegate -= new ItemManager.CanUseItemDelegate(CanUseItem);
                itemManager.canUseItemDelegate += new ItemManager.CanUseItemDelegate(CanUseItem);
            }
        }

        private void OnDestroy()
        {
            var itemManager = GetComponent<ItemManager>();
            if (itemManager)
                // remove the event when this gameObject is destroyed
                itemManager.canUseItemDelegate -= new ItemManager.CanUseItemDelegate(CanUseItem);
        }

        private void CanUseItem(Item item, ref List<bool> validateResult)
        {
            // search for the attribute 'Health' 
            if (item.GetItemAttribute(ItemAttributes.Health) != null)
            {
                // the variable valid will identify if the currentHealth is lower than the maxHealth, allowing to use the item
                var valid = tpController.CurrentHealth < tpController.maxHealth;
                if (valid != canUse || !firstRun)
                {
                    canUse = valid;
                    firstRun = true;
                    // trigger a custom text if there is a HUDController in the scene
                    HUDController.instance.ShowText(valid ? "Increase health" : "Can't use " + item.name + " because your health is full", 4f);
                }

                if (!valid)
                    validateResult.Add(valid);
            }
        }
    }
}