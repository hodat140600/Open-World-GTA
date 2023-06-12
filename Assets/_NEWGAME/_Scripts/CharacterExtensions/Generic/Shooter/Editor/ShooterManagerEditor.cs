using UnityEditor;
using UnityEngine;
namespace _GAME._Scripts._CharacterController._Shooter
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(ShooterManager), true)]
    public class ShooterManagerEditor : EditorBase
    {
        ShooterManager manager;
        protected override void OnEnable()
        {
            base.OnEnable();
        }

        protected override void AdditionalGUI()
        {
            if (!manager)
                manager = (ShooterManager)this.target;

            var color = GUI.color;
            if (toolbars[selectedToolBar].title.Equals("IK Adjust"))
            {
                if (!Application.isPlaying && GUILayout.Button("Create New IK Adjust List"))
                {
                    CreateNewIKAdjustList(manager);
                }

                if (manager.weaponIKAdjustList != null && GUILayout.Button("Edit IK Adjust List"))
                {
                    ShooterIKAdjustWindow.InitEditorWindow();
                }
            }

            if (Application.isPlaying)
            {
                if (manager.tpCamera)
                {
                    GUI.color = Color.red;
                    GUI.color = color;
                    GUI.enabled = ShooterIKAdjustWindow.curWindow == null;
                    GUI.enabled = true;

                    EditorGUILayout.Space();
                    if (GUILayout.Button(manager.showCheckAimGizmos ? "Hide Aim Gizmos" : "Show Aim Gizmos", EditorStyles.toolbarButton))
                    {
                        manager.showCheckAimGizmos = !manager.showCheckAimGizmos;
                    }
                }
            }

            GUI.color = color;
        }
        public void CreateNewIKAdjustList(ShooterManager targetShooterManager)
        {
            WeaponIKAdjustList ikAdjust = ScriptableObject.CreateInstance<WeaponIKAdjustList>();
            AssetDatabase.CreateAsset(ikAdjust, "Assets/" + manager.gameObject.name + "@IKAdjustList.asset");
            targetShooterManager.weaponIKAdjustList = ikAdjust;
            AssetDatabase.SaveAssets();

        }
    }
}