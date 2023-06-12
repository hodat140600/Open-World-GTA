using UnityEngine;
using UnityEditor;

namespace _GAME._Scripts
{
    [CustomPropertyDrawer(typeof(LeoHideInInspectorAttribute), true)]
    public class HideInInspectorDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            label = EditorGUI.BeginProperty(position, label, property);
            LeoHideInInspectorAttribute _attribute = attribute as LeoHideInInspectorAttribute;

            if (_attribute != null && property.serializedObject.targetObject)
            {
                var propertyName = property.propertyPath.Replace(property.name, "");
                var booleamProperties = _attribute.refbooleanProperty.Split(';');
                for (int i = 0; i < booleamProperties.Length; i++)
                {
                    var booleanProperty = property.serializedObject.FindProperty(propertyName + booleamProperties[i]);
                    if (booleanProperty != null)
                    {
                        _attribute.hideProperty = _attribute.invertValue ? booleanProperty.boolValue : !booleanProperty.boolValue;
                        if (_attribute.hideProperty)
                        {
                            break;
                        }
                    }
                    else
                    {

                        EditorGUI.PropertyField(position, property, label, true);
                    }
                }
                if (!_attribute.hideProperty)
                {
                    EditorGUI.PropertyField(position, property, label, true);
                }
            }
            else
                EditorGUI.PropertyField(position, property, label, true);
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            LeoHideInInspectorAttribute _attribute = attribute as LeoHideInInspectorAttribute;
            if (_attribute != null)
            {
                var propertyName = property.propertyPath.Replace(property.name, "");
                var booleamProperties = _attribute.refbooleanProperty.Split(';');
                var valid = true;
                for (int i = 0; i < booleamProperties.Length; i++)
                {
                    var booleamProperty = property.serializedObject.FindProperty(propertyName + booleamProperties[i]);
                    if (booleamProperty != null)
                    {
                        valid = _attribute.invertValue ? !booleamProperty.boolValue : booleamProperty.boolValue;
                        if (!valid) break;
                    }
                }
                if (valid) return base.GetPropertyHeight(property, label);
                else return 0;
            }
            return base.GetPropertyHeight(property, label);
        }

    }
}