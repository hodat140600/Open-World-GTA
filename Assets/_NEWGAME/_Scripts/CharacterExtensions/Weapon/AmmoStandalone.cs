using UnityEngine;
using System.Collections;

namespace _GAME._Scripts._ItemManager
{
    using _GAME._Scripts._CharacterController._Actions;
    [ClassHeader("AmmoStandalone")]
    public class AmmoStandalone : TriggerGenericAction
    {
        [Header("Ammo Standalone Options")]
        [Tooltip("Use the same name as in the AmmoManager")]
        public string weaponName;
        public int ammoID;
        public int ammoAmount;
        private AmmoManager ammoManager;

        public override IEnumerator OnPressActionDelay(GameObject cc)
        {
            yield return StartCoroutine(base.OnPressActionDelay(cc));

            ammoManager = cc.gameObject.GetComponent<AmmoManager>();
            if (ammoManager != null)
                ammoManager.AddAmmo(weaponName, ammoID, ammoAmount);
        }
    }
}