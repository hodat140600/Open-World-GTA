using System;
using _GAME._Scripts.Game;
using _SDK.Money;
using _SDK.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _GAME._Scripts
{
    public class WastedPanel : AbstractPanel
    {
        [SerializeField] private Button   ReviveByMoneyButton, ReviveByAdsButton;
        [SerializeField] private TMP_Text WastedDescription;

        private const string EndOfDateDescription = "Date ended, but you still haven't killed all of them";
        private const string OutOfHpDescription   = "Out of HP";
        private const string UnknownDescription   = "You died somehow";

        private void Start()
        {
            ReviveByMoneyButton.onClick.AddListener(() =>
            {
                //if (GameManager.Instance.Wallet.Credit(2000))
                Revive();
            });

            ReviveByAdsButton.onClick.AddListener(() =>
            {
                // if (GameManager.Instance.Wallet.Credit(new Price(Currency.Ads, 1)))
                Revive();
            });
        }

        private void UpdateWastedDescription(GameplayTrigger reason) =>
            WastedDescription.text = reason switch
            {
                // GameplayTrigger.OutOfHp   => OutOfHpDescription,
                // GameplayTrigger.EndOfDay => EndOfDateDescription,
                _                         => UnknownDescription
            };
        
        private void Revive()
        {
            Gameplay.Instance.MissionManager.Fire(MissionTrigger.Retry);
        }

        private void OnDisable()
        {
//            Gameplay.Instance.WastedReason -= UpdateWastedDescription;
        }
    }
}