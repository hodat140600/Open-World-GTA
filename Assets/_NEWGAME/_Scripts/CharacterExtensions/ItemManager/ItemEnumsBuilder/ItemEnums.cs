using System.ComponentModel;
namespace _GAME._Scripts._ItemManager {
     public enum ItemType {
        [Description("")] Consumable = 0,
        [Description("Melee")] MeleeWeapon = 1,
        [Description("Shooter")] ShooterWeapon = 2,
        [Description("(VALUE)")] Ammo = 3,
        [Description("")] Archery = 4,
        [Description("")] Builder = 5,
        [Description("")] Defense = 6,
        [Description("")] CraftingMaterials = 7
    }
     public enum ItemAttributes {
        [Description("")] Health = 0,
        [Description("")] Stamina = 1,
        [Description("<i>Damage</i> : <color=red>(VALUE)</color>")] Damage = 2,
        [Description("")] StaminaCost = 3,
        [Description("")] DefenseRate = 4,
        [Description("")] DefenseRange = 5,
        [Description("(VALUE)")] AmmoCount = 6,
        [Description("")] MaxHealth = 7,
        [Description("")] MaxStamina = 8,
        [Description("(VALUE)")] SecundaryAmmoCount = 9,
        [Description("")] SecundaryDamage = 10
    }
}
