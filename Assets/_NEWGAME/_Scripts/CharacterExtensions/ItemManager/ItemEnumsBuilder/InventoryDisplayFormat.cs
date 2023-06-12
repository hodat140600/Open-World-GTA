using _GAME._Scripts._ItemManager;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using UnityEngine;

public static class InventoryDisplayFormat
{
    static readonly List<string> ItemTypeFormats = new List<string>();
    static readonly List<string> ItemAttributeFormats = new List<string>();

    /// <summary>
    /// Get Item type string format using Description in <seealso cref="_GAME._Scripts._ItemManager.ItemType"/> value
    /// </summary>
    /// <param name="value">target Item type</param>
    /// <returns></returns>
    public static string DisplayFormat(this ItemType value)
    {
        if (ItemTypeFormats.Count == 0)
        {

            var values = System.Enum.GetValues(typeof(ItemType)).OfType<ItemType>().ToArray();
            for (int i = 0; i < values.Length; i++)
            {
                ItemType v = values[i];
                ItemTypeFormats.Add(GetDisplayFormat(v));
            }
        }
        return ItemTypeFormats[(int)value];
    }

    /// <summary>
    /// Get Item Attribute string format using Description in <seealso cref="_GAME._Scripts._ItemManager.ItemAttributes"/> value  
    /// </summary>
    /// <param name="value">target Item Attribute</param>
    /// <returns></returns>
    public static string DisplayFormat(this ItemAttributes value)
    {
        if (ItemAttributeFormats.Count == 0)
        {
            var values = System.Enum.GetValues(typeof(ItemAttributes)).OfType<ItemAttributes>().ToArray();
            for (int i = 0; i < values.Length; i++)
            {
                ItemAttributes v = values[i];
                ItemAttributeFormats.Add(GetDisplayFormat(v));
            }
        }
        return ItemAttributeFormats[(int)value];
    }

    static string GetDisplayFormat<T>(this T value) where T : System.Enum
    {
        return
        value
        .GetType()
        .GetMember(value.ToString())
        .FirstOrDefault()
        ?.GetCustomAttribute<DescriptionAttribute>()
        ?.Description
        ?? value.ToString();
    }

}
