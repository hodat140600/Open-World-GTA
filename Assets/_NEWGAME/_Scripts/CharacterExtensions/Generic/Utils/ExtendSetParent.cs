using UnityEngine;
namespace _GAME._Scripts._Utils
{
    public class ExtendSetParent : MonoBehaviour
    {
        public void RemoveParent()
        {
            transform.parent = null;
        }

        public void RemoveParent(Transform target)
        {
            target.parent = null;
        }

        public void SetParent(Transform parent)
        {
            transform.parent = parent;
        }

    }
}