using DG.Tweening;
using MyBox;
using UnityEngine;

namespace _NEWGAME._Scripts.Utils
{
    public class Bounce : MonoBehaviour
    {
        private Transform _transform;
        private Vector3?  _startingRotation;

        [SerializeField] private float angle, duration;
        [SerializeField] private Ease  ease;

        private void OnEnable()
        {
            _transform        ??= this.transform;
            _startingRotation ??= _transform.eulerAngles;

            _transform
                .DORotate(_startingRotation?.SetZ(angle) ?? Vector3.zero, duration, RotateMode.WorldAxisAdd)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(ease)
                .SetUpdate(true);
        }

        private void OnDisable()
        {
            _transform.DOKill();
        }
    }
}