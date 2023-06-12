using System;
using UnityEngine;
namespace _GAME._Scripts
{
    [Serializable]
    public abstract class AnimatorSetValue<T> : StateMachineBehaviour where T : IConvertible
    {
        public string animatorParameter = "My Animator Parameter";
        public bool setOnEnter;
        [_Scripts.LeoHideInInspector("setOnEnter")]
        public T enterValue;
        public bool setOnExit;
        [_Scripts.LeoHideInInspector("setOnExit")]
        public T exitValue;

        protected virtual T GetEnterValue()
        {
            return enterValue;
        }

        protected virtual T GetExitValue()
        {
            return exitValue;
        }

        override public sealed void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (setOnEnter)
            {
                if (typeof(T).Equals(typeof(int)))
                    animator.SetInteger(animatorParameter, (int)(object)GetEnterValue());
                else if (typeof(T).Equals(typeof(float)))
                    animator.SetFloat(animatorParameter, (float)(object)GetEnterValue());
                else if (typeof(T).Equals(typeof(bool)))
                    animator.SetBool(animatorParameter, (bool)(object)GetEnterValue());
            }
        }

        override public sealed void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (setOnExit)
            {
                if (typeof(T).Equals(typeof(int)))
                    animator.SetInteger(animatorParameter, (int)(object)GetExitValue());
                else if (typeof(T).Equals(typeof(float)))
                    animator.SetFloat(animatorParameter, (float)(object)GetExitValue());
                else if (typeof(T).Equals(typeof(bool)))
                    animator.SetBool(animatorParameter, (bool)(object)GetExitValue());
            }
        }
    }
}