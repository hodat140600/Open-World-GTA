using System.Collections.Generic;
using UnityEngine;
namespace _GAME._Scripts._ItemManager
{
    public class EquipmentReferenceControl : MonoBehaviour
    {
        [System.Serializable]
        public class vEquipmentReference
        {
            public string name;
            public int id;
            public Equipment equipment;

        }
        public List<vEquipmentReference> equipmentReferences;

        private void Awake()
        {
            ItemManager itemManager = GetComponentInParent<ItemManager>();
            itemManager.onEquipItem.AddListener(OnEquip);
            itemManager.onUnequipItem.AddListener(OnUniquip);
        }

        protected virtual void OnEquip(EquipArea equipArea, Item equipment)
        {
            if (equipment) SetActiveEquipment(equipment, true);
        }

        protected virtual void OnUniquip(EquipArea equipArea, Item equipment)
        {
            if (equipment) SetActiveEquipment(equipment, false);
        }

        public virtual void SetActiveEquipment(Item item, bool active)
        {
            var equipments = equipmentReferences.FindAll(e => e.id.Equals(item.id));

            for (int i = 0; i < equipments.Count; i++)
            {
                if (equipments[i].equipment)
                {
                    equipments[i].equipment.gameObject.SetActive(active);
                    if (active)
                    {
                        equipments[i].equipment.OnEquip(item);
                    }
                    else equipments[i].equipment.OnUnequip(item);
                }
            }
        }
    }
}