using System;
using _GAME._Scripts.Game;
using _GAME._Scripts.Inventory;
using Assets._SDK.UI;
using DG.Tweening;
using Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using GameObject = UnityEngine.GameObject;

namespace _SDK.Shop
{
    public class ItemDetailsPanel : AbstractEntityPanel<IShopItem>
    {
        [SerializeField] private Image  frontImage;
        [SerializeField] private Slider damage0to300Slider, damage301to600Slider, damage601to1000Slider, fireRateSlider;

        [SerializeField] private TMP_Text    weaponName, weaponPrice,    ammoAmountText;
        [SerializeField] private CanvasGroup buyButton, buyInactiveButton , buyByAdsButton, equipButton, equippedButton;

        [SerializeField] private CanvasGroup wholeBuy, wholeEquip;
        // [SerializeField] private CanvasGroup EquippedImage, UnequippedImage;

        [SerializeField] private TextMeshProUGUI _description;
        [SerializeField] private GameObject _descriptionPanel;

        private Weapon Weapon => (Weapon)_item;

        public override void SetData(IShopItem item)
        {
            base.SetData(item);
            DataChanged();

            if (Weapon.Damage > 300)
            {
                damage301to600Slider.SlideToValue(Weapon.Damage, 1f).SetDelay(.3f);
                damage601to1000Slider.SlideToValue(Weapon.Damage, .9f).SetDelay(.6f);
            }
            else
            {
                damage601to1000Slider.SlideToValue(Weapon.Damage, 1f).SetDelay(.3f);
                damage301to600Slider.SlideToValue(Weapon.Damage, .9f).SetDelay(.6f);
            }

            damage0to300Slider.SlideToValue(Weapon.Damage);
            fireRateSlider.SlideToValue(1f / Weapon.FireRate);
            weaponName.ChangeTextTo(Weapon.Name, 25f);
            weaponPrice.ChangeTextTo(GameFormat.Cash(Weapon.Price.Balance), 25f);
            frontImage.FadeToSprite(Weapon.ImageFront);
        
            if (Weapon.weaponType == WeaponType.Skin)
            {
                _descriptionPanel.SetActive(true);
                _description.SetText(Weapon.Description);
            }
            else
            {
                _descriptionPanel.SetActive(false);
                int ammoAmount = GameManager.Instance.Resources.WeaponResources.WeaponTypeToAmmoData[Weapon.weaponType].AmmoAmount;
                ammoAmountText.ChangeTextTo(ammoAmount.ToString(), 30f);
            }


        }

        public virtual void SetEventHandler(
            Func<IShopItem, bool> Buy, Action<IShopItem> BuyByAds, Action<IShopItem> BuyAmmo, Action<IShopItem> Equip)
        {
            buyButton.GetComponent<Button>().onClick.AddListener(() => Buy(_item));
            equipButton.GetComponent<Button>().onClick.AddListener(() => Equip(_item));
            buyByAdsButton.GetComponent<Button>().onClick.AddListener(() => BuyByAds(_item));
        }

        private bool bought, equipped, canBuyAmmo;

        public override void DataChanged()
        {
            bought   = Weapon.IsBought;
            equipped = Weapon.IsActivated;

            wholeBuy.ShowIf(!bought);
            wholeEquip.ShowIf(bought);

            buyButton.ShowIf(!bought && !equipped);
            buyInactiveButton.ShowIf(!GameManager.Instance.Wallet.DefaultAccount.CanCredit(Weapon.Price.Balance));

            equipButton.ShowIf(bought && !equipped);
            equippedButton.ShowIf(bought && equipped);


            // EquippedImage.ShowIf(bought && equipped);
            // UnequippedImage.ShowIf(!bought || !equipped);
        }
    }
}