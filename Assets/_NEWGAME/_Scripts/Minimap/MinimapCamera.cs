using System;
using System.Collections.Generic;
using DG.Tweening;
using MyBox;
using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    [SerializeField]         Transform      followPositionOf;
    [SerializeField]         Transform      followRotationOf;
    [SerializeField] private Transform      followRotationSprite;
    [SerializeField] private SpriteRenderer minimapWithNameRenderer;

    [field: SerializeField]
    public Camera Camera { get; private set; }

    private Vector3   followVelocity = Vector3.zero;
    private float     rotateVelocity = 0;
    private Transform cachedTransform;
    private float     y;

    private bool freezeRotationAndPosition;

    private       List<Transform>         lockRotations;
    public static Action<Transform, bool> LockRotate;


    private const int MINFOV = 80, MAXFOV = 200;

    private void Awake()
    {
        y               =  followPositionOf.position.y;
        cachedTransform =  transform;
        lockRotations   =  new();
        LockRotate      += AddToLockRotationsList;
    }

    private void OnDestroy()
    {
        LockRotate -= AddToLockRotationsList;
    }

    private void AddToLockRotationsList(Transform transform, bool isActive)
    {
        if (isActive)
            lockRotations.Add(transform);
        else if (lockRotations.Contains(transform))
            lockRotations.Remove(transform);
    }

    private void LateUpdate()
    {
        Vector3 angle = followRotationOf.eulerAngles;
        RotateSprite(angle);

        if (freezeRotationAndPosition) return;

        MoveCamera();
        RotateCamera(angle);
    }

    private void RotateCamera(Vector3 angle)
    {
        float yDampAngle = Mathf.SmoothDampAngle(cachedTransform.eulerAngles.y, angle.y, ref rotateVelocity, .25f);
        cachedTransform.rotation = Quaternion.Euler(90f, yDampAngle, 1f);

        foreach (Transform lockRotation in lockRotations)
        {
            lockRotation.rotation = Quaternion.Euler(90f, yDampAngle, 1f);
        }
    }

    private void RotateSprite(Vector3 angle)
    {
        Vector3 flatAngle = new(90, 0, -angle.y);
        followRotationSprite.eulerAngles = flatAngle;
    }

    private void MoveCamera()
    {
        cachedTransform.position = followPositionOf.position.SetY(y);
    }

    public void ZoomOut()
    {
        freezeRotationAndPosition = true;
        transform.DORotate(new Vector3(90, 0, 0), .53f).SetUpdate(true);
        Camera.DOOrthoSize(MAXFOV, .53f).SetEase(Ease.OutFlash).SetUpdate(true);
        minimapWithNameRenderer.DOFade(1, .53f).SetEase(Ease.OutFlash).SetUpdate(true);
    }

    public void ZoomIn()
    {
        Camera.DOOrthoSize(MINFOV, .53f).SetEase(Ease.OutFlash).SetUpdate(true);
        minimapWithNameRenderer.DOFade(0, .53f).SetEase(Ease.OutFlash).SetUpdate(true);
        freezeRotationAndPosition = false;
    }
}