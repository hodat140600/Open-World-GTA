using _GAME._Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace _GAME._Scripts._ItemManager
{
    [ClassHeader("Item Collected Display", helpBoxText = "Use this to display the name of collected items", openClose = false)]
    public class ItemCollectionDisplay : ExtendMonoBehaviour
    {
        private static ItemCollectionDisplay instance;
        /// <summary>
        /// Instance of the <seealso cref="ItemCollectionDisplay"/>
        /// </summary>
        /// 
        public static ItemCollectionDisplay Instance
        {
            get
            {
                if (instance == null) { instance = FindObjectOfType<ItemCollectionDisplay>(); }
                return instance;
            }
        }

        public ItemCollectionTextHUD itemCollectedDiplayPrefab;

        /// <summary>
        /// Send text to display in the HUD with fade in <seealso cref="ItemCollectionTextHUD"/>
        /// </summary>
        /// <param name="message">message to show</param>
        /// <param name="timeToStay">time to stay showing</param>
        /// <param name="timeToFadeOut">time to fade out</param>
        public void FadeText(string message, float timeToStay, float timeToFadeOut)
        {
            var display = Instantiate(itemCollectedDiplayPrefab);
            display.transform.SetParent(transform, false);
            display.transform.SetAsFirstSibling();
            display.Show(message, timeToStay, timeToFadeOut);
        }
    }
}

