using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace _GAME._Scripts
{
    public class PlayerCurrencyDisplay : MonoBehaviour
    {
        public Text amountText;
        public Image icon;

        public void Start()
        {
            var currencyManager = GetComponentInParent<CurrencyManager>();
            if (currencyManager == null)
                Debug.LogError(gameObject.name + "Cant find a currency manager");

            amountText.text = currencyManager.amount.ToString();
            icon.sprite = currencyManager.icon;
            currencyManager.onAmountChanged.AddListener(Repaint);
        }
        /// <summary>
        /// Repaints the display
        /// </summary>
        public void Repaint(int amount)
        {
            amountText.text = amount.ToString();
        }
    }
}

