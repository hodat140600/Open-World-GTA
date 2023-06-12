using System;
using System.Collections;
using System.Collections.Generic;
using _GAME._Scripts.Inventory;
using MyBox;
using UnityEngine;
using UnityEngine.UI;

namespace _GAME._Scripts.Shop
{
    public class WeaponTypeButton : MonoBehaviour
    {
        [field: SerializeField] public Button Button { get; private set; }
        [SerializeField] private Image    avatar;
        [SerializeField] private Animator animator;

        private static readonly int       IsSelectedHash = Animator.StringToHash("isSelected");
        private                 Transform _transform;

        // store the first weapon in this weapon type shop
        // public Weapon       firstWeapon = null;
        public List<WeaponButton> weaponButtonList;
        public WeaponButton       defaultWeaponButton;
        public WeaponButton       activatingWeaponButton;

        public WeaponTypeShop shop { get; private set; }

        public bool isSelected { get; private set; }

        private void Awake()
        {
            _transform = transform;
        }

        public WeaponTypeButton Init(Transform parent, WeaponTypeShop shop, Action OnClick)
        {
            this.shop = shop;
            _transform.SetParent(parent);
            _transform.localScale    = Vector3.one;
            _transform.localPosition = Vector3.zero;
            _transform.name          = shop.WeaponType.ToString();

            avatar.sprite = shop.Avatar;

            Button.onClick.AddListener(() => OnClick());
            return this;
        }

        public void Reload()
        {
            foreach (WeaponButton weaponButton in weaponButtonList)
            {
                weaponButton.UpdateOwnedIconState();
                if (weaponButton.Weapon.IsSelected)
                {
                    defaultWeaponButton    = weaponButton;
                    activatingWeaponButton = weaponButton;
                }
            }
        }

        public void Select()
        {
            isSelected = true;
            animator.SetBool(IsSelectedHash, isSelected);

            if (defaultWeaponButton != null)
            {
                activatingWeaponButton = defaultWeaponButton;
                StartCoroutine(SelectAfter(.25f));
            }
        }

        public void Deselect()
        {
            isSelected = false;
            animator.SetBool(IsSelectedHash, isSelected);
            if (activatingWeaponButton != null)
                activatingWeaponButton.Deselect();
        }

        private IEnumerator SelectAfter(float t)
        {
            yield return new WaitForSeconds(t);
            activatingWeaponButton.Select();
        }
    }
}