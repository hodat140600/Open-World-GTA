using UnityEngine;
using UnityEngine.Audio;

public class VolumeLoader : MonoBehaviour
{
    [SerializeField] private AudioMixer SfxMixer, BgmMixer;

    private void Start()
    {
        string SfxVolumeKey     = nameof(SfxVolumeKey);
        string BgmVolumeKey     = nameof(BgmVolumeKey);
        string Volume_Parameter = "Volume";
        SfxMixer.SetFloat(Volume_Parameter, PlayerPrefs.GetFloat(SfxVolumeKey, 0));
        BgmMixer.SetFloat(Volume_Parameter, PlayerPrefs.GetFloat(BgmVolumeKey, 0));
    }
}