using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace _GAME._Scripts._ItemManager
{
    public class ItemCollectionDisplay_v2 : MonoBehaviour
    {
        public ItemManager itemManager;
        public ItemDisplay displayPrefab;
        public RectTransform content;
        public float displayTime = 3f;
        private void Start()
        {
            itemManager.onCollectItem.AddListener(OnAddItem);
        }

        private void OnAddItem(ItemManager.CollectedItemInfo info)
        {
            var display = Instantiate(displayPrefab);
            display.transform.SetParent(content, false);
            display.DisplayItem(info);
            display.transform.SetAsFirstSibling();
            Destroy(display.gameObject, displayTime);
        }
    }
}