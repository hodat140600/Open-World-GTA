using _GAME._Scripts._Melee;
using UnityEngine;
namespace _GAME._Scripts._ItemManager
{
    [ClassHeader("Melee Equipment", openClose = false, useHelpBox = true, helpBoxText = "Use this component if you also use the ItemManager in your Character")]
    public class MeleeEquipment : Equipment
    {
        MeleeWeapon _weapon;
        protected bool withoutMeleeWeapon;

        protected virtual MeleeWeapon meleeWeapon
        {
            get
            {
                if (!_weapon && !withoutMeleeWeapon)
                {
                    _weapon = GetComponent<MeleeWeapon>();
                    if (!_weapon) withoutMeleeWeapon = true;
                }

                return _weapon;
            }
        }

        public override void OnEquip(Item item)
        {
            if (meleeWeapon)
            {
                var damage = item.GetItemAttribute(ItemAttributes.Damage);
                var staminaCost = item.GetItemAttribute(ItemAttributes.StaminaCost);
                var defenseRate = item.GetItemAttribute(ItemAttributes.DefenseRate);
                var defenseRange = item.GetItemAttribute(ItemAttributes.DefenseRange);
                if (damage != null) meleeWeapon.damage.damageValue = damage.value;
                if (staminaCost != null) meleeWeapon.staminaCost = staminaCost.value;
                if (defenseRate != null) meleeWeapon.defenseRate = defenseRate.value;
                if (defenseRange != null) meleeWeapon.defenseRange = defenseRate.value;
            }

            base.OnEquip(item);
        }
        public override void OnUnequip(Item item)
        {
            base.OnUnequip(item);
        }
    }
}
