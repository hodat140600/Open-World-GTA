using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;
using _GAME._Scripts._ItemManager;

namespace _GAME._Scripts
{
    [CustomEditor(typeof(Vendor))]
    public class VendorEditor : Editor
    {
        private GUISkin skin;
        protected Vendor vendor;

        private Vector2 scroll;
        ItemType filter;
        bool inEvents;
        bool inSettings;
        bool inItems = true;
        bool inAdd;
        bool useFilter;
        void OnEnable()
        {
            skin = Resources.Load("dSkin") as GUISkin;
            vendor = (Vendor)target;
        }
        public override void OnInspectorGUI()
        {
            // Update the serializedProperty - always do this in the beginning of OnInspectorGUI.
            serializedObject.Update();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("itemListDataWrapper"));
            if (skin) GUI.skin = skin;
            if (vendor.itemListDataWrapper)
            {
                EditorGUILayout.BeginHorizontal("Box");
                if (GUILayout.Button("Input Mapping"))
                {
                    inSettings = true;
                    inItems = false;
                    inEvents = false;
                }
                if (GUILayout.Button("Events"))
                {
                    inSettings = false;
                    inItems = false;
                    inEvents = true;
                }
                if (GUILayout.Button("Items"))
                {
                    inSettings = false;
                    inItems = true;
                    inEvents = false;
                }

                EditorGUILayout.EndHorizontal();

                if (inItems)
                {

                    if (inAdd)
                    {

                        List<ItemType> types = new List<ItemType>() { filter };
                        List<ItemRef> items = useFilter ? GetItemByFilter(vendor.itemListDataWrapper.items, types) : vendor.itemListDataWrapper.items;
                        GUILayout.BeginVertical("Box");

                        GUIStyle boxStyle = new GUIStyle(GUI.skin.box);



                        var name = "Back";
                        var content = new GUIContent(name, null, "Click to go Back");
                        if (GUILayout.Button(content))
                        {
                            inAdd = false;
                        }
                        GUILayout.BeginHorizontal("Box");
                        name = "Add All";
                        content = new GUIContent(name, null, useFilter ? "Add all items with this filter" : "Add all items");
                        if (GUILayout.Button(content, GUILayout.Width(60)))
                        {
                            for (int i = 0; i < items.Count; i++)
                            {
                                var itemCheck = vendor.items.Find(c => c.id == items[i].id);

                                if (!items[i].blocked && itemCheck == null)
                                {
                                    vendor.items.Add(items[i]);
                                }
                            }
                            EditorUtility.SetDirty(target);
                            inAdd = false;
                        }
                        useFilter = GUILayout.Toggle(useFilter, "Use Filter");
                        if (useFilter)
                            filter = (ItemType)EditorGUILayout.EnumPopup(filter);


                        GUILayout.EndHorizontal();

                        scroll = GUILayout.BeginScrollView(scroll, GUILayout.MinHeight(600), GUILayout.ExpandHeight(false), GUILayout.ExpandWidth(false));

                        for (int i = 0; i < items.Count; i++)
                        {
                            if (!items[i].blocked)
                            {
                                EditorGUILayout.BeginHorizontal("Box");
                                EditorGUILayout.BeginVertical("Box", GUILayout.MinWidth(40));
                                var rect = GUILayoutUtility.GetRect(40, 40, GUILayout.ExpandWidth(false));
                                if (items[i].icon != null)
                                {
                                    DrawTextureGUI(rect, items[i].icon, new Vector2(40, 40));
                                }
                                EditorGUILayout.EndVertical();
                                var itemCheck = vendor.items.Find(c => c.id == items[i].id);
                                if (itemCheck != null)
                                {
                                    name = "<color=red> -LISTED  </color>" + items[i].name + " id:" + items[i].id;
                                }
                                else
                                {
                                    name = items[i].name + " id:" + items[i].id;
                                }


                                content = new GUIContent(name, null, "Add item");
                                if (GUILayout.Button(content, GUILayout.MinHeight(50)))
                                {
                                    if (itemCheck != null)
                                    {

                                    }
                                    else
                                    {
                                        inAdd = false;
                                        vendor.items.Add(items[i]);
                                        EditorUtility.SetDirty(target);
                                    }

                                }
                                EditorGUILayout.EndHorizontal();
                            }
                        }

                        GUILayout.EndScrollView();

                        GUILayout.EndVertical();
                    }
                    else
                    {
                        GUILayout.BeginVertical("Box");
                        var name = "Add Item To Vendor";
                        var content = new GUIContent(name, null, "Click to add item");
                        if (GUILayout.Button(content))
                        {
                            inAdd = true;
                        }
                        GUIStyle boxStyle = new GUIStyle(GUI.skin.box);

                        GUILayout.BeginHorizontal("Box");
                        useFilter = GUILayout.Toggle(useFilter, "Use Filter");
                        if (useFilter)
                            filter = (ItemType)EditorGUILayout.EnumPopup(filter);
                        GUILayout.EndHorizontal();

                        scroll = GUILayout.BeginScrollView(scroll, GUILayout.MinHeight(300), GUILayout.ExpandHeight(false), GUILayout.ExpandWidth(false));
                        List<ItemType> types = new List<ItemType>() { filter };
                        List<ItemRef> items = useFilter ? GetItemByFilter(vendor.items, types) : vendor.items;
                        for (int i = 0; i < items.Count; i++)
                        {
                            GUILayout.BeginVertical("Box");
                            EditorGUILayout.BeginHorizontal();
                            var rect = GUILayoutUtility.GetRect(50, 50);
                            if (items[i].icon != null)
                            {
                                DrawTextureGUI(rect, items[i].icon, new Vector2(50, 50));
                            }

                            name = " ID " + items[i].id.ToString("00") + "\n - " + items[i].name + "\n - " + items[i].type.ToString();
                            content = new GUIContent(name);
                            GUILayout.Label(content, EditorStyles.miniLabel);

                            name = "\n- Buy Cost:" + items[i].buyValue + "\n - " + "Sell Cost: " + items[i].sellValue;
                            content = new GUIContent(name);
                            GUILayout.Label(content, EditorStyles.miniLabel);
                            name = "x";
                            content = new GUIContent(name, null, "Remove item");
                            if (GUILayout.Button(content, GUILayout.Height(25), GUILayout.Width(25)))
                            {
                                if (useFilter)
                                {
                                    for (int o = 0; o < vendor.items.Count; o++)
                                    {
                                        if (vendor.items[o].id == items[i].id)
                                        {
                                            vendor.items.RemoveAt(o);
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    vendor.items.RemoveAt(i);
                                }
                                EditorUtility.SetDirty(target);
                            }
                            EditorGUILayout.EndHorizontal();
                            GUILayout.EndVertical();
                        }
                        GUILayout.EndScrollView();
                        GUILayout.EndVertical();
                    }
                }
                else if (inSettings)
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("openVendor"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("horizontal"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("vertical"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("submit"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("cancel"));
                }
                else if (inEvents)
                {
                    GUI.skin = null;
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("onOpenVendor"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("onCloseVendor"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("onBuyItem"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("onSellItem"));
                    GUI.skin = skin;
                }

            }

            else if (!vendor.itemListDataWrapper)
            {
                GUILayout.BeginVertical("Box");
                EditorGUILayout.HelpBox("***WARNING*** Item data list wrapper field is null!", MessageType.Info);
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