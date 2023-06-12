using UnityEngine;

namespace _GAME._Scripts._CharacterController
{
    public class BlockUnarmedAttack : MonoBehaviour
    {
        private MeleeCombatInput meleeCombatInput;
        [SerializeField] protected bool useUnarmedAttack;
        public bool IsActiveUnarmedAttack
        {
            get
            {
                return useUnarmedAttack;
            }
            protected set
            {
                useUnarmedAttack = value;
            }
        }
        void Start()
        {
            ///Get the melee combat input component
            meleeCombatInput = GetComponent<MeleeCombatInput>();
            ///Use update event of the input to handle attack input
            meleeCombatInput.onUpdate += HandleAttackInput;
        }

        private void HandleAttackInput()
        {
            ///Disable input usage if Unarmed
            if (!IsActiveUnarmedAttack)
            {
                meleeCombatInput.weakAttackInput.useInput = meleeCombatInput.isArmed;
                meleeCombatInput.strongAttackInput.useInput = meleeCombatInput.isArmed;
            }
        }

        public void SetActiveUnarmedAttack(bool value)
        {
            if (value != IsActiveUnarmedAttack)
            {
                IsActiveUnarmedAttack = value;
                meleeCombatInput.weakAttackInput.useInput = value;
                meleeCombatInput.strongAttackInput.useInput = value;
            }
        }
    }
}