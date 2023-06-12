using System.Collections;
using _GAME._Scripts.Game;
using _SDK.UI;
using DG.Tweening;
using Extensions;
using MyBox;
using TMPro;
using UniRx;
using UnityEngine;

namespace _GAME._Scripts.Wallet.UI
{
    public class CurrencyPanel : AbstractPanel
    {
        [SerializeField] private TMP_Text  moneyText;
        [SerializeField] private Transform _transform;

        private int currentValue;

        private IEnumerator Start()
        {
            yield return new WaitUntil(() => GameManager.Instance.Wallet != null);
            currentValue = GameManager.Instance.Wallet.DefaultAccount.Balance.Value.RoundToInt();
            GameManager.Instance.Wallet.DefaultAccount.Balance.Subscribe(UpdateText).AddTo(this);
        }

        private void UpdateText(float value)
        {
            int  newActualValue = value.RoundToInt();
            bool isAdding       = newActualValue > 0;
            int  c              = currentValue;

            if (isAdding)
            {
                DOVirtual.Int(currentValue, newActualValue, Mathf.Min((float)(newActualValue - currentValue) / 500, 3), i =>
                {
                    _transform.DOScale(c == i ? 1f : 1.15f, .3f);
                    c = i;

                    moneyText.SetText($"{i}");
                }).SetEase(Ease.OutExpo).OnComplete(() => _transform.DOScale(1, .3f));
            }
            else
            {
                moneyText.ChangeTextTo($"{newActualValue}", 20f);
            }

            currentValue = newActualValue;
        }
    }
}