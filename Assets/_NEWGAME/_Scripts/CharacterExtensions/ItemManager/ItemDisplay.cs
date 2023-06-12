﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace _GAME._Scripts._ItemManager
{
    [ClassHeader("ItemDisplay")]
    public class ItemDisplay : ExtendMonoBehaviour
    {
        [EditorToolbar("Settings")]
        public Image icon;
        public Text itemName;
        public Text type;
        public Text amount;
        [EditorToolbar("ColorByType")]
        public bool useColorByType;
        public MaskableGraphic bgColor;
        public ColorByType[] colorByTypes;


        [System.Serializable]
        public class ColorByType
        {
            public List<ItemType> types;
            public Color color = Color.white;
            public ColorByType()
            {
                Color color = Color.white;
            }
        }


        [HelpBox("Use {0} inside string format to inset the value")]

        public string nameFormat = "Name: {0}";
        public string typeFormat = "Type: {0}";
        public string amountFormat = "Amount: {0}";
        public bool displayAmountOnlyGreaterOne = true;
        public void DisplayItem(ItemManager.CollectedItemInfo info)
        {
            if (useColorByType)
            {
                var colorType = System.Array.Find(colorByTypes, c => c.types.Contains(info.item.type));
                if (colorType != null)
                    bgColor.color = colorType.color;
            }

            icon.sprite = info.item.icon;
            itemName.text = FormatText(nameFormat, info.item.name);
            type.text = FormatText(typeFormat, info.item.type.ToString());
            if (info.amount > 1 || !displayAmountOnlyGreaterOne)
            {
                amount.text = FormatText(amountFormat, info.amount.ToString());
            }
            else amount.text = "";
        }

        public string FormatText(string format, string value)
        {
            if (string.IsNullOrEmpty(format)) return value;

            return string.Format(format, value);
        }
    }
}