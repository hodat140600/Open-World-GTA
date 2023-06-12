using UnityEngine;

namespace _GAME._Scripts._ItemManager
{
    [System.Obsolete("Class is no longer used and will be removed in the future")]
    public interface IEquipment
    {
        Transform transform { get; }
        GameObject gameObject { get; }
        bool isEquiped { get; }
        EquipPoint equipPoint { get; set; }
        Item referenceItem { get; }
        void OnEquip(Item item);
        void OnUnequip(Item item);
    }
}