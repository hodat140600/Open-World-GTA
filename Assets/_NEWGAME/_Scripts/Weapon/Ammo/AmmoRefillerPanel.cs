using _GAME._Scripts._CharacterController._Shooter;
using _GAME._Scripts._ItemManager;
using _GAME._Scripts.Game;
using System;
using System.Collections.Generic;
using Assets._SDK.Ads;
using Assets._SDK.Analytics;
using MyBox;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

namespace _GAME._Scripts.Inventory.Ammo
{
    public class AmmoRefillerPanel : MonoBehaviour
    {
        public static Action<WeaponType> OnOutOfAmmo;
        public static Action<WeaponType, int> OnTakenAmmo;

        [SerializeField] private Animator animator;
        [SerializeField] private TMP_Text ammoText, cashText;
        [SerializeField] private Button buyByAdsButton, buyByCashButton, cancelButton, buyByCashInactiveButton;

        private static readonly int IsOpeningHash = Animator.StringToHash("isOpening");




        #region Ammo Event
        private List<CheckAmmoEvent> checkAmmoEvents = new List<CheckAmmoEvent>();
        private void HandleCheckAmmoEvents()
        {
            var events = checkAmmoEvents.FindAll(e => e.ammoCompare == CheckAmmoEvent.AmmoCompare.Equals && _shooterManager.ExtraAmmo.Equals(e.ammoToCheck) ||
                                                        e.ammoCompare == CheckAmmoEvent.AmmoCompare.HigherThan && _shooterManager.ExtraAmmo > e.ammoToCheck ||
                                                        e.ammoCompare == CheckAmmoEvent.AmmoCompare.LessThan && _shooterManager.ExtraAmmo < e.ammoToCheck);
            for (int i = 0; i < events.Count; i++)
            {
                events[i].OnCheckAmmo.Invoke();
            }
        }

        [Serializable]
        public class CheckAmmoEvent
        {
            public int ammoToCheck;
            public bool disableEventOnCheck;

            public CheckAmmoEvent(int ammoToCheck, bool disableEventOnCheck = false, AmmoCompare ammoCompare = AmmoCompare.Equals)
            {
                this.ammoToCheck = ammoToCheck;
                this.disableEventOnCheck = disableEventOnCheck;
                this.ammoCompare = ammoCompare;
                OnCheckAmmo = new UnityEvent();
            }

            public enum AmmoCompare
            {
                Equals,
                HigherThan,
                LessThan
            }

            public AmmoCompare ammoCompare;

            public UnityEvent OnCheckAmmo;
        }
        #endregion


        private ShooterManager _shooterManager;
        private bool isShowedPanel = false;
        private const float HALFSECOND = 0.5f;
        private void OnEnable()/*() => OnOutOfAmmo?.Invoke((WeaponType)Enum.ToObject(typeof(WeaponType), _shooterManager.CurrentWeapon.ammoID)*/
        {
            OnOutOfAmmo += ShowPanel;
            _shooterManager = Gameplay.Instance.ItemManager.GetComponent<ShooterManager>();
            CheckAmmoEvent checkAmmoEvent = new CheckAmmoEvent(0);
            checkAmmoEvent.OnCheckAmmo.AddListener(() => OnOutOfAmmo?.Invoke((WeaponType)Enum.ToObject(typeof(WeaponType), _shooterManager.CurrentWeapon.ammoID)));
            checkAmmoEvents.Add(checkAmmoEvent);
            _shooterManager.onOutAmmoWeapon = new ShooterManager.OnShootWeapon();
            _shooterManager.onOutAmmoWeapon.AddListener((CurrentWeapon) =>
            {
                if (!isShowedPanel)
                    HandleCheckAmmoEvents();
            });
            cancelButton.onClick.AddListener(HidePanel);
        }

        private void TogglePanel()
        {
            isShowedPanel = !isShowedPanel;
        }

        private void OnDisable()
        {
            OnOutOfAmmo -= ShowPanel;
        }

        private void ShowPanel(WeaponType weaponType)
        {
            TogglePanel();
            AmmoData ammoData = GameManager.Instance.Resources.WeaponResources.WeaponTypeToAmmoData[weaponType];

            // ammoText.SetText($"Refill {ammoData.Name} ammo");
            ammoText.SetText($"Refill ammo");
            cashText.SetText(GameFormat.Cash(ammoData.Cash) + " Refill");

            buyByAdsButton.onClick.RemoveAllListeners();
            buyByAdsButton.onClick.AddListener(() => { BuyByAds(weaponType, ammoData.AmmoAmount); });

            buyByCashButton.onClick.RemoveAllListeners();
            buyByCashButton.onClick.AddListener(() => { BuyByCash(weaponType, ammoData.AmmoAmount, ammoData.Cash); });

            buyByCashInactiveButton.gameObject.SetActive(!GameManager.Instance.Wallet.DefaultAccount.CanCredit(ammoData.Cash));

            Gameplay.Instance.Fire(GameplayTrigger.Pause);
            animator.SetBool(IsOpeningHash, true);
        }

        private void BuyByCash(WeaponType weaponType, int amount, int cash)
        {
            if (GameManager.Instance.Wallet.Credit(cash))
            {
                CollectAndReloadAmmo((int)weaponType, amount);
                OnTakenAmmo?.Invoke(weaponType, amount);
                HidePanel();
            }
        }

        private void BuyByAds(WeaponType weaponType, int amount)
        {
            AdsManager.Instance.ShowRewarded(result =>
            {
                if (result == AdsResult.Success)
                {
                    CollectAndReloadAmmo((int)weaponType, amount);
                    OnTakenAmmo?.Invoke(weaponType, amount);
                    HidePanel();
                }
            }, levelIndex: Gameplay.Instance.MissionManager.CurrentMission.order, placement: AnalyticParamKey.REWARD_REFILL);


        }

        private void HidePanel()
        {
            animator.SetBool(IsOpeningHash, false);
            Gameplay.Instance.Fire(GameplayTrigger.Unpause);
            Invoke(nameof(TogglePanel), HALFSECOND);
        }

        private void CollectAndReloadAmmo(int weaponType, int amount)
        {
            Gameplay.Instance.CollectAmmo(weaponType, amount);
            _shooterManager.ReloadWeapon();
        }
    }
}