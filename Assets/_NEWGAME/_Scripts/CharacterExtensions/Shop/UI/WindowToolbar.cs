using _GAME._Scripts._CharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
namespace _GAME._Scripts
{
    public class WindowToolbar : MonoBehaviour
    {
        public GenericInput selectRightInput;
        public GenericInput selectLeftInput;
        public WindowButton[] selectables;
        public int index;

        private void Start()
        {
            SetWindowActive();
        }

        public void Update()
        {
            if (selectRightInput.GetButtonDown())
            {
                SelectRight();
            }
            else if (selectLeftInput.GetButtonDown())
            {
                SelectLeft();
            }
        }
        public void SelectRight()
        {
            index = (index + 1) % selectables.Length;
            SetWindowActive();
        }
        public void SelectLeft()
        {
            index--; if (index < 0) index = selectables.Length - 1;
            SetWindowActive();
        }

        public void SelectThis(GameObject gameObject)
        {
            for (int i = 0; i < selectables.Length; i++)
            {
                if (selectables[i].gameObject == gameObject)
                {
                    index = i;
                    SetWindowActive();
                    break;
                }
            }
        }

        void SetWindowActive()
        {
            foreach (WindowButton wb in selectables)
            {
                wb.window.gameObject.SetActive(false);
                wb.onDeselect.Invoke();
            }

            selectables[index].window.gameObject.SetActive(true);
            selectables[index].onSelect.Invoke();
        }
    }
}