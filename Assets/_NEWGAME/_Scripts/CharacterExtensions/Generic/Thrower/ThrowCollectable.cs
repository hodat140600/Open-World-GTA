using UnityEngine;
using System.Collections;

namespace _GAME._Scripts._CharacterController
{
    [ClassHeader("THROW COLLECTABLE", false)]
    public class ThrowCollectable : ExtendMonoBehaviour
    {
        public int amount = 1;
        public bool destroyAfter = true;
        ThrowManager throwManager;

        public UnityEngine.Events.UnityEvent onCollectObject;
        public UnityEngine.Events.UnityEvent onReachMaxObjects;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player") && other.GetComponentInChildren<ThrowManager>() != null)
                throwManager = other.GetComponentInChildren<ThrowManager>();
        }

        public void UpdateThrowObj(Rigidbody throwObj)
        {
            if (throwManager.currentThrowObject < throwManager.maxThrowObjects)
            {
                throwManager.SetAmount(amount);
                throwManager.objectToThrow = throwObj;
                onCollectObject.Invoke();
                if (destroyAfter) Destroy(this.gameObject);
            }
            else
            {
                onReachMaxObjects.Invoke();
            }
        }
    }
}