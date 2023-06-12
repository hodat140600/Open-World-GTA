using _GAME._Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace _GAME._Scripts._ItemManager
{
    /// <summary>
    /// Equipments of the Inventory that needs to be instantiated
    /// </summary>
    [ClassHeader("Equipment", openClose = false, helpBoxText = "Use this component if you also use the ItemManager in your Character")]
    public partial class Equipment : ExtendMonoBehaviour
    {
        public OnHandleItemEvent onEquip, onUnequip;

        public EquipPoint equipPoint { get; set; }


        /// <summary>
        /// Event called when equipment is destroyed
        /// </summary>
        public virtual void OnDestroy()
        {

        }

        /// <summary>
        /// Item representing the equipment
        /// </summary>
        public Item referenceItem;

        //{
        //    get;
        //    protected set;
        //}

        /// <summary>
        /// Event called when the item is equipped
        /// </summary>
        /// <param name="item">target item</param>
        public virtual void OnEquip(Item item)
        {
            referenceItem = item;
            onEquip.Invoke(item);
        }

        /// <summary>
        /// Event called when the item is unquipped
        /// </summary>
        /// <param name="item">target item</param>
        public virtual void OnUnequip(Item item)
        {
            onUnequip.Invoke(item);
        }
    }
}