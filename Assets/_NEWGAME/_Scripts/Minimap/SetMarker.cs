using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace _GAME._Scripts
{
    public class SetMarker : MonoBehaviour, IPointerDownHandler
    {
        private                  int   clickCount = 0;
        [SerializeField] private float delay      = .5f;

        public void OnPointerDown(PointerEventData eventData)
        {
            clickCount++;
            if (clickCount == 1)
                StartCoroutine(WaitForSecondClickIfHappen());
        }

        private float tmpDelay;

        private IEnumerator WaitForSecondClickIfHappen()
        {
            tmpDelay = delay;

            Assert.IsTrue(Time.timeScale == 0, "Time.timeScale is not 0 during pausing!");
            while ((tmpDelay -= Time.fixedDeltaTime) > 0)
            {
                if (clickCount >= 2)
                {
                    DoubleClick();

                    // cooling
                    yield return new WaitForSeconds(1f);

                    clickCount = 0;
                    yield break;
                }

                yield return null;
            }

            SingleClick();

            // cooling
            yield return new WaitForSeconds(1f);

            clickCount = 0;
        }

        public TMP_Text test;

        private string testString
        {
            set { test.SetText(value); }
        }

        private void DoubleClick()
        {
            testString = "double clicked";
        }

        private void SingleClick()
        {
            testString = "single clicked";
        }
    }
}