using UnityEngine;
using UnityEditor;

namespace _GAME._Scripts
{
    class HelperEditor : EditorWindow
    {
        //GUISkin skin;
        //private Texture2D m_Logo = null;
        //Vector2 rect = new Vector2(380, 500);

        //void OnEnable()
        //{
        //    m_Logo = (Texture2D)Resources.Load("logo", typeof(Texture2D));           
        //}

        [MenuItem("DatHQ/Import ProjectSettings")]
        public static void ImportProjectSettings()
        {
            AssetDatabase.ImportPackage("Assets/_NEWGAME/Resources/ThirdPersonExtensions/vProjectSettings.unitypackage", true);
        }

        //[MenuItem("DatHQ/Help/Check for Updates")]
        //public static void About()
        //{
        //    GetWindow(typeof(vHelperEditor));
        //}

        //[MenuItem("DatHQ/Help/Forum")]
        //public static void Forum()
        //{
        //    Application.OpenURL("");
        //}

        //[MenuItem("DatHQ/Help/FAQ")]
        //public static void FAQMenu()
        //{
        //    Application.OpenURL("");
        //}

        //[MenuItem("DatHQ/Help/API")]
        //public static void APIMenu()
        //{
        //    Application.OpenURL("");
        //}

        //[MenuItem("DatHQ/Help/Release Notes")]
        //public static void ReleaseNotes()
        //{
        //    Application.OpenURL("");
        //}

        //[MenuItem("DatHQ/Help/Youtube Tutorials")]
        //public static void Youtube()
        //{
        //    Application.OpenURL("");
        //}

        //[MenuItem("DatHQ/Help/Online Documentation")]
        //public static void Documentation()
        //{
        //    Application.OpenURL("");
        //}

        //void OnGUI()
        //{
        //    this.titleContent = new GUIContent("About");
        //    this.minSize = rect;

        //    GUILayout.Label(m_Logo, GUILayout.MaxHeight(240));

        //    if (!skin) skin = Resources.Load("dSkin") as GUISkin;
        //    GUI.skin = skin;

        //    GUILayout.BeginVertical("window");

        //    GUILayout.BeginHorizontal("box");
        //    GUILayout.FlexibleSpace();
        //    GUILayout.Label("Basic Locomotion Version: 2.4.2", EditorStyles.boldLabel);
        //    GUILayout.FlexibleSpace();

        //    if (GUILayout.Button("Check for Update"))
        //    {
        //        UnityEditorInternal.AssetStore.Open("/content/59332");
        //        this.Close();
        //    }
        //    GUILayout.EndHorizontal();

        //    GUILayout.BeginHorizontal("box");
        //    GUILayout.FlexibleSpace();
        //    GUILayout.Label("Melee Combat Version: 2.4.2", EditorStyles.boldLabel);
        //    GUILayout.FlexibleSpace();

        //    if (GUILayout.Button("Check for Update"))
        //    {
        //        UnityEditorInternal.AssetStore.Open("/content/44227");
        //        this.Close();
        //    }
        //    GUILayout.EndHorizontal();

        //    GUILayout.BeginHorizontal("box");
        //    GUILayout.FlexibleSpace();
        //    GUILayout.Label("Shooter Version: 1.3.2", EditorStyles.boldLabel);
        //    GUILayout.FlexibleSpace();
        //    if (GUILayout.Button("Check for Update"))
        //    {
        //        UnityEditorInternal.AssetStore.Open("/content/84583");
        //        this.Close();
        //    }
        //    GUILayout.EndHorizontal();

        //    EditorGUILayout.Space();
        //    EditorGUILayout.HelpBox("UPDATE INSTRUCTIONS: \n\n *ALWAYS BACKUP YOUR PROJECT BEFORE UPDATE!* \n\n Delete the DatHQ's Folder from the Project before import the new version", MessageType.Info);

        //    GUILayout.EndVertical();

        //    EditorGUILayout.Space();
        //    EditorGUILayout.Space();
        //}
    }
}