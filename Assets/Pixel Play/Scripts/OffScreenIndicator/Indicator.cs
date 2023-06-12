using System;
using Assets._SDK.Logger;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Assign this script to the indicator prefabs.
/// </summary>
public class Indicator : MonoBehaviour
{
    [SerializeField] private IndicatorType indicatorType;
    [SerializeField] private Image         indicatorImage;
    private                  TMP_Text      distanceText;

    /// <summary>
    /// Gets if the game object is active in hierarchy.
    /// </summary>
    public bool Active
    {
        get { return transform.gameObject.activeInHierarchy; }
    }

    /// <summary>
    /// Gets the indicator type
    /// </summary>
    public IndicatorType Type => indicatorType;

    public bool isBox => Type == IndicatorType.BOX;
    public bool isArrow => Type == IndicatorType.ARROW;

    void Awake()
    {
        distanceText   = transform.GetComponentInChildren<TMP_Text>();
    }

    /// <summary>
    /// Sets the image color for the indicator.
    /// </summary>
    /// <param name="color"></param>
    public void SetImageColor(Color color)
    {
        indicatorImage.color = color;
    }

    /// <summary>
    /// Sets the distance text for the indicator.
    /// </summary>
    /// <param name="value"></param>
    public void SetDistanceText(float value)
    {
        distanceText.text = value >= 0 ? Mathf.Floor(value) + "m" : "";
    }

    /// <summary>
    /// Sets the distance text rotation of the indicator.
    /// </summary>
    /// <param name="rotation"></param>
    public void SetTextRotation(Quaternion rotation)
    {
        distanceText.rectTransform.rotation = rotation;
    }

    public void SetTextPosition(Vector3 textOffset)
    {
        distanceText.rectTransform.position = transform.position + textOffset;
    }
    
    /// <summary>
    /// Sets the indicator as active or inactive.
    /// </summary>
    /// <param name="value"></param>
    public void Activate(bool value)
    {
        transform.gameObject.SetActive(value);
    }

    public void SetSprite(Sprite sprite)
    {
        indicatorImage.sprite = sprite;
    }
}

public enum IndicatorType
{
    BOX,
    ARROW
}