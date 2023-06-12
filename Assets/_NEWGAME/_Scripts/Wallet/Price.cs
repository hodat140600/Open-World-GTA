using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _SDK.Money
{
    [Serializable]
    public struct Price
    {
        [HideLabel, HorizontalGroup("inline",.3f)]
        public float Balance;

        [HideLabel, HorizontalGroup("inline"), EnumToggleButtons, GUIColor(214f / 255f, 202f / 255f, 26f / 255f)]
        public Currency Currency;

        public Price(Currency currency, float balance)
        {
            Currency = currency;
            Balance  = balance;
        }

        public override string ToString()
        {
            return $"{Balance}";
        }
    }
}