using _GAME._Scripts._ItemManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace _GAME._Scripts
{
    public class BaseVendorMenu : MonoBehaviour
    {
        [Header("Item Info")]
        public int itemId;
        public int amount = 1;
        public int clampValue = 1000;
        [Header("Menu Panels")]
        public Transform itemsMenu;
        public Transform amountMenu;
        public Transform confirmMenu;
        [Header("Items Menu")]
        public GameObject vendorItemPrefab;
        public Transform vendorItemListContainer;
        public CurrencyDisplay itemsCurrencyDisplay;
        [Header("Amount Menu")]
        public Text amountText;
        public Text totalText;
        public ItemUI amountItemUI;
        public CurrencyDisplay amountCurrencyDisplay;
        [Header("Confirm Menu")]

        public ChatBubble confirmChatBubble;
        public ItemUI confirmItemUI;
        public CurrencyDisplay confirmCurrencyDisplay;
        protected List<GameObject> vendorItems = new List<GameObject>();
        [Header("Tooltip")]
        public Tooltip tooltip;
        [Header("Messages")]
        public ChatBubble generalChatBubble;
        public float textSpeed = 0.05f;
        public string inefficientMsg = "You do not have sufficient currency.";
        public string successfulMsg = "Nice doing business with you.";
        public string confirmMsg = "Are you sure ?";
        protected Vendor vendor;
        protected CurrencyManager currencyManager;
        /// <summary>
        /// Turns on items menu
        /// </summary>
        public virtual void ToItemsMenu() { }
        /// <summary>
        /// Turns on amount menu
        /// </summary>
        public virtual void ToAmountMenu() { }
        /// <summary>
        /// Turns on confirm menu
        /// </summary>
        public virtual void ToConfirmMenu() { }
        /// <summary>
        /// Increase the amount of items
        /// </summary>
        public virtual void IncreaseAmount()
        {
            amount++;
            amount = Mathf.Clamp(amount, 1, clampValue);
            amountText.text = amount.ToString();
        }
        /// <summary>
        /// Decrease the amount of items
        /// </summary>
        public virtual void DecreaseAmount()
        {
            amount--;
            amount = Mathf.Clamp(amount, 1, clampValue);
            amountText.text = amount.ToString();
        }
        /// <summary>
        /// Repaints total text
        /// </summary>
        public virtual void RepaintTotalText() { }
        /// <summary>
        /// Sets Selectable
        /// </summary>
        public virtual void SetSelectable(GameObject target)
        {
            if (vendorItemListContainer.GetComponent<SetFirstSelectable>())
            {
                vendorItemListContainer.GetComponent<SetFirstSelectable>().firstSelectable = target;
            }
            else
            {
                vendorItemListContainer.gameObject.AddComponent<SetFirstSelectable>();
                vendorItemListContainer.GetComponent<SetFirstSelectable>().firstSelectable = target;
            }
        }
        /// <summary>
        /// Confirm function
        /// </summary>
        public virtual void Confirm() { }
        /// <summary>
        /// Populates vendor
        /// </summary>
        public virtual void Populate() { }
    }
}