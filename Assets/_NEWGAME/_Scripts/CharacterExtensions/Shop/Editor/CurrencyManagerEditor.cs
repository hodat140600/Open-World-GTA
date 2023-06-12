using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
namespace _GAME._Scripts
{
    [CustomEditor(typeof(CurrencyManager))]
    public class CurrencyManagerEditor : Editor
    {
        GUISkin skin;
        SerializedProperty nameProp;
        SerializedProperty amountProp;
        SerializedProperty iconProp;


        void OnEnable()
        {
            // Setup the SerializedProperties.
            nameProp = serializedObject.FindProperty("name");
            amountProp = serializedObject.FindProperty("amount");
            iconProp = serializedObject.FindProperty("icon");
            skin = Resources.Load("dSkin") as GUISkin;
        }

        public override void OnInspectorGUI()
        {

            // Update the serializedProperty - always do this in the beginning of OnInspectorGUI.
            serializedObject.Update();
            if (skin) GUI.skin = skin;
            GUILayout.BeginVertical("Currency Manager", "window");
            GUILayout.Space(30);

            EditorGUILayout.PropertyField(nameProp);
            EditorGUILayout.PropertyField(amountProp);
            EditorGUILayout.PropertyField(iconProp);
            GUILayout.EndVertical();
            // Apply changes to the serializedProperty - always do this in the end of OnInspectorGUI.
            serializedObject.ApplyModifiedProperties();
        }
    }
}
