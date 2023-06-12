using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace _GAME._Scripts
{
    public class RandomTalkingState : StateMachineBehaviour
    {

        public string[] animNames;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.Play(animNames[Random.Range(0, animNames.Length)]);
        }

    }

}
