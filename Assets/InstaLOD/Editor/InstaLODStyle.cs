/**
 * InstaLODStyle.cs (InstaLODUnity)
 *
 * Copyright © 2019 InstaLOD GmbH - All Rights Reserved.
 *
 * Unauthorized copying of this file, via any medium is strictly prohibited.
 * This file and all it's contents are proprietary and confidential.
 *
 * @file InstaLODStyle.cs
 * @copyright 2017-2019 InstaLOD GmbH. All rights reserved.
 * @section License
 */

using UnityEngine;
using UnityEditor;
using System;

namespace InstaLOD
{
	public static class InstaLODStyleUtilities
	{
		public static GUIStyle GetEditorStyle(string name)
		{
			GUISkin skinScene = EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector);

			foreach (GUIStyle style in skinScene.customStyles)
			{
				if (style.name == name)
				{
					return style;
				}
			}

			return null;
		}
	}

	/// <summary>
	/// The InstaLODStyle class manages style for the integration.
	/// </summary>
	public static class InstaLODStyle
	{
		public static GUIStyle collapsiblePanelHeaderStyle;
		public static GUIStyle collapsiblePanelBodyStyle;
		public static GUIStyle darkStyleDefault;
		public static GUIStyle collapsiblePanelFoldOutStyle;
		public static GUIStyle sliderStyle;
		public static GUIStyle primaryButtonStyle, secondaryButtonStyle;
		public static GUIStyle toolbarButtonStyle;

		public static GUIStyle listItemStyle;
		public static GUIStyle listItemStyleIndentend;
		public static GUIStyle listItemStyleWarning;
		public static GUIStyle listItemStyleIndentendWarning;
		public static GUIStyle listItemStyleOdd;
		public static GUIStyle listItemStyleEven;

		public static GUIStyle helpBoxBackgroundStyle;

		public static Color darkGreyColor = new Color(0.17f, 0.17f, 0.17f);
		public static Color lightGreyColor = new Color(0.51f, 0.51f, 0.51f);
		public static Color veryLightGreyColor = new Color(0.8f, 0.8f, 0.8f);
		public static Color instaLODAccentColor = new Color(1.0f, 0.18f, 0.55f);
		public static Color controlGreyColor = new Color(0.36f, 0.36f, 0.36f);
		public static Color windowColor = new Color(0.17f, 0.17f, 0.17f);
		public static Color tabContentColor = new Color(0.27f, 0.27f, 0.27f);

		public static bool darkStyleEnabled = true;

		public static Texture2D logoForCurrentStyleType 
		{
			get {
				return darkStyleEnabled ? logoWhite : logoBlack;
			}
		}

		public static string GetResourcePath(string resource)
		{
			return "Assets/InstaLOD/Editor/Resources/" + resource;
		}

		private static Texture2D _logoBlack;
		public static Texture2D logoBlack 
		{
			get 
			{
				if (_logoBlack == null)
				{
					_logoBlack = new Texture2D(151, 32);
					_logoBlack.LoadImage(System.IO.File.ReadAllBytes(GetResourcePath("InstaLOD_Logo-Black_151x32.png")));
					_logoBlack.Apply();
				}
				return _logoBlack;
			}
		}

		private static Texture2D _logoWhite;
		public static Texture2D logoWhite 
		{
			get 
			{
				if (_logoWhite == null)
				{
					_logoWhite = new Texture2D(151, 32);
					_logoWhite.LoadImage(System.IO.File.ReadAllBytes(GetResourcePath("InstaLOD_Logo-White_151x32.png")));
					_logoWhite.Apply();
				}
				return _logoWhite;
			}
		}

		private static Texture2D _headerIcon;
		public static Texture2D headerIcon 
		{
			get 
			{
				if (_headerIcon == null)
				{
					_headerIcon = new Texture2D(16, 16); 
					_headerIcon.LoadImage(System.IO.File.ReadAllBytes(GetResourcePath("InstaLOD-Button-Black_32x32.png")));
					_headerIcon.Apply();
				}
				return _headerIcon;
			}
		}

		private static Texture2D _tabbarInactiveTexture = null;
		public static Texture2D tabbarInactiveTexture 
		{
			get 
			{
				if (_tabbarInactiveTexture == null)
				{ 
					_tabbarInactiveTexture = new Texture2D(142, 32);
					_tabbarInactiveTexture.alphaIsTransparency = true; 
					_tabbarInactiveTexture.filterMode = FilterMode.Point;
					_tabbarInactiveTexture.LoadImage(System.IO.File.ReadAllBytes(GetResourcePath("TabBar_Inactive" + (darkStyleEnabled ? "" : "-Light") + ".png")));
					_tabbarInactiveTexture.Apply();
				}
				return _tabbarInactiveTexture;
			}
		}

		private static Texture2D _tabbarActiveTexture = null;
		public static Texture2D tabbarActiveTexture
		{
			get 
			{
				if (_tabbarActiveTexture == null)
				{
					_tabbarActiveTexture = new Texture2D(142, 33);
					_tabbarActiveTexture.filterMode = FilterMode.Point;
					_tabbarActiveTexture.LoadImage(System.IO.File.ReadAllBytes(GetResourcePath("TabBar_Active" + (darkStyleEnabled ? "" : "-Light") + ".png")));
					_tabbarActiveTexture.Apply(); 
				}
				return _tabbarActiveTexture;
			}
		}

		static InstaLODStyle()
		{
		}
 
		private static bool _isInitialized = false;

		public static void InitializeStyle()
		{
			if (!darkStyleEnabled)
			{
				tabContentColor = new Color(0.69f, 0.69f, 0.69f);
				windowColor = new Color(0.76f, 0.76f, 0.76f);
			}

			Texture2D lightGreyBackground = new Texture2D(1, 1);
			lightGreyBackground.SetPixel(0, 0, veryLightGreyColor);
			
			Texture2D darkGreyBackground = new Texture2D(1, 1);
			darkGreyBackground.SetPixel(0, 0, darkGreyColor);

			helpBoxBackgroundStyle = new GUIStyle();
			helpBoxBackgroundStyle.padding = new RectOffset(0, 0, 0, 0);
			helpBoxBackgroundStyle.margin = new RectOffset(5, 5, 0, 0);
			helpBoxBackgroundStyle.normal.background = UnityEditorInternal.InternalEditorUtility.HasPro() ? darkGreyBackground : lightGreyBackground;
			helpBoxBackgroundStyle.normal.background.Apply();

			GUIStyle listItem = InstaLODStyleUtilities.GetEditorStyle("OL EntryBackEven");
			listItemStyleEven = new GUIStyle(listItem);
			listItemStyleEven.normal.textColor = InstaLODStyle.darkGreyColor;

			listItemStyleOdd = new GUIStyle(listItem);
			listItemStyleOdd.normal.background = lightGreyBackground;
			listItemStyleOdd.normal.background.Apply();
			listItemStyleOdd.normal.textColor = InstaLODStyle.darkGreyColor;

			if (UnityEditorInternal.InternalEditorUtility.HasPro())
			{
				listItemStyleOdd.normal.background = darkGreyBackground;
				listItemStyleOdd.normal.background.Apply();
				listItemStyleEven.normal.background = darkGreyBackground;
				listItemStyleEven.normal.background.Apply();
			}

			GUIStyle menuItem = new GUIStyle();
			listItemStyle = new GUIStyle(menuItem);
			listItemStyle.padding = new RectOffset(5, 0, 1, 2);
			listItemStyle.normal.textColor = Color.black;
			listItemStyle.onActive.textColor = Color.white;
			listItemStyle.onActive.background = new Texture2D(1, 1);
			listItemStyle.onActive.background.SetPixel(0, 0, new Color(0.243f, 0.49f, 0.906f));
			listItemStyle.onActive.background.Apply();
			listItemStyle.active.background = listItemStyle.onActive.background;
			listItemStyle.active.textColor = listItemStyle.onActive.textColor;
			listItemStyle.onNormal.background = listItemStyle.onActive.background;
			listItemStyle.onNormal.textColor = listItemStyle.onActive.textColor;
			listItemStyle.onFocused.background = listItemStyle.onActive.background;
			listItemStyle.onFocused.textColor = listItemStyle.onActive.textColor;

			listItemStyleIndentend = new GUIStyle(menuItem);
			listItemStyleIndentend.padding = new RectOffset(25, 0, 1, 2);
			listItemStyleIndentend.onNormal = listItemStyle.onNormal;
			listItemStyleIndentend.onFocused = listItemStyle.onFocused;

			Color warningTextColor = new Color(0.8f, 0.4f, 0);
			listItemStyleWarning = new GUIStyle(listItemStyle);
			listItemStyleWarning.normal.textColor = listItemStyleWarning.onNormal.textColor = warningTextColor;

			listItemStyleIndentendWarning = new GUIStyle(listItemStyleIndentend);
			listItemStyleIndentendWarning.fontStyle = FontStyle.Bold;
			listItemStyleIndentendWarning.normal.textColor = listItemStyleIndentendWarning.onNormal.textColor = warningTextColor;

			GUIStyle template = InstaLODStyleUtilities.GetEditorStyle("grey_border");

			collapsiblePanelHeaderStyle = new GUIStyle();
			collapsiblePanelHeaderStyle.margin.left = 5;
			collapsiblePanelHeaderStyle.margin.right = 5;
			collapsiblePanelHeaderStyle.margin.bottom = -1;
			collapsiblePanelHeaderStyle.padding = new RectOffset() { bottom = 2, left = 2, top = 2, right = 2 };
			collapsiblePanelHeaderStyle.border = template.border;
			collapsiblePanelHeaderStyle.focused = new GUIStyleState();
			collapsiblePanelHeaderStyle.active = new GUIStyleState();

			collapsiblePanelBodyStyle = new GUIStyle(); 
			collapsiblePanelBodyStyle.margin.left = 5;
			collapsiblePanelBodyStyle.margin.right = 5;
			collapsiblePanelBodyStyle.margin.bottom = 5;
			collapsiblePanelBodyStyle.margin.top = -1;
			collapsiblePanelBodyStyle.border = template.border;
			collapsiblePanelBodyStyle.normal = template.normal;
			collapsiblePanelBodyStyle.normal.textColor = Color.white;
			collapsiblePanelBodyStyle.normal.textColor = Color.white;
			collapsiblePanelBodyStyle.active.textColor = Color.white;
			collapsiblePanelBodyStyle.hover.textColor = Color.white;
			collapsiblePanelBodyStyle.focused.textColor = Color.white;
			collapsiblePanelBodyStyle.active.textColor = Color.white;

			Color textColor = darkGreyColor;
			collapsiblePanelFoldOutStyle = new GUIStyle(EditorStyles.foldout);
			collapsiblePanelFoldOutStyle.fontStyle = FontStyle.Bold;
			collapsiblePanelFoldOutStyle.normal.textColor = textColor;
			collapsiblePanelFoldOutStyle.active.textColor = textColor;
			collapsiblePanelFoldOutStyle.hover.textColor = textColor;
			collapsiblePanelFoldOutStyle.focused.textColor = textColor;
			collapsiblePanelFoldOutStyle.active.textColor = textColor;
			collapsiblePanelFoldOutStyle.onNormal.textColor = textColor;
			collapsiblePanelFoldOutStyle.onHover.textColor = textColor;
			collapsiblePanelFoldOutStyle.onFocused.textColor = textColor;
			collapsiblePanelFoldOutStyle.onActive.textColor = textColor;

			primaryButtonStyle = new GUIStyle();
			primaryButtonStyle.wordWrap = true;
			primaryButtonStyle.alignment = TextAnchor.MiddleCenter;
			primaryButtonStyle.normal.textColor = textColor;
			primaryButtonStyle.active.textColor = textColor;
			primaryButtonStyle.hover.textColor = textColor;
			primaryButtonStyle.focused.textColor = textColor;
			primaryButtonStyle.active.textColor = textColor;
			primaryButtonStyle.onNormal.textColor = textColor;
			primaryButtonStyle.onHover.textColor = textColor;
			primaryButtonStyle.onFocused.textColor = textColor;
			primaryButtonStyle.onActive.textColor = textColor;
			primaryButtonStyle.margin = new RectOffset() { bottom = 5, left = 5, top = 5, right = 5 };
			primaryButtonStyle.padding = new RectOffset() { bottom = 5, left = 5, top = 5, right = 5 };

			textColor = Color.white;
			secondaryButtonStyle = new GUIStyle("Button");
			secondaryButtonStyle.wordWrap = true;
			secondaryButtonStyle.normal.textColor = textColor;
			secondaryButtonStyle.active.textColor = textColor;
			secondaryButtonStyle.hover.textColor = textColor;
			secondaryButtonStyle.focused.textColor = textColor;
			secondaryButtonStyle.active.textColor = textColor;
			secondaryButtonStyle.onNormal.textColor = textColor;
			secondaryButtonStyle.onHover.textColor = textColor;
			secondaryButtonStyle.onFocused.textColor = textColor;
			secondaryButtonStyle.onActive.textColor = textColor;

			darkStyleDefault = new GUIStyle(InstaLODStyleUtilities.GetEditorStyle("OL ToggleWhite"));
			darkStyleDefault.normal.textColor = Color.white;

			sliderStyle = new GUIStyle(collapsiblePanelFoldOutStyle);

			textColor = Color.white;
			toolbarButtonStyle = new GUIStyle();
			toolbarButtonStyle.fontSize = 10;
			toolbarButtonStyle.fixedHeight = 22;
			toolbarButtonStyle.border = new RectOffset(5, 5, 5, 0);
			toolbarButtonStyle.normal.textColor = textColor;
			toolbarButtonStyle.active.textColor = textColor;
			toolbarButtonStyle.hover.textColor = textColor;
			toolbarButtonStyle.focused.textColor = textColor;
			toolbarButtonStyle.active.textColor = textColor;
			toolbarButtonStyle.onNormal.textColor = textColor;
			toolbarButtonStyle.onHover.textColor = textColor;
			toolbarButtonStyle.onFocused.textColor = textColor;
			toolbarButtonStyle.onActive.textColor = textColor;
			toolbarButtonStyle.alignment = TextAnchor.MiddleCenter;
			
			_isInitialized = true;
		}

		public static void EnsureStyleTextures()
		{
			if (!_isInitialized)
				InitializeStyle();

			InitializeStyle();

			collapsiblePanelHeaderStyle.normal = new GUIStyleState() { background = Texture2D.whiteTexture };
			collapsiblePanelBodyStyle.normal = new GUIStyleState() { background = Texture2D.whiteTexture };
			primaryButtonStyle.normal = new GUIStyleState() { background = Texture2D.whiteTexture };

			toolbarButtonStyle.normal.background = tabbarInactiveTexture;
			toolbarButtonStyle.active.background = tabbarActiveTexture;
			toolbarButtonStyle.onNormal.background = tabbarActiveTexture;
		}

		public static bool CollapsiblePanelHeader(bool state, string label)
		{
			GUI.backgroundColor = lightGreyColor;
			EditorGUILayout.BeginVertical(collapsiblePanelHeaderStyle);
			state = EditorGUILayout.Foldout(state, label, true, collapsiblePanelFoldOutStyle);
			EditorGUILayout.EndVertical();
			GUI.backgroundColor = new Color(0.44f, 0.32f, 0.3f);

			if (!state)
			{
				GUILayout.Space(10.0f);
			}
			
			return state;
		}

		public static void BeginCollapsiblePanelBody()
		{
			float brightness = UnityEditorInternal.InternalEditorUtility.HasPro() ? 0.17f : 0.8f;
			GUI.backgroundColor = new Color(0.44f, 0.32f, 0.3f);
			EditorGUILayout.BeginVertical(collapsiblePanelBodyStyle);
			GUI.backgroundColor = new Color(0.44f, 0.32f, 0.3f);
		}

		public static void EndCollapsiblePanelBody()
		{
			EditorGUILayout.EndVertical();
			GUILayout.Space(10.0f);
		}

		public static bool PrimaryButton(string title)
		{
			GUI.backgroundColor = instaLODAccentColor;
			bool value = GUILayout.Button(title, primaryButtonStyle);

			GUI.backgroundColor = new Color(0.44f, 0.32f, 0.3f);

			return value;
		}

		public static bool PrimaryButton(GUIContent content)
		{
			GUI.backgroundColor = instaLODAccentColor;
			bool value = GUILayout.Button(content, primaryButtonStyle);

			GUI.backgroundColor = new Color(0.44f, 0.32f, 0.3f);

			return value;
		}

		public static Texture2D alertIcon
		{
			get
			{
				return (Texture2D)EditorGUIUtility.LoadRequired("icons/d_console.warnicon.sml.png");
			}
		}

		public static bool SecondaryButton(string title)
		{
			GUI.backgroundColor = lightGreyColor;
			bool value = GUILayout.Button(title, secondaryButtonStyle);

			GUI.backgroundColor = new Color(0.44f, 0.32f, 0.3f);

			return value;
		}

		public static void CollapsiblePanel(string title, ref bool value, Action childControlAction)
		{
			value = InstaLODStyle.CollapsiblePanelHeader(value, title);
			if (value)
			{
				InstaLODStyle.BeginCollapsiblePanelBody();
				childControlAction();
				InstaLODStyle.EndCollapsiblePanelBody();
			}
		}

		public class VerticalLayout : IDisposable
		{
			public VerticalLayout(int padding)
			{
				EditorGUILayout.BeginVertical(new GUIStyle() { padding = new RectOffset(padding, padding, 0, 0) });
			}

			public void Dispose()
			{
				EditorGUILayout.EndVertical();
			}
		}

		public class FooterContent : IDisposable
		{
			Action ContentDelegate;

			public FooterContent(Action func)
			{
				ContentDelegate = func;
			}

			public void Dispose()
			{
				ContentDelegate();
			}
		}

		public class TabLayout : IDisposable
		{
			public TabLayout(Action tabHeader)
			{
				GUI.backgroundColor = new Color(0.44f, 0.32f, 0.3f);
				GUILayout.BeginHorizontal(new GUIStyle() { margin = new RectOffset(5, 5, 5, 0) });
				tabHeader();
				GUILayout.EndHorizontal();
			
				GUI.backgroundColor = tabContentColor;
				GUILayout.BeginVertical(new GUIStyle() { margin = new RectOffset(5, 5, 0, 0), padding = new RectOffset(0, 0, 5, 0), normal = new GUIStyleState() { background = Texture2D.whiteTexture } });

				GUI.backgroundColor = new Color(0.44f, 0.32f, 0.3f);
			}

			public void Dispose()
			{
				GUILayout.EndVertical();
			}
		}
	}
}