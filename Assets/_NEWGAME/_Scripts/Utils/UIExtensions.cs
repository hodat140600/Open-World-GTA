using _GAME._Scripts.Inventory;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Extensions
{
    public static class UIExtensions
    {
        public static TweenerCore<float, float, FloatOptions> SlideToValue(this Slider slider, float value, float duration = .3f)
        {
            slider.DOKill();
          return  slider.DOValue(value, duration).SetEase(Ease.OutFlash);
        }

        public static void FadeToSprite(this Image image, Sprite newImage)
        {
            image.DOFade(0, .1f).OnComplete(() => image.sprite = newImage);
            image.DOFade(1, .1f).SetDelay(.1f);
        }

        public static void ChangeTextTo(this TMP_Text text, string newString, float speed)
        {
            text.DOText(newString, speed).SetSpeedBased().SetEase(Ease.Linear);
        }

        public static void ShowIf(this CanvasGroup canvasGroup, bool condition)
        {
            if (condition)
            {
                canvasGroup.gameObject.SetActive(true);
                canvasGroup.DOFade(1, .1f)
                    .OnComplete(() => canvasGroup.interactable = true);
            }
            else
            {
                canvasGroup.interactable = false;
                canvasGroup.DOFade(0, .1f)
                    .OnComplete(() => { canvasGroup.gameObject.SetActive(false); });
            }
        }

        public static void ShowIf(this Image image, bool condition)
        {
            image.DOFade(condition ? 1 : 0, .1f);
        }
        
        public static void ShowIf(this TMP_Text text, bool condition)
        {
            text.DOFade(condition ? 1 : 0, .1f);
        }
    }
}