using System;
using _GAME._Scripts;
using _GAME._Scripts.Game;
using _NEWGAME._Scripts.Game;
using UnityEngine;
using UnityEngine.UI;

namespace _SDK.UI
{
    public abstract class AbstractPanel : MonoBehaviour
    {
        public Animator animator { get; private set; }

        private void Awake()
        {
            if (TryGetComponent(out Animator Animator))
            {
                animator = Animator;
            }
        }

        protected void LetFireOnClick(Button button, GameTrigger trigger, Sounds sound = Sounds.Null)
        {
            button.onClick.AddListener(() =>
            {
                GameManager.Instance.Fire(trigger);
                SfxManager.Instance.Play(sound);
            });
        }

        protected void LetFireOnGameplayClick(Button button, GameplayTrigger trigger, Sounds sound = Sounds.Null)
        {
            button.onClick.AddListener(() =>
            {
                Gameplay.Instance.Fire(trigger);
                SfxManager.Instance.Play(sound);
            });
        }
    }
}