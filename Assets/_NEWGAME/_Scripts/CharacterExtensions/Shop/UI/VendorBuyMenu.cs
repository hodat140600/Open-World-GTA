using _GAME._Scripts._ItemManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace _GAME._Scripts
{
    public class VendorBuyMenu : BaseVendorMenu
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
            amount = 1;
            amountText.text = amount.ToString();
            amountItemUI.Repaint(vendor.itemListDataWrapper.itemDataList.items[itemId], amount);
            RepaintTotalText();
            amountMenu.gameObject.SetActive(true);
            itemsMenu.gameObject.SetActive(false);
            confirmMenu.gameObject.SetActive(false);
        }
        public override void ToConfirmMenu()
        {
            ItemRef itemRef = vendor.itemListDataWrapper.items[itemId];
            if (amount * itemRef.buyValue <= currencyManager.amount)
            {
                confirmItemUI.Repaint(vendor.itemListDataWrapper.itemDataList.items[itemId], amount);

                amountMenu.gameObject.SetActive(false);
                itemsMenu.gameObject.SetActive(false);
                confirmMenu.gameObject.SetActive(true);
                confirmChatBubble.AddMessage(confirmMsg, textSpeed);
            }
            else
            {
                generalChatBubble.AddMessage(inefficientMsg, textSpeed);
            }

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
            totalText.color = amount * itemRef.buyValue > currencyManager.amount ? Color.red : Color.white;
            totalText.text = (amount * itemRef.buyValue).ToString();
        }
        public override void Populate()
        {
            if (vendorItems.Count > 0)
            {
                for (int i = vendorItems.Count - 1; i >= 0; i--)
                    Destroy(vendorItems[i]);

                vendorItems.Clear();
            }

            for (int i = 0; i < vendor.items.Count; i++)
            {
                var go = Instantiate(vendorItemPrefab) as GameObject;
                go.transform.SetParent(vendorItemListContainer, false);
                go.GetComponent<VendorItem>().Init(vendor.items[i].id, this);
                vendorItems.Add(go);

            }
            if (vendorItems.Count > 0)
            {
                SetSelectable(vendorItems[0]);
            }

        }

        public override void Confirm()
        {
            Item itemToAdd = vendor.itemListDataWrapper.itemDataList.items[itemId];
            ItemRef itemToAddRef = vendor.itemListDataWrapper.items[itemId];
            ItemManager itemManager = vendor.user.GetComponent<ItemManager>();
            CurrencyManager currencyManager = vendor.user.GetComponent<CurrencyManager>();

            currencyManager.Sub(amount * itemToAddRef.buyValue);
            ItemReference itemRef = new ItemReference(itemId);
            if (itemToAdd.stackable)
            {
                if (amount > itemToAdd.maxStack)
                {
                    int remainder = amount;
                    while (remainder > 0)
                    {
                        if (amount > itemToAdd.maxStack)
                        {
                            itemRef.amount = itemToAdd.maxStack;
                            remainder -= itemToAdd.maxStack;
                            itemManager.AddItem(itemRef);
                        }
                        else
                        {
                            itemRef.amount = remainder;
                            remainder -= remainder;
                            itemManager.AddItem(itemRef);
                        }
                    }
                }
                else
                {
                    itemRef.amount = amount;
                    itemManager.AddItem(itemRef);
                }
            }
            else
            {
                for (int i = 0; i < amount; i++)
                {
                    itemManager.AddItem(itemRef);
                }
            }
            generalChatBubble.AddMessage(successfulMsg, textSpeed);
            ToItemsMenu();
            vendor.onBuyItem.Invoke();

        }


    }
}