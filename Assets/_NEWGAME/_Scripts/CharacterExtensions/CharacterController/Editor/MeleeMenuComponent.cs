using UnityEngine;
using UnityEditor;
using _GAME._Scripts._CharacterController;
using _GAME._Scripts._ItemManager;

namespace _GAME._Scripts
{
    // MELEE COMBAT FEATURES
    public partial class MenuComponent
    {
        public const string path = "DatHQ/Melee Combat/Components/";

        [MenuItem(path + "Melee Manager")]
        static void MeleeManagerMenu()
        {
            if (Selection.activeGameObject)
                Selection.activeGameObject.AddComponent<_Melee.MeleeManager>();
            else
                Debug.Log("Please select a vCharacter to add the component.");
        }

        [MenuItem(path + "WeaponHolderManager (Player Only)")]
        static void WeaponHolderMenu()
        {
            if (Selection.activeGameObject && Selection.activeGameObject.GetComponent<ThirdPersonInput>() != null)
                Selection.activeGameObject.AddComponent<WeaponHolderManager>();
            else
                Debug.Log("Please select the Player to add the component.");
        }

        [MenuItem(path + "LockOn (Player Only)")]
        static void LockOnMenu()
        {
            if (Selection.activeGameObject && Selection.activeGameObject.GetComponent<ThirdPersonInput>() != null)
                Selection.activeGameObject.AddComponent<_CharacterController.ExtendLockOn>();
            else
                Debug.Log("Please select a Player to add the component.");
        }

        [MenuItem(path + "DrawHide MeleeWeapons")]
        static void DrawMeleeWeaponMenu()
        {
            if (Selection.activeGameObject && Selection.activeGameObject.GetComponent<MeleeCombatInput>() != null)
                Selection.activeGameObject.AddComponent<ExtendDrawHideMeleeWeapons>();
            else
                Debug.Log("Please select a Player to add the component.");
        }
    }
}
