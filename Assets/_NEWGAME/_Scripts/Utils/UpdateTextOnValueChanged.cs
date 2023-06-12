using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _GAME._Scripts
{
    public class UpdateTextOnValueChanged : MonoBehaviour
    {
        [SerializeField] private Slider   slider;
        [SerializeField] private TMP_Text value;

        public void UpdateText()
        {
            value.SetText(Mathf.CeilToInt(slider.value).ToString());
        }
    }
}