using UnityEngine;

namespace _GAME._Scripts
{
    public class HealthItem : MonoBehaviour
    {
        [Tooltip("How much health will be recovery")]
        public float value;
        public string tagFilter = "Player";

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag(tagFilter))
            {
                // access the basic character information
                var healthController = other.GetComponent<HealthController>();
                if (healthController != null)
                {

                    // heal only if the character's health isn't full
                    if (healthController.CurrentHealth < healthController.maxHealth)
                    {
                        // limit healing to the max health
                        healthController.AddHealth((int)value);
                        Destroy(gameObject);
                    }
                }
            }
        }
    }
}