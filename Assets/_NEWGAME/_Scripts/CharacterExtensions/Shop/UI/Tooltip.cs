using _GAME._Scripts._ItemManager;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
namespace _GAME._Scripts
{
    public class Tooltip : MonoBehaviour
    {
        public Text infoTxt;
        public Transform menu;
        private ItemListDataWrapper itemListDataWrapper;

        private void Awake()
        {
            itemListDataWrapper = GetComponentInParent<Vendor>().itemListDataWrapper;
        }
        /// <summary>
        /// Enable/Disable tooltip
        /// </summary>
        public void SetVisable(bool value)
        {
            menu.gameObject.SetActive(value);
        }
        /// <summary>
        /// Repaints tooltip
        /// </summary>
        public void Repaint(int id, BaseVendorMenu baseVendorMenu)
        {
            StringBuilder builder = new StringBuilder();
            Item item = itemListDataWrapper.itemDataList.items[id];
            ItemRef itemRef = itemListDataWrapper.items[id];
            CurrencyManager currencyManager = GetComponentInParent<Vendor>().user.GetComponent<CurrencyManager>();
            builder.Append("<color=#C38000><size=12>").Append(item.name).Append("</size></color>").AppendLine();
            foreach (ItemAttribute attribute in item.attributes)
            {
                builder.Append(attribute.name).Append(": ").Append(attribute.value).AppendLine();
            }
            if (item.type == ItemType.Consumable)
            {
                builder.Append("<color=green><size=9>").Append("Use:").Append(item.description).Append("</size></color>").AppendLine();
            }
            else
            {
                builder.Append("<color=#B7A583><size=9>").Append(item.description).Append("</size></color>").AppendLine();
            }


            if (baseVendorMenu.GetComponent<VendorBuyMenu>())
                builder.Append("Buy Price: ").Append("<color=yellow>").Append(itemRef.buyValue).Append(" <size=9>").Append(currencyManager.name).Append("</size></color>");
            if (baseVendorMenu.GetComponent<VendorSellMenu>())
                builder.Append("Sell Price: ").Append("<color=yellow>").Append(itemRef.sellValue).Append(" <size=9>").Append(currencyManager.name).Append("</size></color>");
            infoTxt.text = builder.ToString();
            menu.GetComponent<ContentSizeFitter>().SetLayoutVertical(); //ContentSizeFitter.SetLayoutVertical();
            infoTxt.GetComponent<ContentSizeFitter>().SetLayoutVertical();
        }
    }
}