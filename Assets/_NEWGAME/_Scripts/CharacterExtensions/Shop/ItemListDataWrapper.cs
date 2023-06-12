using _GAME._Scripts._ItemManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Shop/Vendor/New Item List Wrapper")]
public class ItemListDataWrapper : ScriptableObject
{
    public ItemListData itemDataList;
    public List<ItemRef> items = new List<ItemRef>();

    public void PullItemDataList()
    {

        if (items.Count <= 0)
        {
            List<ItemRef> temp = new List<ItemRef>();
            for (int i = 0; i < itemDataList.items.Count; i++)
            {
                temp.Add(new ItemRef(itemDataList.items[i].name, itemDataList.items[i].id, itemDataList.items[i].icon, itemDataList.items[i].type, false));
            }
            items = temp;
        }

        else if (items.Count < itemDataList.items.Count || items.Count > itemDataList.items.Count)
        {
            List<ItemRef> temp = new List<ItemRef>();
            for (int i = 0; i < itemDataList.items.Count; i++)
            {
                temp.Add(new ItemRef(itemDataList.items[i].name, itemDataList.items[i].id, itemDataList.items[i].icon, itemDataList.items[i].type, false));
            }

            for (int i = 0; i < items.Count; i++)
            {
                ItemRef itemRef = temp.Find(item => item.id == items[i].id);
                if(itemRef != null)
                {
                    itemRef.buyValue = items[i].buyValue;
                    itemRef.sellValue = items[i].sellValue;
                    itemRef.blocked = items[i].blocked;
                }
            }
            items = temp;
        }

      

    }
  
}
[System.Serializable]
public class ItemRef
{
    public string name;
    public int id = -1;
    public Sprite icon;
    public ItemType type;
    public int sellValue;
    public int buyValue;
    public bool blocked;

    public ItemRef() { }
    public ItemRef(string name, int id, Sprite icon, ItemType type, bool blocked) { this.name = name; this.id = id; this.icon = icon; this.type = type; this.blocked = blocked; }
    public ItemRef(string name, int id, Sprite icon, ItemType type, bool blocked, int sellValue, int buyValue) { this.name = name; this.id = id; this.icon = icon; this.type = type; this.blocked = blocked; this.sellValue = sellValue; this.buyValue = buyValue; }
    public ItemRef(ItemRef item) { this.name = item.name; this.id = item.id; this.icon = item.icon; this.type = item.type; this.blocked = item.blocked; this.sellValue = item.sellValue; this.buyValue = item.buyValue; }
}