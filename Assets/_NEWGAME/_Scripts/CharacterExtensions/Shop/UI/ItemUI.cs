using _GAME._Scripts._ItemManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace _GAME._Scripts
{
    public class ItemUI : MonoBehaviour
    {
        public Image icon;
        public Text amountText;
        /// <summary>
        /// Repaints the item
        /// </summary>
        public void Repaint(Item item, int amount)
        {
            icon.sprite = item.icon;
            amountText.text = amount.ToString();
        }
    }
}