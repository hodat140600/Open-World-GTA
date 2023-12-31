using _GAME._Scripts;
using _GAME._Scripts._CharacterController;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace _GAME._Scripts._CharacterController._Actions
{
    [ClassHeader("Trigger Generic Action", false, iconName = "ThirdPersonExtensions/triggerIcon")]
    public class TriggerGenericAction : ExtendMonoBehaviour
    {
        [EditorToolbar("Input", order = 1)]

        public InputType inputType = InputType.GetButtonDown;

        [Tooltip("Input to make the action")]
        public GenericInput actionInput = new GenericInput("E", "A", "A");

        public enum InputType
        {
            GetButtonDown,
            GetDoubleButton,
            GetButtonTimer,
            AutoAction
        };

        [HelpBox("Time you have to hold the button *Only for GetButtonTimer*")]
        public float buttonTimer = 3f;
        [HelpBox("Add delay to start the input count *Only for GetButtonTimer*")]
        public float inputDelay = 0.1f;
        [HelpBox("*Only for GetButtonTimer* \n\n<b>TRUE: </b> Play the animation while you're holding the button \n" +
            "<b>FALSE: </b>Play the animation after you finish holding the button")]
        public bool playAnimationWhileHoldingButton = true;

        [HelpBox("Time to press the button twice *Only for GetDoubleButton*")]
        public float doubleButtomTime = 0.25f;

        [EditorToolbar("Trigger", order = 2)]
        public string actionName = "Action";
        public string actionTag = "Action";
        [HelpBox("Disable this trigger OnStart")]
        public bool disableOnStart = false;
        [HelpBox("Disable the Player's Capsule Collider Collision, useful for animations with closer interactions")]
        public bool disableCollision;
        [HelpBox("Disable the Player's Rigidbody Gravity, useful for on air animations")]
        public bool disableGravity;
        [HelpBox("It will only use the trigger if the forward of the character is close to the forward of this transform")]
        public bool activeFromForward;
        [HelpBox("Max angle between character forward and trigger forward to active trigger"), Range(5, 180)]
        public float forwardAngle = 55;
        [HelpBox("Rotate Character to the Forward Rotation of this Trigger")]
        public bool useTriggerRotation;
        [HelpBox("Destroy this Trigger after pressing the Input or AutoAction or finishing the Action")]
        public bool destroyAfter = false;
        [_Scripts.LeoHideInInspector("destroyAfter")]
        public float destroyDelay = 0f;
        [HelpBox("Change your CameraState to a Custom State while playing the animation")]
        public string customCameraState;

        [EditorToolbar("Animation", order = 2)]

        [HelpBox("Trigger a Animation - Use the exactly same name of the AnimationState you want to trigger, " +
            "don't forget to add a vAnimatorTag to your State")]
        public string playAnimation;

        public float crossFadeTransition = 0.25f;

        public int animatorLayer = 0;

        [HelpBox("Check the Exit Time of your animation (if it doesn't loop) and insert here. \n\n" +
            "For example if your Exit Time is 0.82 you need to insert 0.82" +
            "\n\nAlways check with the Debug of the GenericAction if your animation is finishing correctly, " +
            "otherwise the controller won't reset to the default physics and collision.", HelpBoxAttribute.MessageType.Warning)]
        [Tooltip("You can use this to make a persistent action, and finish the action calling FinishAction method of the vGenericAction  component in your character")]
        public bool endActionManualy = false;
        [_Scripts.LeoHideInInspector("endActionManualy", invertValue = true)]
        public float endExitTimeAnimation = 0.8f;
        [HelpBox("Use a ActionState value to apply special conditions for your AnimatorController transitions")]
        public int animatorActionState = 0;
        [HelpBox("Reset the ActionState parameter to 0 after playing the animation")]
        public bool resetAnimatorActionState = true;
        [HelpBox("Use a empty transform as reference for the MatchTarget")]
        public Transform matchTarget;
        [HelpBox("Select the bone you want to use as reference to the Match Target")]
        public AvatarTarget avatarTarget;
        [Header("Curve Match target system")]
        public bool useLocalX = false;
        public bool useLocalZ = true;
        public AnimationCurve matchPositionXZCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(.5f, 1), new Keyframe(1, 1));
        public AnimationCurve matchPositionYCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(.5f, 1), new Keyframe(1, 1));
        public AnimationCurve matchRotationCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(.5f, 1), new Keyframe(1, 1));

        [EditorToolbar("Events", order = 3)]

        [Tooltip("Delay to run the OnDoAction Event")]
        [FormerlySerializedAs("onDoActionDelay")]
        public float onPressActionDelay;

        [Header("--- INPUT EVENTS ---")]
        [FormerlySerializedAs("OnDoAction")]
        public UnityEvent OnPressActionInput;
        public OnDoActionWithTarget onPressActionInputWithTarget;

        [Header("--- ONLY FOR GET BUTTON TIMER ---")]
        public UnityEvent OnCancelActionInput;
        public UnityEvent OnFinishActionInput;
        public OnUpdateValue OnUpdateButtonTimer;

        [Header("--- ANIMATION EVENTS ---")]
        public UnityEvent OnStartAnimation;
        public UnityEvent OnEndAnimation;

        [Header("--- PLAYER AND TRIGGER DETECTION ---")]
        public OnDoActionWithTarget OnPlayerEnter;
        public OnDoActionWithTarget OnPlayerStay;
        public OnDoActionWithTarget OnPlayerExit;
        [Header("--- ACTION VALIDATION  ---")]
        public OnDoActionWithTarget OnValidate;
        public OnDoActionWithTarget OnInvalidate;
        private float currentButtonTimer;
        internal Collider _collider;

        protected virtual void Start()
        {
            gameObject.tag = actionTag;
            gameObject.layer = LayerMask.NameToLayer("Triggers");
            _collider = GetComponent<Collider>();
            _collider.isTrigger = true;
            if (disableOnStart)
                enabled = false;
        }

        public virtual IEnumerator OnPressActionDelay(GameObject obj)
        {
            yield return new WaitForSeconds(onPressActionDelay);
            OnPressActionInput.Invoke();
            if (obj)
                onPressActionInputWithTarget.Invoke(obj);
        }

        public void UpdateButtonTimer(float value)
        {
            if (value != currentButtonTimer)
            {
                currentButtonTimer = value;
                OnUpdateButtonTimer.Invoke(value);
            }
        }

        [System.Serializable]
        public class OnUpdateValue : UnityEvent<float>
        {

        }
    }

    [System.Serializable]
    public class OnDoActionWithTarget : UnityEvent<GameObject>
    {

    }

}