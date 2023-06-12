using System;
using Assets._SDK.Logger;
using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace _GAME._Scripts.Npc
{
    public class HeadshotPanel : MonoBehaviour
    {
        [SerializeField] private Image headshotImage;

        private Vector2 _defaultPos, _stutterPos;

        // uncomment Start to use this
        // private void Start()
        // {
        //     _defaultPos = headshotImage.rectTransform.anchoredPosition;
        //     MessageBroker.Default.Receive<NpcKilledEvent>().Where(enemy => enemy.IsHeadshot)
        //         .Subscribe(_ => ShowHeadshotEffect()).AddTo(this);
        // }

        private void ShowHeadshotEffect()
        {
            headshotImage ??= GetComponentInChildren<Image>();
            _stutterPos   =   headshotImage.rectTransform.anchoredPosition + Random.insideUnitCircle * 20;

            headshotImage.DOKill();
            headshotImage.transform.DOScale(1.2f, .2f).From(0f).SetUpdate(true);
            headshotImage.DOFade(1, .2f).From(0f).SetUpdate(true);
            headshotImage.transform.DOScale(1f, .2f).SetDelay(.2f).SetUpdate(true);

            // stutter
            headshotImage.rectTransform.DOAnchorPos(_stutterPos, .1f).SetDelay(.1f).SetUpdate(true);
            headshotImage.rectTransform.DOAnchorPos(_defaultPos, .1f).SetDelay(.2f).SetUpdate(true);
            headshotImage.DOFade(0, 1f).SetDelay(1f).SetUpdate(true);
        }
    }
}