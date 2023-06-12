using System;
using UnityEngine;

[DefaultExecutionOrder(0)] public class Target : MonoBehaviour
{
    public Sprite arrowSprite;
    public Sprite boxSprite;

    public Color targetColor        = Color.red;
    public bool  needBoxIndicator   = true;
    public bool  needArrowIndicator = true;
    public bool  needDistanceText   = true;
    public bool  activateOnStart    = false;
    
    
    [HideInInspector] public Indicator indicator;

    private void Start()
    {
        if(activateOnStart)
            Activate(true);
    }

    public void Activate(bool isActivated)
    {
        OffScreenIndicator.TargetStateChanged?.Invoke(this, isActivated);
        gameObject.SetActive(isActivated);
    }

    public float GetDistanceFromCamera(Vector3 cameraPosition)
    {
        return Vector3.Distance(cameraPosition, transform.position);
    }

    private void OnDisable()
    {
        Activate(false);
    }
}