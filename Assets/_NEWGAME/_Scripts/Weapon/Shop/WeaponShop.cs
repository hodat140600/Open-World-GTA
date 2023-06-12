using System;
using System.Collections.Generic;
using _GAME._Scripts.Inventory;
using _GAME._Scripts.Game;
using _SDK.Shop;
using Assets._SDK.Ads;
using Assets._SDK.Game;
using Assets._SDK.Shop;
using UnityEngine;

namespace _GAME._Scripts.UI.Lobby.Shop
{
    public class WeaponShop : AbstractShop<IShopItem>
    {
        public WeaponTypeShop SelectingTypeShop { get; private set; }

        public Dictionary<WeaponType, WeaponTypeShop> WeaponTypeShops { get; private set; }

        public override void Load()
        {
            var weaponResources = GameManager.Instance.Resources.WeaponResources;

            if (weaponResources == null)
            {
                Debug.Log("weaponResources is null");
            }
            else if (weaponResources.WeaponSettingsByType == null)
            {
                Debug.Log("weaponResources.WeaponSettingsByType is null");
                weaponResources.LoadFromSerializedList();
            }

            WeaponTypeShops = new Dictionary<WeaponType, WeaponTypeShop>();
            foreach (KeyValuePair<WeaponType, List<WeaponSettings>> typeShop in weaponResources.WeaponSettingsByType)
            {
                if (typeShop.Value.Count == 0) continue;
                WeaponTypeShops.Add(typeShop.Key, new WeaponTypeShop(typeShop.Value));
            }
        }

        public void SelectTypeShop(WeaponType weaponType) => SelectingTypeShop = WeaponTypeShops[weaponType];

        // public bool BuyAmmo(IShopItem item)
        // {
        //     bool isCreditSuccess = AbstractGameManager.Instance.Wallet.Credit(item.AmmoPrice);
        //
        //     if (isCreditSuccess)
        //         item.BoughtAmmo();
        //
        //     return isCreditSuccess;
        // }

        public void Equip(IShopItem item)
        {
            var currentActivatedWeapon = SelectingTypeShop.ActivatingWeapon;

            // does not do anything if item is already activated
            if (currentActivatedWeapon == item) return;

            // does not deactivate if there is no current activated weapon
            if (currentActivatedWeapon != default)
                currentActivatedWeapon.DeActivate();

            // lastly activate new item
            item.Selected();
        }

        public void BuyByAds(IShopItem item, Action OnSuccess)
        {
            AdsManager.Instance.ShowRewarded(result =>
            {
                if (result != AdsResult.Success) return;
                item.Bought();
                OnSuccess();
            });
        }

        public void EquipImplicit(Weapon item)
        {
            if (WeaponTypeShops == null)
                Load();

            var typeShop               = WeaponTypeShops[item.weaponType];
            var currentActivatedWeapon = typeShop.ActivatingWeapon;

            // does not do anything if item is already activated
            if (currentActivatedWeapon == item) return;

            // does not deactivate if there is no current activated weapon
            if (currentActivatedWeapon != default)
                currentActivatedWeapon.DeActivate();

            // lastly activate new item
            item.Selected();
        }

        public void Try(Weapon item)
        {
            // if (WeaponTypeShops == null)
            //     Load();

            var typeShop               = WeaponTypeShops[item.weaponType];
            var currentActivatedWeapon = typeShop.ActivatingWeapon;

            // does not do anything if item is already activated
            if (currentActivatedWeapon == item) return;

            // does not deactivate if there is no current activated weapon
            if (currentActivatedWeapon != default)
                currentActivatedWeapon.DeActivate();

            // lastly activate new item
            item.Selected();
        }
    }
}