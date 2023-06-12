using UnityEditor;
using UnityEngine;

namespace _GAME._Scripts._Utils
{
    [CustomEditor(typeof(Comment))]
    public class CommentEditor : Editor
    {
        SerializedProperty inEdit;
        SerializedProperty comment;
        SerializedProperty header;
        GUIContent textContent, headerContent, editButtonContent;
        GUIStyle window, iconStyle, textStyle;
        GUISkin skin;

        private void OnEnable()
        {
            inEdit = serializedObject.FindProperty("inEdit");
            comment = serializedObject.FindProperty("comment");
            header = serializedObject.FindProperty("header");
            skin = Resources.Load("dSkin") as GUISkin;
            textContent = new GUIContent();
            editButtonContent = new GUIContent("", Resources.Load("ThirdPersonExtensions/CommentEditIcon") as Texture2D, "Enable or Disable Edit Mode");
            headerContent = new GUIContent(Resources.Load("ThirdPersonExtensions/icon_d2") as Texture2D);
        }

        private void OnDisable()
        {
            inEdit.boolValue = false; if (serializedObject != null && serializedObject.targetObject != null) serializedObject.ApplyModifiedProperties();
        }

        public override bool UseDefaultMargins()
        {
            return false;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            textStyle = new GUIStyle(EditorStyles.label);
            window = skin.GetStyle("CommentWindow");
            iconStyle = skin.GetStyle("CommentHeader");
            EditorGUILayout.BeginVertical(window);
            {
                GUILayout.Space(-(window.padding.top - 5));

                GUILayout.BeginHorizontal();
                {
                    Color color = GUI.color;
                    headerContent.text = header.stringValue;
                    GUILayout.Box(headerContent, iconStyle, GUILayout.Height(30));
                    GUILayout.Space(-10);
                    if (GUILayout.Button(editButtonContent, GUIStyle.none, GUILayout.Width(30), GUILayout.Height(30)))
                    {
                        GenericMenu menu = new GenericMenu();
                        menu.AddSeparator("");
                        menu.AddItem(new GUIContent(!inEdit.boolValue ? "Edit Comment" : "Exit Edit"), false, () => { inEdit.boolValue = !inEdit.boolValue; serializedObject.ApplyModifiedProperties(); });
                        menu.AddSeparator("");
                        menu.ShowAsContext();
                    }
                }
                GUILayout.EndHorizontal();

                GUILayout.Space(window.padding.top);

                if (inEdit.boolValue)
                {
                    EditorGUILayout.HelpBox("You can use RichText to customize your comment and header", MessageType.Info);
                    GUILayout.Label("Header", EditorStyles.centeredGreyMiniLabel);
                    header.stringValue = GUILayout.TextField(header.stringValue, 50);
                    GUILayout.Label("Comment", EditorStyles.boldLabel);
                    EditorGUILayout.PropertyField(comment, GUIContent.none);
                }
                else
                {
                    if (textContent != null && textStyle != null)
                    {
                        textStyle.richText = true;
                        textStyle.normal.background = null;
                        textStyle.wordWrap = true;
                        textStyle.font = window.font;
                        textStyle.fontStyle = window.fontStyle;
                        textStyle.fontSize = window.fontSize;
                        textStyle.alignment = window.alignment;
                        GUILayout.Box(comment.stringValue, textStyle);
                    }
                }
            }
            EditorGUILayout.EndVertical();
            serializedObject.ApplyModifiedProperties();
        }
    }
}
