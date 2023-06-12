using _GAME._Scripts._ItemManager;
using System.Collections.Generic;
using UnityEngine;

public class ItemDisplay3D : MonoBehaviour
{
    [System.Serializable]
    public class ExtendDisplay
    {
        public int itemId;
        public GameObject itemModel;
    }
    public GameObject currentItemModel;
    public List<ExtendDisplay> displays;
    public void Display(ItemSlot slot)
    {
       if(slot) Display(slot.item);
    }

    public void Display(int id)
    {
        ExtendDisplay display = displays.Find(d => d.itemId.Equals(id));
        if(display!=null)
        {
            if (currentItemModel) currentItemModel.SetActive(false);
            display.itemModel.SetActive(true);
            currentItemModel = display.itemModel;
        }
    }

    public void Display(Item item)
    {
        if(item)Display(item.id);
    }
}
