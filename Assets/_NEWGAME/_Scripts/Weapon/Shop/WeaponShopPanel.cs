using System;
using System.Collections;
using System.Linq;
using _GAME._Scripts._ItemManager;
using _GAME._Scripts.Game;
using _GAME._Scripts.Inventory;
using _GAME._Scripts.UI.Lobby.Shop;
using _NEWGAME._Scripts.Game;
using _SDK.Shop;
using Assets._SDK.Logger;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Extensions;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _GAME._Scripts.Shop
{
    public class WeaponShopPanel : AbstractShoppingPanel<WeaponShop>
    {
        [SerializeField] private Transform weaponTypeListHolder, weaponListHolder;

        [SerializeField] private ItemDetailsPanel itemDetailsPanel;

        [SerializeField] private WeaponTypeButton weaponTypeButtonPrefab;
        [SerializeField] private WeaponButton weaponButtonPrefab;
        [SerializeField] private Transform itemListPrefab;

        private WeaponResources Resources => GameManager.Instance.Resources.WeaponResources;
        private SkinManager SkinManager => GameManager.Instance.SkinManager;

        private ItemManager ItemManager => GameManager.Instance.ItemManager;

        private static readonly float FADE_ANIMATION_TIME_LENGTH = .04f;
        private static readonly float TIME_BETWEEN_ITEM_SHOW = .05f;

        private Transform defaultWeaponTypeHolder;
        private Transform currentWeaponListHolder;

        private WeaponTypeButton defaultWeaponTypeButton;
        private WeaponTypeButton currentWeaponTypeButton;

        public bool loadingItemList = false;

        private void Start()
        {
            Shop = new WeaponShop();
            Shop.Load();

            defaultWeaponTypeButton = null;
            defaultWeaponTypeHolder = null;

            LoadButtons();
            ConfigureStates();
            OnEquipFirstLoad();

            itemDetailsPanel.SetEventHandler(
                Buy: Buy,
                BuyByAds: BuyByAds,
                BuyAmmo: null, //BuyAmmo,
                Equip: Equip
            );

            StartCoroutine(OnLoadDefaultWeapon());
        }


        private void ConfigureStates()
        {
            GameManager.Instance.CurrentState
                .Where(v => v == GameState.LobbyHome)
                .Where(_ => GameManager.Instance.PreviousState.Value != GameState.LobbySettings)
                .Subscribe(_ => StartCoroutine(ShowDefaultShopListAfter(.25f)))
                .AddTo(gameObject);
        }

        private IEnumerator ShowDefaultShopListAfter(float t)
        {
            yield return new WaitForSeconds(t);

            if (currentWeaponTypeButton != null)
                currentWeaponTypeButton.Deselect();

            ShowShopList(defaultWeaponTypeButton, defaultWeaponTypeHolder);
        }

        protected bool Buy(IShopItem item)
        {
            bool isTransactionSuccessful = Shop.Buy(item);

            if (isTransactionSuccessful)
            {
                AddWeaponToInvetory(item.Id, false);
                Equip(item);
                currentWeaponTypeButton.activatingWeaponButton.UpdateOwnedIconState();
                OnPaySuccess(item.Id);
            }
            else
                OnPayFailed(item.Id);

            return isTransactionSuccessful;
        }

        private void BuyByAds(IShopItem item)
        {
            var Shop = new WeaponShop();
            Shop.BuyByAds(item, () =>
            {
                AddWeaponToInvetory(item.Id, false);
                Equip(item);
                currentWeaponTypeButton.activatingWeaponButton.UpdateOwnedIconState();
                OnPaySuccess(item.Id);
            });


            return;
        }

        private IEnumerator OnLoadDefaultWeapon()
        {
            yield return new WaitUntil(() => ItemManager.inventory != null);
            yield return new WaitUntil(() => ItemManager.inventory.equipAreas != null);
            if (ItemManager.items.Count == 0)
            {
                foreach (WeaponType type in Enum.GetValues(typeof(WeaponType)))
                {
                    if (type == WeaponType.Skin) continue;
                    int id = Resources.GetDefaultWeapon(type).Id;
                    AddWeaponToInvetory(id, true);
                    int weaponType = ((int)Resources.GetWeaponSettings(id).Entity.weaponType);
                    AddItemToEquipSlot(id, weaponType, false);
                }

            }

            //EquipByWeaponType(ItemManager.inventory.equipAreas.FirstOrDefault(), (int)WeaponType.Handgun);
        }
        private void OnEquipFirstLoad()
        {
            ItemManager.onLoadItems.AddListener(() =>
            {
                EquipByWeaponType(ItemManager.inventory.equipAreas.FirstOrDefault(), (int)WeaponType.Handgun);
            });
        }

        private void Equip(IShopItem item)
        {
            Shop.Equip(item);
            OnItemSelected(item.Id);
            //AddWeaponToEquipSlot(item.Id);

            if(Resources.GetWeaponSettings(item.Id).Entity.weaponType == WeaponType.Skin)
            {
                SkinManager.EquipSkin((Weapon)item);
            }
            else
            {
                SetUpEquipArea(ItemManager.inventory.equipAreas.FirstOrDefault(), item.Id);
                GameManager.Instance.ItemManager.SaveItemsExample();
            }

        }

        protected override void OnPayFailed(int itemId)
        {
        }

        protected override void OnPaySuccess(int itemId)
        {
            GameManager.Instance.ItemManager.SaveItemsExample();
            itemDetailsPanel.DataChanged();
        }

        protected override void OnItemSelected(int itemId)
        {
            itemDetailsPanel.DataChanged();
            currentWeaponTypeButton.Reload();
        }

        private void AddWeaponToInvetory(int itemId, bool isIgnore)
        {
            ItemReference itemRef = new ItemReference(itemId);
            itemRef.indexArea = 0;
            itemRef.autoEquip = false;
            itemRef.addToEquipArea = true;

            ItemManager.AddItem(itemRef, isIgnore);
            //SetUpEquipArea(ItemManager.inventory.equipAreas.FirstOrDefault(), itemId);
        }

        private void SetUpEquipArea(EquipArea equipArea, int itemId)
        {
            int type = ((int)Resources.GetWeaponSettings(itemId).Entity.weaponType);
            AddItemToEquipSlot(itemId, type, true);
            equipArea.EquipCurrentSlot();
        }

        private void AddItemToEquipSlot(int itemId, int type, bool autoEquip)
        {
            ItemManager.inventory.equipAreas.FirstOrDefault()?.AddItemToEquipSlot(type, ItemManager.items.FirstOrDefault(_ => _.id == itemId), autoEquip);
        }

        private void EquipByWeaponType(EquipArea equipArea, int type)
        {
            equipArea.indexOfEquippedItem = type;
            equipArea.SetEquipSlot(type);
            //if (equipArea.currentEquippedSlot.isValid)
            equipArea.EquipCurrentSlot();
        }

        private void SetDetails(IShopItem item)
        {
            itemDetailsPanel.SetData(item);
            SfxManager.Instance.Play(Sounds.GunSelect);
        }

        private void ShowItemList(WeaponTypeButton button, Transform itemList)
        {
            loadingItemList = true;
            button.Button.interactable = false;

            Sequence seq = DOTween.Sequence();
            if (currentWeaponListHolder != null)
            {
                foreach (Transform item in currentWeaponListHolder)
                {
                    seq.Append(FadeItem(item, 1, 0, false));
                    seq.AppendInterval(FADE_ANIMATION_TIME_LENGTH);
                }

                seq.AppendInterval(TIME_BETWEEN_ITEM_SHOW);
                seq.AppendCallback(() => currentWeaponListHolder.gameObject.SetActive(false));
            }

            seq.AppendCallback(() =>
            {
                itemList.gameObject.SetActive(true);
                SetDetails(button.defaultWeaponButton.Weapon);

                button.Select();
                Shop.SelectTypeShop(button.shop.WeaponType);
            });

            foreach (Transform item in itemList)
            {
                seq.Append(FadeItem(item, 0, 1, true));
                seq.AppendInterval(FADE_ANIMATION_TIME_LENGTH);
            }

            seq.AppendCallback(() =>
            {
                currentWeaponListHolder = itemList;
                button.Button.interactable = true;
                loadingItemList = false;
            });

            seq.Play();
        }

        private TweenerCore<float, float, FloatOptions> FadeItem(Transform item, float fromVal, float toVal, bool clickable)
        {
            CanvasGroup canvasGroup = item.GetComponent<CanvasGroup>();
            canvasGroup.interactable = clickable;
            canvasGroup.DOKill();
            return canvasGroup.DOFade(toVal, .0125f).From(fromVal);
        }

        private void LoadButtons()
        {
            weaponListHolder.DestroyAllChildren();
            weaponTypeListHolder.DestroyAllChildren();

            bool foundFirstEquippedWeaponInShop = false;
            foreach (WeaponTypeShop weaponTypeShop in Shop.WeaponTypeShops.Values)
            {
                Transform weaponListHolder = CreateWeaponListHolder(weaponTypeShop);

                WeaponTypeButton weaponTypeButton = Instantiate(weaponTypeButtonPrefab);

                weaponTypeButton.Init(weaponTypeListHolder, weaponTypeShop, () =>
                {
                    if (!loadingItemList)
                    {
                        if(weaponTypeShop.WeaponType != WeaponType.Skin)
                            EquipByWeaponType(ItemManager.inventory.equipAreas.FirstOrDefault(), (int)weaponTypeShop.WeaponType);
                        if (weaponTypeButton.isSelected) return;

                        if (currentWeaponTypeButton != null)
                            currentWeaponTypeButton.Deselect();
                        weaponTypeButton.Reload();
                        ShowShopList(weaponTypeButton, weaponListHolder);
                    }
                });

                foreach (Weapon item in weaponTypeShop)
                {
                    WeaponButton weaponButton = Instantiate(weaponButtonPrefab, weaponListHolder);
                    weaponTypeButton.weaponButtonList.Add(weaponButton);
                    weaponButton.Init(item, () =>
                    {
                        if (weaponButton.isSelected) return;

                        if (weaponTypeButton.activatingWeaponButton != null)
                            weaponTypeButton.activatingWeaponButton.Deselect();

                        weaponButton.Select();
                        weaponTypeButton.activatingWeaponButton = weaponButton;

                        SetDetails(item);
                    });


                    // For each weapontype, if weaponytype doesn't have any equipped weapon
                    // then show first weapon of that weapontype.
                    weaponTypeButton.defaultWeaponButton ??= weaponButton;

                    // For each weapontype select if this is equipped weapon.
                    if (weaponButton.Weapon.IsSelected)
                    {
                        weaponTypeButton.defaultWeaponButton = weaponButton;
                        //First equipped weapon has higher priority than
                        //default weapon from weapon resource
                        if (!foundFirstEquippedWeaponInShop)
                        {
                            foundFirstEquippedWeaponInShop = true;
                            SetDefaultWeaponOfTheShop(weaponTypeButton, weaponListHolder, weaponButton);
                        }
                    }

                    // True if match default weapon from weapon resource.
                    if (weaponButton.Weapon.Id == Resources.GetDefaultWeapon(weaponButton.Weapon.weaponType).Id && !foundFirstEquippedWeaponInShop)
                        SetDefaultWeaponOfTheShop(weaponTypeButton, weaponListHolder, weaponButton);
                }

                weaponListHolder.gameObject.SetActive(false);
            }
        }

        private void SetDefaultWeaponOfTheShop(WeaponTypeButton weaponTypeButton, Transform weaponListHolder, WeaponButton weaponButton)
        {
            weaponTypeButton.defaultWeaponButton = weaponButton;
            defaultWeaponTypeButton = weaponTypeButton;
            defaultWeaponTypeHolder = weaponListHolder;
        }

        private Transform CreateWeaponListHolder(WeaponTypeShop weaponTypeShop)
        {
            Transform weaponListHolder = Instantiate(itemListPrefab, this.weaponListHolder);
            weaponListHolder.name = weaponTypeShop.WeaponType.ToString();
            return weaponListHolder;
        }


        private void ShowShopList(WeaponTypeButton weaponTypeButton, Transform weaponListHolder)
        {
            currentWeaponTypeButton = weaponTypeButton;
            ShowItemList(weaponTypeButton, weaponListHolder);
        }
    }
}