using _GAME._Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace _GAME._Scripts._ItemManager
{
    [System.Serializable]
    public class ItemFilter
    {
        //  [vHelpBox("if true, the filter will validate just item types that is in filter list else  will validate the item types out of filter list")]
        public bool invertFilterResult;
        public List<ItemType> filter;

        public ItemFilter()
        {
            filter = new List<ItemType>();
        }
        public ItemFilter(bool invertFilterResult = false, params ItemType[] itemTypesToFilter)
        {
            if (itemTypesToFilter != null && itemTypesToFilter.Length > 0)
                filter = itemTypesToFilter.dToList();
            else filter = new List<ItemType>();
            this.invertFilterResult = invertFilterResult;
        }
        public bool Validate(Item item)
        {
            if (item == null) return false;
            return invertFilterResult ? !filter.Contains(item.type) : filter.Contains(item.type);
        }
    }
}