/**
 * InstaLODWindowShared.cs (InstaLODUnity)
 *
 * Copyright © 2019 InstaLOD GmbH - All Rights Reserved.
 *
 * Unauthorized copying of this file, via any medium is strictly prohibited.
 * This file and all it's contents are proprietary and confidential.
 *
 * @file InstaLODWindowShared.cs 
 * @copyright 2017-2019 InstaLOD GmbH. All rights reserved.
 * @section License
 */

using System;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace InstaLOD
{
    /// <summary>
    /// The InstaLODEditorWindow provides a base class for all windows created
    /// by the InstaLOD integration.
    /// </summary>
    public abstract class InstaLODEditorWindow : EditorWindow
    {
        public bool isVisible = false;

        public virtual void OnBecameVisible()
        {
            isVisible = true;
        }

        public virtual void OnBecameInvisible()
        {
            isVisible = false;
        }

        /// <summary>
        /// Notifies this instance.
        /// </summary>
        /// <returns>The notify.</returns>
        /// <param name="message">Message.</param>
        public abstract void Notify(string message);
    }

    /// <summary>
    /// The InstaLODMainThreadAction class manages code that should be run 
    /// on the main thread. Every open InstaLODEditorWindow will execute
    /// the scheduled main thread actions in it's Update method.
    /// </summary>
    public static class InstaLODMainThreadAction
    {
        private static List<Action> mainThreadActions = new List<Action>();

        /// <summary>
        /// Runs the main thread actions.
        /// </summary>
        public static void RunMainThreadActions()
        {
            if (System.Threading.Thread.CurrentThread.IsBackground)
                throw new Exception("Cannot run main thread actions on background thread");

            if (InstaLODMainThreadAction.mainThreadActions.Count > 0)
            {
                List<Action> pendingActions = null;

                lock (InstaLODMainThreadAction.mainThreadActions)
                {
                    if (InstaLODMainThreadAction.mainThreadActions.Count > 0)
                    {
                        pendingActions = InstaLODMainThreadAction.mainThreadActions;
                        InstaLODMainThreadAction.mainThreadActions = new List<Action>();
                    }
                }

                if (pendingActions != null)
                {
                    foreach (Action action in pendingActions)
                    {
                        action();
                    }
                }
            }
        }

        /// <summary>
        /// Enqueues the main thread action.
        /// </summary>
        /// <param name="theAction">The action.</param>
        public static void EnqueueMainThreadAction(Action theAction)
        {
            lock (InstaLODMainThreadAction.mainThreadActions)
            {
                InstaLODMainThreadAction.mainThreadActions.Add(theAction);
            }
        }
    }

    /// <summary>
    /// The InstaLODWindowShared class provides functionality that is commonly shared accross
    /// multiple InstaLODEditorWindows.
    /// </summary>
    public static class InstaLODWindowShared
    {
		public static void HelpBox(string message, UnityEditor.MessageType messageType)
		{
			EditorGUILayout.BeginVertical(InstaLODStyle.helpBoxBackgroundStyle);
			EditorGUILayout.HelpBox(message, messageType);
			EditorGUILayout.EndVertical();
		}

		static string currentUsername = String.Empty;
        static string currentPassword = String.Empty;

        public static void OnGUIForNotAuthorized()
        {
			InstaLODWindowShared.HelpBox("InstaLOD requires a valid license.\n" +
                "Please enter your licensing information in the fields below. " +
                "In order to acquire a license an active internet connection is required. " +
                "InstaLOD periodically connects to the InstaLOD servers to validate and refresh the license.", MessageType.Warning);

            GUILayout.Space(10.0f);

            currentUsername = EditorGUILayout.TextField("Account", currentUsername, EditorStyles.textField).Trim();
            currentPassword = EditorGUILayout.PasswordField("Serial/Password", currentPassword, EditorStyles.textField).Trim();

            GUILayout.Space(10.0f);

            if (InstaLODStyle.SecondaryButton("Authorize Workstation"))
            {
                if (string.IsNullOrEmpty(currentUsername) || string.IsNullOrEmpty(currentPassword))
                {
                    EditorUtility.DisplayDialog("InstaLOD Authorization Failed",
                        "Please enter your license data.", "Ok");
                    return;
                }

                if (!InstaLODNative.AuthorizeMachine(currentUsername, currentPassword))
                {
                    EditorUtility.DisplayDialog("InstaLOD Authorization Failed",
                        "Failed to authorize workstation.\n\nAuthorization Log:\n" + InstaLODNative.GetAuthorizationInformationString(), "Ok");
                    return;
                }

                EditorUtility.DisplayDialog("InstaLOD Authorization",
                    "This workstation has been successfully authorized.\n\nLicense Information:\n" + InstaLODNative.GetAuthorizationInformationString(), "Ok");

                // clear license data
                currentPassword = "";
            }
        }

        public static void OnGUIForDeauthorize()
        {
			InstaLODWindowShared.HelpBox("Deauthorization takes 24 hours to complete. This node will remain locked until the deauthorization is finished.", MessageType.Warning);

            GUILayout.Space(10.0f);

            currentUsername = EditorGUILayout.TextField("Account", currentUsername, EditorStyles.textField).Trim();
            currentPassword = EditorGUILayout.PasswordField("Serial/Password", currentPassword, EditorStyles.textField).Trim();

            GUILayout.Space(10.0f);

            if (InstaLODStyle.SecondaryButton("Deauthorize Workstation"))
            {
                if (string.IsNullOrEmpty(currentUsername) || string.IsNullOrEmpty(currentPassword))
                {
                    EditorUtility.DisplayDialog("InstaLOD Deauthorization Failed",
                        "Please enter your license data.", "Ok");
                    return;
                }

                if (!InstaLODNative.DeauthorizeMachine(currentUsername, currentPassword))
                {
                    EditorUtility.DisplayDialog("InstaLOD Deauthorization Failed",
                        "Failed to deauthorize workstation.\n\nAuthorization Log:\n" + InstaLODNative.GetAuthorizationInformationString(), "Ok");
                    return;
                }

                EditorUtility.DisplayDialog("InstaLOD Deauthorization",
                    "This workstation has been successfully deauthorized.\n\nPlease note that it will take 24h until the deauthorization has been completed and the node is activatable on another machine.", "Ok");

                // clear license data
                currentPassword = "";
            }
        }

        #region About GUI
        class AboutGUIState
        {
            public bool licenseInfo = true;
            public bool deauthorize = false;
            public bool authorize = true;
            public bool sdkVersion = false;
        }

        public static void OnGUIForAbout(InstaLODEditorWindow owner)
        {
            AboutGUIState state = GetEditorGuiState<AboutGUIState>(owner, "AboutGUIState");

            EditorGUILayout.BeginVertical();

            InstaLODStyle.CollapsiblePanel("License Info", ref state.licenseInfo, delegate ()
                {
                    GUILayout.TextArea(InstaLODNative.GetAuthorizationInformationString());
                });

            InstaLODStyle.CollapsiblePanel("SDK Version", ref state.sdkVersion, delegate ()
                {
                    GUILayout.Label(InstaLODNative.GetSDKVersionString());
                });

            InstaLODStyle.CollapsiblePanel("Deauthorize", ref state.deauthorize, delegate ()
                {
                    InstaLODWindowShared.OnGUIForDeauthorize();
                });

            if (owner is InstaLODToolkitWindow)
            {
                if (InstaLODStyle.SecondaryButton("Reset Settings"))
                {
                    owner.Notify("ResetSettings");
                }
            }

            GUILayout.Space(10);

            if (InstaLODStyle.SecondaryButton("Online Help"))
            {
                Application.OpenURL("http://www.InstaLOD.com/GettingStartedWithUnity");
            }

            GUILayout.Space(10);

            if (InstaLODStyle.SecondaryButton("Submit Feedback"))
            {
                bool isAuthorized = InstaLODNative.IsAuthorized();
                string licenseInfo = InstaLODNative.GetAuthorizationInformationString();

                string feedbackEmail = "mailto:hello@InstaLOD.com" +
                                       "?Subject=InstaLOD SDK2 Feedback" +
                                       "&Body=Please enter your feedback below this line:\n" +
                                       "----------------------------------------------------------\n" +
                                       "\n\n\n" +
                                       "\n\n\n" +
                                       "\n\n\n" +
                                       "----------------------------------------------------------\n" +
                                       "Thank you for submitting feedback!\n" +
                                       "\n\n\n" +
                                       "App: InstaLOD for Unity\n" +
                                       "Version: " + InstaLODNative.PluginVersion + "\n" +
                                       "SDK Version: " + InstaLODNative.GetBuildDateString() + "\n" +
                                       "License Info:\n" + (isAuthorized ? licenseInfo : "N.A.") + "\n";

                feedbackEmail = feedbackEmail.Replace("\r\n", "%0D%0A");
                feedbackEmail = feedbackEmail.Replace("\n", "%0D%0A");
                feedbackEmail = feedbackEmail.Replace(" ", "%20");
                Application.OpenURL(feedbackEmail);
            }


            EditorGUILayout.EndVertical();
        }
        #endregion

        #region Texture/Material Management
        public struct TexturePageData
        {
            public string texturePageName;
            public Texture texture;
            public bool isNormalMap;
			public bool isSRGB;
		}

        public static List<TexturePageData> GetMaterialTextures(Material material)
        {
            List<TexturePageData> textures = new List<TexturePageData>();
            string assetPath = AssetDatabase.GetAssetPath(material);

            if (String.IsNullOrEmpty(assetPath))
                return textures;

            Shader shader = material.shader;
            for (int i = 0; i < ShaderUtil.GetPropertyCount(shader); i++)
            {
                if (ShaderUtil.GetPropertyType(shader, i) == ShaderUtil.ShaderPropertyType.TexEnv)
                {
                    string textureName = ShaderUtil.GetPropertyName(shader, i);
                    Texture texture = material.GetTexture(textureName);

                    // UnityEngine.Debug.LogFormat("Shader property {0} texture {1}", textureName, texture);

                    if (texture == null)
                        continue;

                    string textureAssetPath = AssetDatabase.GetAssetPath(texture);
                    bool isNormalMap = false;
					bool isSRGB = false;

                    if (!string.IsNullOrEmpty(textureAssetPath))
                    {
                        TextureImporter importer = (TextureImporter)TextureImporter.GetAtPath(textureAssetPath);
                        isNormalMap = importer.textureType == TextureImporterType.NormalMap;
						isSRGB = importer.sRGBTexture;
                    }
					// NOTE: importer may return wrong value for some normal maps (especially if not configured correctly)
					// to avoid this we let the shader property override this 
                    if (textureName == "_BumpMap")
                    {
                        isNormalMap = true;
                    }

					if (textureName == "_MainTex" && InstaLODNative.IsRenderPipelineAssetEnabled())
						continue;

                    textures.Add(new TexturePageData() { texture = texture, texturePageName = textureName, isNormalMap = isNormalMap, isSRGB = isSRGB });
                }
            }

            return textures;
        }

        /// <summary>
        /// This dictionary conatins states for specific UI pages for each window that is rendering the contents.
        /// </summary>
        private static Dictionary<EditorWindow, Dictionary<string, object>> _editorWindowGUIState = new Dictionary<EditorWindow, Dictionary<string, object>>();

        private static T GetEditorGuiState<T>(EditorWindow owner, string key) where T : new()
        {
            if (_editorWindowGUIState.ContainsKey(owner) == false)
            {
                _editorWindowGUIState[owner] = new Dictionary<string, object>();
            }

            if (_editorWindowGUIState[owner].ContainsKey(key) == false)
                _editorWindowGUIState[owner][key] = new T();

            return (T)_editorWindowGUIState[owner][key];
        }

        class GameObjectSelectionMaterialGUIState
        {
            public GameObjectSelectionMaterialGUIState()
            {
                materialScrollView = new Vector2();
                textureListCollapsiblePanelState = true;
            }

            public Vector2 materialScrollView;
            public bool textureListCollapsiblePanelState;
        }

        public static List<Renderer> OnGUIForGameObjectSelectionMaterials(EditorWindow owner, bool spawnWarningsForCameras, bool spawnWarningsForMixedRenderers)
        {
            HashSet<Material> uniqueMaterials = new HashSet<Material>();
            Shader primaryShader = null;
            string invalidMaterialNames = string.Empty;
            bool isSameShaderForAllMaterials = true;
            List<Renderer> renderers = new List<Renderer>();

            bool hasSkeletalMeshRenderer = false;
            bool hasStaticMeshRenderer = false;

            foreach (GameObject gameObject in Selection.gameObjects)
            {
                Renderer renderer = gameObject.GetComponent<Renderer>();

                if (renderer == null)
                {
					// NOTE: we do not display the warning for cameras
					if (gameObject.GetComponent<Camera>() == null || spawnWarningsForCameras)
						InstaLODWindowShared.HelpBox(string.Format("Ignoring game object '{0}' without 'Renderer' component.", gameObject.name), MessageType.Warning);
                    continue;
                }

                if (renderer is SkinnedMeshRenderer)
                    hasSkeletalMeshRenderer = true;
                else
                    hasStaticMeshRenderer = true;

                bool isValid = true;

                foreach (Material material in renderer.sharedMaterials)
                {
                    if (primaryShader != null && primaryShader != material.shader)
                    {
                        invalidMaterialNames += string.Format(" - shader '{0}' used by material '{1}' on game object '{2}'\n", material.shader.name, material.name, gameObject.name);
                        isSameShaderForAllMaterials = false;
                        isValid = false;
                        continue;
                    }

                    if (material == null)
                    {
                        invalidMaterialNames += string.Format(" - null material on game object '{0}'\n", gameObject.name);
                        isSameShaderForAllMaterials = false;
                        isValid = false;
                        continue;
                    }

                    primaryShader = material.shader;
                    uniqueMaterials.Add(material);
                }

                if (isValid)
                    renderers.Add(renderer);
            }

            if (!isSameShaderForAllMaterials)
            {
				InstaLODWindowShared.HelpBox("All materials applied to selected game objects must use the same shader (" + (primaryShader != null ? primaryShader.name : "null") + ").\n\n" +
					"The following shaders, materials and game objects will be excluded from the operation:\n\n" +
					invalidMaterialNames, MessageType.Warning);
            }

            if (renderers.Count == 0)
                return renderers;

            GameObjectSelectionMaterialGUIState state = GetEditorGuiState<GameObjectSelectionMaterialGUIState>(owner, "OnGUIForGameObjectSelectionMaterials");

            int textureCount = 0;

            foreach (Material material in uniqueMaterials)
            {
                List<TexturePageData> textures = GetMaterialTextures(material);
                textureCount += textures.Count;
            }

            state.textureListCollapsiblePanelState = InstaLODStyle.CollapsiblePanelHeader(state.textureListCollapsiblePanelState, string.Format("{0} {1} and {2} {3} in the selection",
                    textureCount, textureCount == 1 ? "Texture" : "Textures",
                    renderers.Count, renderers.Count == 1 ? "Mesh" : "Meshes"));

            if (state.textureListCollapsiblePanelState)
            {
                InstaLODStyle.BeginCollapsiblePanelBody();
                
                state.materialScrollView = GUILayout.BeginScrollView(state.materialScrollView, false, false, GUILayout.MinHeight(100));
                foreach (Material material in uniqueMaterials)
                {
                    List<TexturePageData> textures = GetMaterialTextures(material);

                    bool odd = true;
                    foreach (TexturePageData textureData in textures)
                    {
                        EditorGUILayout.BeginHorizontal(new GUIStyle() { normal = (odd ? InstaLODStyle.listItemStyleOdd : InstaLODStyle.listItemStyleEven).normal });
                        GUILayout.Label(string.Format("{0}\n     {1} on {2}", textureData.texture.name, textureData.texturePageName, material.name));

                        GUILayout.Label(new GUIContent(textureData.texture), new GUIStyle() { fixedHeight = 32, fixedWidth = 32 });

                        EditorGUILayout.EndHorizontal();

                        odd = !odd;
                    }
                }
                GUILayout.EndScrollView();

                InstaLODStyle.EndCollapsiblePanelBody();
            }

            if (spawnWarningsForMixedRenderers && hasStaticMeshRenderer && hasSkeletalMeshRenderer)
            {
				InstaLODWindowShared.HelpBox("Combining static meshes and skinned meshes into a single output mesh results in the combined static mesh geometry being invisible due to missing skinning weights for the static mesh geometry.", MessageType.Error);
            }

            return renderers;
        }

        #endregion

        class MeshOperationSettingsGUIState
        {
            public bool optimizeUIState_Settings = true;
            public bool optimizeUIState_FeatureImportance = true;
            public bool optimizeUIState_NormalRecalculation = false;
            public bool optimizeUIState_VertexWelding = false;
            public bool optimizeUIState_Advanced = true;
            public bool optimizeUIState_SkeletonOptimization = true;

            public bool imposterizeUIState_SettingsFoldout = true;
            public bool imposterizeUIState_DetailSettingsFoldout = true;
			public bool imposterizeUIState_AlphaCutoutSettingsFoldout = false;

            public bool occlusionCullUIState_SettingsFoldout = true;

            public bool materialMergeUIState_FeatureImportanceFoldout = true;
            public bool materialMergeUIState_SettingsFoldout = true;

            public bool bakeOutputUIState_SettingsFoldout = true;

            public bool remeshUIState_SettingsFoldout = true;
            public bool remeshUIState_DetailSettingsFoldout = true;
        }

        public static void OnGUIForFooter(EditorWindow owner)
        {
            GUILayout.Space(30.0f);

            GUIStyle footerStyle = new GUIStyle() { fontSize = 10, alignment = TextAnchor.MiddleCenter };
            footerStyle.normal.textColor = InstaLODStyle.controlGreyColor;

            GUILayout.Label("InstaLOD GmbH 2016-2019", footerStyle);
            if (GUILayout.Button("http://www.InstaLOD.com", footerStyle))
            {
                Application.OpenURL("Http://www.InstaLOD.com");
            }
            GUILayout.Label(InstaLODNative.PluginVersion, footerStyle);
        }

        public static void OnUIForBakeOutput(EditorWindow owner, ref InstaLODBakeOutputSettings bakeOutput)
        {
            MeshOperationSettingsGUIState state = GetEditorGuiState<MeshOperationSettingsGUIState>(owner, "OptimizeSettingsGUIState");
            InstaLODBakeOutputSettings tempBakeOutput = bakeOutput;

            InstaLODStyle.CollapsiblePanel("Bake Output Settings", ref state.bakeOutputUIState_SettingsFoldout, delegate ()
                {
                    tempBakeOutput.Width = (uint)EditorGUILayout.IntField(new GUIContent("Width", "The output texture width."), (int)tempBakeOutput.Width);
                    tempBakeOutput.Height = (uint)EditorGUILayout.IntField(new GUIContent("Height", "The output texture height."), (int)tempBakeOutput.Height);

                    tempBakeOutput.SourceMeshUVChannelIndex = (uint)EditorGUILayout.IntSlider(new GUIContent("Source Mesh UV Channel", "The UV channel of the source mesh used when sampling texture pages."), (int)tempBakeOutput.SourceMeshUVChannelIndex, 0, 3);
                    tempBakeOutput.SuperSampling = (SuperSampling)EditorGUILayout.EnumPopup(new GUIContent("SuperSampling", "Super sampling."), tempBakeOutput.SuperSampling);
                    tempBakeOutput.SolidifyTexturePages = EditorGUILayout.Toggle(new GUIContent("Solidify TexturePages", "Enables solidification of texture pages to avoid pixel bleed."), tempBakeOutput.SolidifyTexturePages);


                    GUILayout.Space(10.0f);

                    EditorGUILayout.BeginHorizontal();
                    tempBakeOutput.TexturePageNormalTangentSpace = EditorGUILayout.Toggle(new GUIContent("NormalTangentSpace", "8bpp dithered RGB, Name: NormalTangentSpace"), tempBakeOutput.TexturePageNormalTangentSpace);
                    tempBakeOutput.TexturePageNormalObjectSpace = EditorGUILayout.Toggle(new GUIContent("NormalObjectSpace", "8bpp dithered RGB, Name: NormalObjectSpace"), tempBakeOutput.TexturePageNormalObjectSpace);
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    tempBakeOutput.TexturePageMeshID = EditorGUILayout.Toggle(new GUIContent("MeshID", "8bpp RGB, Name: MeshID"), tempBakeOutput.TexturePageMeshID);
                    tempBakeOutput.TexturePageVertexColor = EditorGUILayout.Toggle(new GUIContent("VertexColor", "8bpp dithered RGBA, Name: VertexColor"), tempBakeOutput.TexturePageVertexColor);
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    tempBakeOutput.TexturePageAmbientOcclusion = EditorGUILayout.Toggle(new GUIContent("AmbientOcclusion", "8bpp LUMINANCE, Name: AmbientOcclusion"), tempBakeOutput.TexturePageAmbientOcclusion);
                    tempBakeOutput.TexturePageThickness = EditorGUILayout.Toggle(new GUIContent("Thickness", "8bpp dithered LUMINANCE, Name: Thickness"), tempBakeOutput.TexturePageThickness);
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    tempBakeOutput.TexturePageCustom = EditorGUILayout.Toggle(new GUIContent("Custom", "Output textures configured in materials."), tempBakeOutput.TexturePageCustom);
                    tempBakeOutput.TexturePagePosition = EditorGUILayout.Toggle(new GUIContent("Position", "8bpp dithered RGB, Name: Position"), tempBakeOutput.TexturePagePosition);
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    tempBakeOutput.TexturePageDisplacement = EditorGUILayout.Toggle(new GUIContent("Displacement", "8bpp dithered LUMINANCE, Name: Displacement "), tempBakeOutput.TexturePageDisplacement);
                    tempBakeOutput.TexturePageCurvature = EditorGUILayout.Toggle(new GUIContent("Curvature", "8bpp dithered LUMINANCE, Name: Curvature"), tempBakeOutput.TexturePageCurvature);
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    tempBakeOutput.TexturePageTransfer = EditorGUILayout.Toggle(new GUIContent("Transfer", "8bpp dithered RGB, Name: Transfer"), tempBakeOutput.TexturePageTransfer);
                    tempBakeOutput.TexturePageOpacity = EditorGUILayout.Toggle(new GUIContent("Opacity", "8bpp LUMINANCE, Name: Opacity"), tempBakeOutput.TexturePageOpacity);
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    tempBakeOutput.TexturePageBentNormals = EditorGUILayout.Toggle(new GUIContent("BentNormals", "8bpp dithered RGB, Name: BentNormals"), tempBakeOutput.TexturePageBentNormals);
                    EditorGUILayout.EndHorizontal();

                }
            );

            bakeOutput = tempBakeOutput;
        }

        public static void OnGUIForRemeshSettings(EditorWindow owner, ref InstaLODRemeshingSettings inRemeshSettings)
        {
            MeshOperationSettingsGUIState state = GetEditorGuiState<MeshOperationSettingsGUIState>(owner, "OptimizeSettingsGUIState");

            InstaLODRemeshingSettings remeshSettings = inRemeshSettings;

            bool wasGUIEnabled = GUI.enabled;

            InstaLODStyle.CollapsiblePanel("Settings", ref state.remeshUIState_SettingsFoldout, delegate ()
                {
                    remeshSettings.SurfaceMode = (RemeshSurfaceMode)EditorGUILayout.EnumPopup(new GUIContent("Mode", "The surface construction mode."), remeshSettings.SurfaceMode);

                    GUI.enabled = remeshSettings.ScreenSizeInPixels == 0 && remeshSettings.MaximumTriangles == 0;
                    remeshSettings.FaceCountTarget = (RemeshFaceCountTarget)EditorGUILayout.EnumPopup(new GUIContent("Fuzzy Face Count Target", "The fuzzy face count target of the output geometry."), remeshSettings.FaceCountTarget);

                    GUI.enabled = remeshSettings.ScreenSizeInPixels == 0;
                    remeshSettings.MaximumTriangles = (uint)EditorGUILayout.IntField(new GUIContent("Maximum Triangles", "The maximum amount of polygons for the target mesh. If set, this value precedes Face Count Target."), (int)remeshSettings.MaximumTriangles);

                    GUI.enabled = remeshSettings.MaximumTriangles == 0;
                    remeshSettings.ScreenSizeInPixels = (uint)EditorGUILayout.IntSlider(new GUIContent("Screensize in Pixels", "If set, InstaLOD calculates the amount of polygons to remove based on the display size of the output model. If set, this value precedes Face Count Target."), (int)remeshSettings.ScreenSizeInPixels, 0, 1024);

                    if (remeshSettings.ScreenSizeInPixels > 0)
                    {
                        if (remeshSettings.SurfaceMode == RemeshSurfaceMode.Reconstruct)
                        {
                            remeshSettings.ScreenSizePixelMergeDistance = (uint)EditorGUILayout.IntSlider(new GUIContent("Pixel Merge Distance", "When 'Mode' is set to 'Reconstruct' and 'ScreenSizeInPixels' is enabled, the pixel distance in screen size that will be merged together.\n" +
                                                         "This can be used to avoid the construction of unnecessary geometrical detail.\n" +
                                                         "NOTE: When 'ScreenSizeInPixels' is enabled, the resolution is automatically based on 'ScreenSizeInPixels' and 'ScreenSizePixelMergeDistance'."), (int)remeshSettings.ScreenSizePixelMergeDistance, 1, 16);
                        }
                        remeshSettings.ScreenSizeInPixelsAutomaticTextureSize = EditorGUILayout.Toggle(new GUIContent("Automatic Texture Size", "When 'ScreenSizeInPixels' is enabled, automatically compute output texture dimensions based on screen size in pixels."), remeshSettings.ScreenSizeInPixelsAutomaticTextureSize);

                    }

                    GUI.enabled = wasGUIEnabled;
                });


            InstaLODStyle.CollapsiblePanel("Surface Construction", ref state.remeshUIState_DetailSettingsFoldout, delegate ()
                {
                    if (remeshSettings.SurfaceMode == RemeshSurfaceMode.Reconstruct)
                    {
                        GUI.enabled = wasGUIEnabled && remeshSettings.ScreenSizeInPixels == 0;
                        remeshSettings.Resolution = (RemeshResolution)EditorGUILayout.EnumPopup(new GUIContent("Resolution", "The resolution of the remeshing process."), remeshSettings.Resolution);
                        GUI.enabled = wasGUIEnabled;

                        remeshSettings.SurfaceConstructionIgnoreBackface = EditorGUILayout.Toggle(new GUIContent("Ignore Backfaces", "When 'Mode' is set to 'Reconstruct', ignores backfaces during surface construction.\n" +
                                                     "NOTE: This can cause holes in the constructed geometry if face normals are not pointing outwards."), remeshSettings.SurfaceConstructionIgnoreBackface);
                    }

                    remeshSettings.HardAngleThreshold = EditorGUILayout.Slider(new GUIContent("Hard Angle Threshold", "Smooth faces if the normal angle is below this value (in degrees)."), remeshSettings.HardAngleThreshold, 0, 180);
                    remeshSettings.WeldDistance = EditorGUILayout.FloatField(new GUIContent("Weld Distance", "The welding distance can be used to weld holes in the input geometry. The welded mesh is only used for the surface construction it is not used during baking."), remeshSettings.WeldDistance);
                    remeshSettings.GutterSizeInPixels = (uint)EditorGUILayout.IntSlider(new GUIContent("Gutter Size In Pixels", "The minimum distance between two UV shells in pixels."), (int)remeshSettings.GutterSizeInPixels, 1, 32);
                    remeshSettings.UnwrapStrategy = (UVUnwrapStrategy)EditorGUILayout.EnumPopup(new GUIContent("Unwrap Strategy", "Strategy used during unwrap."), remeshSettings.UnwrapStrategy);
                });

            InstaLODWindowShared.OnUIForBakeOutput(owner, ref remeshSettings.BakeOutput);

            inRemeshSettings = remeshSettings;
        }

        public static void OnGUIForMeshMergeSettings(EditorWindow owner, ref InstaLODMeshMergeSettings inMeshMergeSettings)
        {
            MeshOperationSettingsGUIState state = GetEditorGuiState<MeshOperationSettingsGUIState>(owner, "OptimizeSettingsGUIState");

            InstaLODMeshMergeSettings meshMergeSettings = inMeshMergeSettings;

            InstaLODStyle.CollapsiblePanel("Settings", ref state.materialMergeUIState_SettingsFoldout, delegate ()
                {
                    meshMergeSettings.BakeOutput.Width = (uint)EditorGUILayout.IntField(new GUIContent("Width", "The output texture width."), (int)meshMergeSettings.BakeOutput.Width);
                    meshMergeSettings.BakeOutput.Height = (uint)EditorGUILayout.IntField(new GUIContent("Height", "The output texture height."), (int)meshMergeSettings.BakeOutput.Height);
                    meshMergeSettings.GutterSizeInPixels = (uint)EditorGUILayout.IntSlider(new GUIContent("Gutter Size In Pixels", "The minimum distance between two UV shells in pixels."), (int)meshMergeSettings.GutterSizeInPixels, 1, 32);
                    meshMergeSettings.WorldspaceNormalizeShells = EditorGUILayout.Toggle(new GUIContent("Worldpsace Normalize", "Ensure that output uv shells are scaled according to worldspace geometry."), meshMergeSettings.WorldspaceNormalizeShells);

                    meshMergeSettings.SuperSampling = (SuperSampling)EditorGUILayout.EnumPopup(new GUIContent("SuperSampling", "Super sampling."), meshMergeSettings.SuperSampling);
                    meshMergeSettings.ShellRotation = (UVPackShellRotation)EditorGUILayout.EnumPopup(new GUIContent("ShellRotation", "Enable to allow shells to be rotated for improved packing. Disabling rotations can cause certain scenarios to be unpackable."), meshMergeSettings.ShellRotation);
                });

            InstaLODStyle.CollapsiblePanel("Feature Importance", ref state.materialMergeUIState_FeatureImportanceFoldout, delegate ()
                {
                    meshMergeSettings.UVImportance = (MeshFeatureImportance)EditorGUILayout.EnumPopup(new GUIContent("UV Importance", "Determines importance of weights generated by evaluating occupied UV space."), meshMergeSettings.UVImportance);
                    meshMergeSettings.GeometricImportance = (MeshFeatureImportance)EditorGUILayout.EnumPopup(new GUIContent("Geometric Importance", "Determines importance of weights generated by evaluating worldspace geometry."), meshMergeSettings.GeometricImportance);
                    meshMergeSettings.TextureImportance = (MeshFeatureImportance)EditorGUILayout.EnumPopup(new GUIContent("Texture Importance", "Determines importance of weights generated by evaluating input texture dimensions."), meshMergeSettings.TextureImportance);
                    meshMergeSettings.VisualImportance = (MeshFeatureImportance)EditorGUILayout.EnumPopup(new GUIContent("Visual Importance", "Determines importance of weights generated by evaluating visual importance."), meshMergeSettings.VisualImportance);
                });


            inMeshMergeSettings = meshMergeSettings;
        }

        public static List<Camera> OnGUIForOcclusionCullSettings(EditorWindow owner, ref InstaLODOcclusionCullSettings inOcclusionCullSettings)
        {
            MeshOperationSettingsGUIState state = GetEditorGuiState<MeshOperationSettingsGUIState>(owner, "OptimizeSettingsGUIState");

            List<Camera> cameras = new List<Camera>();

            InstaLODOcclusionCullSettings occlusionCullSettings = inOcclusionCullSettings;

            bool isSkeletalMeshSelected = new List<GameObject>(Selection.gameObjects).Find(item => item.GetComponent<SkinnedMeshRenderer>() != null) ? true : false;

            foreach (GameObject gameObject in Selection.gameObjects)
            {
                Camera camera = gameObject.GetComponent<Camera>();

                if (camera == null)
                    continue;

                cameras.Add(camera);
            }

            if (occlusionCullSettings.Mode == OcclusionCullMode.CameraBased && cameras.Count == 0)
            {
				InstaLODWindowShared.HelpBox("Select at least one game object with a 'Camera' component.", MessageType.Error);
            }

            if (occlusionCullSettings.Mode == OcclusionCullMode.CameraBased && Camera.main == null)
            {
				InstaLODWindowShared.HelpBox("Setup a camera in your scene as the main camera by selecting the tag 'MainCamera' in the inspector.", MessageType.Error);
            }

            if (isSkeletalMeshSelected)
            {
				InstaLODWindowShared.HelpBox("Performing occlusion culling on 'SkinnedMeshRenderer' components requires baking the mesh. A new bindpose will be created for 'SkinnedMeshRenderer' components based on the current animation state.", MessageType.Info);
            }

            InstaLODStyle.CollapsiblePanel("Settings", ref state.occlusionCullUIState_SettingsFoldout, delegate ()
                {
                    occlusionCullSettings.Mode = (OcclusionCullMode)EditorGUILayout.EnumPopup(new GUIContent("Mode", "The operation mode."), occlusionCullSettings.Mode);
                    occlusionCullSettings.CullingStrategy = (OcclusionCullingStrategy)EditorGUILayout.EnumPopup(new GUIContent("Culling Strategy", "The culling strategy."), occlusionCullSettings.CullingStrategy);
                    occlusionCullSettings.DataUsage = (OcclusionCullDataUsage)EditorGUILayout.EnumPopup(new GUIContent("Data Usage", "The occlusion data usage."), occlusionCullSettings.DataUsage);

                    occlusionCullSettings.Resolution = (uint)EditorGUILayout.IntField(new GUIContent("Resolution", "The resolution of the rasterization. Higher resolutions improve quality by reducing the amount of missed faces due to subpixel inaccuracies.\n" +
                                                 "Dense geometry benefits from a higher resolution. However, a low resolution can be accommodated by increasing 'AdjacencyDepth' or 'AutomaticPrecision'."), (int)occlusionCullSettings.Resolution);

                    if (occlusionCullSettings.Mode == OcclusionCullMode.AutomaticInterior)
                        occlusionCullSettings.AutomaticPrecision = (MeshFeatureImportance)EditorGUILayout.EnumPopup(new GUIContent("Precision", "When 'Mode' is set to 'AutomaticInterior': the precision of the automatic interior culling."), occlusionCullSettings.AutomaticPrecision);

                    if (occlusionCullSettings.DataUsage == OcclusionCullDataUsage.WriteOptimizerWeightsToVertexColors)
                        occlusionCullSettings.OptimizerWeight = EditorGUILayout.Slider(new GUIContent("Optimizer Weight Value", "When 'DataUsage' is set to 'WriteOptimizerWeights' or 'WriteOptimizerWeightsToWedgeColors':\n" +
                                                 "Optimizer weight value[-1...1] assigned to occluded faces."), occlusionCullSettings.OptimizerWeight, -1.0f, 1.0f);

                    occlusionCullSettings.AdjacencyDepth = (uint)EditorGUILayout.IntSlider(new GUIContent("Adjacency Depth", "When 'CullingStrategy' is set to 'Face': the recursion depth when protecting neighbors of visible faces." +
                                                "This can be used to prevent holes in outside geometry that occured due to a low resolution or rotation count.\n" +
                                                 "0: all faces that are not visible during rasterization will be culled.\n" +
                                                 "1: direct neighbors of faces that are visible will also be marked visible.\n" +
                                                 "2..: neighbors of neighbors will also be marked visible."), (int)occlusionCullSettings.AdjacencyDepth, 0, 32);

                    if (occlusionCullSettings.CullingStrategy != OcclusionCullingStrategy.PerFace)
                        occlusionCullSettings.SubMeshVisbilityFaceThreshold = (uint)EditorGUILayout.IntField(new GUIContent("SubMesh Visbility Face Threshold", "A submesh is marked visible if it's visible face count is greater or equal this value."), (int)occlusionCullSettings.SubMeshVisbilityFaceThreshold);

                    occlusionCullSettings.AlphaMaskThreshold = EditorGUILayout.Slider(new GUIContent("AlphaMask Threshold", "Alpha mask values equal or below this threshold will be considered transparent."), occlusionCullSettings.AlphaMaskThreshold, 0.0f, 1.0f);
                });

            inOcclusionCullSettings = occlusionCullSettings;

            return cameras;
        }

        public static void OnGUIForImposterizeSettings(EditorWindow owner, ref InstaLODImposterizeSettings inImposterizeSettings)
        {
            MeshOperationSettingsGUIState state = GetEditorGuiState<MeshOperationSettingsGUIState>(owner, "OptimizeSettingsGUIState");

            InstaLODImposterizeSettings imposterizeSettings = inImposterizeSettings;

            InstaLODStyle.CollapsiblePanel("Settings", ref state.imposterizeUIState_SettingsFoldout, delegate ()
                {
                    imposterizeSettings.Type = (ImposterType)EditorGUILayout.EnumPopup(new GUIContent("Type", "The type of the imposter."), imposterizeSettings.Type);
                    imposterizeSettings.GutterSizeInPixels = (uint)EditorGUILayout.IntSlider(new GUIContent("Gutter Size In Pixels", "The minimum distance between two UV shells in pixels."), (int)imposterizeSettings.GutterSizeInPixels, 1, 32);
                });
            if (imposterizeSettings.Type == ImposterType.Billboard)
            {
                InstaLODStyle.CollapsiblePanel("Billboard Settings", ref state.imposterizeUIState_DetailSettingsFoldout, delegate ()
                    {
                        imposterizeSettings.QuadXYCount = (uint)EditorGUILayout.IntSlider(new GUIContent("QuadXY Count", "The amount of quads to spawn for the XY axis of the input geometry AABB. Additional quads will be rotated by 180°/Count along the Y axis."), (int)imposterizeSettings.QuadXYCount, 0, 32);
                        imposterizeSettings.QuadXZCount = (uint)EditorGUILayout.IntSlider(new GUIContent("QuadXZ Count", "The amount of quads to spawn for the XZ axis of the input geometry AABB. Additional quads will be rotated by 180°/Count along the Z axis."), (int)imposterizeSettings.QuadXZCount, 0, 32);
                        imposterizeSettings.QuadYZCount = (uint)EditorGUILayout.IntSlider(new GUIContent("QuadYZ Count", "The amount of quads to spawn for the YZ axis of the input geometry AABB. Additional quads will be rotated by 180°/Count along the Z axis."), (int)imposterizeSettings.QuadYZCount, 0, 32);
                        imposterizeSettings.TwoSidedQuads = EditorGUILayout.Toggle(new GUIContent("Two Sided Quads", "Generate Two Sided Quads."), imposterizeSettings.TwoSidedQuads);
						EditorGUILayout.Space();

						if (!imposterizeSettings.AlphaCutOut)
						{
							imposterizeSettings.QuadSubdivisionsU = (uint)EditorGUILayout.IntSlider(new GUIContent("Subdivisions U", "Subdivisions along the U axis for billboard imposters."), (int)imposterizeSettings.QuadSubdivisionsU, 1, 32);
							imposterizeSettings.QuadSubdivisionsV = (uint)EditorGUILayout.IntSlider(new GUIContent("Subdivisions V", "Subdivisions along the V axis for billboard imposters"), (int)imposterizeSettings.QuadSubdivisionsV, 1, 32);
						}
					});
            }
            else if (imposterizeSettings.Type == ImposterType.HybridBillboardCloud)
            {
                InstaLODStyle.CollapsiblePanel("Hybrid Billboard Cloud Settings", ref state.imposterizeUIState_DetailSettingsFoldout, delegate ()
                    {
                        imposterizeSettings.CloudFaceCount = (uint)EditorGUILayout.IntSlider(new GUIContent("Maximum Face Count", "The amount of faces of the resulting mesh."), (int)imposterizeSettings.CloudFaceCount, 4, 10000);
                        imposterizeSettings.CloudPolyFaceFactor = EditorGUILayout.Slider(new GUIContent("CloudPolyFaceFactor", "The amount of faces in percentage of the CloudFaceCount to allocate for the polygon part of hybrid billboard cloud imposters."), imposterizeSettings.CloudPolyFaceFactor, 0f, 0.5f);
                        imposterizeSettings.EnableCloudQuadXY = EditorGUILayout.Toggle(new GUIContent("XY Quads", "Spawn XY quads for cloud imposters."), imposterizeSettings.EnableCloudQuadXY);
                        imposterizeSettings.EnableCloudQuadXZ = EditorGUILayout.Toggle(new GUIContent("XZ Quads", "Spawn XZ quads for cloud imposters."), imposterizeSettings.EnableCloudQuadXZ);
                        imposterizeSettings.EnableCloudQuadYZ = EditorGUILayout.Toggle(new GUIContent("YZ Quads", "Spawn YZ quads for cloud imposters."), imposterizeSettings.EnableCloudQuadYZ);
                        imposterizeSettings.TwoSidedQuads = EditorGUILayout.Toggle(new GUIContent("Two Sided Quads", "Generate Two Sided Quads."), imposterizeSettings.TwoSidedQuads);

                        bool wasGUIEnabled = GUI.enabled;
                        GUI.enabled = false;

                        EditorGUILayout.TextField(new GUIContent("Hybrid Poly Mesh Suffix", "Mesh names matching the specified suffix will be used for polygonal mesh output when creating a hybrid billboard cloud imposter."), "_cloudpoly");

                        GUI.enabled = wasGUIEnabled;
                    });
            }
            else if (imposterizeSettings.Type == ImposterType.AABB)
            {
                InstaLODStyle.CollapsiblePanel("AABB Settings", ref state.imposterizeUIState_DetailSettingsFoldout, delegate ()
                    {
                        imposterizeSettings.AABBDisplacement = EditorGUILayout.Slider(new GUIContent("Displacement", "AABB imposter faces will be displaced along the face normal by the specified value in world space units.\n" + "This is useful to create hay or hedges with overhanging faces."), imposterizeSettings.AABBDisplacement, -10f, 10.0f);
                    });
            }
            else if (imposterizeSettings.Type == ImposterType.Flipbook)
            {
                InstaLODStyle.CollapsiblePanel("Flipbook Settings", ref state.imposterizeUIState_DetailSettingsFoldout, delegate ()
                    {
                        imposterizeSettings.FlipbookFramesPerAxis = (uint)EditorGUILayout.IntSlider(new GUIContent("Flipbook Frames Per Axis", "The amount of flipbook frames to generates for each rotation axis."), (int)imposterizeSettings.FlipbookFramesPerAxis, 2, 32);
                    });
            }

			InstaLODStyle.CollapsiblePanel("Alpha Cutout (PREVIEW)", ref state.imposterizeUIState_AlphaCutoutSettingsFoldout, delegate () 
			{
				imposterizeSettings.AlphaCutOut = EditorGUILayout.Toggle(new GUIContent("Enable", "Removes transparent areas by cutting out the opaque shape."), imposterizeSettings.AlphaCutOut);
				imposterizeSettings.AlphaCutOutSubdivide = EditorGUILayout.Toggle(new GUIContent("Subdivide", "Subdividews the cutout result based on the Resolution."), imposterizeSettings.AlphaCutOutSubdivide);
				imposterizeSettings.AlphaCutOutResolution = (uint)EditorGUILayout.IntSlider(new GUIContent("Resolution", "The resolution for the alpha cutout."), (int)imposterizeSettings.AlphaCutOutResolution, 4, 128);
			});

			InstaLODWindowShared.OnUIForBakeOutput(owner, ref imposterizeSettings.BakeOutput);

            inImposterizeSettings = imposterizeSettings;
        }

        public static void OnGUIForOptimizeSettings(EditorWindow owner, ref InstaLODOptimizeSettings inOptimizeSettings, bool isSkeletalMesh)
        {
            MeshOperationSettingsGUIState state = GetEditorGuiState<MeshOperationSettingsGUIState>(owner, "OptimizeSettingsGUIState");
            bool isGUIEnabled = GUI.enabled;

            InstaLODOptimizeSettings optimizeSettings = inOptimizeSettings;

            InstaLODStyle.CollapsiblePanel("Settings", ref state.optimizeUIState_Settings, delegate ()
                {
                    GUI.enabled = optimizeSettings.AbsoluteTriangles == 0 && optimizeSettings.ScreenSizeInPixels == 0 && isGUIEnabled;
                    optimizeSettings.PercentTriangles = EditorGUILayout.Slider(new GUIContent("Percent Triangles", "The amount of polygons in percent of the output mesh in relation to the input mesh polygon count."), optimizeSettings.PercentTriangles * 100.0f, 0f, 100.0f) / 100.0f;
                    GUI.enabled = optimizeSettings.PercentTriangles == 1 && optimizeSettings.ScreenSizeInPixels == 0 && isGUIEnabled;
                    optimizeSettings.AbsoluteTriangles = (uint)EditorGUILayout.IntField(new GUIContent("Absolute Triangles", "The absolute amount of polygons for the target mesh. If set, this value precedes PercentTriangles."), (int)optimizeSettings.AbsoluteTriangles);
                    GUI.enabled = optimizeSettings.PercentTriangles == 1 && optimizeSettings.AbsoluteTriangles == 0 && isGUIEnabled;
                    optimizeSettings.ScreenSizeInPixels = (uint)EditorGUILayout.IntField(new GUIContent("Screen Size In Pixels", "If set calculates the amount of polygons to remove based on the bounds of the input model."), (int)optimizeSettings.ScreenSizeInPixels);
                    GUI.enabled = isGUIEnabled;
                    optimizeSettings.MaxDeviation = EditorGUILayout.FloatField(new GUIContent("Maximum Deviation", "Controls the maximum allowed mesh error. The optimizer stops before exceeding this value."), optimizeSettings.MaxDeviation);
                });
            InstaLODStyle.CollapsiblePanel("Feature Importance", ref state.optimizeUIState_FeatureImportance, delegate ()
                {
                    optimizeSettings.TextureImportance = (MeshFeatureImportance)EditorGUILayout.EnumPopup(new GUIContent("Texture Importance", "Determines importance of weights generated by evaluating the UV space."), optimizeSettings.TextureImportance);
                    optimizeSettings.ShadingImportance = (MeshFeatureImportance)EditorGUILayout.EnumPopup(new GUIContent("Shading Importance", "Determines importance of weights generated by evaluating the tangent space."), optimizeSettings.ShadingImportance);
                    optimizeSettings.SilhouetteImportance = (MeshFeatureImportance)EditorGUILayout.EnumPopup(new GUIContent("Silhouette Importance", "Determines importance of weights generated by evaluating the geometry."), optimizeSettings.SilhouetteImportance);
                    {
                        bool wasEnabled = GUI.enabled;
                        GUI.enabled = isSkeletalMesh;
                        optimizeSettings.SkinningImportance = (MeshFeatureImportance)EditorGUILayout.EnumPopup(new GUIContent("Skinning Importance", "Recommended to keep at Normal and only tweak if necessary."), optimizeSettings.SkinningImportance);
                        GUI.enabled = wasEnabled;
                    }
                });
            InstaLODStyle.CollapsiblePanel("Normal Recalculation", ref state.optimizeUIState_NormalRecalculation, delegate ()
                {
                    optimizeSettings.RecalculateNormals = EditorGUILayout.Toggle(new GUIContent("Recalculate Normals", "Recalculate normals of the output mesh."), optimizeSettings.RecalculateNormals);
                    optimizeSettings.HardAngleThreshold = EditorGUILayout.Slider(new GUIContent("Hard Angle Threshold", "When recalculating normals: smooth faces if the normal angle is below this value (in degrees)."), optimizeSettings.HardAngleThreshold, 0, 180);
                });
            InstaLODStyle.CollapsiblePanel("Skeleton Optimization", ref state.optimizeUIState_SkeletonOptimization, delegate ()
                {
                    optimizeSettings.SkeletonSettings.LeafBoneWeldDistance = EditorGUILayout.Slider(new GUIContent("Leaf Bone Weld Distance", "Leaf bone weld distance. Leaf bones with a distance to their parent under the specified threshold will be culled."), optimizeSettings.SkeletonSettings.LeafBoneWeldDistance, 0f, 100.0f);
                    optimizeSettings.SkeletonSettings.MaximumBoneDepth = (uint)EditorGUILayout.IntSlider(new GUIContent("Maximum Bone Depth", "Bones that are below the specified level in the hierarchy will be removed."), (int)optimizeSettings.SkeletonSettings.MaximumBoneDepth, 0, 64);
                    optimizeSettings.SkeletonSettings.MaximumBoneInfluencesPerVertex = (uint)EditorGUILayout.IntSlider(new GUIContent("Bone Weigths Per Vertex", "The maximum amount of bone influences per vertex. If a vertex references more bones than allowed, the lowest bone influences will be removed first."), (int)optimizeSettings.SkeletonSettings.MaximumBoneInfluencesPerVertex, 1, 4);
                    optimizeSettings.SkeletonSettings.MinimumBoneInfluenceThreshold = EditorGUILayout.Slider(new GUIContent("Minimum Bone Influence", "The minimum threshold for bone influences. Influences that fall below the threshold will be removed."), optimizeSettings.SkeletonSettings.MinimumBoneInfluenceThreshold, 0, 1);
                });
            InstaLODStyle.CollapsiblePanel("Vertex Welding", ref state.optimizeUIState_VertexWelding, delegate ()
                {
                    optimizeSettings.WeldingThreshold = EditorGUILayout.FloatField(new GUIContent("Welding Threshold", "Controls the welding distance. Set to 0 to disable welding."), optimizeSettings.WeldingThreshold);
                });
            InstaLODStyle.CollapsiblePanel("Advanced", ref state.optimizeUIState_Advanced, delegate ()
                {
                    optimizeSettings.OptimizerVertexWeights = EditorGUILayout.Toggle(new GUIContent("Vertex Colors As Weights", "Generate additional optimizer vertex weights based on vertex color data."), optimizeSettings.OptimizerVertexWeights);
                });

            inOptimizeSettings = optimizeSettings;
        }

        public static void OnGUIForAsyncMeshOperationInProgress(EditorWindow owningWindow)
        {
            Rect progressRect = new Rect(5, 60, owningWindow.position.width - 10, 20);
            float progress = InstaLODNative.currentMeshOperationState.progress * 0.95f;
            Color progressBackgroundColor = InstaLODStyle.darkStyleEnabled ? Color.black : InstaLODStyle.darkGreyColor;

            // draw progressbar back fill
            {
                GUI.color = progressBackgroundColor;
                GUI.DrawTexture(progressRect, Texture2D.whiteTexture);
            }

            // draw progressbar progress fill
            {
                GUI.color = InstaLODStyle.instaLODAccentColor;
                progressRect.width *= progress;
                GUI.DrawTexture(progressRect, Texture2D.whiteTexture);
            }

            // draw progress bar info label
            {
                GUI.color = Color.white;
                GUI.Label(progressRect, string.Format("{0:0.00}%", progress * 100.0f), new GUIStyle()
                {
                    normal = new GUIStyleState() { textColor = new Color(progressBackgroundColor.r, progressBackgroundColor.g, progressBackgroundColor.b, Mathf.Min(Mathf.Max(progress - 0.05f, 0.0f) * 10.0f, 1.0f)) },
                    fontStyle = FontStyle.Bold,
                    alignment = TextAnchor.MiddleCenter
                });
            }

            // draw status label
            {
                GUILayout.Space(70.0f);
                Color progressLabelTextColor = InstaLODStyle.darkStyleEnabled ? Color.white : Color.black;

                if (InstaLODNative.currentMeshOperationState.progress < 1.0f)
                {
                    GUILayout.Label(string.Format("{0} currently in progress...", InstaLODNative.currentMeshOperationState.type), new GUIStyle()
                    {
                        alignment = TextAnchor.MiddleCenter,
                        normal = new GUIStyleState()
                        {
                            textColor = progressLabelTextColor
                        }
                    });
                }
                else
                {
                    GUILayout.Label(string.Format("Importing {0} results...", InstaLODNative.currentMeshOperationState.type), new GUIStyle()
                    {
                        alignment = TextAnchor.MiddleCenter,
                        normal = new GUIStyleState()
                        {
                            textColor = progressLabelTextColor
                        }
                    });
                }
            }
        }
    }
}

