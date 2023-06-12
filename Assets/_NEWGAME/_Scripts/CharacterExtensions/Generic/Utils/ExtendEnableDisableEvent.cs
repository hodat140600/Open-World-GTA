using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtendEnableDisableEvent : MonoBehaviour
{
    public UnityEngine.Events.UnityEvent onEnable, onDisable;
    private void OnEnable()
    {
        onEnable.Invoke();
    }
    private void OnDisable()
    {
        onDisable.Invoke();
    }
}
