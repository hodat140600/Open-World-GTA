using _GAME._Scripts._CharacterController._Shooter;
using UnityEngine;

namespace _GAME._Scripts._ItemManager
{
    [ClassHeader("Shooter Equipment", openClose = false, useHelpBox = true, helpBoxText = "Use this component if you also use the ItemManager in your Character")]
    public class ShooterEquipment : MeleeEquipment
    {
        protected ShooterWeapon _shooter;
        protected Equipment _secundaryEquipment;

        protected bool withoutShooterWeapon;

        public virtual Equipment secundaryEquipment
        {
            get
            {
                return _secundaryEquipment;
            }
        }
        public virtual ShooterWeapon shooterWeapon
        {
            get
            {
                if (!_shooter && !withoutShooterWeapon)
                {
                    _shooter = GetComponent<ShooterWeapon>();
                    if (!_shooter) withoutShooterWeapon = true;
                }

                return _shooter;
            }
        }

        public override void OnEquip(Item item)
        {

            if (shooterWeapon)
            {
                shooterWeapon.changeAmmoHandle = new ShooterWeapon.ChangeAmmoHandle(ChangeAmmo);
                shooterWeapon.checkAmmoHandle = new ShooterWeapon.CheckAmmoHandle(CheckAmmo);
                var damageAttribute = item.GetItemAttribute(ItemAttributes.Damage);

                if (damageAttribute != null)
                {
                    shooterWeapon.maxDamage = damageAttribute.value;
                }

                if (secundaryEquipment)
                {
                    secundaryEquipment.OnEquip(item);
                }
            }
            base.OnEquip(item);
        }

        public override void OnUnequip(Item item)
        {
            if (shooterWeapon)
            {
                shooterWeapon.changeAmmoHandle = null;
                shooterWeapon.checkAmmoHandle = null;

                if (secundaryEquipment)
                {
                    secundaryEquipment.OnUnequip(item);
                }
            }

            base.OnUnequip(item);
        }

        protected virtual bool CheckAmmo(ref bool isValid, ref int totalAmmo)
        {
            if (!referenceItem) return false;
            var ammoAttribute = referenceItem.GetItemAttribute(ItemAttributes.AmmoCount);
            isValid = ammoAttribute != null && !ammoAttribute.isBool;
            if (isValid) totalAmmo = ammoAttribute.value;
            return isValid && ammoAttribute.value > 0;
        }

        protected virtual void ChangeAmmo(int value)
        {
            if (!referenceItem) return;
            var ammoAttribute = referenceItem.GetItemAttribute(ItemAttributes.AmmoCount);

            if (ammoAttribute != null)
            {
                ammoAttribute.value += value;
            }
        }

    }
}