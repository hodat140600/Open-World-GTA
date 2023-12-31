using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace _GAME._Scripts
{
    public class SliderSizeControl : MonoBehaviour
    {
        public Slider slider;
        public RectTransform rectTransform;
        public float multipScale = 0.1f;
        float oldMaxValue;
        void OnDrawGizmosSelected()
        {
            UpdateScale();
        }
        public void UpdateScale()
        {
            if (rectTransform && slider)
            {
                if (slider.maxValue != oldMaxValue)
                {
                    var sizeDelta = rectTransform.sizeDelta;
                    sizeDelta.x = slider.maxValue * multipScale;
                    rectTransform.sizeDelta = sizeDelta;
                    oldMaxValue = slider.maxValue;
                }
            }
        }
    }
}
