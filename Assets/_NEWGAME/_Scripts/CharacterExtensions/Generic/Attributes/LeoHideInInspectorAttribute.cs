using UnityEngine;
using System.Collections;
using System;

namespace _GAME._Scripts
{
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = true)]
    public class LeoHideInInspectorAttribute : PropertyAttribute
    {
        public bool hideProperty { get; set; }
        public string refbooleanProperty;
        public bool invertValue;
        public LeoHideInInspectorAttribute(string refbooleanProperty, bool invertValue = false)
        {
            this.refbooleanProperty = refbooleanProperty;
            this.invertValue = invertValue;
        }

    }
}
