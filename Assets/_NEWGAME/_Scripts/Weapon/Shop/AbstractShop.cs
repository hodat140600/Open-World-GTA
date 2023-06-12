using System.Collections.Generic;
using System.Linq;
using _GAME._Scripts.Inventory;
using _SDK.Entities;
using _SDK.Shop;
using Assets._SDK.Game;
using Assets._SDK.Inventory.Interfaces;
using UnityEngine;

namespace Assets._SDK.Shop
{
    public abstract class AbstractShop<T> where T : IShopItem
    {
        public List<T> Items { get; protected set; }

        public abstract void Load();

        private T Get(int itemId)
        {
            return Items.First(item => item.Id == itemId);
        }


        public void Select(int itemId)
        {
            T selectedItem = Items.Find(item => item.Id == itemId);
            selectedItem.Selected();
        }

        // public IEntity Deselect()
        // {
        //     T activatingItem = Items.Find(item => item.IsSelected);
        //
        //     if (activatingItem == null) return null;
        //
        //     activatingItem?.DeActivate();
        //
        //     return activatingItem;
        // }

        internal bool Buy(IShopItem item)
        {
            bool isCreditSuccess = AbstractGameManager.Instance.Wallet.Credit(item.Price);

            if (isCreditSuccess)
            {
                item.Bought();
            }

            return isCreditSuccess;
        }
    }
}