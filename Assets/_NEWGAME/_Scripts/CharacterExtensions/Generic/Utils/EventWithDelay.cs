using System.Collections;
using UnityEngine;

namespace _GAME._Scripts._Utils
{
    [ClassHeader("Events With Delay")]
    public class EventWithDelay : ExtendMonoBehaviour
    {
        public bool triggerOnStart;
        public bool triggerOnEnable;

        [_Scripts.LeoHideInInspector("triggerOnStart")]
        public bool all;
        [_Scripts.LeoHideInInspector("triggerOnStart")]
        public int eventIndex;

        private void OnEnable()
        {
            if (triggerOnEnable)
            {
                if (all) DoEvents();
                else DoEvent(eventIndex);
            }
        }

        private void Start()
        {
            if (triggerOnStart)
            {
                if (all) DoEvents();
                else DoEvent(eventIndex);
            }
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }
        [SerializeField] private EventWithDelayObject[] events = new EventWithDelayObject[0];
        public void DoEvents()
        {
            for (int i = 0; i < events.Length; i++)
                StartCoroutine(DoEventWithDelay(events[i]));
        }

        public void DoEvent(int index)
        {

            if (index < events.Length && events.Length > 0) StartCoroutine(DoEventWithDelay(events[index]));
        }

        public void DoEvent(string name)
        {
            EventWithDelayObject _e = System.Array.Find(events, e => e.name.Equals(name));
            if (_e != null) StartCoroutine(DoEventWithDelay(_e));
        }

        IEnumerator DoEventWithDelay(EventWithDelayObject _event)
        {
            yield return new WaitForSeconds(_event.delay);
            _event.onDoEvent.Invoke();
        }

        [System.Serializable]
        public class EventWithDelayObject
        {
            public string name = "EventName";
            public float delay;
            public UnityEngine.Events.UnityEvent onDoEvent;
        }
    }
}