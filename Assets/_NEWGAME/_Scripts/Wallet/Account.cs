using System;
using UniRx;
using _SDK.Entities;
using UnityEngine;
using Assets._SDK.Entities;

namespace _SDK.Money
{
    public enum Currency
    {
        Cash    = 0,
        Diamond = 1,
        Ads     = 2,
        // Medkit  = 3, // yes medkit is a currency
    }

    [Serializable]
    public class Account : AbstractEntity
    {
        public Currency Currency { get; private set; }
        public override int Id => (nameof(Account) + Name).GetHashCode();

        public ReactiveProperty<float> Balance { get; private set; }

        public Account(Currency currency)
        {
            Currency = currency;
            Name     = nameof(Account) + currency;
            Balance  = new ReactiveProperty<float>(0);
        }

        public Account(Currency currency, float value) : this(currency)
        {
            Currency      = currency;
            Name          = nameof(Account) + currency;
            Balance.Value = value;
        }

        public void Deposit(float addedValue)
        {
            Balance.Value += addedValue;
            Save();
        }

        public bool Credit(float reducingValue)
        {
            if (!CanCredit(reducingValue)) return false;

            Balance.Value -= reducingValue;

            Save();
            return true;
        }

        public bool CanCredit(float reducingValue) => Balance.Value >= reducingValue;

        public void Save(string keyPrefix = "")
        {
            PlayerPrefs.SetFloat(keyPrefix + Id, Balance.Value);
        }

        public void LoadPlayerPrefs(string keyPrefix = "")
        {
            Balance.Value = PlayerPrefs.GetFloat(keyPrefix + Id, 0);
        }
    }
}