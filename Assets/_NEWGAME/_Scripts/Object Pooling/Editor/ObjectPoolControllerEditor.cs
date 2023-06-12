using _GAME._Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ObjectPoolController), true)]
public class ObjectPoolControllerEditor : EditorBase
{
    public ObjectPoolController controller;
    public GUIStyle fontLabelStyle = new GUIStyle();

    protected override void OnEnable()
    {
        base.OnEnable();
        controller = (ObjectPoolController)target;
    }

    protected virtual void DrawLabel(Vector3 position, string label, int size = 25)
    {
        float zoom = Vector3.Distance(position, SceneView.currentDrawingSceneView.camera.transform.position);
        int fontSize = size;
        fontLabelStyle.fontSize = Mathf.FloorToInt(fontSize / zoom);
        fontLabelStyle.normal.textColor = Color.black;
        fontLabelStyle.alignment = TextAnchor.MiddleLeft;
        Handles.Label(position, label, fontLabelStyle);
    }
}
