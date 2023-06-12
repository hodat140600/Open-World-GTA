using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using _GAME._Scripts;
using _GAME._Scripts._ItemManager;

namespace _GAME._Scripts
{
    [CustomEditor(typeof(ItemListDataWrapper))]
    public class ItemListDataWrapperEditor : Editor
    {
        SerializedProperty itemDataListProp;
        protected ItemListDataWrapper wrapper;
        private GUISkin skin;
        private Vector2 scroll;
        private bool inEdit;
        private int itemId;
        private bool confirmClear;
        private bool useFilter;
        private ItemType filter;
        void OnEnable()
        {
            // Setup the SerializedProperties.
            itemDataListProp = serializedObject.FindProperty("itemDataList");
            skin = Resources.Load("dSkin") as GUISkin;
            wrapper = (ItemListDataWrapper)target;
        }
        [MenuItem("Shop/Create ItemListDataWrapper")]
        static void CreateNewListData()
        {
            ItemListDataWrapper listData = CreateInstance<ItemListDataWrapper>();
            AssetDatabase.CreateAsset(listData, "Assets/ItemListDataWrapper.asset");
        }
        public override void OnInspectorGUI()
        {

            // Update the serializedProperty - always do this in the beginning of OnInspectorGUI.
            serializedObject.Update();
            if (skin) GUI.skin = skin;
            EditorGUILayout.PropertyField(itemDataListProp);
            if (wrapper.itemDataList)
            {
                if (!confirmClear)
                {
                    if (!inEdit)
                    {
                        GUILayout.BeginVertical("Box");

                        GUILayout.BeginHorizontal("Box");
                        useFilter = GUILayout.Toggle(useFilter, "Use Filter");
                        if (useFilter)
                            filter = (ItemType)EditorGUILayout.EnumPopup(filter);
                        GUILayout.EndHorizontal();

                        GUIStyle boxStyle = new GUIStyle(GUI.skin.box);
                        scroll = GUILayout.BeginScrollView(scroll, GUILayout.MinHeight(600), GUILayout.ExpandHeight(false), GUILayout.ExpandWidth(false));
                        List<ItemType> types = new List<ItemType>() { filter };
                        List<ItemRef> items = useFilter ? GetItemByFilter(wrapper.items, types) : wrapper.items;
                        for (int i = 0; i < items.Count; i++)
                        {
                            EditorGUILayout.BeginHorizontal("Box");
                            EditorGUILayout.BeginVertical("Box", GUILayout.MinWidth(40));
                            var rect = GUILayoutUtility.GetRect(40, 40, GUILayout.ExpandWidth(false));
                            if (items[i].icon != null)
                            {
                                DrawTextureGUI(rect, items[i].icon, new Vector2(40, 40));
                            }
                            EditorGUILayout.EndVertical();
                            var name = items[i].name + " id:" + items[i].id;
                            var openContent = new GUIContent(name, null, "Click to Open");
                            if (GUILayout.Button(openContent, GUILayout.MinHeight(50)))
                            {
                                inEdit = true;
                                itemId = items[i].id;
                            }
                            name = items[i].blocked ? "Unblock" : "Block";
                            var blockContent = new GUIContent(name, null, items[i].blocked ? "Click to Unblock" : "Click to Block");
                            if (GUILayout.Button(blockContent, GUILayout.Height(50), GUILayout.Width(60)))
                            {
                                items[i].blocked = !items[i].blocked;
                                EditorUtility.SetDirty(target);
                            }
                            EditorGUILayout.EndHorizontal();

                        }
                        GUILayout.EndScrollView();
                        GUILayout.EndVertical();
                        name = "PULL DATA FROM ITEM DATA LIST";
                        var content = new GUIContent(name, null, "Click to Pull");
                        if (GUILayout.Button(content))
                        {
                            wrapper.PullItemDataList();
                        }
                        name = "CLEAR VENDOR DATA LIST";
                        content = new GUIContent(name, null, "Click to Clear");
                        if (GUILayout.Button(content))
                        {
                            confirmClear = true;
                        }
                    }
                    else if (inEdit)
                    {
                        EditorGUILayout.BeginVertical("Box");
                        EditorGUILayout.BeginHorizontal();
                        var rect = GUILayoutUtility.GetRect(50, 50, GUILayout.ExpandWidth(false));
                        if (wrapper.items[itemId].icon != null)
                        {
                            DrawTextureGUI(rect, wrapper.items[itemId].icon, new Vector2(50, 50));
                        }

                        var name = " ID " + wrapper.items[itemId].id.ToString("00") + "\n - " + wrapper.items[itemId].name + "\n - " + wrapper.items[itemId].type.ToString();
                        var content = new GUIContent(name);
                        GUILayout.Label(content, EditorStyles.miniLabel);

                        EditorGUILayout.EndHorizontal();
                        wrapper.items[itemId].buyValue = EditorGUILayout.IntField("Buy Value", wrapper.items[itemId].buyValue);
                        wrapper.items[itemId].sellValue = EditorGUILayout.IntField("Sell Value", wrapper.items[itemId].sellValue);
                        name = "Back";
                        content = new GUIContent(name, null, "Click to go Back");
                        if (GUILayout.Button(content))
                        {
                            inEdit = false;
                            itemId = -1;
                            EditorUtility.SetDirty(target);
                        }

                        EditorGUILayout.EndVertical();
                    }



                }
                else if (confirmClear)
                {
                    GUILayout.BeginVertical("Box");
                    EditorGUILayout.HelpBox("***WARNING*** Clearing the list will remove all vendor data!", MessageType.Info);
                    var name = "Clear";
                    var content = new GUIContent(name, null, "Click to Clear");
                    GUILayout.Label(content, EditorStyles.boldLabel);
                    if (GUILayout.Button(content))
                    {
                        confirmClear = false;
                        wrapper.items.Clear();
                    }
                    name = "Back";
                    content = new GUIContent(name, null, "Click to go Back");
                    if (GUILayout.Button(content))
                    {
                        confirmClear = false;
                    }
                    GUILayout.EndVertical();
                }


            }
            else if (!wrapper.itemDataList)
            {
                GUILayout.BeginVertical("Box");
                EditorGUILayout.HelpBox("***WARNING*** Item data list field is null!", MessageType.Info);
                GUILayout.EndVertical();
            }

            // Apply changes to the serializedProperty - always do this in the end of OnInspectorGUI.
            serializedObject.ApplyModifiedProperties();
        }


        List<ItemRef> GetItemByFilter(List<ItemRef> items, List<ItemType> filter)
        {
            return items.FindAll(i => filter.Contains(i.type));
        }

        void DrawTextureGUI(Rect position, Sprite sprite, Vector2 size)
        {
            Rect spriteRect = new Rect(sprite.rect.x / sprite.texture.width, sprite.rect.y / sprite.texture.height,
                                       sprite.rect.width / sprite.texture.width, sprite.rect.height / sprite.texture.height);
            Vector2 actualSize = size;
            actualSize.y *= sprite.rect.height / sprite.rect.width;
            GUI.DrawTextureWithTexCoords(new Rect(position.x, position.y + (size.y - actualSize.y) / 2, actualSize.x, actualSize.y), sprite.texture, spriteRect);

        }


    }
}