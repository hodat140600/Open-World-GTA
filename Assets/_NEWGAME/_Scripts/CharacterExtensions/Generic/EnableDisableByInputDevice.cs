using _GAME._Scripts._CharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableDisableByInputDevice : MonoBehaviour
{
    public InputDevice inputDevice;
    public CheckMethod methodToCheck;
    public enum CheckMethod
    {
        Equals,
        Different,
    }
    void Start()
    {
        ExtendInput.instance.onChangeInputType -= OnChangeInput;
        ExtendInput.instance.onChangeInputType += OnChangeInput;
        OnChangeInput(ExtendInput.instance.inputDevice);
    }

    public void OnChangeInput(InputDevice type)
    {       
        bool validate = methodToCheck == CheckMethod.Different ? type != inputDevice : type == inputDevice;
        if(gameObject.activeSelf!=validate)gameObject.SetActive(validate);      
    }
  
}
