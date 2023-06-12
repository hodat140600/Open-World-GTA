using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _GAME._Scripts._ItemManager
{
    /// <summary>
    /// Attribute of the item including the value (int)
    /// </summary>
    [System.Serializable]
    public class ItemAttribute
    {
        public ItemAttributes name = 0;
        public int value = 0;
        public bool isOpen;
        public bool isBool;
        public string displayFormat
        {
            get
            {
                return name.DisplayFormat();
            }
        }
        /// <summary>
        /// Get attribute text
        /// </summary>
        /// <param name="format">custom format, if null, the format will be <seealso cref=" displayFormat"/></param>
        /// <returns>Formated attribute text</returns>
        public string GetDisplayText(string format = null)
        {
            {
                var _text = string.IsNullOrEmpty(format) ? displayFormat : format;
                if (string.IsNullOrEmpty(_text))
                {
                    _text = name.ToString().InsertSpaceBeforeUpperCase().RemoveUnderline();
                    _text += " : " + value.ToString();
                }
                else
                {
                    if (_text.Contains("(NAME)"))
                    {
                        _text = _text.Replace("(NAME)", name.ToString().InsertSpaceBeforeUpperCase().RemoveUnderline());
                    }

                    if (_text.Contains("(VALUE)"))
                    {
                        _text = _text.Replace("(VALUE)", value.ToString());
                    }
                }
                return _text;
            }
        }
        public ItemAttribute(ItemAttributes name, int value)
        {
            this.name = name;
            this.value = value;
        }
    }

    public static class ItemAttributeHelper
    {
        public static void CopyTo(this ItemAttribute itemAttribute, ItemAttribute to)
        {
            to.isBool = itemAttribute.isBool;
            to.name = itemAttribute.name;
            to.value = itemAttribute.value;
        }

        public static bool Contains(this List<ItemAttribute> attributes, ItemAttributes name)
        {
            var attribute = attributes.Find(at => at.name == name);
            return attribute != null;
        }

        public static ItemAttribute GetAttributeByType(this List<ItemAttribute> attributes, ItemAttributes name)
        {
            var attribute = attributes.Find(at => at.name == name);
            return attribute;
        }

        public static bool Equals(this ItemAttribute attributeA, ItemAttribute attributeB)
        {
            return attributeA.name == attributeB.name;
        }

        public static List<ItemAttribute> CopyAsNew(this List<ItemAttribute> copy)
        {
            var target = new List<ItemAttribute>();

            if (copy != null)
            {
                for (int i = 0; i < copy.Count; i++)
                {
                    ItemAttribute attribute = new ItemAttribute(copy[i].name, copy[i].value);
                    target.Add(attribute);
                }
            }
            return target;
        }
    }
}