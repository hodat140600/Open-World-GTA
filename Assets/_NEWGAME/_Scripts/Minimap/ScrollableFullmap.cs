using System;
using MyBox;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _PHAT.Scripts.UI.Map
{
    public class ScrollableFullmap : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private Camera minimapCamera;

        private       Vector3 _originalPosition;
        private const float   DragRatio     = 0.2f;
        private const float   MinPinchSpeed = 0.5f;

        private float currentPinchDist = 0;
        private float lastPinchDist    = 0;

        private Vector2 currentDragPos;
        private Vector2 lastDragPos;
        private Vector2 dragOffset;

        private bool IsDragging => Input.touchCount == 1;
        private bool IsPinching => Input.touchCount == 2 && Input.GetTouch(0).phase == TouchPhase.Moved && Input.GetTouch(1).phase == TouchPhase.Moved;


        public TMP_Text test;

        private string testString
        {
            set { test.SetText(value); }
        }

        private void Awake()
        {
            var minLength = Mathf.Min(Screen.width, Screen.height);
            transform.GetComponent<RectTransform>().sizeDelta = minLength * Vector2.one;
        }

        // Vector3 curDist;
        // Vector3 prevDist;
        // float   touchDelta;
        // float   speedTouch0;
        // float   speedTouch1;
        //
        // private float minPinchSpeed       = 1f;
        // private float speed               = 1f;
        // private float varianceInDistances = .5f;

        // void Update()
        // {
        // if (IsDragging)
        // {
        //     // testString = "dragging";
        //     Touch touch0 = Input.GetTouch(0);
        //
        //     if (touch0.phase == TouchPhase.Began)
        //     {
        //         currentDragPos = touch0.position;
        //         dragOffset     = touch0.position;
        //     }
        //
        //     if (touch0.phase == TouchPhase.Moved)
        //     {
        //         var newPos = touch0.position;
        //         currentDragPos = lastDragPos - newPos;
        //         lastDragPos    = newPos;
        //
        //         minimapCamera.transform.position = -((Vector3)dragOffset + (.2f * new Vector3(currentDragPos.x, 0, currentDragPos.y)));
        //         testString                       = minimapCamera.transform.position.ToString();
        //     }
        // }
        // else if (IsPinching)
        // {
        //     testString = "pinching";
        //     Touch touch0 = Input.GetTouch(0);
        //     Touch touch1 = Input.GetTouch(1);
        //
        //     curDist     = touch0.position - touch1.position;                                                 //current distance between finger touches
        //     prevDist    = touch0.position - touch0.deltaPosition - (touch1.position - touch1.deltaPosition); //difference in previous locations using delta positions
        //     touchDelta  = curDist.magnitude - prevDist.magnitude;
        //     // speedTouch0 = touch0.deltaPosition.magnitude / touch0.deltaTime;
        //     // speedTouch1 = touch1.deltaPosition.magnitude / touch1.deltaTime;
        //
        //     if ((touchDelta + varianceInDistances <= 1) )
        //     {
        //         minimapCamera.orthographicSize = Mathf.Clamp(minimapCamera.orthographicSize + (1 * speed), 15, 90);
        //     }
        //
        //     if ((touchDelta + varianceInDistances > 1) )
        //     {
        //         minimapCamera.orthographicSize = Mathf.Clamp(minimapCamera.orthographicSize - (1 * speed), 15, 90);
        //     }
        // }
        // else
        // {
        //     testString = "nothing";
        // }
        // }

        // private float _mouseX, _mouseY;
        // private float mouseSpeedMultiplier = 1f;
        // private float smoothSpeed          = .04f;
        // private float x, y, z;
        // private bool  _dragging;

        // public void OnDrag(PointerEventData eventData)
        // {
        //     _dragging = true;
        //
        //     _mouseX += eventData.delta.x * mouseSpeedMultiplier;
        //     _mouseY += eventData.delta.y * mouseSpeedMultiplier;
        // }
        //
        // public void OnEndDrag(PointerEventData eventData)
        // {
        //     _dragging = false;
        // }

        private Vector3 _followVelocity;
        private float   height = 200f;
        private Vector3 target;


        public void OnBeginDrag(PointerEventData eventData)
        {
            target = minimapCamera.transform.position;
        }

        public void OnDrag(PointerEventData eventData)
        {
            target += new Vector3(eventData.delta.x, 0, eventData.delta.y);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
        }

        private void LateUpdate()
        {
            // var position = Vector3.SmoothDamp(minimapCamera.transform.position, target, ref _followVelocity, 1.5f);
            // minimapCamera.transform.position = position.SetY(height);
        }
    }
}