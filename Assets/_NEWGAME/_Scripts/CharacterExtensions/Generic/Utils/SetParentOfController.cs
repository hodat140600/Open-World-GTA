using _GAME._Scripts._CharacterController;
using UnityEngine;

namespace _GAME._Scripts._Utils
{
    public class SetParentOfController : MonoBehaviour
    {
        [HelpBox("Set this GameObject as parent of the Controller")]

        private ThirdPersonController cc;

        public UnityEngine.Events.UnityEvent onStart;

        private void Start()
        {
            cc = GetComponentInParent<ThirdPersonController>();
            transform.parent = cc.transform;

            onStart.Invoke();
        }
    }
}