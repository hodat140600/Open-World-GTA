using System.Collections.Generic;
using _GAME._Scripts.Inventory;
using _SDK.Shop;
using UnityEngine;

namespace _GAME._Scripts.Shop
{
    public abstract class AbstractTypeShop<T> where T : IShopItem
    {
        [field: SerializeField] public List<T> Items { get; protected set; }
        
        public abstract void Load();
        
        public abstract T DefaultItem { get; }
    }
}