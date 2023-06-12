using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace _GAME._Scripts
{
    public class WindowButton : MonoBehaviour
    {
        public Transform window;
        public UnityEvent onSelect;
        public UnityEvent onDeselect;
    }
}