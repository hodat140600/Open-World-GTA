using UnityEngine;
using UnityEditor;
using System;

namespace _GAME._Scripts
{
    [CustomPropertyDrawer(typeof(ToggleOptionAttribute), true)]
    public class ToggleOptionAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType == SerializedPropertyType.Boolean)
            {

                var toogle = attribute as ToggleOptionAttribute;
                if (toogle.label != "") label.text = toogle.label;
                var options = new GUIContent[] { new GUIContent(toogle.falseValue), new GUIContent(toogle.trueValue) };
                property.boolValue = Convert.ToBoolean(EditorGUI.Popup(position, label, Convert.ToInt32(property.boolValue), options));
            }
            else
            {
                EditorGUI.PropertyField(position, property, label, true);
            }
        }
    }
}