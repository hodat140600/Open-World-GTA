using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace _GAME._Scripts
{
    [ClassHeader("HITDAMAGE PARTICLE", "Default hit Particle to instantiate every time you receive damage and Custom hit Particle to instantiate based on a custom DamageType that comes from the MeleeControl Behaviour (AnimatorController)")]
    public class HitDamageParticle : ExtendMonoBehaviour
    {
        public List<GameObject> defaultDamageEffects = new List<GameObject>();
        public List<DamageEffect> customDamageEffects = new List<DamageEffect>();

        private FisherYatesRandom _random;

        IEnumerator Start()
        {
            yield return new WaitForEndOfFrame();
            var healthController = GetComponent<HealthController>();
            if (healthController != null)
            {
                healthController.onReceiveDamage.AddListener(OnReceiveDamage);
            }
        }

        public void OnReceiveDamage(Damage damage)
        {
            // instantiate the hitDamage particle - check if your character has a HitDamageParticle component
            var damageDirection = damage.hitPosition - new Vector3(transform.position.x, damage.hitPosition.y, transform.position.z);
            var hitrotation = damageDirection != Vector3.zero ? Quaternion.LookRotation(damageDirection) : transform.rotation;

            if (damage.damageValue > 0)
            {
                TriggerEffect(new DamageEffectInfo(damage.hitPosition, hitrotation, damage.damageType, damage.receiver));
            }
        }

        /// <summary>
        /// Raises the hit event.
        /// </summary>
        /// <param name="damageEffectInfo">Hit effect info.</param>
        void TriggerEffect(DamageEffectInfo damageEffectInfo)
        {
            if (_random == null)
            {
                _random = new FisherYatesRandom();
            }
            var damageEffect = customDamageEffects.Find(effect => effect.damageType.Equals(damageEffectInfo.damageType));

            if (damageEffect != null)
            {
                damageEffect.onTriggerEffect.Invoke();
                if (damageEffect.customDamageEffect != null && damageEffect.customDamageEffect.Count > 0)
                {
                    var randomCustomEffect = damageEffect.customDamageEffect[_random.Next(damageEffect.customDamageEffect.Count)];

                    Instantiate(randomCustomEffect, damageEffectInfo.position,
                        damageEffect.rotateToHitDirection ? damageEffectInfo.rotation : randomCustomEffect.transform.rotation,
                        damageEffect.attachInReceiver && damageEffectInfo.receiver ? damageEffectInfo.receiver : ObjectContainer.root);
                }
            }
            else if (defaultDamageEffects.Count > 0 && damageEffectInfo != null)
            {
                var randomDefaultEffect = defaultDamageEffects[_random.Next(defaultDamageEffects.Count)];
                Instantiate(randomDefaultEffect, damageEffectInfo.position, damageEffectInfo.rotation, ObjectContainer.root);
            }
        }

        private void Reset()
        {
            defaultDamageEffects = new List<GameObject>();
            var defaultEffect = Resources.Load("defaultDamageEffect");

            if (defaultEffect != null)
            {
                defaultDamageEffects.Add(defaultEffect as GameObject);
            }
        }
    }



    public class DamageEffectInfo
    {
        public Transform receiver;
        public Vector3 position;
        public Quaternion rotation;
        public string damageType;

        public DamageEffectInfo(Vector3 position, Quaternion rotation, string damageType = "", Transform receiver = null)
        {
            this.receiver = receiver;
            this.position = position;
            this.rotation = rotation;
            this.damageType = damageType;
        }
    }

    [System.Serializable]
    public class DamageEffect
    {
        public string damageType = "";
        public List<GameObject> customDamageEffect;
        public bool rotateToHitDirection = true;
        [Tooltip("Attach prefab in Damage Receiver transform")]
        public bool attachInReceiver = false;
        public UnityEvent onTriggerEffect;
    }
}
