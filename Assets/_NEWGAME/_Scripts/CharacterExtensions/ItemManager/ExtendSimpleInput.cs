using _GAME._Scripts;
using _GAME._Scripts._CharacterController;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

[ClassHeader("Simple Input", openClose = false)]
public class ExtendSimpleInput : ExtendMonoBehaviour
{
    [Tooltip("Input to press")]
    public GenericInput input = new GenericInput("Escape", "B", "B");
    [Tooltip("This Gameobject will turn off after the input is pressed")]
    public bool disableThisObjectAfterInput = true;
    public UnityEvent OnPressInput;

    void Update()
    {
        if (input.GetButtonDown() && gameObject.activeSelf)
        {
            if (disableThisObjectAfterInput)
            {
                this.gameObject.SetActive(false);
            }

            OnPressInput.Invoke();
        }
    }
}
