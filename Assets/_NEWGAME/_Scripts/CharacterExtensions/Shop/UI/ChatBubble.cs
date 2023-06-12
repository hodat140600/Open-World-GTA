using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
namespace _GAME._Scripts
{
    public class ChatBubble : MonoBehaviour
    {
        public Text messageText;
        public bool useAnimation;
        public Animator animator;
        public string talkingAnimation = "Talk";
        public string idleAnimation = "Stop";

        public UnityEvent onMessageStart;
        public UnityEvent onCharacterChange;
        public UnityEvent onMessageComplete;

        private int characterIndex;
        private string message;


        public void StopAnimation()
        {
            if (useAnimation && animator)
                animator.CrossFade(idleAnimation, 0);

        }
        /// <summary>
        /// Add a message
        /// </summary>
        public void AddMessage(string message, float timePerCharacter)
        {
            StartCoroutine(WriteText(message, timePerCharacter));
            onMessageStart.Invoke();

            if (useAnimation && animator)
                animator.CrossFade(talkingAnimation, 0);
            if (useAnimation && !animator)
                Debug.LogError("No animator has been set on " + gameObject.name);

        }
        /// <summary>
        /// Writes Chat text
        /// </summary>
        private IEnumerator WriteText(string message, float timePerCharacter)
        {
            characterIndex = 0;
            float timer;
            timer = Time.time + timePerCharacter;
            this.message = message;
            while (true)
            {
                yield return new WaitForSeconds(0.01f);
                if (Time.time >= timer)
                {
                    characterIndex++;
                    timer = Time.time + timePerCharacter;
                    string text = message.Substring(0, characterIndex);
                    text += "<color=#00000000>" + message.Substring(characterIndex) + "</color>";
                    messageText.text = text;
                    onCharacterChange.Invoke();
                    if (characterIndex >= message.Length)
                    {
                        onMessageComplete.Invoke();
                        if (useAnimation && animator)
                            animator.CrossFade(idleAnimation, 0);
                        break;
                    }
                }
            }

        }

        public void Skip()
        {
            StopAllCoroutines();
            messageText.text = message;
            StopAnimation();
            onMessageComplete.Invoke();
        }
    }
}