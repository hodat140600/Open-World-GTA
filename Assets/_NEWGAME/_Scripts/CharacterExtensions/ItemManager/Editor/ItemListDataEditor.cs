using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace _GAME._Scripts._ItemManager
{
    [CustomEditor(typeof(ItemListData))]
    public class ItemListEditor : Editor
    {
        [SerializeField]
        protected GUISkin skin;
        [SerializeField]
        protected ItemListData itemList;
        protected Texture2D m_Logo = null;

        void OnEnable()
        {
            itemList = (ItemListData)target;
            skin = Resources.Load("dSkin") as GUISkin;
            m_Logo = (Texture2D)Resources.Load("ThirdPersonExtensions/icon_d2", typeof(Texture2D));
        }

        [MenuItem("DatHQ/Inventory/Create New ItemListData")]

        static void CreateNewListData()
        {
            ItemListData listData = CreateInstance<ItemListData>();
            AssetDatabase.CreateAsset(listData, "Assets/ItemListData.asset");
        }

        public override void OnInspectorGUI()
        {
            if (skin) GUI.skin = skin;

            serializedObject.Update();

            GUI.enabled = !Application.isPlaying;

            GUILayout.BeginVertical("Item List", "window");
            GUILayout.Label(m_Logo, GUILayout.MaxHeight(25));

            OpenCloseItemWindow();

            GUILayout.Space(20);
            if (GUILayout.Button(itemList.itemsHidden ? "Show items in Hierarchy" : "Hide items in Hierarchy"))
            {
                ShowAllItems();
            }
            GUILayout.EndVertical();
            if (GUI.changed || serializedObject.ApplyModifiedProperties())
            {
                EditorUtility.SetDirty(target);
            }
        }

        protected virtual void OpenCloseItemWindow()
        {
            if (itemList.itemsHidden && !itemList.inEdition && GUILayout.Button("Edit Items in List"))
            {
                ItemListWindow.CreateWindow(itemList);
            }
            else if (itemList.inEdition)
            {
                if (ItemListWindow.Instance != null)
                {
                    if (ItemListWindow.Instance.itemList == null)
                    {
                        ItemListWindow.Instance.Close();
                    }
                    else
                        EditorGUILayout.HelpBox("The Item List Window is open", MessageType.Info);
                }
                else
                {
                    itemList.inEdition = false;
                }
            }
        }

        public virtual void ShowAllItems()
        {
            if (itemList.itemsHidden)
            {
                foreach (Item item in itemList.items)
                {
                    item.hideFlags = HideFlags.None;
                    EditorUtility.SetDirty(item);
                }
                itemList.itemsHidden = false;
            }
            else
            {
                foreach (Item item in itemList.items)
                {
                    item.hideFlags = HideFlags.HideInHierarchy;
                    EditorUtility.SetDirty(item);
                }
                itemList.itemsHidden = true;
            }
            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(target);
            AssetDatabase.SaveAssets();

        }
    }
}
