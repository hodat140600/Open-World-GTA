using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class ShowLayerNames
{
    static readonly int IgnoreLayer = LayerMask.NameToLayer("Default");

    static ShowLayerNames()
    {
        EditorApplication.hierarchyWindowItemOnGUI += HandleHierarchyWindowItemOnGUI;
    }

    static readonly GUIStyle _style = new GUIStyle()
    {
        fontSize  = 12,
        alignment = TextAnchor.MiddleRight,
        normal = new GUIStyleState()
        {
            textColor = Color.white
        },
    };


    static void HandleHierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
    {
        var gameObject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;

        if (gameObject != null)
        {
            EditorGUI.LabelField(selectionRect, LayerMask.LayerToName(gameObject.layer), _style);
        }
    }
}