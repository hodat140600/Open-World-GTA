using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
namespace _GAME._Scripts
{
    public class CreateCurrencyManagerEditor : EditorWindow
    {
        GUISkin skin;
        GameObject selected;
        bool hasManager;
        [MenuItem("Shop/Currency/Components/CurrencyManager", false, 1)]
        public static void CreateNewCharacter()
        {
            GetWindow<CreateCurrencyManagerEditor>();
        }

        void OnEnable()
        {
            selected = Selection.activeGameObject;

        }

        void OnGUI()
        {
            if (!skin) skin = Resources.Load("dSkin") as GUISkin;
            GUI.skin = skin;
            GUILayout.BeginVertical("Currency Creator Window", "window");
            GUILayout.Space(30);
            if (selected != null)
            {
                var manager = selected.GetComponent<CurrencyManager>();
                if (manager)
                {
                    EditorGUILayout.HelpBox("This object allready has a Currency Manager!", MessageType.Info);
                    if (GUILayout.Button("Close"))
                    {
                        Close();
                    }
                }
                else
                {
                    selected.AddComponent<CurrencyManager>();
                    Close();
                }
            }
            else
            {
                selected = EditorGUILayout.ObjectField("Target", selected, typeof(GameObject), true, GUILayout.ExpandWidth(true)) as GameObject;
            }
            GUILayout.EndVertical();
        }


    }
}