using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace _GAME._Scripts
{
    public class VendorMainMenu : MonoBehaviour
    {
        public Transform greetingMenu;
        public Transform shopMenu;
        public Transform mainMenu;
        public ChatBubble chatBubble;
        public float textSpeed = 0.05f;
        public UnityEvent onSelectGreetingMenu;
        public UnityEvent onSelectShopMenu;

        public VendorBuyMenu vendorBuyMenu;
        public VendorSellMenu vendorSellMenu;

        [HideInInspector] public Vendor vendor;
        private void Start()
        {
            vendor = GetComponentInParent<Vendor>();
        }
        /// <summary>
        /// Turns on greeting menu
        /// </summary>
        public void ToGreetingMenu()
        {
            chatBubble.messageText.text = "";
            onSelectGreetingMenu.Invoke();
            greetingMenu.gameObject.SetActive(true);
            shopMenu.gameObject.SetActive(false);
            chatBubble.AddMessage("Hello traveler, come in and browse my wears.", textSpeed);
        }
        /// <summary>
        /// Turns on shop menu
        /// </summary>
        public void ToShopMenu()
        {
            onSelectShopMenu.Invoke();
            greetingMenu.gameObject.SetActive(false);
            shopMenu.gameObject.SetActive(true);
        }
        /// <summary>
        /// Returns to game
        /// </summary>
        public void ToGame()
        {
            vendor.CloseVendor();
        }

        public void DisableChatBubbleAnimations()
        {
            chatBubble.StopAnimation();
            vendorBuyMenu.generalChatBubble.StopAnimation();
            vendorBuyMenu.confirmChatBubble.StopAnimation();
            vendorSellMenu.generalChatBubble.StopAnimation();
            vendorSellMenu.confirmChatBubble.StopAnimation();
        }

        public void SetMainMenuVisable(bool value)
        {
            mainMenu.gameObject.SetActive(value);
        }

    }
}