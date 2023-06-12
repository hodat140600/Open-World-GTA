/**
 * InstaLODSettingsStructs.cs (InstaLODUnity)
 *
 * Copyright © 2019 InstaLOD GmbH - All Rights Reserved.
 *
 * Unauthorized copying of this file, via any medium is strictly prohibited.
 * This file and all it's contents are proprietary and confidential.
 *
 * @file InstaLODSettingsStructs.cs
 * @copyright 2017-2019 InstaLOD GmbH. All rights reserved.
 * @section License
 */

using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace InstaLOD
{
	public enum MeshFeatureImportance
	{
		Off = 0,
		Lowest = 1,
		Low = 2,
		Normal = 3,
		High = 4,
		Highest = 5
	};

	[System.Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct NativeBool 
	{
		NativeBool(bool value) 
		{
			_value = value ? (byte)1 : (byte)0;
		}

		private byte _value;
		public bool Value {
			get { return _value != 0; }
			set { _value = value ? (byte)1 : (byte)0; }
		}

		public static implicit operator NativeBool(bool b) 
		{ 
			return new NativeBool(b);
		}

		static public implicit operator bool(NativeBool binary)
		{
			return (binary.Value);
		}
	}

	[System.Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct InstaLODSkeletonOptimizeSettings
	{
		public InstaLODSkeletonOptimizeSettings(bool defaults)
		{
			LeafBoneWeldDistance = (0.0f);
			MaximumBoneDepth = (0);
			MaximumBoneInfluencesPerVertex = (4);
			MinimumBoneInfluenceThreshold = (0.0f);
			IgnoreJointIndices = IntPtr.Zero;
			IgnoreJointIndicesCount = (0);
		}

		public float LeafBoneWeldDistance;
		public UInt32 MaximumBoneDepth;		
		public UInt32 MaximumBoneInfluencesPerVertex;	
		public float MinimumBoneInfluenceThreshold;	
		public IntPtr IgnoreJointIndices;
		public UInt32 IgnoreJointIndicesCount;
	};

	[System.Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct InstaLODOptimizeSettings
	{
		public float PercentTriangles;
		public UInt32 AbsoluteTriangles;
		public float MaxDeviation;
		public float WeldingThreshold;
		public UInt32 ScreenSizeInPixels;
		public NativeBool OptimizerVertexWeights;
		public NativeBool RecalculateNormals;
		public float HardAngleThreshold;

		public MeshFeatureImportance SilhouetteImportance;
		public MeshFeatureImportance TextureImportance;
		public MeshFeatureImportance ShadingImportance;
		public MeshFeatureImportance SkinningImportance;

		public InstaLODSkeletonOptimizeSettings SkeletonSettings;

		public InstaLODOptimizeSettings(float percentTriangles)
		{
			PercentTriangles = 1.0f;
			AbsoluteTriangles = 0;
			MaxDeviation = 0f;
			WeldingThreshold = 0f;
			ScreenSizeInPixels = 0;
			OptimizerVertexWeights = false;
			RecalculateNormals = false;
			HardAngleThreshold = 80f;

			SilhouetteImportance = MeshFeatureImportance.Normal;
			TextureImportance = MeshFeatureImportance.Normal;
			ShadingImportance = MeshFeatureImportance.Normal;
			SkinningImportance = MeshFeatureImportance.Normal;

			SkeletonSettings = new InstaLODSkeletonOptimizeSettings(true);
		}

		public override string ToString()
		{
			return "(Percent: " + PercentTriangles + ")";
		}
	}

	public enum SuperSampling
	{
		Off = 0,
		x2 = 1,
		x4 = 2
	}

	public enum UVPackShellRotation
	{
		None = 0, /**< Do not rotate shells. */
		Allow90 = 1, /**< Allow 90 degree shell rotations when packing. */
		Arbitrary = 2 /**< Allow arbitrary rotations when packing. */
	}

	public enum UVUnwrapStrategy
	{
		Organic = 0,			/**< Stretch based unwrapping suitable for organic type meshes. Computationally intensive for high poly meshes. */
		HardSurfaceAngle = 1,	/**< Angle-based projections, suitable for hard-surface type meshes. */
		HardSurfaceAxial = 2,	/**< Axial projections, suitable for hard-surface type meshes. */
		Auto = 3
	}

	[System.Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct InstaLODMeshMergeSettings
	{
		public UInt32 GutterSizeInPixels;
		public SuperSampling SuperSampling;

		public UVPackShellRotation ShellRotation;

		public MeshFeatureImportance UVImportance;
		public MeshFeatureImportance GeometricImportance;
		public MeshFeatureImportance TextureImportance;
		public MeshFeatureImportance VisualImportance;
    
    	public NativeBool WorldspaceNormalizeShells;

		public InstaLODBakeOutputSettings BakeOutput;

		public InstaLODMeshMergeSettings(UInt32 width, UInt32 height)
		{
			BakeOutput = new InstaLODBakeOutputSettings(width, height);
			GutterSizeInPixels = 2;
			SuperSampling = SuperSampling.x2;
			ShellRotation = UVPackShellRotation.Arbitrary;
			UVImportance = MeshFeatureImportance.Normal;
			GeometricImportance = MeshFeatureImportance.Normal;
			TextureImportance = MeshFeatureImportance.Normal;
			VisualImportance = MeshFeatureImportance.Normal;
			WorldspaceNormalizeShells = true;
		}
	}

	public enum ImposterType
	{
		AABB = 0,
		Billboard = 1,
		HybridBillboardCloud = 2,
		Flipbook = 3
	}



	[System.Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct InstaLODImposterizeSettings
	{
		public ImposterType Type;
		public NativeBool TwoSidedQuads;

		public UInt32 QuadXYCount;
		public UInt32 QuadXZCount;
		public UInt32 QuadYZCount;

		public UInt32 CloudFaceCount;
		public float CloudPolyFaceFactor;
		public NativeBool EnableCloudQuadXY;
		public NativeBool EnableCloudQuadXZ;
		public NativeBool EnableCloudQuadYZ;
		public UInt32 QuadSubdivisionsU;
		public UInt32 QuadSubdivisionsV;

		public float AABBDisplacement;

		public UInt32 FlipbookFramesPerAxis;

		public UInt32 GutterSizeInPixels;

		public NativeBool AlphaCutOut;
		public UInt32 AlphaCutOutResolution;
		public NativeBool AlphaCutOutSubdivide;

		public InstaLODBakeOutputSettings BakeOutput;

		public InstaLODImposterizeSettings(UInt32 width, UInt32 height)
		{
			BakeOutput = new InstaLODBakeOutputSettings(width, height);
			// NOTE: we disable super sampling by default for imposters
			BakeOutput.SuperSampling = SuperSampling.Off;

			TwoSidedQuads = true;
			Type = ImposterType.Billboard;

			QuadXYCount = 1;
			QuadXZCount = 0;
			QuadYZCount = 1;

			QuadSubdivisionsU = 1;
			QuadSubdivisionsV = 1;

			CloudFaceCount = 700;
			CloudPolyFaceFactor = 0.5f;
			EnableCloudQuadXY = true;
			EnableCloudQuadXZ = false;
			EnableCloudQuadYZ = true;

			AABBDisplacement = 0.0f;

			FlipbookFramesPerAxis = 8;
			GutterSizeInPixels = 2;
			
			AlphaCutOut = false;
			AlphaCutOutResolution = 16;
			AlphaCutOutSubdivide = false;
		}
	}

	/**
	 * The RemeshFaceCountTarget enumeration specifies different types of fuzzy
	 * face count targets used by the IRemeshingOperation.
	 */
	public enum RemeshFaceCountTarget
	{
		Lowest = 0,
		Low = 1,
		Normal = 2,
		High = 3,
		Highest
	}

	public enum RemeshResolution
	{
		Lowest = 100,
		Low = 150,
		Normal = 256,
		High = 512,
		Highest = 1024
	}

	/**
	 * The RemeshSurfaceMode enumeration specifies different types of
	 * surface construction used by the IRemeshingOperation.
	 */
	public enum RemeshSurfaceMode
	{
		Reconstruct = 0,
		Optimize = 1
	}

	[System.Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct InstaLODBakeOutputSettings
	{
		public InstaLODBakeOutputSettings(UInt32 width, UInt32 height)
		{
			Width = width;
			Height = height;

			SourceMeshUVChannelIndex = 0;
			SolidifyTexturePages = true;
			SuperSampling = SuperSampling.x2;

			TexturePageNormalTangentSpace = true;	
			TexturePageNormalObjectSpace = false;	
			TexturePageMeshID = false;			
			TexturePageVertexColor = false;	
			TexturePageAmbientOcclusion = false;
			TexturePageBentNormals = false;
			TexturePageThickness = false;		
			TexturePageDisplacement = false;	
			TexturePagePosition = false;			
			TexturePageCurvature = false;		
			TexturePageTransfer = false;		
			TexturePageOpacity = false;	
			TexturePageCustom = true;	
		}

		public UInt32 Width;
		public UInt32 Height;

		public UInt32 SourceMeshUVChannelIndex;
		public NativeBool SolidifyTexturePages;
		public SuperSampling SuperSampling;

		public NativeBool TexturePageNormalTangentSpace;
		public NativeBool TexturePageNormalObjectSpace;
		public NativeBool TexturePageMeshID;
		public NativeBool TexturePageVertexColor;
		public NativeBool TexturePageAmbientOcclusion;
		public NativeBool TexturePageBentNormals;
		public NativeBool TexturePageThickness;
		public NativeBool TexturePageDisplacement;
		public NativeBool TexturePagePosition;
		public NativeBool TexturePageCurvature;
		public NativeBool TexturePageTransfer;
		public NativeBool TexturePageOpacity;
		public NativeBool TexturePageCustom;
	}

	[System.Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct InstaLODRemeshingSettings
	{
		public InstaLODRemeshingSettings(UInt32 width, UInt32 height)
		{
			BakeOutput = new InstaLODBakeOutputSettings(width, height);

			SurfaceMode = RemeshSurfaceMode.Reconstruct;
			SurfaceConstructionIgnoreBackface = false;
			Resolution = RemeshResolution.Normal;
			FaceCountTarget = RemeshFaceCountTarget.Normal;
			MaximumTriangles = 0;
			ScreenSizeInPixels = 0;
			ScreenSizePixelMergeDistance = 2;
			ScreenSizeInPixelsAutomaticTextureSize = false;
			HardAngleThreshold = 70.0f;
			WeldDistance = 0.0f;
			GutterSizeInPixels = 2;
			UnwrapStrategy = UVUnwrapStrategy.Auto;
		}

		public RemeshSurfaceMode SurfaceMode;
		public NativeBool SurfaceConstructionIgnoreBackface;
		public RemeshResolution Resolution;
		public RemeshFaceCountTarget FaceCountTarget;
		public UInt32 MaximumTriangles;
		public UInt32 ScreenSizeInPixels;
		public UInt32 ScreenSizePixelMergeDistance;
		public NativeBool ScreenSizeInPixelsAutomaticTextureSize;
		public float HardAngleThreshold;
		public float WeldDistance;
		public UInt32 GutterSizeInPixels;
		public UVUnwrapStrategy UnwrapStrategy;
		public InstaLODBakeOutputSettings BakeOutput;
	}

	[System.Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct InstaLODOcclusionCullCamera
	{
		public Vector3 Position;
		public Vector3 Right;
		public Vector3 Up;
		public Vector3 Forward;
		public float NearPlane;
		public float FarPlane;
		public float FieldOfViewInDegrees;
		public NativeBool IsOrthogonal;
		public float OrthogonalScale;
		public float Aspect;
	}

	/**
	 * The OcclusionCullingStrategy enum represents different types of strategies
	 * used by the occlusion operation.
	 */
	public enum OcclusionCullingStrategy
	{
		PerFace = 0,
		PerMesh = 1,
		PerVertexAdjacency = 2
	}

	/**
	 * The OcclusionCullDataUsage enum represents different ways on how
	 * the computed occlusion data can be used by the occlusion operation.
	 */
	public enum OcclusionCullDataUsage
	{
		RemoveGeometry = 0,
		WriteToVertexColors = 1,
		WriteOptimizerWeightsToVertexColors = 3
	}

	/**
	 * The OcclusionCullMode enum represents different ways on how
	 * the occlusion culling will operate.
	 */
	public enum OcclusionCullMode
	{
		AutomaticInterior = 0,
		CameraBased = 1
	}

	[System.Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct InstaLODOcclusionCullSettings
	{
		public InstaLODOcclusionCullSettings(UInt32 resolution)
		{
			Mode = (OcclusionCullMode.AutomaticInterior);
			CullingStrategy = (OcclusionCullingStrategy.PerFace);
			DataUsage = (OcclusionCullDataUsage.RemoveGeometry);
			AutomaticPrecision = (MeshFeatureImportance.Normal);
			Resolution = resolution;
			AdjacencyDepth = (5);
			SubMeshVisbilityFaceThreshold = (1);
			IgnoreBackface = (false);
			AlphaMaskThreshold = (0.5f);
			OptimizerWeight = (-0.8f);
		}

		public OcclusionCullMode Mode;		
		public OcclusionCullingStrategy	CullingStrategy;
		public OcclusionCullDataUsage DataUsage;		
		public MeshFeatureImportance AutomaticPrecision;
		public UInt32 Resolution;			
		public UInt32 AdjacencyDepth;	
		public UInt32 SubMeshVisbilityFaceThreshold;
		public NativeBool IgnoreBackface;		
		public float AlphaMaskThreshold;
		public float OptimizerWeight;
	}
}

