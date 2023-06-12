using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace _GAME._Scripts
{
    [System.AttributeUsage(System.AttributeTargets.Field | System.AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class BarDisplayAttribute : PropertyAttribute
    {
        public readonly string maxValueProperty;
        public readonly bool showJuntInPlayMode;
        public BarDisplayAttribute(string maxValueProperty, bool showJuntInPlayMode = false)
        {
            this.maxValueProperty = maxValueProperty;
            this.showJuntInPlayMode = showJuntInPlayMode;
        }
    }
}