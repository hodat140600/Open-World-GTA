using UnityEngine;
namespace _GAME._Scripts._Utils
{
    public class ResetTrigger : StateMachineBehaviour
    {
        public bool resetOnEnter, resetOnExit;
        public string trigger;
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (resetOnEnter)
                animator.ResetTrigger(trigger);
        }
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (resetOnExit)
                animator.ResetTrigger(trigger);
        }
    }
}