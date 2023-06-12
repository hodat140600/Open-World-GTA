using _GAME._Scripts._CharacterController;
using UnityEngine;
using UnityEngine.Events;

namespace _GAME._Scripts._Utils
{
    /// <summary>
    /// Simple Checkpoint Example, works by updating the vGameController SpawnPoint to this transform position/rotation.
    /// </summary>    
    [RequireComponent(typeof(BoxCollider))]
    public class CheckpointExample : MonoBehaviour
    {
        GameController gm;

        public UnityEvent onTriggerEnter;

        void Start()
        {
            gm = GetComponentInParent<GameController>();
            GetComponent<BoxCollider>().isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                HUDController.instance.ShowText("Checkpoint reached!");
                gm.spawnPoint = gameObject.transform;
                onTriggerEnter.Invoke();
                gameObject.SetActive(false);
            }
        }
    }
}


