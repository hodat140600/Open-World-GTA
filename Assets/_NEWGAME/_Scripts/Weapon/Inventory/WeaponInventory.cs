using System.Collections.Generic;
using System.Linq;
using _GAME._Scripts.Game;
using _SDK.Inventory;
using Assets._SDK.Inventory.Interfaces;
using UnityEngine;

namespace _GAME._Scripts.Inventory
{
    public class WeaponInventory : MonoBehaviour
    {
        // private                int    usingWeaponId;
        // public static readonly string UsingWeaponKey = nameof(WeaponInventory) + "Using";
        //
        // private WeaponResources Resources => GameManager.Instance.Resources.WeaponResources;
        // public Weapon UsingWeapon { get; private set; }
        //
        // public Dictionary<WeaponType, WeaponTypeInventory> Inventory { get; private set; }
        //
        // public WeaponInventory()
        // {
        //     Load();
        // }

        // public void Load()
        // {
        //     Inventory = new Dictionary<WeaponType, WeaponTypeInventory>();
        //
        //     foreach (KeyValuePair<WeaponType, List<WeaponSettings>> typeShop in Resources.WeaponSettingsByType)
        //     {
        //         if (typeShop.Value.Count == 0) continue;
        //         Inventory.Add(typeShop.Key, new WeaponTypeInventory(typeShop.Value));
        //     }
        // }
        //     public void LoadCurrentWeapon()
        // {
        //     usingWeaponId = PlayerPrefs.GetInt(UsingWeaponKey, Resources.DefaultWeaponSetting.Entity.Id);
        //     UseWeapon(Resources.GetWeapon(usingWeaponId));
        // }
        //
        // private void UseWeapon(Weapon weapon)
        // {
        //     Inventory[weapon.weaponType].ActivatingWeapon?.DeActivate();
        //     weapon.Activate();
        //
        //     UsingWeapon = weapon;
        //     PlayerPrefs.SetInt(UsingWeaponKey, weapon.Id);
        //     GameplayContext.OnWeaponChanged?.Invoke(weapon);
        // }
        //
        // public void Activate(Weapon weapon)
        // {
        //     if (UsingWeapon == weapon) return;
        //     if (weapon == default(Weapon)) return;
        //
        //     var currentWeaponInThisSlot = Inventory[weapon.weaponType]?.ActivatingWeapon;
        //
        //     if (currentWeaponInThisSlot != default(Weapon))
        //         currentWeaponInThisSlot.DeActivate();
        //
        //     UseWeapon(weapon);
        }
        //
        // public void ActivateNextWeapon()
        // {
        //     var ActivatingWeapons =
        //         Inventory.Values
        //             .Select(v => v.ActivatingWeapon)
        //             .Where(v => v != default(Weapon))
        //             .ToList();
        //
        //     var nextWeapon =
        //         ActivatingWeapons                     // get the current weapon in list
        //             .SkipWhile(w => w != UsingWeapon) // pick the current using weapon
        //             .Skip(1)                          // take the next one from it
        //             .FirstOrDefault()                 // if it is null
        //         ?? ActivatingWeapons                  // take the first one instead
        //             .FirstOrDefault();
        //
        //     UseWeapon(nextWeapon);
        // }
    // }
}