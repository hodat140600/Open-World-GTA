using UnityEngine;
using UnityEditor;
using System;

namespace _GAME._Scripts
{
    [CustomPropertyDrawer(typeof(CheckPropertyAttribute), true)]
    public class CheckPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            label = EditorGUI.BeginProperty(position, label, property);
            CheckPropertyAttribute _attribute = attribute as CheckPropertyAttribute;

            if (_attribute != null && property.serializedObject.targetObject)
            {
                var propertyName = property.propertyPath.Replace(property.name, "");
                var checkValues = _attribute.checkValues;

                var valid = Validate(property, _attribute);
                if (valid || !_attribute.hideInInspector)
                {
                    GUI.enabled = valid;
                    GUI.color = valid ? Color.white : Color.grey * 0.5f;

                    EditorGUI.PropertyField(position, property, label, true);
                    GUI.enabled = true;
                    GUI.color = Color.white;
                }

            }
            else
                EditorGUI.PropertyField(position, property, true);
            EditorGUI.EndProperty();
        }

        private bool Validate(SerializedProperty property, CheckPropertyAttribute _attribute)
        {
            var propertyName = property.propertyPath.Replace(property.name, "");
            var checkValues = _attribute.checkValues;
            var valid = true;
            for (int i = 0; i < checkValues.Count; i++)
            {
                var prop = property.serializedObject.FindProperty(propertyName + checkValues[i].property);

                switch (prop.propertyType)
                {
                    case SerializedPropertyType.Boolean:
                        valid = prop.boolValue.Equals(checkValues[i].value);
                        break;
                    case SerializedPropertyType.Enum:
                        int index = Array.IndexOf(Enum.GetValues(checkValues[i].value.GetType()), checkValues[i].value);
                        valid = prop.enumValueIndex.Equals(index);
                        break;
                }

                if (!valid) break;
            }

            return valid;
        }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            CheckPropertyAttribute _attribute = attribute as CheckPropertyAttribute;

            var valid = Validate(property, _attribute) || !_attribute.hideInInspector;
            return valid ? base.GetPropertyHeight(property, label) : 0;
        }
    }
}