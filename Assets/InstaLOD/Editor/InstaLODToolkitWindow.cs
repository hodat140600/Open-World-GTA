/**
 * InstaLODToolkitWindow.cs (InstaLODUnity)
 *
 * Copyright © 2019 InstaLOD GmbH - All Rights Reserved.
 *
 * Unauthorized copying of this file, via any medium is strictly prohibited.
 * This file and all it's contents are proprietary and confidential.
 *
 * @file InstaLODToolkitWindow.cs 
 * @copyright 2017-2019 InstaLOD GmbH. All rights reserved.
 * @section License
 */

using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEditor;

namespace InstaLOD
{
	/// <summary>
	/// The InstaLODToolkitWindow class manages an editor window that provides
	/// direct access to mesh processing functionality outside of any LOD mechanism.
	/// </summary>
	[System.Serializable]
	public class InstaLODToolkitWindow : InstaLODEditorWindow
	{
		public enum MeshOperationType
		{
			Remesh = 0,
			Imposterize = 1,
			MaterialMerge = 2,
			OcclusionCull = 3,
			Optimize = 4,
			License = 5
		}

		public MeshOperationType currentMeshOperationType;
		public InstaLODMeshMergeSettings meshMergeSettings;
		public InstaLODImposterizeSettings imposterizeSettings;
		public InstaLODRemeshingSettings remeshSettings;
		public InstaLODOcclusionCullSettings occlusionCullSettings;
		public InstaLODNativeMeshOperationSettings meshOperationSettings;
		public InstaLODOptimizeSettings optimizeSettings;

		[MenuItem("Window/InstaLOD Toolkit")]
		static void Init()
		{
			GetWindow<InstaLODToolkitWindow>();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:InstaLOD.InstaLODToolkitWindow"/> class.
		/// </summary>
		public InstaLODToolkitWindow()
		{
			currentMeshOperationType = MeshOperationType.Remesh;
			meshMergeSettings = new InstaLODMeshMergeSettings(1024, 1024);
			imposterizeSettings = new InstaLODImposterizeSettings(1024, 1024);
			remeshSettings = new InstaLODRemeshingSettings(1024, 1024);
			occlusionCullSettings = new InstaLODOcclusionCullSettings(1024);
			meshOperationSettings = new InstaLODNativeMeshOperationSettings(true);
			optimizeSettings = new InstaLODOptimizeSettings(1.0f);
		}

		/// <summary>
		/// Update this instance.
		/// </summary>
		void Update()
		{
			if (InstaLODNative.currentMeshOperationState == null)
				return;

			InstaLODMainThreadAction.RunMainThreadActions();

			Repaint();
		}

		/// <summary>
		/// Notify this instance.
		/// </summary>
		/// <returns>The notify.</returns>
		/// <param name="message">Message.</param>
		public override void Notify(string message)
		{
			if (message == "ResetSettings")
			{
				meshMergeSettings = new InstaLODMeshMergeSettings(1024, 1024);
				imposterizeSettings = new InstaLODImposterizeSettings(1024, 1024);
				remeshSettings = new InstaLODRemeshingSettings(1024, 1024);
				occlusionCullSettings = new InstaLODOcclusionCullSettings(1024);
				meshOperationSettings = new InstaLODNativeMeshOperationSettings(true);
				optimizeSettings = new InstaLODOptimizeSettings(1.0f);
			}
		}

		void OnEnable()
		{
			titleContent = new GUIContent("Toolkit", InstaLODStyle.headerIcon);
			minSize = new Vector2(400, 620);

			if (!InstaLODNative.IsInitialized())
			{
				if (InstaLODNative.Initialize())
				{
					InstaLODNative.SetRenderPipelineAssetActive(InstaLODNative.IsRenderPipelineAssetEnabled());
					Debug.Log("InstaLOD initialized (" + InstaLODNative.GetSDKVersionString() + ")");
				}
				else
				{
					throw new Exception("Failed to initialized InstaLOD SDK");
				}
			}
		}

		void OnSelectionChange()
		{
			Repaint();
		}
		
		void OnDestroy()
		{
			// NOTE: always respawn the window if a async operation is in progress
			// this is necessary as the main thread events are handled in our windows
			if (InstaLODNative.currentMeshOperationState != null)
			{
				InstaLODToolkitWindow newWin = Instantiate<InstaLODToolkitWindow>(this);
				newWin.Show();
			}
		}

		void OnGUI()
		{
			InstaLODStyle.EnsureStyleTextures();

			// NOTE: we need to set the content as the header icon might be null after a scene play event
			if (titleContent == null || titleContent.image == null)
				titleContent = new GUIContent("InstaLOD", InstaLODStyle.headerIcon);

			// draw window background fill
			{
				GUI.color = InstaLODStyle.windowColor;
				GUI.DrawTexture(new Rect(0, 0, position.width, position.height), Texture2D.whiteTexture);
				GUI.color = Color.white;
			}

			// draw logo
			{
				GUILayout.Space(10.0f);
				GUILayout.Label(InstaLODStyle.logoForCurrentStyleType, GUIStyle.none);
				GUILayout.Space(5.0f);
			}

			using (new InstaLODStyle.FooterContent(delegate() { } ))
			{
				using (new InstaLODStyle.VerticalLayout(5))
				{
					if (!InstaLODNative.IsAuthorized())
					{
						bool value = true;
						InstaLODStyle.CollapsiblePanel("Authorize", ref value, delegate()
							{
								InstaLODWindowShared.OnGUIForNotAuthorized();
							});
						InstaLODWindowShared.OnGUIForFooter(this);
						return;
					}

					if (InstaLODNative.currentMeshOperationState != null)
					{
						InstaLODWindowShared.OnGUIForAsyncMeshOperationInProgress(this);
						InstaLODWindowShared.OnGUIForFooter(this);
						return;
					}


					GUI.color = Color.white;

					_mainScrollViewPosition = EditorGUILayout.BeginScrollView(_mainScrollViewPosition);

					using (new InstaLODStyle.TabLayout(delegate()
						{
							// NOTE: we have temporarily disabled this until combine mesh is implemented for optimize
							/*
							if (GUILayout.Toggle(currentMeshOperationType == MeshOperationType.Optimize, "Optimize", InstaLODStyle.toolbarButtonStyle))
								currentMeshOperationType = MeshOperationType.Optimize;
							*/
							
							if (GUILayout.Toggle(currentMeshOperationType == MeshOperationType.Remesh, "Remesh", InstaLODStyle.toolbarButtonStyle))
								currentMeshOperationType = MeshOperationType.Remesh;

							if (GUILayout.Toggle(currentMeshOperationType == MeshOperationType.Imposterize, "Imposterize", InstaLODStyle.toolbarButtonStyle))
								currentMeshOperationType = MeshOperationType.Imposterize;
				
							if (GUILayout.Toggle(currentMeshOperationType == MeshOperationType.MaterialMerge, "Material Merge", InstaLODStyle.toolbarButtonStyle))
								currentMeshOperationType = MeshOperationType.MaterialMerge;

							if (GUILayout.Toggle(currentMeshOperationType == MeshOperationType.OcclusionCull, "Occlusion Cull", InstaLODStyle.toolbarButtonStyle))
								currentMeshOperationType = MeshOperationType.OcclusionCull;

							if (GUILayout.Toggle(currentMeshOperationType == MeshOperationType.License, "?", InstaLODStyle.toolbarButtonStyle))
								currentMeshOperationType = MeshOperationType.License;
						}))
					{
						GUILayout.Space(10.0f);

						if (currentMeshOperationType == MeshOperationType.License)
						{
							InstaLODWindowShared.OnGUIForAbout(this);
						}
						else
						{
							bool spawnWarningsForMixedRenderers = currentMeshOperationType == MeshOperationType.Imposterize || currentMeshOperationType == MeshOperationType.Remesh ||
																  meshOperationSettings.combineOutputMeshes;
							List<Renderer> renderers = InstaLODWindowShared.OnGUIForGameObjectSelectionMaterials(this, currentMeshOperationType != MeshOperationType.OcclusionCull, spawnWarningsForMixedRenderers);


							if (Selection.activeGameObject == null || renderers.Count == 0)
							{
								InstaLODWindowShared.HelpBox("Select at least one gameobject with a 'Renderer' component.", MessageType.Info);
							}
							else
							{
								switch (currentMeshOperationType)
								{
									case MeshOperationType.Optimize:
										OnGUIForOptimize(renderers);
										break;
									case MeshOperationType.OcclusionCull:
										OnGUIForOcclusionCull(renderers);
										break;
									case MeshOperationType.MaterialMerge:
										OnGUIForMaterialMerge(renderers);
										break;
									case MeshOperationType.Imposterize:
										OnGUIForImposterize(renderers);
										break;
									case MeshOperationType.Remesh:
										OnGUIForRemesh(renderers);
										break;
									default:
										break;
								}
							}
						}
					}
					InstaLODWindowShared.OnGUIForFooter(this);

					EditorGUILayout.EndScrollView();
				}
			}
		}

		void OnGuiForMeshOperationSettings()
		{
			InstaLODStyle.CollapsiblePanel("Operation Settings", ref _materialOperationSettingsUIState_OperationSettingsFoldOut, delegate()
				{
					bool wasGUIEnabled = GUI.enabled;

					string hideMode = "Clone Source Game Objects";

					// NOTE: operations that only generate a single mesh cannot "clone"
					if (currentMeshOperationType == MeshOperationType.Remesh ||
						currentMeshOperationType == MeshOperationType.Imposterize)
						hideMode = "Hide Source Game Objects";

					meshOperationSettings.hideSourceGameObjects = EditorGUILayout.Toggle(new GUIContent(hideMode, "Hide all input meshes after the mesh operation."), meshOperationSettings.hideSourceGameObjects);

					if (currentMeshOperationType == MeshOperationType.OcclusionCull ||
						currentMeshOperationType == MeshOperationType.Optimize)
					{
						GUI.enabled = false;
						meshOperationSettings.combineOutputMeshes = false;
					}
					else if (currentMeshOperationType != MeshOperationType.MaterialMerge && 
						currentMeshOperationType != MeshOperationType.Optimize)
					{
						GUI.enabled = false;
						meshOperationSettings.combineOutputMeshes = true;
					}
					meshOperationSettings.combineOutputMeshes = EditorGUILayout.Toggle(new GUIContent("Combine Output Meshes", "Combines all input meshes into a single output mesh and parents it into the root node."), meshOperationSettings.combineOutputMeshes);
					GUI.enabled = wasGUIEnabled;

					if (currentMeshOperationType == MeshOperationType.Remesh)
						meshOperationSettings.skinnedMeshEnforceBindPose = EditorGUILayout.Toggle(new GUIContent("Enforce Bindpose", "Transforms all skinned meshes back into the bindpose before remeshing the selected meshes."), meshOperationSettings.skinnedMeshEnforceBindPose);
				});
		}

		void OnGUIForOptimize(List<Renderer> renderers)
		{
			bool isSkeletalMeshSelected = renderers.Find(item => item.GetComponent<Renderer>() is SkinnedMeshRenderer) != null ? true : false;

			InstaLODWindowShared.OnGUIForOptimizeSettings(this, ref optimizeSettings, isSkeletalMeshSelected);

			OnGuiForMeshOperationSettings();

			if (InstaLODStyle.PrimaryButton(string.Format("Optimize {0} game objects", renderers.Count)))
			{
				InstaLODNative.Optimize(renderers, optimizeSettings, meshOperationSettings);
			}
		}

		void OnGUIForRemesh(List<Renderer> renderers)
		{
			bool isSkeletalMeshSelected = renderers.Find(item => item.GetComponent<Renderer>() is SkinnedMeshRenderer) != null ? true : false;

			if (isSkeletalMeshSelected)
			{
				if (!meshOperationSettings.skinnedMeshEnforceBindPose)
					InstaLODWindowShared.HelpBox("To avoid close mesh parts from being fused together, it recommended to remesh 'SkinnedMeshRenderer' components in the bindpose.\nSelect 'Enforce Bindpose' in the 'Operation Settings' below.", MessageType.Warning);
			}

			EditorGUILayout.BeginVertical();

			InstaLODWindowShared.OnGUIForRemeshSettings(this, ref remeshSettings);
		

			OnGuiForMeshOperationSettings();

			if (InstaLODStyle.PrimaryButton(string.Format("Remesh {0} game objects", renderers.Count)))
			{
				InstaLODNative.Remesh(renderers, remeshSettings, meshOperationSettings);
			}

			EditorGUILayout.EndVertical();
		}

		void OnGUIForImposterize(List<Renderer> renderers)
		{
			EditorGUILayout.BeginVertical();

			InstaLODWindowShared.OnGUIForImposterizeSettings(this, ref imposterizeSettings);

			OnGuiForMeshOperationSettings();

			if (InstaLODStyle.PrimaryButton(string.Format("Imposterize {0} game objects", renderers.Count)))
			{
				InstaLODNative.Imposterize(renderers, imposterizeSettings, meshOperationSettings);
			}

			EditorGUILayout.EndVertical();
		}
 
		void OnGUIForOcclusionCull(List<Renderer> renderers)
		{
			List<Camera> cameras = InstaLODWindowShared.OnGUIForOcclusionCullSettings(this, ref occlusionCullSettings);


			OnGuiForMeshOperationSettings();

			bool wasEnabled = GUI.enabled;
			GUI.enabled = occlusionCullSettings.Mode == OcclusionCullMode.AutomaticInterior || cameras.Count > 0;
			if (InstaLODStyle.PrimaryButton(string.Format("Occlusion Cull {0} game objects", renderers.Count)))
			{
				InstaLODNative.OcclusionCull(renderers, cameras, occlusionCullSettings, meshOperationSettings);
			}
			GUI.enabled = wasEnabled;
		}

		void OnGUIForMaterialMerge(List<Renderer> renderers)
		{
			InstaLODWindowShared.OnGUIForMeshMergeSettings(this, ref meshMergeSettings);

			OnGuiForMeshOperationSettings();

			if (InstaLODStyle.PrimaryButton(string.Format("Merge {0} game objects", renderers.Count)))
			{
				InstaLODNative.MeshMerge(renderers, meshMergeSettings, meshOperationSettings);
			}
		}
		
		private Vector2 _mainScrollViewPosition;
		private bool _materialOperationSettingsUIState_OperationSettingsFoldOut = true;
	}
}

