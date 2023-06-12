using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace _GAME._Scripts
{
    public class EventDelay : MonoBehaviour
    {
        public UnityEvent onEvent;

        public void InvokeEvent(int delay)
        {
            StartCoroutine(Delay(delay));
        }

        IEnumerator Delay(int delay)
        {
            yield return new WaitForSeconds(delay);
            onEvent.Invoke();
        }
    }
}