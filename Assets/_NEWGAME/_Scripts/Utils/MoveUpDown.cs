using DG.Tweening;
using UnityEngine;

public class MoveUpDown : MonoBehaviour
{
    [SerializeField] private float offset, duration;
    [SerializeField] private Ease  ease;
    [SerializeField] private bool  update;

    void Start()
    {
        transform
            .DOLocalMoveY(transform.localPosition.y + offset, duration)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(ease)
            .SetUpdate(update);
    }
}