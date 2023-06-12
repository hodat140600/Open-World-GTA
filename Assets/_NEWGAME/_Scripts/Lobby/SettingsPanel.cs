using System;
using _GAME._Scripts.Game;
using _NEWGAME._Scripts.Game;
using _SDK.UI;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace _GAME._Scripts.LobbyUI
{
    public class SettingsPanel : AbstractPanel
    {
        public Button backToPlayingButton;
        public Slider soundSlider, musicSlider;
        public Button backToLobbyButton;

        [SerializeField] private AudioMixer SfxMixer, BgmMixer;
        private const            string     Volume_Parameter = "Volume";

        private string SfxVolumeKey = nameof(SfxVolumeKey);
        private string BgmVolumeKey = nameof(BgmVolumeKey);

        private void Start()
        {
            Load();
            LetFireOnClick(backToLobbyButton, GameTrigger.BackToLobby, Sounds.Select);

            soundSlider.onValueChanged.AddListener(SoundSliderValueChanged);
            musicSlider.onValueChanged.AddListener(MusicSliderValueChanged);

            if (backToPlayingButton)
            {
                LetFireOnGameplayClick(backToPlayingButton, GameplayTrigger.BackToPlaying, Sounds.Select);
            }
        }

        private void Load()
        {
            float sfxValue = PlayerPrefs.GetFloat(SfxVolumeKey, 0);
            float bgmValue = PlayerPrefs.GetFloat(BgmVolumeKey, 0);
            BgmMixer.SetFloat(Volume_Parameter, bgmValue);
            SfxMixer.SetFloat(Volume_Parameter, sfxValue);
            soundSlider.value = sfxValue;
            musicSlider.value = bgmValue;
        }

        private void SaveSfx(float value)
        {
            PlayerPrefs.SetFloat(SfxVolumeKey, value);
        }

        private void SaveBgm(float value)
        {
            PlayerPrefs.SetFloat(BgmVolumeKey, value);
        }

        private void MusicSliderValueChanged(float value)
        {
            BgmMixer.SetFloat(Volume_Parameter, value);
            SaveBgm(value);
        }

        private void SoundSliderValueChanged(float value)
        {
            SfxMixer.SetFloat(Volume_Parameter, value);
            SaveSfx(value);
        }
    }
}