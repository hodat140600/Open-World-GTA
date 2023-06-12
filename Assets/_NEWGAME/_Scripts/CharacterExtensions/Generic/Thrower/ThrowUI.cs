using UnityEngine;
using UnityEngine.UI;

namespace _GAME._Scripts._CharacterController
{
    public class ThrowUI : MonoBehaviour
    {
        public Text maxThrowCount;
        public Text currentThrowCount;

        internal virtual void UpdateCount(ThrowManager throwManager)
        {
            currentThrowCount.text = throwManager.currentThrowObject.ToString();
            maxThrowCount.text = throwManager.maxThrowObjects.ToString();
        }
    }
}