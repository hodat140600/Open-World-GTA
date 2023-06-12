using UnityEngine;
using System.Collections;

namespace _GAME._Scripts
{
    [ClassHeader("Destroy GameObject", openClose = false)]
    public class ExtendDestroyGameObject : ExtendMonoBehaviour
    {
        public float delay;
        public UnityEngine.Events.UnityEvent onDestroy;
        IEnumerator Start()
        {
            yield return new WaitForSeconds(delay);
            onDestroy.Invoke();
            Destroy(gameObject);
        }
    }
}