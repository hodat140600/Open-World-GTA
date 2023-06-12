using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using _GAME._Scripts._CharacterController;
using _GAME._Scripts._Camera;

namespace _GAME._Scripts
{
    [InitializeOnLoad]
    public class EditorIcon
    {
        static Texture2D texturePanel;
        static List<int> markedObjects;
        static EditorIcon()
        {
            EditorApplication.hierarchyWindowItemOnGUI += ThirdPersonControllerIcon;
            EditorApplication.hierarchyWindowItemOnGUI += ThirPersonCameraIcon;
        }
        static void ThirPersonCameraIcon(int instanceId, Rect selectionRect)
        {
            GameObject go = EditorUtility.InstanceIDToObject(instanceId) as GameObject;
            if (go == null) return;

            var tpCamera = go.GetComponent<ThirdPersonCamera>();
            if (tpCamera != null) DrawIcon("ThirdPersonExtensions/tp_camera", selectionRect);
        }

        static void ThirdPersonControllerIcon(int instanceId, Rect selectionRect)
        {
            GameObject go = EditorUtility.InstanceIDToObject(instanceId) as GameObject;
            if (go == null) return;

            var controller = go.GetComponent<ThirdPersonController>();
            if (controller != null) DrawIcon("ThirdPersonExtensions/controllerIcon", selectionRect);
        }


        private static void DrawIcon(string texName, Rect rect)
        {
            Rect r = new Rect(rect.x + rect.width - 16f, rect.y, 16f, 16f);
            GUI.DrawTexture(r, GetTex(texName));
        }

        private static Texture2D GetTex(string name)
        {
            return (Texture2D)Resources.Load(name);
        }
    }
}