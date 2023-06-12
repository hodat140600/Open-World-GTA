using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace _GAME._Scripts
{
    public class CurrencyDisplay : MonoBehaviour
    {
        public Text amountText;
        public Image icon;

        public void Init(CurrencyManager currencyManager)
        {
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