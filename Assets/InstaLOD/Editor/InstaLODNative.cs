/**
 * InstaLODNative.cs (InstaLODUnity)
 *
 * Copyright © 2019 InstaLOD GmbH - All Rights Reserved.
 *
 * Unauthorized copying of this file, via any medium is strictly prohibited.
 * This file and all it's contents are proprietary and confidential.
 *
 * @file InstaLODNative.cs
 * @copyright 2017-2019 InstaLOD GmbH. All rights reserved.
 * @section License
 */

using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.IO;
using System.Reflection;
using System.Threading;

namespace InstaLOD
{
	public enum InstaLODNativeMeshOperationType
	{
		Optimize,
		Remesh,
		Imposterize,
		MeshMerge,
		OcclusionCull,
		UVUnwrap,
		UVPack,
		Bake
	}

	/// <summary>
	/// The InstaLODNativeMeshOperationSettings struct represents settings that are
	/// common to most mesh operations.
	/// </summary>
	[Serializable]
	public struct InstaLODNativeMeshOperationSettings
	{
		public InstaLODNativeMeshOperationSettings(bool initDefaults)
		{
			hideSourceGameObjects = true;
			combineOutputMeshes = false;
			skinnedMeshEnforceBindPose = false;
		}

		public bool hideSourceGameObjects;
		public bool combineOutputMeshes;
		public bool skinnedMeshEnforceBindPose;
	}

	internal struct InstaLODNativeConstants
	{
		public static UInt32 MaxMeshTextureCoords = 8;
		public static UInt32 MaxMeshWedgeColors = 4;
		public static UInt32 BoneIndexInvalid = ~0u;
		public static UInt32 JointIndexInvalid = ~0u;
	}

	/// <summary>
	/// The InstaLODNative class provides C# access to the C++ SDK.
	/// </summary>
	public class InstaLODNative
	{
		public static readonly string PluginVersion = "SDK2020";

		#region native methods

		[DllImport("InstaLODUnityNative", EntryPoint = "Initialize")]
		public static extern bool Initialize();

		[DllImport("InstaLODUnityNative", EntryPoint = "Initialize")]
		public static extern bool IsInitialized();

		[DllImport("InstaLODUnityNative", EntryPoint = "Dispose")]
		public static extern bool Dispose();

		[DllImport("InstaLODUnityNative", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		private static extern System.IntPtr GetSDKVersion();

		public static string GetSDKVersionString()
		{
			return Marshal.PtrToStringAnsi(GetSDKVersion());
		}


		[DllImport("InstaLODUnityNative", CallingConvention = CallingConvention.Cdecl)]
		private static extern System.IntPtr GetBuildDate();

		public static string GetBuildDateString()
		{
			return Marshal.PtrToStringAnsi(GetBuildDate());
		}


		[DllImport("InstaLODUnityNative", CallingConvention = CallingConvention.Cdecl)]
		private static extern System.IntPtr GetAuthorizationInformation();

		public static string GetAuthorizationInformationString()
		{
			return Marshal.PtrToStringAnsi(GetAuthorizationInformation());
		}

		[DllImport("InstaLODUnityNative", CallingConvention = CallingConvention.Cdecl)]
		public static extern bool AuthorizeMachine(string username, string password);

		[DllImport("InstaLODUnityNative", CallingConvention = CallingConvention.Cdecl)]
		public static extern bool DeauthorizeMachine(string username, string password);

		[DllImport("InstaLODUnityNative", CallingConvention = CallingConvention.Cdecl)]
		public static extern bool IsAuthorized();

		[DllImport("InstaLODUnityNative", CallingConvention = CallingConvention.Cdecl)]
		public static extern void ClearErrorLog();

		[DllImport("InstaLODUnityNative", EntryPoint = "GetErrorLog", CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr GetErrorLogInt();

		public static string GetErrorLog()
		{
			return Marshal.PtrToStringAnsi(GetErrorLogInt());
		}

		[DllImport("InstaLODUnityNative", CallingConvention = CallingConvention.Cdecl)]
		public static extern void SetRenderPipelineAssetActive(bool isActive);

		[DllImport("InstaLODUnityNative", CallingConvention = CallingConvention.Cdecl)]
		public static extern void InstaLODMesh_Initialize();

		[DllImport("InstaLODUnityNative", CallingConvention = CallingConvention.Cdecl)]
		public static extern void InstaLODMesh_Append(ref InstaLODMesh mesh);

		[DllImport("InstaLODUnityNative", CallingConvention = CallingConvention.Cdecl)]
		public static extern void InstaLODMesh_Get(ref InstaLODMesh mesh);

		[DllImport("InstaLODUnityNative", CallingConvention = CallingConvention.Cdecl)]
		public static extern void InstaLODMesh_Extract(ref InstaLODMesh mesh, UInt32 index, UInt32 count);


		[DllImport("InstaLODUnityNative", CallingConvention = CallingConvention.Cdecl)]
		public static extern void InstaLODMesh_InitializeAuxiliary();

		[DllImport("InstaLODUnityNative", CallingConvention = CallingConvention.Cdecl)]
		public static extern void InstaLODMesh_DeallocAuxiliary();

		[DllImport("InstaLODUnityNative", CallingConvention = CallingConvention.Cdecl)]
		public static extern void InstaLODMesh_AppendAuxiliary(ref InstaLODMesh mesh);


		[DllImport("InstaLODUnityNative", CallingConvention = CallingConvention.Cdecl)]
		public static extern void InstaLODSkeleton_Initialize();

		[DllImport("InstaLODUnityNative", CallingConvention = CallingConvention.Cdecl)]
		public static extern void InstaLODSkeleton_Dealloc();

		[DllImport("InstaLODUnityNative", CallingConvention = CallingConvention.Cdecl)]
		public static extern UInt32 InstaLODSkeleton_AddJoint(UInt32 parentIndex, UInt32 jointIndex, ref Vector3 position, ref Quaternion orientation, ref Vector3 scale, string name);

		[DllImport("InstaLODUnityNative", CallingConvention = CallingConvention.Cdecl)]
		protected static extern void OptimizeMesh(ref InstaLODOptimizeSettings optimizeSettings, ref OptimizeResult outResult);

		[DllImport("InstaLODUnityNative", CallingConvention = CallingConvention.Cdecl)]
		protected static extern void OptimizeMesh2(ref InstaLODMesh inputMesh, ref InstaLODMesh outputMesh, ref InstaLODOptimizeSettings optimizeSettings, ref OptimizeResult outResult);

		[DllImport("InstaLODUnityNative", CallingConvention = CallingConvention.Cdecl)]
		protected static extern void InstaLODMaterialData_Initialize();

		[DllImport("InstaLODUnityNative", CallingConvention = CallingConvention.Cdecl)]
		protected static extern void InstaLODMaterialData_Finalize();


		[DllImport("InstaLODUnityNative", CallingConvention = CallingConvention.Cdecl)]
		protected static extern void InstaLODMaterial_Begin(int materialID);

		[DllImport("InstaLODUnityNative", CallingConvention = CallingConvention.Cdecl)]
		protected static extern void InstaLODMaterial_SetDefaultColor(ref Color color);

		[DllImport("InstaLODUnityNative", CallingConvention = CallingConvention.Cdecl)]
		protected static extern void InstaLODMaterial_AddTexturePage(ref InstaLODTexturePage page);

		[DllImport("InstaLODUnityNative", CallingConvention = CallingConvention.Cdecl)]
		protected static extern UInt32 InstaLODMaterial_GetTexturePageCount();

		[DllImport("InstaLODUnityNative", CallingConvention = CallingConvention.Cdecl)]
		protected static extern void InstaLODMaterial_GetTexturePageAtIndex(uint index, ref InstaLODTexturePage page);

		[DllImport("InstaLODUnityNative", CallingConvention = CallingConvention.Cdecl)]
		protected static extern void InstaLODMaterial_WriteTexturePageAtIndexAsPNG(uint index, string path);

		[DllImport("InstaLODUnityNative", CallingConvention = CallingConvention.Cdecl)]
		protected static extern bool MeshMerge(ref InstaLODMeshMergeSettings settings);

		[DllImport("InstaLODUnityNative", CallingConvention = CallingConvention.Cdecl)]
		protected static extern bool OcclusionCull(ref InstaLODOcclusionCullSettings settings, IntPtr occlusionCullCameras, int cameraCount);

		[DllImport("InstaLODUnityNative", CallingConvention = CallingConvention.Cdecl)]
		protected static extern bool Imposterize(ref InstaLODImposterizeSettings settings);

		[DllImport("InstaLODUnityNative", CallingConvention = CallingConvention.Cdecl)]
		protected static extern bool Remesh(ref InstaLODRemeshingSettings settings);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void MeshOperationProgressDelegate(float progress);

		[DllImport("InstaLODUnityNative", CallingConvention = CallingConvention.Cdecl)]
		protected static extern void SetProgressDelegate(MeshOperationProgressDelegate callback);

		#endregion

		/// <summary>
		/// Ensures that the asset data folder exists.
		/// </summary>
		public static void EnsureAssetDataFolder()
		{
			if (!AssetDatabase.IsValidFolder("Assets/InstaLOD Data"))
				AssetDatabase.CreateFolder("Assets", "InstaLOD Data");
		}

		/// <summary>
		/// Gets the asset save path for object.
		/// </summary>
		/// <returns>The asset path for object.</returns>
		/// <param name="obj">Object.</param>
		/// <param name="initialPath">Initial save path.</param>
		public static string GetAssetSavePathForObject(UnityEngine.Object obj, string initialPath)
		{
			string title = "Save New Mesh";
			string extension = "asset";

			if (obj is Material)
			{
				title = "Save New Material";
				extension = "mat";
			}

			string path = Path.GetDirectoryName(initialPath);
			string fileName = Path.GetFileName(initialPath);
			string savePath = EditorUtility.SaveFilePanelInProject(title, fileName, extension, "Save asset in directory", path);

			if (savePath.Length == 0)
				return initialPath;

			return savePath;
		}

		/// <summary>
		/// Determines whether this is a programmable pipeline.
		/// </summary>
		/// <returns>True if URP or HDRP.</returns>
		public static bool IsRenderPipelineAssetEnabled()
		{
			return UnityEngine.Rendering.GraphicsSettings.renderPipelineAsset != null;
		}

		/// <summary>
		/// Dumps the InstaLOD native error log.
		/// </summary>
		/// <returns><c>true</c>, if native error log was dumped, <c>false</c> otherwise.</returns>
		internal static bool DumpNativeErrorLog()
		{
			string instaErrorLog = GetErrorLog();

			if (string.IsNullOrEmpty(instaErrorLog))
				return false;


			UnityEngine.Debug.LogWarningFormat("InstaLOD {0} operation generated a error/warning log:\n{1}", currentMeshOperationState.type, instaErrorLog);
			return true;
		}

		/// <summary>
		/// The InstaLODMeshOperationState manages state for a mesh operation.
		/// </summary>
		public class InstaLODMeshOperationState
		{
			public InstaLODMeshOperationState(List<Renderer> renderers, InstaLODNativeMeshOperationType meshOperationType)
			{
				renderers = new List<Renderer>();
				startTime = DateTime.Now;
				progress = 0.0f;
				type = meshOperationType;
				owningWindow = null;
				repaintOwningWindow = false;
			}

			public static void NativeProgressCallback(float progress)
			{
				if (currentMeshOperationState == null)
					return;

				currentMeshOperationState.progress = progress;

				if (currentMeshOperationState.owningWindow != null &&
					currentMeshOperationState.repaintOwningWindow == true)
				{
					if (Thread.CurrentThread.IsBackground)
						throw new Exception("Cannot repaint from background thead");
				}
			}

			public List<Renderer> renderers;
			public DateTime startTime;
			public float progress;
			public InstaLODNativeMeshOperationType type;
			public EditorWindow owningWindow;
			public bool repaintOwningWindow;

			public TimeSpan elapsedTimeSpan
			{
				get
				{
					return DateTime.Now - startTime;
				}
			}
		}

		public static InstaLODMeshOperationState currentMeshOperationState;

		/// <summary>
		/// Begins the native mesh operation.
		/// </summary>
		/// <remarks>This method is guaranteed to be invoked on the mainthread</remarks>
		/// <returns><c>true</c>, if native mesh operation was begun, <c>false</c> otherwise.</returns>
		/// <param name="meshOperationState">Mesh operation state.</param>
		public static bool BeginNativeMeshOperation(InstaLODMeshOperationState meshOperationState)
		{
			lock ("CurrentMeshOperationState")
			{
				if (currentMeshOperationState != null)
					return false;

				currentMeshOperationState = meshOperationState;

				SetProgressDelegate(InstaLODMeshOperationState.NativeProgressCallback);
				return true;
			}
		}

		/// <summary>
		/// Finishes the native mesh operation.
		/// </summary>
		/// <remarks>This method is guaranteed to be invoked on the mainthread</remarks>
		/// <returns><c>true</c>, if native mesh operation was finished, <c>false</c> otherwise.</returns>
		public static bool FinishNativeMeshOperation()
		{
			lock ("CurrentMeshOperationState")
			{
				DumpNativeErrorLog();

				currentMeshOperationState = null;
				return true;
			}
		}

		/// <summary>
		/// Helper method to build the native skeleton.
		/// </summary>
		/// <returns><c>true</c>, if skeleton add joint recursive was natived, <c>false</c> otherwise.</returns>
		/// <param name="meshOperation">Mesh operation.</param>
		/// <param name="renderer">Renderer.</param>
		/// <param name="joint">Joint.</param>
		/// <param name="parentJointIndex">Parent joint index.</param>
		internal static bool NativeSkeleton_AddJointRecursive(InstaLODNativeMeshOperationWrapper meshOperation, SkinnedMeshRenderer renderer, Transform joint, UInt32 parentJointIndex)
		{	
			int jointIndex = Array.FindIndex(renderer.bones, transform => transform == joint);

			// FIXME: we need to setup a global bone ID for all meshes in the operation no matter what if we're combining or not
			// once done we can use 'meshOperation.GetGlobalBoneID(joint);'

			if (jointIndex == -1)
				return false;

			Vector3 position = joint.position;
			Quaternion rotation = joint.rotation;
			Vector3 localScale = joint.localScale;

			UInt32 finalJointIndex = InstaLODSkeleton_AddJoint(parentJointIndex, (uint)jointIndex, ref position, ref rotation, ref localScale, joint.name);

			if (finalJointIndex == InstaLODNativeConstants.JointIndexInvalid)
				return false;

			for (int i = 0; i < joint.childCount; i++)
			{
				NativeSkeleton_AddJointRecursive(meshOperation, renderer, joint.GetChild(i), finalJointIndex);
			}

			return true;
		}

		/// <summary>
		/// Helper method to build the native skeleton.
		/// </summary>
		/// <param name="meshOperation">Mesh operation.</param>
		/// <param name="renderers">Renderers.</param>
		internal static void NativeSkeleton_Initialize(InstaLODNativeMeshOperationWrapper meshOperation, List<Renderer> renderers)
		{
			// NOTE: ensure the previous skeleton is deallocated
			InstaLODSkeleton_Dealloc();

			if (renderers == null)
				return;

			if (meshOperation == null)
				throw new NullReferenceException("meshOperation");

			if (renderers.Count > 1)
			{
				// FIXME: we need to fully implement this to allow optimizing multiple meshes via the toolkit
				// what's missing here is remapping weights to a global space and undoing the mapping post optimization
				// when we do not combine the output mesh
				throw new NotImplementedException();
			}

			bool didInitializeSkeleton = false;

			foreach(Renderer baseRenderer in renderers)
			{
				if (!(baseRenderer is SkinnedMeshRenderer))
					continue;

				SkinnedMeshRenderer renderer = baseRenderer as SkinnedMeshRenderer;

				if (renderer.rootBone == null)
					continue;

				if (!didInitializeSkeleton)
					InstaLODSkeleton_Initialize();

				NativeSkeleton_AddJointRecursive(meshOperation, renderer, renderer.rootBone, InstaLODNativeConstants.JointIndexInvalid);
			}
		}

		/// <summary>
		/// The InstaLODNativeMeshOperationWrapper class manages execution of a mesh operation on the specified
		/// Unity renderer instances. It is responsible for conversion of the meshes to InstaLOD meshes, materials as well as post processing.
		/// </summary>
		internal class InstaLODNativeMeshOperationWrapper : IDisposable
		{
			public bool combineOutputMeshes;
			public bool hideSourceGameObjects;
			public bool extractMaterials;
			public bool bakeSkinnedMeshes;

			public string auxiliaryGameObjectSuffix;
			public bool auxiliaryMeshInitialized;

			public Shader primaryShader;
			public Transform rootBone;
			public List<Matrix4x4> worldSpaceBindPoses;
			public List<InstaLODMeshWrapper> meshes;
			public List<GCHandle> textureHandles;
			public Dictionary<InstaLODMeshWrapper, UInt32> wrapperSubmeshOffsets;
			public Dictionary<Material, int> globalMaterialMap;
			public List<Transform> uniqueBones;

			/// <summary>
			/// Initializes a new instance of the
			/// <see cref="T:InstaLOD.InstaLODNative.InstaLODNativeMeshOperationWrapper"/> class.
			/// </summary>
			public InstaLODNativeMeshOperationWrapper()
			{
				meshes = new List<InstaLODMeshWrapper>();
				wrapperSubmeshOffsets = new Dictionary<InstaLODMeshWrapper, uint>();
				textureHandles = new List<GCHandle>();
				globalMaterialMap = new Dictionary<Material, int>();
				primaryShader = null;
				uniqueBones = new List<Transform>();
				worldSpaceBindPoses = new List<Matrix4x4>();
				rootBone = null;
				combineOutputMeshes = false;
				hideSourceGameObjects = true;
				extractMaterials = true;
				bakeSkinnedMeshes = false;
				auxiliaryGameObjectSuffix = string.Empty;
				auxiliaryMeshInitialized = false;

				InstaLODMesh_Initialize();
			}

			#region IDisposable implementation

			public void Dispose()
			{
				foreach (InstaLODMeshWrapper wrapper in meshes)
				{
					wrapper.Dispose();
				}
				foreach (GCHandle handle in textureHandles)
				{
					handle.Free();
				}

				meshes.Clear();
				textureHandles.Clear();
			}

			#endregion

			/// <summary>
			/// Gets the global bone identifier.
			/// </summary>
			/// <returns>The global bone identifier.</returns>
			/// <param name="bone">Bone.</param>
			public int GetGlobalBoneID(Transform bone)
			{
				return uniqueBones.IndexOf(bone);
			}

			/// <summary>
			/// Adds the mesh.
			/// </summary>
			/// <param name="owner">Owner.</param>
			/// <param name="inputMesh">Input mesh.</param>
			/// <param name="materials">Materials.</param>
			/// <param name="transform">Transform.</param>
			/// <param name="skinnedMeshRenderer">Skinned mesh renderer.</param>
			public void AddMesh(GameObject owner, Mesh inputMesh, Material[] materials, Transform transform, SkinnedMeshRenderer skinnedMeshRenderer)
			{
				if (materials != null)
				{
					foreach (Material material in materials)
					{
						if (globalMaterialMap.ContainsKey(material))
							continue;

						if (primaryShader == null)
							primaryShader = material.shader;

						if (material.shader != primaryShader)
							throw new Exception("All materials must use the same shader");

						globalMaterialMap.Add(material, globalMaterialMap.Count);
					}
				}

				Vector3[] transformedVertices = null;
				Dictionary<int, int> localToGlobalBoneMap = null;

				if (skinnedMeshRenderer != null && combineOutputMeshes)
				{
					if (rootBone == null)
						rootBone = skinnedMeshRenderer.rootBone;
					
					// take a snapshot of the mesh as our new bindpose
					Mesh tempMesh = new Mesh();
					skinnedMeshRenderer.BakeMesh(tempMesh);
					transformedVertices = tempMesh.vertices;

					// NOTE: vertices for combined meshes will be in world space 
					// with a bindpose base on the current bone state
					// so to do the bone transform we just need to do the world to local for each bone
					foreach (Transform bone in skinnedMeshRenderer.bones)
					{
						if (uniqueBones.Contains(bone))
							continue;

						uniqueBones.Add(bone);
						worldSpaceBindPoses.Add(bone.worldToLocalMatrix);
					}

					// create local to global bone map
					localToGlobalBoneMap = new Dictionary<int, int>();
					for (int i = 0; i < skinnedMeshRenderer.bones.Length; i++)
					{
						localToGlobalBoneMap[i] = GetGlobalBoneID(skinnedMeshRenderer.bones[i]);
					}
				}
				else if (skinnedMeshRenderer != null && bakeSkinnedMeshes)
				{
					Mesh tempMesh = new Mesh();
					skinnedMeshRenderer.BakeMesh(tempMesh);
					transformedVertices = tempMesh.vertices;
				}
				
				InstaLODMeshWrapper wrapper = new InstaLODMeshWrapper(owner, inputMesh, materials, globalMaterialMap, transform, localToGlobalBoneMap, transformedVertices);

				if (meshes.Count > 0)
				{
					InstaLODMeshWrapper lastMesh = meshes[meshes.Count - 1];
					// NOTE: submesh indices are now discared. each mesh is now a submesh itself if used in a single operation with multiple game objects
					wrapperSubmeshOffsets[wrapper] = wrapperSubmeshOffsets[lastMesh] + 1;
				}
				else
				{
					wrapperSubmeshOffsets[wrapper] = 0;
				}

				meshes.Add(wrapper);
			
				// append to aux mesh
				if (!string.IsNullOrEmpty(this.auxiliaryGameObjectSuffix) && owner.name.EndsWith(this.auxiliaryGameObjectSuffix))
				{
					if (!this.auxiliaryMeshInitialized)
					{
						InstaLODMesh_InitializeAuxiliary();
						this.auxiliaryMeshInitialized = true;
					}

					InstaLODMesh_AppendAuxiliary(ref wrapper.nativeMesh);
					wrapperSubmeshOffsets[wrapper] = 0;
				}
				else
				{
					InstaLODMesh_Append(ref wrapper.nativeMesh);
				}
			}

			/// <summary>
			/// Extracts the combined mesh.
			/// </summary>
			/// <returns>The combined mesh.</returns>
			public Mesh ExtractCombinedMesh()
			{
				using (InstaLODMeshWrapper wrapper = new InstaLODMeshWrapper(null, null))
				{
					InstaLODMesh nativeMesh = new InstaLODMesh();
					InstaLODMesh_Get(ref nativeMesh);

					return wrapper.ConvertNativeMesh(nativeMesh);
				}
			}

			/// <summary>
			/// Extracts the mesh.
			/// </summary>
			/// <returns>The mesh.</returns>
			/// <param name="owningObject">Owning object.</param>
			public Mesh ExtractMesh(GameObject owningObject)
			{
				InstaLODMeshWrapper wrapper = meshes.Find(item => item.owningObject == owningObject);
			
				if (wrapper == null)
					return null;
				
				InstaLODMesh nativeMesh = new InstaLODMesh();
				
				// NOTE: submesh count is not in use anymore
				InstaLODMesh_Extract(ref nativeMesh, wrapperSubmeshOffsets[wrapper], (UInt32)1);

				bool applyInverseWorldTransform = true;

				if (nativeMesh.BoneWeights != IntPtr.Zero && bakeSkinnedMeshes)
					applyInverseWorldTransform = false;

				return wrapper.ConvertNativeMesh(nativeMesh, applyInverseWorldTransform);
			}

			private static Shader _normalMapConversionShader;
			static Shader normalMapConversionShader
			{
				get
				{
					if (_normalMapConversionShader == null)
					{
						_normalMapConversionShader = Shader.Find("InstaLOD/UnpackNormal");

						if (_normalMapConversionShader == null)
							throw new Exception("Failed to load 'UnpackNormal' shader"); 
					}
					return _normalMapConversionShader;
				}
			}

			private static Material _normalMapConversionMaterial;
			static Material normalMapConversionMaterial
			{
				get
				{
					if (_normalMapConversionMaterial == null)
					{
						_normalMapConversionMaterial = new Material(normalMapConversionShader);
					}
					return _normalMapConversionMaterial;
				}
			}

			/// <summary>
			/// Gets the texture data in a safe manner. This means that it is guaranteed to return valid image data.
			/// If a normal map is being processed, it converted back from a Unity packed normal map, to a regular tangent-space normal mpa.
			/// </summary>
			/// <returns>The texture data safe.</returns>
			/// <param name="texturePageData">Texture page data.</param>
			public static Color32[] GetTextureDataSafe(InstaLODWindowShared.TexturePageData texturePageData)
			{
				Texture texture = texturePageData.texture;
				RenderTextureReadWrite colorSpace = texturePageData.isSRGB ? RenderTextureReadWrite.sRGB : RenderTextureReadWrite.Linear;

				RenderTexture rt = RenderTexture.GetTemporary(texture.width, texture.height, 0, RenderTextureFormat.ARGB32, colorSpace);
				rt.filterMode = FilterMode.Point;
				RenderTexture.active = rt;

				if (texturePageData.isNormalMap)
				{
					Graphics.Blit(texture, rt, normalMapConversionMaterial);
				}
				else
				{
					Graphics.Blit(texture, rt);
				}

				Texture2D img2 = new Texture2D(texture.width, texture.height, TextureFormat.RGBA32, false, false);
				img2.ReadPixels(new Rect(0, 0, texture.width, texture.height), 0, 0);
				img2.Apply();

				RenderTexture.active = null;
				RenderTexture.ReleaseTemporary(rt);

				return img2.GetPixels32();
			}

			/// <summary>
			/// Builds the native material data object.
			/// </summary>
			public void BuildMaterialData()
			{
				InstaLODMaterialData_Initialize();

				foreach (Material material in globalMaterialMap.Keys)
				{
					int materialID = globalMaterialMap[material];
					List<InstaLODWindowShared.TexturePageData> textures = InstaLODWindowShared.GetMaterialTextures(material);
				
					InstaLODMaterial_Begin(materialID);

					Color materialColor = material.color; 
					InstaLODMaterial_SetDefaultColor(ref materialColor);

					foreach (InstaLODWindowShared.TexturePageData texturePageData in textures)
					{
						if (texturePageData.texture is Texture2D == false)
							continue;

						Color32[] textureData = GetTextureDataSafe(texturePageData);

						GCHandle dataHandle = GCHandle.Alloc(textureData, GCHandleType.Pinned);
						textureHandles.Add(dataHandle);

						InstaLODTexturePage instaPage = new InstaLODTexturePage();

						instaPage.Width = (UInt32)texturePageData.texture.width;
						instaPage.Height = (UInt32)texturePageData.texture.height;
						instaPage.Name = Marshal.StringToHGlobalAnsi(texturePageData.texturePageName);
						instaPage.IsNormalMap = texturePageData.isNormalMap;
						instaPage.Data = dataHandle.AddrOfPinnedObject();
						instaPage.ComponentType = TexturePageComponentType.ComponentTypeUInt8;
						instaPage.PixelType = TexturePagePixelType.PixelTypeRGBA;
						instaPage.DefaultColor = material.color;

						InstaLODMaterial_AddTexturePage(ref instaPage);
					}
				}

				InstaLODMaterialData_Finalize();
			}

			/// <summary>
			/// Gets the texture format for texture page.
			/// </summary>
			/// <returns>The texture format for texture page.</returns>
			/// <param name="page">Page.</param>
			public TextureFormat GetTextureFormatForTexturePage(InstaLODTexturePage page)
			{
				if (page.ComponentType == TexturePageComponentType.ComponentTypeUInt8)
				{
					switch (page.PixelType)
					{
						case TexturePagePixelType.PixelTypeLuminance:
							return TextureFormat.R8;
						case TexturePagePixelType.PixelTypeLuminanceAlpha:
							return TextureFormat.RG16; 
						case TexturePagePixelType.PixelTypeRGB:
							return TextureFormat.RGB24; 
						case TexturePagePixelType.PixelTypeRGBA:
							return TextureFormat.RGBA32; 
					}
				}
				else if (page.ComponentType == TexturePageComponentType.ComponentTypeUInt16)
				{
					switch (page.PixelType)
					{
						case TexturePagePixelType.PixelTypeLuminance:
							return TextureFormat.RHalf;
						case TexturePagePixelType.PixelTypeLuminanceAlpha:
							return TextureFormat.RGHalf; 
						case TexturePagePixelType.PixelTypeRGB:
							return TextureFormat.RGBAHalf; 
						case TexturePagePixelType.PixelTypeRGBA:
							return TextureFormat.RGBAHalf; 
					}
				}
				else if (page.ComponentType == TexturePageComponentType.ComponentTypeFloat32)
				{
					switch (page.PixelType)
					{
						case TexturePagePixelType.PixelTypeLuminance:
							return TextureFormat.RFloat;
						case TexturePagePixelType.PixelTypeLuminanceAlpha:
							return TextureFormat.RGFloat; 
						case TexturePagePixelType.PixelTypeRGB:
							throw new Exception("PixelTypeRGB for Float32 not supported by Unity");
						case TexturePagePixelType.PixelTypeRGBA:
							return TextureFormat.RGBAFloat; 
					}
				}

				throw new Exception("Invalid texture page format");
			}

			/// <summary>
			/// Creates a unity material based on InstaLOD material data.
			/// </summary>
			/// <returns>The material.</returns>
			public Material ExtractMaterial(string materialName = null)
			{
				Material material = new Material(primaryShader);

				material.name = "InstaLOD Material";
				string initialFilePath = "Assets/InstaLOD Data/";

				if (!String.IsNullOrEmpty(materialName))
				{
					initialFilePath = Path.Combine(initialFilePath, materialName);
				}
				else
				{
					initialFilePath = Path.Combine(initialFilePath, "M_NewMaterial_INSTALOD");
				}

				string materialAssetPath = GetAssetSavePathForObject(material, initialFilePath);

				if (String.IsNullOrEmpty(materialAssetPath))
				{
					UnityEngine.Debug.LogError("No save path selected for material.");
					return material;
				}

				if (!AssetDatabase.IsValidFolder("Assets/InstaLOD Data"))
					AssetDatabase.CreateFolder("Assets", "InstaLOD Data");
				
				AssetDatabase.CreateAsset(material, materialAssetPath);
				AssetDatabase.SaveAssets();
				AssetDatabase.Refresh();

				string saveFilePath = Path.GetDirectoryName(materialAssetPath);
				string saveFileName = Path.GetFileNameWithoutExtension(materialAssetPath);

				uint count = InstaLODMaterial_GetTexturePageCount();

				for (uint i = 0; i < count; i++)
				{
					InstaLODTexturePage page = new InstaLODTexturePage();
					InstaLODMaterial_GetTexturePageAtIndex(i, ref page);

					string pageName = Marshal.PtrToStringAnsi(page.Name);
					bool isSRGB = true;

					if (pageName == "NormalTangentSpace")
					{
						if (primaryShader.name == "HDRP/Lit")
						{
							pageName = "_NormalMap";
						}
						else
						{
							pageName = "_BumpMap";
						}
					}
					else if (pageName == "AmbientOcclusion")
					{
						pageName = "_OcclusionMap";
					}
					else if (pageName == "Displacement")
					{
						pageName = "_ParallaxMap";
					}
					else if (pageName == "Opacity")
					{
						pageName = "_TranslucencyMap";
					}
					else if (pageName == "_MaskMap")
					{
						isSRGB = false;
					}

					// write texture to disk using InstaLOD SDK
					string finalTextureAssetPath = Path.Combine(saveFilePath, string.Format("{0}-{1}.png", saveFileName, pageName));
					string writeTexturePath = Path.GetFullPath(finalTextureAssetPath);
					InstaLODMaterial_WriteTexturePageAtIndexAsPNG(i, writeTexturePath);

					// reimport PNG
					AssetDatabase.ImportAsset(finalTextureAssetPath);

					// get texture importer
					AssetImporter baseImporter = TextureImporter.GetAtPath(finalTextureAssetPath);
					TextureImporter importer = baseImporter as TextureImporter;
					importer.textureType = TextureImporterType.Default;
					importer.mipmapEnabled = true;
					importer.sRGBTexture = isSRGB;

					// if it was a bumpmap, set the type to normalmap
					if (page.IsNormalMap)
					{
						importer.textureType = TextureImporterType.NormalMap;
					}
					importer.SaveAndReimport();

					// get the imported texture handle
					Texture importedTexture = AssetDatabase.LoadAssetAtPath<Texture>(finalTextureAssetPath);

					// assign it to the final material
					material.SetTexture(pageName, importedTexture);

					// enable material permutations

					if (primaryShader.name == "HDRP/Lit")
					{
						if (page.IsNormalMap)
						{
							material.EnableKeyword("_NORMALMAP_TANGENT_SPACE");
						}
						else if (pageName == "_MaskMap")
						{
							material.EnableKeyword("_MASKMAP");
						}
						else if (pageName == "_DetailMap")
						{
							material.EnableKeyword("_DETAIL_MAP");
						}
					}
					else
					{
						if (page.IsNormalMap)
						{
							material.EnableKeyword("_NORMALMAP");
						}
						else if (pageName == "_ParallaxMap")
						{
							material.EnableKeyword("_PARALLAXMAP");
						}
					}
				}

				AssetDatabase.Refresh();

				return material;
			}
		}


		/// <summary>
		/// Merges the specified mesh materials.
		/// </summary>
		/// <returns><c>true</c>, if merge was meshed, <c>false</c> otherwise.</returns>
		/// <param name="renderers">Renderers.</param>
		/// <param name="settings">Settings.</param>
		internal static bool ExecuteMeshOperation(List<Renderer> renderers, 
												  Func<InstaLODNativeMeshOperationWrapper, bool> initalizeOperation, 
												  Func<InstaLODNativeMeshOperationWrapper, bool> willExecuteOperation, 
												  Func<InstaLODNativeMeshOperationWrapper, bool> executeOperation, 
												  Func<InstaLODNativeMeshOperationWrapper, Renderer, bool> finalizeOperation,
												  Func<InstaLODNativeMeshOperationWrapper, bool> exitOperation)
		{
			Shader primaryShader = null;

			foreach (Renderer renderer in renderers)
			{
				foreach (Material material in renderer.sharedMaterials)
				{
					if (primaryShader != null && primaryShader != material.shader)
						throw new Exception("All renderers must make use of the same shader");

					primaryShader = material.shader;
				}

				// ensure it has a mesh filter
				if (renderer is MeshRenderer)
				{
					if (renderer.gameObject.GetComponent<MeshFilter>() == null)
						throw new Exception("MeshRenderer without MeshFilter");
				}
			}

			ClearErrorLog();

			InstaLODNative.EnsureAssetDataFolder();

			InstaLODNativeMeshOperationWrapper operation = new InstaLODNativeMeshOperationWrapper();

			try
			{
				initalizeOperation(operation);

				// InstaLODTaskItem::Prime
				foreach (Renderer renderer in renderers)
				{
					GameObject gameObject = renderer.gameObject;
					Mesh mesh = (renderer is SkinnedMeshRenderer) ? (renderer as SkinnedMeshRenderer).sharedMesh : gameObject.GetComponent<MeshFilter>().sharedMesh;

					Transform transform = gameObject.transform;
					operation.AddMesh(gameObject, mesh, renderer.sharedMaterials, transform, renderer as SkinnedMeshRenderer);
				}

				operation.BuildMaterialData();

				willExecuteOperation(operation);
			}
			catch (Exception exception)
			{
				operation.Dispose();
				operation = null;
				throw exception;
			}

			// the mesh operation is ready to go,
			// start a child thread and execute the action
			Thread thread = new Thread(delegate()
				{
					bool result = executeOperation(operation);

					if (!result)
					{
						InstaLODMainThreadAction.EnqueueMainThreadAction(
							delegate()
							{
								exitOperation(operation);
								operation.Dispose();
							});
						return;
					}

					InstaLODMainThreadAction.EnqueueMainThreadAction(
						delegate()
						{
							// InstaLODTaskItem::Finalize()
							try
							{
								string saveFileName = renderers[0].name + "_INSTALOD";
								Material mergeMaterial = operation.extractMaterials ? operation.ExtractMaterial("M_" + saveFileName) : null;

								if (operation.combineOutputMeshes)
								{
									GameObject combinedMesh = new GameObject(string.Format("InstaLOD combined {0} meshes", renderers.Count));
									Mesh newMesh = operation.ExtractCombinedMesh();

									if (operation.rootBone != null)
									{
										SkinnedMeshRenderer skinnedMeshRenderer = combinedMesh.AddComponent<SkinnedMeshRenderer>();
										skinnedMeshRenderer.bones = operation.uniqueBones.ToArray();
										skinnedMeshRenderer.rootBone = operation.rootBone;
										skinnedMeshRenderer.sharedMesh = newMesh;
										skinnedMeshRenderer.sharedMesh.bindposes = operation.worldSpaceBindPoses.ToArray();
										// NOTE: always set to max quality to avoid rendering differences
										skinnedMeshRenderer.quality = SkinQuality.Bone4;
									}
									else
									{
										MeshFilter filter = combinedMesh.AddComponent<MeshFilter>();
										combinedMesh.AddComponent<MeshRenderer>();

										filter.sharedMesh = newMesh;
										filter.sharedMesh.bindposes = null;
									}

									Renderer renderer = combinedMesh.GetComponent<Renderer>();
									renderer.sharedMaterials = new Material[] { mergeMaterial };

									// disable all original game objects in the operation
									if (operation.hideSourceGameObjects)
									{
										foreach (Renderer originalRenderer in renderers)
										{
											//  NOTE: it is possible that the original gameObject has been destroyed
											try
											{
												originalRenderer.gameObject.SetActive(false);
											}
											catch (Exception)
											{
												UnityEngine.Debug.LogWarning("Failed to hide original game object. It is recommended to not remove game objects from the scene while a mesh operation is being executed.");
											}
										}
									}

									string savePath = GetAssetSavePathForObject(newMesh, "Assets/InstaLOD Data/" + "MESH_" + saveFileName);

									if (String.IsNullOrEmpty(savePath))
									{
										UnityEngine.Debug.LogError("No save path selected for mesh");
										return;
									}

									AssetDatabase.CreateAsset(newMesh, savePath);
									finalizeOperation(operation, renderer);
								}
								else
								{
									foreach (Renderer owningRenderer in renderers)
									{
										GameObject owningGameObject = owningRenderer.gameObject;
										Mesh newMesh = operation.ExtractMesh(owningGameObject);
										Renderer renderer = owningRenderer;

										if (operation.hideSourceGameObjects)
										{
											GameObject clone = UnityEngine.Object.Instantiate<GameObject>(renderer.gameObject, renderer.gameObject.transform.parent);

											clone.name = renderer.gameObject.name + " (InstaLOD)";

											renderer.gameObject.SetActive(false);
											Transform cloneTransform = renderer.gameObject.transform;

											renderer = clone.GetComponent<Renderer>();
										}

										if (operation.extractMaterials)
										{
											// remove all submesh data after a material OP
											int[] triangles = newMesh.triangles;
											newMesh.subMeshCount = 1;
											newMesh.SetTriangles(triangles, 0);

											renderer.sharedMaterials = new Material[] { mergeMaterial }; 
										}

										if (renderer is SkinnedMeshRenderer)
										{
											SkinnedMeshRenderer skinnedMeshRenderer = (renderer as SkinnedMeshRenderer);
											skinnedMeshRenderer.sharedMesh = newMesh;
											// NOTE: always set to max quality to avoid rendering differences
											skinnedMeshRenderer.quality = SkinQuality.Bone4;

											if (operation.bakeSkinnedMeshes)
											{
												// NOTE: reset the bone transforms to the current rig as we've baked the pose into the vertices
												List<Matrix4x4> newBindPoses = new List<Matrix4x4>();

												foreach (Transform bone in skinnedMeshRenderer.bones)
												{
													newBindPoses.Add(bone.worldToLocalMatrix);
												}

												skinnedMeshRenderer.sharedMesh.bindposes = newBindPoses.ToArray();
											}
										}
										else
										{
											MeshFilter filter = (renderer as MeshRenderer).gameObject.GetComponent<MeshFilter>();

											filter.mesh = newMesh;
										}

										string savePath = GetAssetSavePathForObject(newMesh, "Assets/InstaLOD Data/" + "MESH_" + owningRenderer.gameObject.name + "_INSTALOD");
										
										if (String.IsNullOrEmpty(savePath))
										{
											UnityEngine.Debug.LogError("No save path selected for mesh");
											return;
										}

										AssetDatabase.CreateAsset(newMesh, savePath);

										finalizeOperation(operation, renderer);
									}
								}

								AssetDatabase.SaveAssets();
								AssetDatabase.Refresh();
							}
							finally
							{
								exitOperation(operation);

								// NOTE: force repaint, without refocusing UI windows
								if (InstaLODGroupWindow.GetWindow<InstaLODGroupWindow>("", false).isVisible)
									InstaLODGroupWindow.GetWindow<InstaLODGroupWindow>().Repaint();

								if (InstaLODToolkitWindow.GetWindow<InstaLODToolkitWindow>("", false).isVisible)
									InstaLODToolkitWindow.GetWindow<InstaLODToolkitWindow>().Repaint();
								
								operation.Dispose();
							}
						});
				});
			
			thread.Start();
			return true;
		}

		/// <summary>
		/// Occlusion culls the specified renderers.
		/// </summary>
		/// <returns><c>true</c>, if cull was occlusioned, <c>false</c> otherwise.</returns>
		/// <param name="renderers">Renderers.</param>
		/// <param name="unityCameras">Unity cameras.</param>
		/// <param name="settings">Settings.</param>
		/// <param name="meshOperationSettings">Mesh operation settings.</param>
		public static bool OcclusionCull(List<Renderer> renderers, List<Camera> unityCameras, InstaLODOcclusionCullSettings settings, InstaLODNativeMeshOperationSettings meshOperationSettings)
		{
			if (currentMeshOperationState != null)
				throw new Exception("Mesh operation already in progress.");

			List<InstaLODOcclusionCullCamera> nativeCameras = new List<InstaLODOcclusionCullCamera>();

			foreach (Camera unityCamera in unityCameras)
			{
				// NOTE: no main camera setup
				if(Camera.main == null)
					return false;

				InstaLODOcclusionCullCamera nativeCamera = new InstaLODOcclusionCullCamera();

				nativeCamera.Position = unityCamera.transform.position;
				nativeCamera.Forward = unityCamera.transform.forward;
				nativeCamera.Right = unityCamera.transform.right;
				nativeCamera.Up = unityCamera.transform.up;
				nativeCamera.NearPlane = unityCamera.nearClipPlane;
				nativeCamera.FarPlane = unityCamera.farClipPlane;
				nativeCamera.Aspect = unityCamera.aspect;
				nativeCamera.IsOrthogonal = unityCamera.orthographic;
				nativeCamera.OrthogonalScale = unityCamera.orthographicSize * 2.0f;

				// NOTE: unity fov is vertical
				float verticalFovInRadians = Camera.main.fieldOfView * Mathf.Deg2Rad;
				float horizontalFovInRadians = 2 * Mathf.Atan(Mathf.Tan(verticalFovInRadians / 2) * Camera.main.aspect);
				nativeCamera.FieldOfViewInDegrees = horizontalFovInRadians * Mathf.Rad2Deg;

				nativeCameras.Add(nativeCamera);
			}

			bool result = ExecuteMeshOperation(renderers, 
				/* initialize */ 
				delegate(InstaLODNativeMeshOperationWrapper wrapper)
				{
					wrapper.hideSourceGameObjects = meshOperationSettings.hideSourceGameObjects;
					wrapper.combineOutputMeshes = false;
					wrapper.extractMaterials = false;
					wrapper.bakeSkinnedMeshes = true;
					return true;
				},
				/* will execute */ 
				delegate(InstaLODNativeMeshOperationWrapper wrapper)
				{
					if (!BeginNativeMeshOperation(new InstaLODMeshOperationState(renderers, InstaLODNativeMeshOperationType.OcclusionCull)))
						throw new Exception("Mesh operation already in progress.");

					return true;
				},
				/* execute */ 
				delegate(InstaLODNativeMeshOperationWrapper wrapper)
				{
					InstaLODOcclusionCullCamera[] nativeCamerasArray = nativeCameras.ToArray();
					GCHandle camerasHandle = GCHandle.Alloc(nativeCamerasArray, GCHandleType.Pinned);

					bool occlusionResult = OcclusionCull(ref settings, nativeCameras.Count == 0 ? IntPtr.Zero : camerasHandle.AddrOfPinnedObject(), nativeCameras.Count);
				
					camerasHandle.Free();

					return occlusionResult;
				},
				/* finalize */ 
				delegate(InstaLODNativeMeshOperationWrapper wrapper, Renderer renderer)
				{
					return true;
				},
				/* exit */ 
				delegate(InstaLODNativeMeshOperationWrapper wrapper)
				{
					FinishNativeMeshOperation();
					return true;
				}
			);

			return result;
		}

		/// <summary>
		/// Mesh merges the specified renderers.
		/// </summary>
		/// <returns><c>true</c>, if merge was meshed, <c>false</c> otherwise.</returns>
		/// <param name="renderers">Renderers.</param>
		/// <param name="settings">Settings.</param>
		/// <param name="meshOperationSettings">Mesh operation settings.</param>
		public static bool MeshMerge(List<Renderer> renderers, InstaLODMeshMergeSettings settings, InstaLODNativeMeshOperationSettings meshOperationSettings)
		{
			if (currentMeshOperationState != null)
				throw new Exception("Mesh operation already in progress.");

			bool result = ExecuteMeshOperation(renderers, 
				/* initialize */ 
				delegate(InstaLODNativeMeshOperationWrapper wrapper)
				{
					wrapper.hideSourceGameObjects = meshOperationSettings.hideSourceGameObjects;
					wrapper.combineOutputMeshes = meshOperationSettings.combineOutputMeshes;
					return true;
				},
				/* will execute */ 
				delegate(InstaLODNativeMeshOperationWrapper wrapper)
				{
					if (!BeginNativeMeshOperation(new InstaLODMeshOperationState(renderers, InstaLODNativeMeshOperationType.MeshMerge)))
						throw new Exception("Mesh operation already in progress.");

					return true;
				},
				/* execute */ 
				delegate(InstaLODNativeMeshOperationWrapper wrapper)
				{
					return MeshMerge(ref settings);
				},
				/* finalize */ 
				delegate(InstaLODNativeMeshOperationWrapper wrapper, Renderer renderer)
				{
					return true;
				},
				/* exit */ 
				delegate(InstaLODNativeMeshOperationWrapper wrapper)
				{
					FinishNativeMeshOperation();
					return true;
				}
			);

			return result;
		}

		/// <summary>
		/// Remeshes the specified renderers.
		/// </summary>
		/// <returns>The remesh.</returns>
		/// <param name="renderers">Renderers.</param>
		/// <param name="settings">Settings.</param>
		/// <param name="meshOperationSettings">Mesh operation settings.</param>
		public static bool Remesh(List<Renderer> renderers, InstaLODRemeshingSettings settings, InstaLODNativeMeshOperationSettings meshOperationSettings)
		{
			if (currentMeshOperationState != null)
				throw new Exception("Mesh operation already in progress.");

			if (meshOperationSettings.skinnedMeshEnforceBindPose)
			{
				ResetSkinnedRendererToBindPose(renderers);
			}
			
			bool result = ExecuteMeshOperation(renderers, 
				/* initialize */ 
				delegate(InstaLODNativeMeshOperationWrapper wrapper)
				{
					wrapper.hideSourceGameObjects = meshOperationSettings.hideSourceGameObjects;
					wrapper.combineOutputMeshes = meshOperationSettings.combineOutputMeshes;
					return true;
				},
				/* will execute */ 
				delegate(InstaLODNativeMeshOperationWrapper wrapper)
				{
					if (!BeginNativeMeshOperation(new InstaLODMeshOperationState(renderers, InstaLODNativeMeshOperationType.Remesh)))
						throw new Exception("Mesh operation already in progress.");
					
					return true;
				},
				/* execute */ 
				delegate(InstaLODNativeMeshOperationWrapper wrapper)
				{
					return Remesh(ref settings);
				},
				/* finalize */ 
				delegate(InstaLODNativeMeshOperationWrapper wrapper, Renderer renderer)
				{
					return true;
				},
				/* exit */ 
				delegate(InstaLODNativeMeshOperationWrapper wrapper)
				{
					FinishNativeMeshOperation();
					return true;
				}
			);

			return result;
		}

		/// <summary>
		/// Imposterizes the specified renderers.
		/// </summary>
		/// <returns>The imposterize.</returns>
		/// <param name="renderers">Renderers.</param>
		/// <param name="settings">Settings.</param>
		/// <param name="meshOperationSettings">Mesh operation settings.</param>
		public static bool Imposterize(List<Renderer> renderers, InstaLODImposterizeSettings settings, InstaLODNativeMeshOperationSettings meshOperationSettings)
		{		
			if (currentMeshOperationState != null)
				throw new Exception("Mesh operation already in progress.");
			
			bool result = ExecuteMeshOperation(renderers, 
				/* initialize */ 
				delegate(InstaLODNativeMeshOperationWrapper wrapper)
				{
					if (settings.Type == ImposterType.HybridBillboardCloud)
						wrapper.auxiliaryGameObjectSuffix = "_cloudpoly";
					
					wrapper.hideSourceGameObjects = meshOperationSettings.hideSourceGameObjects;
					wrapper.combineOutputMeshes = meshOperationSettings.combineOutputMeshes;
					return true;
				},
				/* will execute */ 
				delegate(InstaLODNativeMeshOperationWrapper wrapper)
				{
					if (!BeginNativeMeshOperation(new InstaLODMeshOperationState(renderers, InstaLODNativeMeshOperationType.Imposterize)))
						throw new Exception("Mesh operation already in progress.");

					return true;
				},
				/* execute */ 
				delegate(InstaLODNativeMeshOperationWrapper wrapper)
				{
					return Imposterize(ref settings);
				},
				/* finalize */ 
				delegate(InstaLODNativeMeshOperationWrapper wrapper, Renderer renderer)
				{
					try
					{
						Material standardShaderMaterial = renderer.sharedMaterial;

						standardShaderMaterial.SetInt("_Mode", 1);
						standardShaderMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
						standardShaderMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
						standardShaderMaterial.SetInt("_ZWrite", 1);
						standardShaderMaterial.EnableKeyword("_ALPHATEST_ON");
						standardShaderMaterial.DisableKeyword("_ALPHABLEND_ON");
						standardShaderMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
						standardShaderMaterial.SetFloat("_Cutoff", 0.25f);
						standardShaderMaterial.renderQueue = 2450;

						renderer.sharedMaterial = standardShaderMaterial;
					}
					catch (Exception)
					{
						UnityEngine.Debug.LogErrorFormat("Failed to set material '{0}' blend mode to to 'CutOut'. Material is probably not a standard material.", renderer.sharedMaterial.name);
					}

					return true;
				},
				/* exit */ 
				delegate(InstaLODNativeMeshOperationWrapper wrapper)
				{
					FinishNativeMeshOperation();
					return true;
				}
			);

			return result;
		}
			
		/// <summary>
		/// Perform polygon optimization on the specified renderers.
		/// </summary>
		/// <returns>The optimize.</returns>
		/// <param name="renderers">Renderers.</param>
		/// <param name="settings">Settings.</param>
		/// <param name="meshOperationSettings">Mesh operation settings.</param>
		public static void Optimize(List<Renderer> renderers, InstaLODOptimizeSettings settings, InstaLODNativeMeshOperationSettings meshOperationSettings)
		{
			
			if (currentMeshOperationState != null)
				throw new Exception("Mesh operation already in progress.");

			// dealloc previous skeleton
			NativeSkeleton_Initialize(null, null);	

			ExecuteMeshOperation(renderers, 
				/* initialize */ 
				delegate(InstaLODNativeMeshOperationWrapper wrapper)
				{
					wrapper.hideSourceGameObjects = meshOperationSettings.hideSourceGameObjects;
					wrapper.combineOutputMeshes = false;
					wrapper.extractMaterials = false;
					return true;
				},
				/* will execute */ 
				delegate(InstaLODNativeMeshOperationWrapper wrapper)
				{
					NativeSkeleton_Initialize(wrapper, renderers);		

					if (!BeginNativeMeshOperation(new InstaLODMeshOperationState(renderers, InstaLODNativeMeshOperationType.Optimize)))
						throw new Exception("Mesh operation already in progress.");

					return true;
				},
				/* execute */ 
				delegate(InstaLODNativeMeshOperationWrapper wrapper)
				{
					OptimizeResult result = new OptimizeResult();
					OptimizeMesh(ref settings, ref result);
					return result.Success;
				},
				/* finalize */ 
				delegate(InstaLODNativeMeshOperationWrapper wrapper, Renderer render)
				{
					return true;
				},
				/* exit */ 
				delegate(InstaLODNativeMeshOperationWrapper wrapper)
				{
					FinishNativeMeshOperation();
					return true;
				}
			);
		}

		/// <summary>
		/// Resets the skinned renderer to bind pose.
		/// </summary>
		/// <param name="renderers">Renderers.</param>
		public static void ResetSkinnedRendererToBindPose(List<Renderer> renderers)
		{
			renderers.ForEach(delegate(Renderer render)
				{
					ReflectionRestoreToBindPose(render as SkinnedMeshRenderer);
				});
		}

		/// <summary>
		/// Uses C# reflection to restore the mesh to the bind pose.
		/// </summary>
		/// <param name="renderer">Renderer.</param>
		public static void ReflectionRestoreToBindPose(SkinnedMeshRenderer renderer)
		{
			if (renderer == null)
				return;

			try
			{
				Type type = Type.GetType("UnityEditor.AvatarSetupTool, UnityEditor");
				if (type == null)
					return;
				
				MethodInfo info = type.GetMethod("SampleBindPose", BindingFlags.Static | BindingFlags.Public);
				if (info == null)
					return;
			
				info.Invoke(null, new object[] { renderer.gameObject });
			}
			catch (Exception)
			{
				UnityEngine.Debug.LogErrorFormat("InstaLOD: failed to reset SkinnedRenderer of '{0}' to bind pose", renderer.gameObject);
			}
		}
	}
}