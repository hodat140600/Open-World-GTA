using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace _GAME._Scripts
{
    public class DestroyOnTrigger : MonoBehaviour
    {
        public List<string> targsToDestroy;
        public float destroyDelayTime;

        void OnTriggerEnter(Collider other)
        {
            if (targsToDestroy.Contains(other.gameObject.tag))
            {
                Destroy(other.gameObject, destroyDelayTime);
            }
        }
    }
}