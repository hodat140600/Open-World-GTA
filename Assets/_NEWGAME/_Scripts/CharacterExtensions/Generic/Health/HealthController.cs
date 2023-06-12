using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace _GAME._Scripts
{
    [ClassHeader("HealthController", iconName = "ThirdPersonExtensions/HealthControllerIcon")]
    public class HealthController : ExtendMonoBehaviour, IHealthController
    {
        #region Variables

        [EditorToolbar("Health", order = 0)]
        [SerializeField] [LeoReadOnly] protected bool _isDead;
        [BarDisplay("maxHealth")] [SerializeField] protected float _currentHealth;
        public bool isImmortal = false;
        [HelpBox("If you want to start with different value, uncheck this and make sure that the current health has a value greater zero")]
        public bool fillHealthOnStart = true;
        public int maxHealth = 100;
        public int MaxHealth
        {
            get
            {
                return maxHealth;
            }
            protected set
            {
                maxHealth = value;
            }
        }
        public float CurrentHealth
        {
            get
            {
                return _currentHealth;
            }
            protected set
            {
                if (_currentHealth != value)
                {
                    _currentHealth = value;
                    onChangeHealth?.Invoke(_currentHealth);
                }

                if (!_isDead && _currentHealth <= 0)
                {
                    _isDead = true;
                    onDead?.Invoke(gameObject);
                }
                else if (isDead && _currentHealth > 0)
                {
                    _isDead = false;
                }
            }
        }
        public bool isDead
        {
            get
            {
                if (!_isDead && CurrentHealth <= 0)
                {
                    _isDead = true;
                    onDead?.Invoke(gameObject);
                }
                return _isDead;
            }
            set
            {
                _isDead = value;
            }
        }
        public float healthRecovery = 0f;
        public float healthRecoveryDelay = 0f;
        [@HideInInspector]
        public float currentHealthRecoveryDelay;
        [EditorToolbar("Events", order = 100)]
        public List<CheckHealthEvent> checkHealthEvents = new List<CheckHealthEvent>();
        [SerializeField] protected OnReceiveDamage _onStartReceiveDamage = new OnReceiveDamage();
        [SerializeField] protected OnReceiveDamage _onReceiveDamage = new OnReceiveDamage();
        [SerializeField] protected OnDead _onDead = new OnDead();
        public ValueChangedEvent onChangeHealth;

        public OnReceiveDamage onStartReceiveDamage { get { return _onStartReceiveDamage; } protected set { _onStartReceiveDamage = value; } }
        public OnReceiveDamage onReceiveDamage { get { return _onReceiveDamage; } protected set { _onReceiveDamage = value; } }
        public OnDead onDead { get { return _onDead; } protected set { _onDead = value; } }
        public UnityEvent onResetHealth;
        internal bool inHealthRecovery;

        #endregion

        protected virtual void Start()
        {
            if (fillHealthOnStart)
                CurrentHealth = maxHealth;
            currentHealthRecoveryDelay = healthRecoveryDelay;
        }

        protected virtual bool canRecoverHealth
        {
            get
            {
                return CurrentHealth >= 0 && healthRecovery > 0 && CurrentHealth < maxHealth;
            }
        }

        protected virtual IEnumerator RecoverHealth()
        {
            inHealthRecovery = true;
            while (canRecoverHealth && !isDead)
            {
                HealthRecovery();
                yield return null;
            }
            inHealthRecovery = false;
        }

        protected virtual void HealthRecovery()
        {
            if (!canRecoverHealth || isDead) return;
            if (currentHealthRecoveryDelay > 0)
                currentHealthRecoveryDelay -= Time.deltaTime;
            else
            {
                if (CurrentHealth > maxHealth)
                    CurrentHealth = maxHealth;
                if (CurrentHealth < maxHealth)
                    CurrentHealth += healthRecovery * Time.deltaTime;
            }
        }

        /// <summary>
        /// Increase or decrease  currentHealth (Positive or Negative Values)
        /// </summary>
        /// <param name="value">Value to change</param>
        public virtual void AddHealth(int value)
        {
            CurrentHealth += value;
            CurrentHealth = Mathf.Clamp(CurrentHealth, 0, maxHealth);
            if (!isDead && CurrentHealth <= 0)
            {
                isDead = true;
                onDead.Invoke(gameObject);
            }
            HandleCheckHealthEvents();
        }

        /// <summary>
        /// Change the currentHealth of Character
        /// </summary>
        /// <param name="value"></param>
        public virtual void ChangeHealth(int value)
        {
            CurrentHealth = value;
            CurrentHealth = Mathf.Clamp(CurrentHealth, 0, maxHealth);
            if (!isDead && CurrentHealth <= 0)
            {
                isDead = true;
                onDead.Invoke(gameObject);
            }
            HandleCheckHealthEvents();
        }

        /// <summary>
        /// Reset's current health to specific health value
        /// </summary>
        /// <param name="health">target health</param>
        public virtual void ResetHealth(float health)
        {
            CurrentHealth = health;
            onResetHealth.Invoke();
            if (isDead) isDead = false;
        }
        /// <summary>
        /// Reset's current health to max health
        /// </summary>
        public virtual void ResetHealth()
        {
            CurrentHealth = maxHealth;
            onResetHealth.Invoke();
            if (isDead) isDead = false;
        }

        /// <summary>
        /// Change the MaxHealth of Character
        /// </summary>
        /// <param name="value"></param>
        public virtual void ChangeMaxHealth(int value)
        {
            maxHealth += value;
            if (maxHealth < 0)
                maxHealth = 0;
        }

        /// <summary>
        /// Set a value to HealthRecovery to start recovering health
        /// </summary>
        /// <param name="value"></param>
        public virtual void SetHealthRecovery(float value)
        {
            healthRecovery = value;
            StartCoroutine(RecoverHealth());
        }

        /// <summary>
        /// Apply Damage to Current Health
        /// </summary>
        /// <param name="damage">damage</param>
        public virtual void TakeDamage(Damage damage)
        {
            if (damage != null)
            {
                onStartReceiveDamage.Invoke(damage);
                currentHealthRecoveryDelay = CurrentHealth <= 0 ? 0 : healthRecoveryDelay;

                if (CurrentHealth > 0 && !isImmortal)
                {
                    CurrentHealth -= damage.damageValue;
                }

                if (damage.damageValue > 0)
                    onReceiveDamage.Invoke(damage);
                HandleCheckHealthEvents();
            }
        }

        protected virtual void HandleCheckHealthEvents()
        {
            var events = checkHealthEvents.FindAll(e => e.healthCompare == CheckHealthEvent.HealthCompare.Equals && CurrentHealth.Equals(e.healthToCheck) ||
                                                        e.healthCompare == CheckHealthEvent.HealthCompare.HigherThan && CurrentHealth > e.healthToCheck ||
                                                        e.healthCompare == CheckHealthEvent.HealthCompare.LessThan && CurrentHealth < e.healthToCheck);

            for (int i = 0; i < events.Count; i++)
            {
                events[i].OnCheckHealth.Invoke();
            }
            if (CurrentHealth < maxHealth && gameObject.activeInHierarchy && !inHealthRecovery)
                StartCoroutine(RecoverHealth());
        }

        [Serializable]
        public class CheckHealthEvent
        {
            public int healthToCheck;
            public bool disableEventOnCheck;

            public enum HealthCompare
            {
                Equals,
                HigherThan,
                LessThan
            }

            public HealthCompare healthCompare = HealthCompare.Equals;

            public UnityEvent OnCheckHealth;
        }

        [Serializable]
        public class ValueChangedEvent : UnityEvent<float>
        {

        }
    }
}