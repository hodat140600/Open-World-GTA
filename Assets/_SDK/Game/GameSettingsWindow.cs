using Sirenix.OdinInspector;
using System.Collections;
using UnityEditor;
using UnityEngine;

namespace Assets._SDK.Game
{
#if UNITY_EDITOR
    [EditorWindowTitle(title = "Game Settings")]
    public class GameSettingsWindow : Sirenix.OdinInspector.Editor.OdinEditorWindow
    {
        [BoxGroup("Android Settings")]
        [InfoBox("Cần kiểm tra kĩ các settings phải chính xác với tài liệu của Marketing.", InfoMessageType.Warning)]
        [ReadOnly]
        public string PackageName = "";


        [MenuItem("Game/Settings")]
        private static void OpenWindow()
        {
            GetWindow<GameSettingsWindow>().Show();
        }

        protected override void OnEnable()
        {
            PackageName = PlayerSettings.GetApplicationIdentifier(BuildTargetGroup.Android);
        }


    }
#endif

}