using _GAME._Scripts;
using UnityEngine;

public class ChangeAnimatorUpdateMode : MonoBehaviour
{
    public Animator animator;
    private readonly AnimatorUpdateMode initialState = AnimatorUpdateMode.AnimatePhysics;

    public void Reset()
    {
        animator = GetComponentInParent<Animator>();
    }

    private void Start()
    {
        if (!animator) animator = GetComponentInParent<Animator>();
    }

    public void ChangeToUnscaledTime()
    {
        if (Time.timeScale == 0)
        {
            dTime.useUnscaledTime = true;
            animator.updateMode = AnimatorUpdateMode.UnscaledTime;
        }

    }

    public void ChangeToAnimatePhysics()
    {
        animator.updateMode = AnimatorUpdateMode.AnimatePhysics;
        dTime.useUnscaledTime = false;
    }

    public void ChangeToInitialState()
    {
        animator.updateMode = initialState;
        dTime.useUnscaledTime = initialState == AnimatorUpdateMode.UnscaledTime ? true : false;
    }
}
