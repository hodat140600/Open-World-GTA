using System.Collections;
using UnityEngine;
namespace _GAME._Scripts._Camera
{
    public class ChangeCameraAngleTrigger : MonoBehaviour
    {
        public bool applyY, applyX;
        public Vector2 angle;
        public ThirdPersonCamera tpCamera;
        public bool useSelfWorldAngle;
        private void OnDrawGizmos()
        {
            if (useSelfWorldAngle)
            {
                angle.x = transform.eulerAngles.y;
                angle.y = transform.eulerAngles.x;



            }
        }
        IEnumerator Start()
        {
            tpCamera = FindObjectOfType<ThirdPersonCamera>();
            var collider = GetComponent<Collider>();
            if (collider)
            {
                collider.isTrigger = true;
                collider.enabled = false;
                yield return new WaitForEndOfFrame();
                collider.enabled = true;
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player") && tpCamera)
            {
                if (applyX)
                    tpCamera.lerpState.fixedAngle.x = angle.x;
                if (applyY)
                    tpCamera.lerpState.fixedAngle.y = angle.y;
            }
        }
    }
}