using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEditor;
using UnityEngine;

namespace _NEWGAME._Scripts.Game
{
    public enum Sounds
    {
        Null,
        Select,
        GunSelect,
        ZoomIn,
        Honk,
        SeeTinh,
        MissionReceive,
        Headshot,
    }

    public class SfxManager : GameSingleton<SfxManager>
    {
        [SerializeField]
        private List<Sounds> soundsEnum;

        [SerializeField]
        private List<AudioSource> soundsSources;

        [SerializeField]
        private AudioClip[] soundsNextBot;

        protected override void Awake()
        {
            base.Awake();
            Generate();
        }

        [Button("Generate")]
        private void Generate()
        {
            soundsEnum    = new List<Sounds>();
            soundsSources = new List<AudioSource>();

            foreach (Sounds Sound in Enum.GetValues(typeof(Sounds)))
            {
                AudioSource soundsSource = transform.Find(Sound.ToString())?.GetComponent<AudioSource>();
                soundsEnum.Add(Sound);
                soundsSources.Add(soundsSource);
            }
        }

        public AudioClip GetNextBotDeadAudioClip()
        {
            int audioClipIndex = UnityEngine.Random.Range(0, soundsNextBot.Length);
            return soundsNextBot[audioClipIndex];
        }


        public void Play(Sounds sound)
        {
            if (sound != Sounds.Null)
            {
                AudioSource soundSource = soundsSources[soundsEnum.IndexOf(sound)];
                if (!soundSource.isPlaying) soundSource.Play();
            }    
        }

        public AudioClip GetAudioClip(Sounds sound)
        {
            return sound != Sounds.Null ? soundsSources[soundsEnum.IndexOf(sound)].clip : null;
        }
    }
}