using UnityEngine;
namespace _GAME._Scripts
{
    public class ScaleControl : MonoBehaviour
    {

        Vector3 targetScale;
        Vector3 defaultScale;
        private void Awake()
        {
            defaultScale = transform.localScale;
            targetScale = defaultScale;

        }
        public float scaleX
        {
            set
            {
                targetScale.x = defaultScale.x * value;

                transform.localScale = targetScale;
            }
        }
        public float scaleY
        {
            set
            {
                targetScale.y = defaultScale.y * value;

                transform.localScale = targetScale;
            }
        }
        public float scaleZ
        {
            set
            {
                targetScale.z = defaultScale.z * value;

                transform.localScale = targetScale;
            }
        }
    }
}