﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace _GAME._Scripts._ItemManager
{
    public class ItemSelector : PopupWindowContent
    {
        public UnityEngine.Events.UnityAction<Item> onSelect;
        public List<Item> items;
        public GUIContent[] contents;
        GUIStyle boxStyle;
        public List<ItemType> filter = new List<ItemType>();
        public string search = "";
        bool isOpenFilter;
        Vector2 scroll;
        public override Vector2 GetWindowSize()
        {
            return new Vector2(200, Mathf.Clamp(items.Count * 150, 150, 500));
        }
        public ItemSelector(List<Item> items, ref List<ItemType> filter, UnityEngine.Events.UnityAction<Item> onSelect)
        {
            this.items = items;
            this.filter = filter;
            this.onSelect = onSelect; CreateContent();

        }
        void CreateContent()
        {
            boxStyle = new GUIStyle(GUI.skin.box);
            boxStyle.alignment = TextAnchor.UpperLeft;
            boxStyle.fontStyle = FontStyle.Italic;
            boxStyle.fontSize = 11;
            contents = new GUIContent[items.Count];

            for (int i = 0; i < items.Count; i++)
            {
                var name = " ID " + items[i].id.ToString("00") + "\n - " + items[i].name + "\n - " + items[i].type.ToString();
                var texture = items[i].iconTexture;
                var tooltip = items[i].description;
                contents[i] = new GUIContent(name, texture, tooltip);
            }
        }
        public override void OnGUI(Rect rect)
        {
            if (contents == null) return;
            GUILayout.Label("ItemSelector", EditorStyles.boldLabel);

            DrawFilter();

            scroll = GUILayout.BeginScrollView(scroll, "box");

            for (int i = 0; i < contents.Length; i++)
            {
                if ((filter.Count == 0 || filter.Contains(items[i].type)) && (string.IsNullOrEmpty(search) || items[i].name.ToLower().Contains(search.ToLower())))
                    if (GUILayout.Button(contents[i], boxStyle, GUILayout.Height(150), GUILayout.MinWidth(150)))
                    {
                        onSelect(items[i]);
                        editorWindow.Close();
                    }
            }
            GUILayout.EndScrollView();
        }
        void DrawFilter()
        {
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            isOpenFilter = EditorGUILayout.Foldout(isOpenFilter, "Filters (" + filter.Count + ")");
            if (GUILayout.Button("+", GUILayout.Width(20), GUILayout.Height(15)))
            {
                isOpenFilter = true;
                filter.Add(0);
            }
            GUILayout.EndHorizontal();
            if (isOpenFilter)
            {
                for (int i = 0; i < filter.Count; i++)
                {
                    GUILayout.BeginHorizontal();
                    filter[i] = (ItemType)EditorGUILayout.EnumPopup(filter[i]);
                    if (GUILayout.Button("-", GUILayout.Width(20), GUILayout.Height(15)))
                    {
                        filter.RemoveAt(i);
                        GUILayout.EndHorizontal();
                        break;
                    }
                    GUILayout.EndHorizontal();
                }
            }
            GUILayout.EndVertical();
            GUILayout.BeginHorizontal();
            search = GUILayout.TextField(search, GUILayout.Width(170));
            GUILayout.Label(EditorGUIUtility.IconContent("Search Icon"), GUILayout.Height(20));
            GUILayout.EndHorizontal();
        }

    }
}