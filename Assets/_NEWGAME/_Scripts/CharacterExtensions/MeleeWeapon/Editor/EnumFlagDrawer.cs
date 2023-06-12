using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace _GAME._Scripts
{
    [CustomPropertyDrawer(typeof(EnumFlagAttribute))]
    public class EnumFlagDrawer : PropertyDrawer
    {

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EnumFlagAttribute flagSettings = (EnumFlagAttribute)attribute;

            string propName = flagSettings.enumName;
            if (string.IsNullOrEmpty(propName))
                propName = property.displayName;
            if (property.propertyType == SerializedPropertyType.Enum)
            {
                EditorGUI.BeginProperty(position, label, property);
                property.intValue = EditorGUI.MaskField(position, propName, property.intValue, Enum.GetNames(fieldInfo.FieldType));
                EditorGUI.EndProperty();
            }
            else EditorGUI.PropertyField(position, property, property.hasChildren);

        }
    }

}