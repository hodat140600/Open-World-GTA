using System;
using System.Collections.Generic;
using System.Linq;
using _GAME._Scripts._ItemManager;
using _GAME._Scripts.Game;
using _GAME._Scripts.UI.Lobby.Shop;
using DG.Tweening;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace _GAME._Scripts.Inventory.Bonus
{
    public class LootPanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text weaponName;
        [SerializeField] private Button   takeItButton, tryItButton, cancelButton;
        [SerializeField] private Image    weaponImage;

        [SerializeField] private Animator    animator;
        [SerializeField] private CanvasGroup buttonsParent;

        private static readonly int IsOpeningHash = Animator.StringToHash("isOpening");

        private void Start()
        {
            cancelButton.onClick.AddListener(HidePanel);

            Gameplay.Instance.MissionManager.PreviousMissionState.Where(state => state == MissionState.DayEnded)
                .Subscribe(_ => RemoveTryingWeapon()).AddTo(this);
        }

        private void RemoveTryingWeapon()
        {
            Gameplay.Instance.LoadTommy();
        }

        private void HidePanel()
        {
            animator.SetBool(IsOpeningHash, false);
            Gameplay.Instance.Fire(GameplayTrigger.Unpause);
        }

        public void ShowPanel(Weapon weapon)
        {
            takeItButton.onClick.RemoveAllListeners();
            takeItButton.onClick.AddListener(() => GetThisWeapon(weapon));

            tryItButton.onClick.RemoveAllListeners();
            tryItButton.onClick.AddListener(() => TryThisWeapon(weapon));

            weaponImage.sprite = weapon.ImageFrontYellowGlow;
            weaponName.text    = weapon.Name;

            Gameplay.Instance.Fire(GameplayTrigger.Pause);
            animator.SetBool(IsOpeningHash, true);

            ShowTryAndCancelButtonsLater();
        }

        private ItemManager ItemManager => GameManager.Instance.ItemManager;
        private WeaponResources Resources => GameManager.Instance.Resources.WeaponResources;

        private void GetThisWeapon(Weapon item)
        {
            var Shop = new WeaponShop();
            Shop.BuyByAds(item, () =>
            {
                Equip(item);
                Shop.EquipImplicit(item);
                GameManager.Instance.ItemManager.SaveItemsExample();
                HidePanel();
            });
        }

        private List<ItemReference> tryingItemRefs;

        private void Equip(Weapon item)
        {
            tryingItemRefs ??= new();

            ItemReference itemRef = new(item.Id);
            itemRef.indexArea      = 0;
            itemRef.autoEquip      = true;
            itemRef.addToEquipArea = true;
            tryingItemRefs.Add(itemRef);

            ItemManager.AddItem(itemRef);

            EquipArea equipArea = ItemManager.inventory.equipAreas.FirstOrDefault();

            int type = ((int)Resources.GetWeaponSettings(item.Id).Entity.weaponType);
            ItemManager.inventory.equipAreas.FirstOrDefault()?.AddItemToEquipSlot(type, ItemManager.items.FirstOrDefault(_ => _.id == item.Id), true);

            if (equipArea)
                equipArea.EquipCurrentSlot();
        }

        private void TryThisWeapon(Weapon item)
        {
            Equip(item);
            HidePanel();
        }

        private void ShowTryAndCancelButtonsLater()
        {
            buttonsParent.interactable = false;
            buttonsParent.alpha        = 0;
            buttonsParent.DOFade(1, .5f).SetDelay(3f).SetUpdate(true)
                .OnComplete(() => { buttonsParent.interactable = true; });
        }
    }
}