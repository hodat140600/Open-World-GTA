using _GAME._Scripts._ItemManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace _GAME._Scripts
{
    public class VendorItem : MonoBehaviour, ISelectHandler, ISubmitHandler, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {

        public Image itemIcon;
        public Text itemName;
        public Image currencyIcon;
        public Text currencyAmount;

        public int itemId;
        private BaseVendorMenu baseVendorMenu;
        private Vendor vendor;
        private CurrencyManager currencyManager;
        public void Init(int itemId, BaseVendorMenu baseVendorMenu)
        {
            this.itemId = itemId;
            this.baseVendorMenu = baseVendorMenu;
            vendor = GetComponentInParent<Vendor>();
            currencyManager = vendor.user.GetComponent<CurrencyManager>();
            Repaint();
        }
        /// <summary>
        /// Repaints vendor item
        /// </summary>
        public void Repaint()
        {
            Item item = vendor.itemListDataWrapper.itemDataList.items[itemId];
            ItemRef itemRef = vendor.itemListDataWrapper.items[itemId];

            itemIcon.sprite = item.icon;
            itemName.text = item.name;

            currencyIcon.sprite = currencyManager.icon;
            if (baseVendorMenu.GetComponent<VendorBuyMenu>())
                currencyAmount.text = itemRef.buyValue.ToString();
            if (baseVendorMenu.GetComponent<VendorSellMenu>())
                currencyAmount.text = itemRef.sellValue.ToString();
        }



        public void OnSelect(BaseEventData eventData)
        {
            baseVendorMenu.tooltip.SetVisable(true);
            baseVendorMenu.tooltip.Repaint(itemId, baseVendorMenu);
        }

        public void OnSubmit(BaseEventData eventData)
        {
            baseVendorMenu.itemId = itemId;
            baseVendorMenu.ToAmountMenu();
            baseVendorMenu.tooltip.SetVisable(false);
        }

        public void OnDeselect(BaseEventData eventData)
        {
            baseVendorMenu.tooltip.SetVisable(false);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            baseVendorMenu.tooltip.SetVisable(true);
            baseVendorMenu.tooltip.Repaint(itemId, baseVendorMenu);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            baseVendorMenu.tooltip.SetVisable(false);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            baseVendorMenu.itemId = itemId;
            baseVendorMenu.ToAmountMenu();
            baseVendorMenu.tooltip.SetVisable(false);
        }
    }
}