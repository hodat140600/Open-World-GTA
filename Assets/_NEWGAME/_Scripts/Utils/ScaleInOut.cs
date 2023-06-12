using DG.Tweening;
using UnityEngine;

namespace _NEWGAME._Scripts.Utils
{
    public class ScaleInOut : MonoBehaviour
    {
        private void Start()
        {
            transform.DOScale(1.2f, .5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutFlash).SetUpdate(true);
        }
    }
}