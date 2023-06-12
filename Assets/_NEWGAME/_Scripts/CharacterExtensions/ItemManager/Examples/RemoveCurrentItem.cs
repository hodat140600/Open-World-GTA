using UnityEngine;
using UnityEngine.Events;
namespace _GAME._Scripts._ItemManager
{
    [ClassHeader("Remove Current Item", false)]
    public class RemoveCurrentItem : ExtendMonoBehaviour
    {
        public enum Type
        {
            UnequipItem,
            DestroyItem,
            DropItem
        }

        public Type type = Type.UnequipItem;
        [Tooltip("Immediately equip the item ignoring the Equip animation")]
        public bool immediate = true;
        [Tooltip("Equip Area of your Inventory Prefab")]
        public int equipArea;
        public UnityEvent OnTriggerEnterEvent;

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                var itemManager = other.gameObject.GetComponent<ItemManager>();
                if (itemManager)
                {
                    if (type == Type.UnequipItem)
                        itemManager.UnequipCurrentEquipedItem(equipArea, immediate);
                    else if (type == Type.DestroyItem)
                        itemManager.DestroyCurrentEquipedItem(equipArea, immediate);
                    else
                        itemManager.DropCurrentEquippedItem(equipArea, immediate);
                }
                OnTriggerEnterEvent.Invoke();
            }
        }
    }
}