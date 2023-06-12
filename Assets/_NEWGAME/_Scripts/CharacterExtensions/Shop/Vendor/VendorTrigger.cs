using _GAME._Scripts._ItemManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace _GAME._Scripts
{
    public class VendorTrigger : MonoBehaviour
    {
        private Vendor vendor;
        public UnityEvent OnPlayerEnter;
        public UnityEvent OnPlayerExit;
        bool interacted = false;

        private void Start()
        {
            vendor = GetComponentInParent<Vendor>();
        }

        public void OnTriggerEnter(Collider other)
        {
            if (!interacted && other.gameObject.tag == "Player")
            {
                vendor.user = other.gameObject;
                OnPlayerEnter.Invoke();
                interacted = true;
            }
        }


        private void LateUpdate()
        {
            if (!vendor.user)
                return;

            if (vendor.openVendor.GetButtonDown() && !vendor.user.GetComponent<ItemManager>().inventory.isOpen)
            {
                if (!vendor.isOpen)
                    vendor.OpenVendor();
            }
            if (vendor.cancel.GetButtonDown() && !vendor.user.GetComponent<ItemManager>().inventory.isOpen)
                if (vendor.isOpen)
                    vendor.CloseVendor();

        }

        public void OnTriggerExit(Collider other)
        {
            if (interacted && other.gameObject.tag == "Player")
            {
                vendor.user = null;
                OnPlayerExit.Invoke();
                interacted = false;
            }
        }
    }
}