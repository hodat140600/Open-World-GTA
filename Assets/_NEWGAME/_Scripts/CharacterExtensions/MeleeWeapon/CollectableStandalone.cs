using UnityEngine;
using System.Collections;
using UnityEngine.Events;

namespace _GAME._Scripts._CharacterController._Actions
{
    using _Melee;
    [ClassHeader("Collectable Standalone", "Use this component when your character doesn't have a ItemManager", openClose = false)]
    public class CollectableStandalone : TriggerGenericAction
    {
        [EditorToolbar("Collectable")]
        public string targetEquipPoint;
        public bool twoHandWeapon;
        public GameObject weapon;
        public Sprite weaponIcon;
        public string weaponText;
        [EditorToolbar("Events")]
        public UnityEvent OnEquip;
        public UnityEvent OnDrop;

        private CollectMeleeControl manager;

        public override IEnumerator OnPressActionDelay(GameObject cc)
        {
            yield return StartCoroutine(base.OnPressActionDelay(cc));

            manager = cc.GetComponent<CollectMeleeControl>();

            if (manager != null)
            {
                manager.HandleCollectableInput(this);
            }
        }
    }
}