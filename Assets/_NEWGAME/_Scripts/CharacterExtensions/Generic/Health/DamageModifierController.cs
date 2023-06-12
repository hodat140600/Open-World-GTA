using System.Collections.Generic;
using UnityEngine;
namespace _GAME._Scripts
{
    [ClassHeader("Damage Modifier Controller", openClose = false, useHelpBox = true, helpBoxText = "Needs a HealthController component")]
    public class DamageModifierController : ExtendMonoBehaviour
    {
        public enum GetHealthControllerMethod
        {
            GetComponent,
            GetComponentInParent,
            GetComponentInChildren

        }
        [LeoReadOnly] public bool isInit;
        [SerializeField] protected GetHealthControllerMethod getHealthMethod = GetHealthControllerMethod.GetComponent;
        [Tooltip("Modifier List")]
        public List<DamageModifier> modifiers;
        public UnityEngine.Events.UnityEvent onAllModifiersIsBroken;



        protected IHealthController healthController = null;

        protected virtual void Awake()
        {
            Init();
        }

        protected void Init()
        {
            GetHealthController();
            if (healthController != null)
            {
                AddDamageEvent();
                InitModifiers();
                isInit = true;
            }
        }

        protected virtual void InitModifiers()
        {
            for (int i = 0; i < modifiers.Count; i++)
            {
                modifiers[i].ResetModifier();
                modifiers[i].onBroken.AddListener((m) => { CheckBrokedModifiers(); });
            }
        }

        protected virtual void AddDamageEvent()
        {
            RemoveDamageEvent();
            healthController.onStartReceiveDamage.AddListener(ApplyModifiers);
        }

        protected virtual void RemoveDamageEvent()
        {
            healthController.onStartReceiveDamage.RemoveListener(ApplyModifiers);
        }

        protected virtual void GetHealthController()
        {
            switch (getHealthMethod)
            {
                case GetHealthControllerMethod.GetComponent:
                    healthController = GetComponent<IHealthController>();
                    break;
                case GetHealthControllerMethod.GetComponentInChildren:
                    healthController = GetComponentInChildren<IHealthController>();
                    break;
                case GetHealthControllerMethod.GetComponentInParent:
                    healthController = GetComponentInParent<IHealthController>();
                    break;
            }
        }

        protected virtual void OnEnable()
        {
            if (isInit) AddDamageEvent();
        }

        protected virtual void OnDisable()
        {
            if (isInit) RemoveDamageEvent();
        }


        /// <summary>
        /// Check if all modifiers is broken
        /// </summary>
        /// <param name="modifier"></param>
        protected virtual void CheckBrokedModifiers()
        {
            if (!modifiers.Exists(m => m.isBroken == false)) onAllModifiersIsBroken.Invoke();
        }

        /// <summary>
        /// Apply All Modifiers when healthcontroller takes damage
        /// </summary>
        /// <param name="damage">Damage to modify</param>
        protected virtual void ApplyModifiers(Damage damage)
        {
            for (int i = 0; i < modifiers.Count; i++)
            {
                modifiers[i].ApplyModifier(damage);
            }
        }

        /// <summary>
        /// Reset all breakeable modifiers (will fill yours resistance)
        /// </summary>
        public void ResetAllModifiers()
        {
            for (int i = 0; i < modifiers.Count; i++)
            {
                modifiers[i].ResetModifier();
            }
        }
    }
}
