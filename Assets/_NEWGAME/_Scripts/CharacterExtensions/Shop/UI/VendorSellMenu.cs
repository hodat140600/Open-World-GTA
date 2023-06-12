using _GAME._Scripts._ItemManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace _GAME._Scripts
{
    public class VendorSellMenu : BaseVendorMenu
    {
        public void Init()
        {
            vendor = GetComponentInParent<Vendor>();
            if (vendor == null)
                Debug.LogError("Cant Find Vendor Component");
            else
            {
                currencyManager = vendor.user.GetComponent<CurrencyManager>();
                itemsCurrencyDisplay.Init(currencyManager);
                amountCurrencyDisplay.Init(currencyManager);
                confirmCurrencyDisplay.Init(currencyManager);
                Populate();
                ToItemsMenu();

            }
        }

        private void OnEnable()
        {
            Init();
        }

        public override void ToItemsMenu()
        {
            itemsMenu.gameObject.SetActive(true);
            amountMenu.gameObject.SetActive(false);
            confirmMenu.gameObject.SetActive(false);
            //StartCoroutine(SetSelectableHandle(vendorItems[0]));
        }
        public override void ToAmountMenu()
        {
            ItemManager itemManager = vendor.user.GetComponent<ItemManager>();
            Item item = itemManager.items.Find(i => i.id == itemId);
            amount = 1;
            clampValue = item.amount;
            amountText.text = amount.ToString();
            amountItemUI.Repaint(vendor.itemListDataWrapper.itemDataList.items[itemId], amount);
            RepaintTotalText();
            amountMenu.gameObject.SetActive(true);
            itemsMenu.gameObject.SetActive(false);
            confirmMenu.gameObject.SetActive(false);
        }
        public override void ToConfirmMenu()
        {
            confirmItemUI.Repaint(vendor.itemListDataWrapper.itemDataList.items[itemId], amount);

            amountMenu.gameObject.SetActive(false);
            itemsMenu.gameObject.SetActive(false);
            confirmMenu.gameObject.SetActive(true);
            confirmChatBubble.AddMessage(confirmMsg, textSpeed);
        }

        public override void DecreaseAmount()
        {
            base.DecreaseAmount();
            RepaintTotalText();
        }
        public override void IncreaseAmount()
        {
            base.IncreaseAmount();
            RepaintTotalText();
        }
        public override void RepaintTotalText()
        {
            ItemRef itemRef = vendor.itemListDataWrapper.items[itemId];
            totalText.color = Color.green;
            totalText.text = (amount * itemRef.sellValue).ToString();
        }
        public override void Populate()
        {
            ItemManager itemManager = vendor.user.GetComponent<ItemManager>();
            if (vendorItems.Count > 0)
            {
                for (int i = vendorItems.Count - 1; i >= 0; i--)
                    Destroy(vendorItems[i]);

                vendorItems.Clear();
            }

            for (int i = 0; i < itemManager.items.Count; i++)
            {
                var go = Instantiate(vendorItemPrefab) as GameObject;
                go.transform.SetParent(vendorItemListContainer, false);
                go.GetComponent<VendorItem>().Init(itemManager.items[i].id, this);
                vendorItems.Add(go);

            }

            if (vendorItems.Count > 0)
            {
                SetSelectable(vendorItems[0]);
            }

        }

        public override void Confirm()
        {
            ItemManager itemManager = vendor.user.GetComponent<ItemManager>();
            Item itemToRemove = itemManager.items.Find(i => i.id == itemId);
            ItemRef itemToRemoveRef = vendor.itemListDataWrapper.items[itemId];
            CurrencyManager currencyManager = vendor.user.GetComponent<CurrencyManager>();

            currencyManager.Add(amount * itemToRemoveRef.sellValue);
            if (itemToRemove.stackable)
            {
                if (amount < itemToRemove.amount)
                {
                    itemToRemove.amount -= amount;
                }
                else if (amount >= itemToRemove.amount)
                {
                    itemManager.DestroyItem(itemToRemove);
                }
            }
            else
            {
                itemManager.DestroyItem(itemToRemove);
            }
            generalChatBubble.AddMessage(successfulMsg, textSpeed);
            Populate();
            ToItemsMenu();
            vendor.onSellItem.Invoke();
        }


    }
}