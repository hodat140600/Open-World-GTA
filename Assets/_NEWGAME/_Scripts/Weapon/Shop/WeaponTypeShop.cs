using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _GAME._Scripts.Shop;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _GAME._Scripts.Inventory
{
    [HideReferenceObjectPicker]
    public class WeaponTypeShop : AbstractTypeShop<Weapon>, IEnumerable<Weapon>
    {
        public readonly WeaponType WeaponType;
        public readonly Sprite Avatar;

        public Weapon ActivatingWeapon => Items.FirstOrDefault(w => w.IsActivated);

        public WeaponTypeShop(List<WeaponSettings> weapons)
        {
            WeaponType = weapons[0].Entity.weaponType;

            var avatarSprite = Resources.Load<Sprite>($"Weapons/Avatar/{WeaponType}");
            Avatar = avatarSprite;
            Items = weapons.Select(w => w.Entity).ToList();
        }

        public override void Load()
        {

        }

        public override Weapon DefaultItem => Items[0];

        public Weapon FirstOwnedItem()
        {
            return null;
         }

        public IEnumerator<Weapon> GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public Weapon this[int i] => Items[i];
    }
}