using _GAME._Scripts;
using _GAME._Scripts._ItemManager;
using System.Collections;
using UnityEngine;

public class LoadInventoryItemsExample : MonoBehaviour
{
    GameController gm;

    void Start()
    {
        gm = GetComponent<GameController>();
    }

    public void LoadItemsToInventory()
    {
        if (!gm) return;
        StartCoroutine(LoadItems());
    }

   IEnumerator LoadItems()
    {
        yield return new WaitForSeconds(.1f);
        var loadItems = gm.currentPlayer.GetComponent<ItemManager>();
        loadItems.LoadItemsExample();
    }
}
