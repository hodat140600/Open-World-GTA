using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace _GAME._Scripts
{
    public class CurrencyManager : MonoBehaviour
    {
        public new string name;
        public int amount;
        public Sprite icon;

        public OnValueChanged onAmountChanged;
        /// <summary>
        /// Add's currency
        /// </summary>
        public void Add(int amount)
        {
            this.amount += amount;
            onAmountChanged.Invoke(this.amount);
        }
        /// <summary>
        /// Subtracts currency
        /// </summary>
        public void Sub(int amount)
        {
            this.amount -= amount;
            onAmountChanged.Invoke(this.amount);
        }
    }
    [System.Serializable]
    public class OnValueChanged : UnityEvent<int> { }
}