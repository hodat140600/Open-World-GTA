using System;
using UnityEditor;
using UnityEngine;

namespace _GAME._Scripts._CharacterController
{
    [Serializable]
    public class EditorStartupPrefs : ScriptableObject
    {
        private static EditorStartupPrefs instance;
        public static EditorStartupPrefs Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = Resources.Load<EditorStartupPrefs>("dEditorStartupPrefs");
                    if (instance == null)
                    {
                        instance = CreateInstance<EditorStartupPrefs>();
                    }
                }
                return instance;
            }
        }

        [SerializeField] private bool displayWelcomeScreen = true;

        public static bool DisplayWelcomeScreen
        {
            get { return Instance.displayWelcomeScreen; }
            set
            {
                if (value != Instance.displayWelcomeScreen)
                {
                    Instance.displayWelcomeScreen = value;
                    SaveStartupPrefs();
                }
            }
        }

        public static void SaveStartupPrefs()
        {
            if (!AssetDatabase.Contains(Instance))
            {
                var copy = CreateInstance<EditorStartupPrefs>();
                EditorUtility.CopySerialized(Instance, copy);
                instance = Resources.Load<EditorStartupPrefs>("dEditorStartupPrefs");
                if (instance == null)
                {
                    AssetDatabase.CreateAsset(copy, "Assets/DatHQ-3rdPersonController/Basic Locomotion/Resources/dEditorStartupPrefs.asset");
                    AssetDatabase.Refresh();
                    instance = copy;

                    return;
                }
                EditorUtility.CopySerialized(copy, instance);
            }
            EditorUtility.SetDirty(Instance);
        }
    }
}