using UnityEngine;

namespace _GAME._Scripts._CharacterController
{
    public class PunchingBag : MonoBehaviour
    {
        public Rigidbody _rigidbody;
        public float forceMultipler = 0.5f;
        public SpringJoint joint;
        public HealthController character;
        public bool removeComponentsAfterDie;

        void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            character = GetComponent<HealthController>();
            character.onReceiveDamage.AddListener(TakeDamage);
        }

        public void TakeDamage(Damage damage)
        {
            var point = damage.hitPosition;
            var relativePoint = transform.position;
            relativePoint.y = point.y;
            var forceForward = relativePoint - point;

            if (character != null && joint != null && character.CurrentHealth < 0)
            {
                joint.connectedBody = null;
                if (removeComponentsAfterDie)
                {
                    foreach (MonoBehaviour mono in character.gameObject.GetComponentsInChildren<MonoBehaviour>())
                        if (mono != this)
                            Destroy(mono);
                }
            }

            if (_rigidbody != null)
            {
                _rigidbody.AddForce(forceForward * (damage.damageValue * forceMultipler), ForceMode.Impulse);
            }
        }
    }
}