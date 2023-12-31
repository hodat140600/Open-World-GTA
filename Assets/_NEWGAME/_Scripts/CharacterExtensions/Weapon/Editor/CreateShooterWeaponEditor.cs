﻿using UnityEngine;
using UnityEditor;
using System.Collections;
using System;

namespace _GAME._Scripts._CharacterController._Shooter
{
    public class CreateShooterWeaponEditor : EditorWindow
    {
        GUISkin skin;
        GameObject weaponObj;
        Vector2 rect = new Vector2(400, 100);
        Texture2D m_Logo;

        [MenuItem("DatHQ/Shooter/Create Shooter Weapon")]
        public static void CreateNewCharacter()
        {
            GetWindow<CreateShooterWeaponEditor>();
        }

        void OnGUI()
        {
            if (!skin) skin = Resources.Load("dSkin") as GUISkin;
            GUI.skin = skin;

            minSize = rect;
            titleContent = new GUIContent("ShooterWeapon", null, "Window Creator");
            m_Logo = Resources.Load("ThirdPersonExtensions/icon_d2") as Texture2D;
            GUILayout.BeginVertical("Shooter Weapon Creator Window", "window");
            GUILayout.Label(m_Logo, GUILayout.MaxHeight(25));
            GUILayout.Space(5);

            GUILayout.BeginVertical("box");
            weaponObj = EditorGUILayout.ObjectField("FBX Model", weaponObj, typeof(GameObject), true, GUILayout.ExpandWidth(true)) as GameObject;

            if (weaponObj != null)
            {
                if (GUILayout.Button("Create"))
                    Create();
            }

            GUILayout.EndVertical();
            GUILayout.EndVertical();
        }

        private GameObject InstantiateNewWeapon(GameObject selected)
        {
            if (selected == null) return selected;
            if (selected.scene.IsValid()) return selected;

            return PrefabUtility.InstantiatePrefab(selected) as GameObject;

        }

        private void Create()
        {
            var template = Resources.Load("ShooterWeaponTemplate") as GameObject;
            GameObject weapon;

            if (template)
                weapon = (GameObject)PrefabUtility.InstantiatePrefab(template);
            else
                weapon = new GameObject(" ", typeof(ShooterWeapon));

            var newWeapon = InstantiateNewWeapon(weaponObj);
            if (!newWeapon) return;
            newWeapon.transform.SetParent(weapon.transform);
            newWeapon.transform.localPosition = Vector3.zero;
            newWeapon.transform.localEulerAngles = Vector3.zero;
            Selection.activeGameObject = weapon;
            SceneView.lastActiveSceneView.FrameSelected();

            Close();
        }
    }
}

