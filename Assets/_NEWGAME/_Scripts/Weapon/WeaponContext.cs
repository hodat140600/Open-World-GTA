using System;
using _GAME._Scripts.Inventory;

namespace _GAME._Scripts
{
    public static class GameplayContext
    {
        // Invoke everytime any change on weapon happened (use ammo, buy ammo, change weapon, change active weapon, ..)
        public static Action<Weapon> OnWeaponChanged;
        public static Action<Weapon> OnAmmoBought;
    }
}