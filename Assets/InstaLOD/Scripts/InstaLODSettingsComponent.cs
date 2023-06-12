/**
 * InstaLODSettingsComponent.cs (InstaLODUnity)
 *
 * Copyright © 2019 InstaLOD GmbH - All Rights Reserved.
 *
 * Unauthorized copying of this file, via any medium is strictly prohibited.
 * This file and all it's contents are proprietary and confidential.
 *
 * @file InstaLODSettingsComponent.cs
 * @copyright 2017-2019 InstaLOD GmbH. All rights reserved.
 * @section License
 */

using UnityEngine;
using System;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using UnityEditor;
using System.IO;
using InstaLOD;

[System.Serializable]
public class InstaLODSettingsComponent : MonoBehaviour
{
	public enum OperationType
	{
		Optimize,
		Remesh,
		Imposterize
	}

	[System.Serializable]
	public struct SettingsContainer
	{
		public SettingsContainer(bool defaults)
		{
			optimize = new InstaLODOptimizeSettings(1.0f);
			remeshing = new InstaLODRemeshingSettings(1024, 1024);
			imposterize = new InstaLODImposterizeSettings(1024, 1024);
			operationType = OperationType.Optimize;
		}

		public InstaLODOptimizeSettings optimize;
		public InstaLODRemeshingSettings remeshing;
		public InstaLODImposterizeSettings imposterize;
		public OperationType operationType; 
	}

	[HideInInspector]
	public SettingsContainer settings = new SettingsContainer(true);
}