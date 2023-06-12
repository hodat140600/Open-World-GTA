using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using _GAME._Scripts._CharacterController;

namespace _GAME._Scripts._ItemManager
{
    public class ChangeInputTypeTrigger : MonoBehaviour
    {
        [Header("Events called when InputType changed")]
        public UnityEvent OnChangeToKeyboard;
        public UnityEvent OnChangeToMobile;
        public UnityEvent OnChangeToJoystick;

        void Start()
        {
            ExtendInput.instance.onChangeInputType -= OnChangeInput;
            ExtendInput.instance.onChangeInputType += OnChangeInput;
            OnChangeInput(ExtendInput.instance.inputDevice);
        }

        public void OnChangeInput(InputDevice type)
        {
            switch (type)
            {
                case InputDevice.MouseKeyboard:
                    OnChangeToKeyboard.Invoke();
                    break;
                case InputDevice.Mobile:
                    OnChangeToMobile.Invoke();
                    break;
                case InputDevice.Joystick:
                    OnChangeToJoystick.Invoke();
                    break;
            }
        }
    }

}
