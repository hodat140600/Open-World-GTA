using UnityEngine;
using System.Collections;
using UnityEngine.Events;

namespace _GAME._Scripts._CharacterController
{
    public class OnDeadTrigger : MonoBehaviour
    {

        public UnityEvent OnDead;
        void Start()
        {
            Character character = GetComponent<Character>();
            if (character)
                character.onDead.AddListener(OnDeadHandle);
        }

        public void OnDeadHandle(GameObject target)
        {
            OnDead.Invoke();
        }
    }
}
