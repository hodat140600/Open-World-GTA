using _GAME._Scripts._ItemManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace _GAME._Scripts
{
    public class CurrencyItem : MonoBehaviour
    {

        public int amount;
        /// <summary>
        /// Collect Item
        /// </summary>
        public void OnCollect(GameObject other)
        {
            CurrencyManager currencyManager = other.GetComponent<CurrencyManager>();
            currencyManager.Add(amount);
            ItemCollectionDisplay.Instance.FadeText("Acquired:" + amount + " " + currencyManager.name, 4, 0.25f);
            Destroy(gameObject);
        }

    }
}

