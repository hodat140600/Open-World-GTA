using System.Collections.Generic;
using _SDK.UI;
using Assets._SDK.Shop;
using Assets._SDK.UI;

namespace _SDK.Shop
{
    public abstract class AbstractShoppingPanel<T> : AbstractPanel where T : AbstractShop<IShopItem>
    {
        protected T Shop { get; set; }
        protected List<AbstractEntityPanel<IShopItem>> ShopItemPanels { get; set; }
        public List<IShopItem> Items { get; protected set; }

        public AbstractEntityPanel<IShopItem> GetPanelBy(int itemId)
        {
            if (ShopItemPanels == null || ShopItemPanels.Count == 0) return null;

            return ShopItemPanels.Find(panel => panel.ItemId == itemId);
        }
        
        //
        // protected void BuyByAds(IShopItem item)
        // {
        //     // AdsManager.Instance.AdsClient.ShowRewardedVideo(0, "Shop " + item.Name, (result) =>
        //     // {
        //     //     bool isTransactionSuccessful = result == RocketSg.Sdk.AdsClient.ShowResult.Finished;
        //     //
        //     //     if (isTransactionSuccessful)
        //     //         OnPaySuccess(item.Id);
        //     //     else
        //     //         OnPayFailed(item.Id);
        //     // });
        // }

        protected void SelectItem(IShopItem item)
        {
            OnItemSelected(item.Id);
        }

        protected abstract void OnPayFailed(int itemId);

        protected abstract void OnPaySuccess(int itemId);


        protected abstract void OnItemSelected(int itemId);
    }
}