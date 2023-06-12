using _NEWGAME._Scripts.Game;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    [SerializeField] private Sounds sound;

    public void Play()
    {
        SfxManager.Instance.Play(sound);
    }
}