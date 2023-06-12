using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEngine.Events;
using _GAME._Scripts._CharacterController;
using _GAME._Scripts._ItemManager;

namespace _GAME._Scripts
{
    public class Vendor : MonoBehaviour
    {

        public ItemListDataWrapper itemListDataWrapper;
        public GenericInput openVendor = new GenericInput("E", "Start", "Start");
        public GenericInput horizontal = new GenericInput("HorizontalArrow", "D-Pad Horizontal", "D-Pad Horizontal");
        public GenericInput vertical = new GenericInput("VerticalArrow", "D-Pad Vertical", "D-Pad Vertical");
        public GenericInput submit = new GenericInput("Submit", "A", "A");
        public GenericInput cancel = new GenericInput("Cancel", "B", "B");

        public UnityEvent onOpenVendor;
        public UnityEvent onCloseVendor;
        public UnityEvent onBuyItem;
        public UnityEvent onSellItem;


        [HideInInspector]
        [SerializeField]
        public List<ItemRef> items = new List<ItemRef>();
        [HideInInspector]
        public bool isOpen;
        [HideInInspector]
        public GameObject user;

        private List<string> canvasNames = new List<string>() { "UI", "dUI", "ThrowManager", "Inventory", "AimCanvas" };
        private List<Canvas> invectorCanvas = new List<Canvas>();
        private StandaloneInputModule inputModule;
        private VendorMainMenu vendorMenu;
        private _ItemManager.Inventory inventory;
        private new Camera camera;
        private void Start()
        {

            FindCanvas();
            inputModule = FindObjectOfType<StandaloneInputModule>();

            if (inputModule == null)
                inputModule = new GameObject("EventSystem").AddComponent<StandaloneInputModule>();
            vendorMenu = GetComponentInChildren<VendorMainMenu>();

            camera = GetComponentInChildren<Camera>();
            camera.gameObject.SetActive(false);
        }

        /// <summary>
        /// Disables player
        /// </summary>
        public virtual void OpenVendor()
        {
            if (isOpen) return;
            if (!user.GetComponent<CurrencyManager>())
            {
                Debug.LogError("There is no Currency Manager Component on the player.");
                return;
            }
            if (!user.GetComponent<ItemManager>())
            {
                Debug.LogError("There is no Item Manager Component on the player.");
                return;
            }
            UpdateEventSystemInput();

            isOpen = true;
            _ItemManager.Inventory inventory = user.GetComponent<ItemManager>().inventory;
            inventory.lockInventoryInput = true;
            inventory.openInventory.useInput = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            camera.gameObject.SetActive(true);
            user.GetComponent<ThirdPersonInput>().lockInput = true;
            DisableCanvas();
            vendorMenu.SetMainMenuVisable(true);
            vendorMenu.ToGreetingMenu();

            onOpenVendor.Invoke();
        }

        /// <summary>
        /// Enables player
        /// </summary>
        public virtual void CloseVendor()
        {
            if (!isOpen) return;

            isOpen = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            camera.gameObject.SetActive(false);
            _ItemManager.Inventory inventory = user.GetComponent<ItemManager>().inventory;
            inventory.lockInventoryInput = false;
            inventory.openInventory.useInput = true;
            user.GetComponent<ThirdPersonInput>().lockInput = false;
            EnableCanvas();
            vendorMenu.DisableChatBubbleAnimations();
            vendorMenu.SetMainMenuVisable(false);
            onCloseVendor.Invoke();
        }

        protected virtual void UpdateEventSystemInput()
        {

            if (inputModule)
            {
                inputModule.horizontalAxis = horizontal.buttonName;
                inputModule.verticalAxis = vertical.buttonName;
                inputModule.submitButton = submit.buttonName;
                inputModule.cancelButton = cancel.buttonName;
            }
            else
            {
                inputModule = FindObjectOfType<StandaloneInputModule>();
            }
        }

        void FindCanvas()
        {
            Canvas[] allCanvas = FindObjectsOfType<Canvas>();


            foreach (Canvas canvas in allCanvas)
            {
                if (canvasNames.Contains(canvas.name))
                    invectorCanvas.Add(canvas);
            }

        }
        /// <summary>
        /// Disables player canases
        /// </summary>
        void DisableCanvas()
        {

            for (int i = 0; i < invectorCanvas.Count; i++)
            {
                if (!invectorCanvas[i].GetComponent<CanvasGroup>())
                    invectorCanvas[i].gameObject.AddComponent<CanvasGroup>();

                invectorCanvas[i].targetDisplay = 1;
                invectorCanvas[i].GetComponent<CanvasGroup>().interactable = false;
                invectorCanvas[i].GetComponent<CanvasGroup>().blocksRaycasts = false;
            }

        }
        /// <summary>
        /// Ensables player canases
        /// </summary>
        void EnableCanvas()
        {
            for (int i = 0; i < invectorCanvas.Count; i++)
            {
                if (!invectorCanvas[i].GetComponent<CanvasGroup>())
                    invectorCanvas[i].gameObject.AddComponent<CanvasGroup>();

                invectorCanvas[i].targetDisplay = 0;
                invectorCanvas[i].GetComponent<CanvasGroup>().interactable = true;
                invectorCanvas[i].GetComponent<CanvasGroup>().blocksRaycasts = true;
            }

        }
    }
}