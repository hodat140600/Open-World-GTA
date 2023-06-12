using UnityEngine;
using System.Collections;
using UnityEditor;

namespace _GAME._Scripts._CharacterController._Shooter
{
    public partial class MenuComponent
    {
        [MenuItem("DatHQ/Shooter/Components/LockOn (Player Shooter Only)")]
        static void LockOnShooterMenu()
        {
            if (Selection.activeGameObject && Selection.activeGameObject.GetComponent<_GAME._Scripts._CharacterController.ThirdPersonInput>() != null)
                Selection.activeGameObject.AddComponent<LockOnShooter>();
            else
                Debug.Log("Please select a Player to add the component.");
        }

        [MenuItem("DatHQ/Shooter/Components/DrawHide ShooterWeapons")]
        static void DrawShooterWeaponMenu()
        {
            if (Selection.activeGameObject && Selection.activeGameObject.GetComponent<_GAME._Scripts._CharacterController.ThirdPersonInput>() != null)
                Selection.activeGameObject.AddComponent<DrawHideShooterWeapons>();
            else
                Debug.Log("Please select a Player to add the component.");
        }

        [MenuItem("DatHQ/Shooter/Components/ThrowObject")]
        static void ThrowObjectMenu()
        {
            if (Selection.activeGameObject && Selection.activeGameObject.GetComponent<_GAME._Scripts._CharacterController.ThirdPersonInput>() != null)
                Selection.activeGameObject.AddComponent<ThrowManager>();
            else
                Debug.Log("Please select a Player to add the component.");
        }

    }
}
