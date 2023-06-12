using DG.Tweening;
using UnityEngine;

public class ShadowPeople : MonoBehaviour
{
    [SerializeField] private ParticleSystemEmissionController system;
    [SerializeField] private AudioSource                      whispering;

    [SerializeField] private Transform stopFactor;
    [SerializeField] private             float     ANGLE_TO_DISAPPEAR = 30f;

    private bool _isAppearing = false;

    private void Start()
    {
        system.Stop();
        whispering.volume = 0;
        _isAppearing      = false;
    }

    private void Update()
    {
        if (!_isAppearing) return;

        if (stopFactor.eulerAngles.x >= ANGLE_TO_DISAPPEAR)
            Invoke(nameof(Disappear), 1f);
    }

    public void Appear()
    {
        system.Start();
        whispering.DOFade(.6f, 1f);
        _isAppearing = true;
    }

    private void Disappear()
    {
        system.Stop();
        whispering.DOFade(0f, 1f);
        _isAppearing = false;
    }
}