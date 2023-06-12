using System;
using _GAME._Scripts.Inventory;
using DG.Tweening;
using Extensions;
using MyBox;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _GAME._Scripts.Shop
{
    public class WeaponButton : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private Image  sideImage;
        
        [SerializeField] private Image  backgroundImage;
        [SerializeField] private Image  selectingBackgroundImage;
        
        [SerializeField] private Image       ownedIcon;
        [SerializeField] private Animator    animator;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private Sprite      selectedBackgroundSprite;
        [SerializeField] private Sprite      deselectedBackgroundSprite;

        [SerializeField] private TMP_Text equippedText;

        private static readonly int       IsSelectedHash = Animator.StringToHash("isSelected");
        private                 Transform _transform;
        public Weapon Weapon { get; private set; }

        public bool isSelected { get; private set; }


        public WeaponButton Init(Weapon item, Action OnClick)
        {
            Weapon = item;
            UpdateOwnedIconState();
            _transform               = transform;
            _transform.localScale    = Vector3.one;
            _transform.localPosition = Vector3.zero;
            _transform.name          = Weapon.Id.ToString();

            sideImage.sprite         = Weapon.ImageSide;
            canvasGroup.interactable = false;
            canvasGroup.alpha        = 0;

            button.onClick.AddListener(() => OnClick());
            return this;
        }

        public void UpdateOwnedIconState()
        {
            // ownedIcon.SetAlpha(Weapon.IsActivated ? 1 : Weapon.IsOwned ? .2f : 0);
            DataChanged();
        }

        public void Select()
        {
            isSelected = true;
            DataChanged();
            // animator.SetBool(IsSelectedHash, isSelected);
        }

        public void Deselect()
        {
            isSelected = false;
            DataChanged();
            // animator.SetBool(IsSelectedHash, isSelected);
        }

        public void DataChanged()
        {
            selectingBackgroundImage.ShowIf(isSelected);
            backgroundImage.ShowIf(!isSelected);
            ownedIcon.ShowIf(Weapon.IsOwned);
            equippedText.ShowIf(Weapon.IsActivated);
        }
    }
}