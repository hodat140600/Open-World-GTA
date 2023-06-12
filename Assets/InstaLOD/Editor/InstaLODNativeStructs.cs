/**
 * InstaLODNativeStructs.cs (InstaLODUnity)
 *
 * Copyright © 2019 InstaLOD GmbH - All Rights Reserved.
 *
 * Unauthorized copying of this file, via any medium is strictly prohibited.
 * This file and all it's contents are proprietary and confidential.
 *
 * @file InstaLODNativeStructs.cs
 * @copyright 2017-2019 InstaLOD GmbH. All rights reserved.
 * @section License
 */

using UnityEngine;
using System.Runtime.InteropServices;
using System;

namespace InstaLOD
{
	public enum TexturePagePixelType
	{
		PixelTypeLuminance = 0,
		PixelTypeLuminanceAlpha = 1,
		PixelTypeRGB = 2,
		PixelTypeRGBA = 3
	}

	public enum TexturePageComponentType
	{
		ComponentTypeUInt8 = 0,
		ComponentTypeUInt16 = 1,
		ComponentTypeFloat32 = 2
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct InstaLODBoneWeights
	{
		public int BoneIndex0;
		public int BoneIndex1;
		public int BoneIndex2;
		public int BoneIndex3;

		public float BoneWeight0;
		public float BoneWeight1;
		public float BoneWeight2;
		public float BoneWeight3;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct InstaLODMesh
	{
		public UInt32 VertexCount;
		public UInt32 IndexCount;

		public IntPtr Vertices;
		public IntPtr Indices;
		public IntPtr TexCoords0;
		public IntPtr TexCoords1;
		public IntPtr TexCoords2;
		public IntPtr TexCoords3;
		public IntPtr Tangents;
		public IntPtr Normals;
		public IntPtr Colors;
		public IntPtr MaterialIDs;
		public IntPtr SubmeshIDs;
		public IntPtr BoneWeights;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct InstaLODTexturePage
	{
		public UInt32 Width;
		public UInt32 Height;
		public IntPtr Name;
		public IntPtr Data;
		public Color DefaultColor;
		public NativeBool IsNormalMap;
		public TexturePagePixelType PixelType;
		public TexturePageComponentType ComponentType;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct OptimizeResult
	{
		public NativeBool Success;
		public float MeshDeviation;

		OptimizeResult(bool success, float deviation)
		{
			this.Success = success;
			this.MeshDeviation = deviation;
		}

		public override string ToString()
		{
			return (Success == true ? "Success with mesh deviation: " + MeshDeviation : "Failed");
		}
	}
}