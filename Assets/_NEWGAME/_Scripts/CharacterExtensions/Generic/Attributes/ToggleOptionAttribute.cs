using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _GAME._Scripts
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public class ToggleOptionAttribute : PropertyAttribute
    {
        public string label, falseValue, trueValue;
        public ToggleOptionAttribute(string label = "", string falseValue = "No", string trueValue = "Yes")
        {
            this.label = label;
            this.falseValue = falseValue;
            this.trueValue = trueValue;
        }
    }
}