using PixelPlay.OffScreenIndicator;
using System;
using System.Collections.Generic;
using Assets._SDK.Logger;
using MyBox;
using UnityEngine;

[DefaultExecutionOrder(-1)] public class OffScreenIndicator : MonoBehaviour
{
    [Range(0.5f, 0.9f)] [Tooltip("Distance offset of the indicators from the centre of the screen")] [SerializeField] private float     screenBoundOffset = 0.5f;
    [SerializeField]                                                                                                          Transform targetHolder;

    public  Camera  mainCamera;
    private Vector3 _screenCentre;
    private Vector3 _screenBounds;

    private readonly List<Target> _targets   = new();
    private readonly Vector3      textOffset = new Vector3(0, -40, 0);

    public static Action<Target, bool> TargetStateChanged;

    void Awake()
    {
        _screenCentre      =  new Vector3(Screen.width, Screen.height, 0) / 2;
        _screenBounds      =  _screenCentre * screenBoundOffset;
        TargetStateChanged += HandleTargetStateChanged;

        SetupTarget();
    }

    private void SetupTarget()
    {
        foreach (Transform target in targetHolder)
            target.gameObject.SetActive(false);
    }

    void LateUpdate()
    {
        DrawIndicators();
    }
    
    Vector3   screenPosition;
    bool      isTargetVisible;
    float     distanceFromCamera;
    Indicator indicator;
    
    void DrawIndicators()
    {
        foreach (Target target in _targets)
        {
            screenPosition     = OffScreenIndicatorCore.GetScreenPosition(mainCamera, target.transform.position);
            isTargetVisible    = OffScreenIndicatorCore.IsTargetVisible(screenPosition);
            distanceFromCamera = target.needDistanceText ? target.GetDistanceFromCamera(mainCamera.transform.position) : float.MinValue; // Gets the target distance from the camera.
            indicator          = null;

            if (target.needBoxIndicator && isTargetVisible)
            {
                screenPosition.z = 0;
                indicator        = GetIndicator(ref target.indicator, IndicatorType.BOX); // Gets the box indicator from the pool.
            }
            else if (target.needArrowIndicator && !isTargetVisible)
            {
                float angle = float.MinValue;
                OffScreenIndicatorCore.GetArrowIndicatorPositionAndAngle(ref screenPosition, ref angle, _screenCentre, _screenBounds);
                indicator                    = GetIndicator(ref target.indicator, IndicatorType.ARROW); // Gets the arrow indicator from the pool.
                indicator.transform.rotation = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg);           // Sets the rotation for the arrow indicator.
            }

            if (indicator)
            {
                // Sets the image color of the indicator.
                // indicator.SetImageColor(target.targetColor);
                indicator.SetSprite(indicator.isArrow ? target.arrowSprite : target.boxSprite);

                //Set the distance text for the indicator.
                indicator.SetDistanceText(distanceFromCamera);

                //Sets the position of the indicator on the screen.
                indicator.transform.position = indicator.isBox ? screenPosition + 100f * Vector3.up : screenPosition;

                // Sets the rotation of the distance text of the indicator.
                indicator.SetTextRotation(Quaternion.identity);

                // Sets the position of the distance text of the indicator.
                indicator.SetTextPosition(textOffset);
            }
        }
    }

    private void HandleTargetStateChanged(Target target, bool active)
    {
        if (active)
        {
            $"added {target.transform.parent.name}".Log();
            _targets.Add(target);
        }
        else
        {
            target.indicator?.Activate(false);
            target.indicator = null;
            _targets.Remove(target);
        }
    }

    private Indicator GetIndicator(ref Indicator indicator, IndicatorType type)
    {
        if (indicator != null)
        {
            if (indicator.Type != type)
            {
                indicator.Activate(false);
                indicator = type == IndicatorType.BOX ? BoxObjectPool.current.GetPooledObject() : ArrowObjectPool.current.GetPooledObject();
                indicator.Activate(true); // Sets the indicator as active.
            }
        }
        else
        {
            indicator = type == IndicatorType.BOX ? BoxObjectPool.current.GetPooledObject() : ArrowObjectPool.current.GetPooledObject();
            indicator.Activate(true); // Sets the indicator as active.
        }

        return indicator;
    }

    private void OnDestroy()
    {
        TargetStateChanged -= HandleTargetStateChanged;
    }
}