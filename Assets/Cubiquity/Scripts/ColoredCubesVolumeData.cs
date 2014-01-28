﻿using UnityEngine;

using System;
using System.IO;
using System.Collections;

namespace Cubiquity
{
	[System.Serializable]
	public sealed class ColoredCubesVolumeData : VolumeData
	{		
		public static ColoredCubesVolumeData CreateFromVoxelDatabase(string pathToVoxelDatabase)
		{
			return CreateFromVoxelDatabase<ColoredCubesVolumeData>(pathToVoxelDatabase);
		}
		
		public static ColoredCubesVolumeData CreateEmptyVolumeData(Region region)
		{
			return CreateEmptyVolumeData<ColoredCubesVolumeData>(region);
		}
		
		public static ColoredCubesVolumeData CreateEmptyVolumeData(Region region, string pathToCreateVoxelDatabase)
		{
			return CreateEmptyVolumeData<ColoredCubesVolumeData>(region, pathToCreateVoxelDatabase);
		}
		
		public QuantizedColor GetVoxel(int x, int y, int z)
		{
			QuantizedColor result;
			if(volumeHandle.HasValue)
			{
				CubiquityDLL.GetVoxel(volumeHandle.Value, x, y, z, out result);
			}
			else
			{
				//Should maybe throw instead.
				result = new QuantizedColor();
			}
			return result;
		}
		
		public void SetVoxel(int x, int y, int z, QuantizedColor quantizedColor)
		{
			if(volumeHandle.HasValue)
			{
				if(x >= enclosingRegion.lowerCorner.x && y >= enclosingRegion.lowerCorner.y && z >= enclosingRegion.lowerCorner.z
					&& x <= enclosingRegion.upperCorner.x && y <= enclosingRegion.upperCorner.y && z <= enclosingRegion.upperCorner.z)
				{						
					CubiquityDLL.SetVoxel(volumeHandle.Value, x, y, z, quantizedColor);
				}
			}
		}
		
		protected override void InitializeEmptyCubiquityVolume()
		{				
			// This function might get called multiple times. E.g the user might call it striaght after crating the volume (so
			// they can add some initial data to the volume) and it might then get called again by OnEnable(). Handle this safely.
			if((volumeHandle == null) && (pathToVoxelDatabase != null))
			{
				// Create an empty region of the desired size.
				volumeHandle = CubiquityDLL.NewEmptyColoredCubesVolume(enclosingRegion.lowerCorner.x, enclosingRegion.lowerCorner.y, enclosingRegion.lowerCorner.z,
					enclosingRegion.upperCorner.x, enclosingRegion.upperCorner.y, enclosingRegion.upperCorner.z, pathToVoxelDatabase, DefaultBaseNodeSize);
			}
		}

		protected override void InitializeExistingCubiquityVolume()
		{				
			// This function might get called multiple times. E.g the user might call it striaght after crating the volume (so
			// they can add some initial data to the volume) and it might then get called again by OnEnable(). Handle this safely.
			if((volumeHandle == null) && (pathToVoxelDatabase != null))
			{
				// Create an empty region of the desired size.
				volumeHandle = CubiquityDLL.NewColoredCubesVolumeFromVDB(pathToVoxelDatabase, DefaultBaseNodeSize);
			}
		}
		
		protected override void ShutdownCubiquityVolume()
		{
			if(volumeHandle.HasValue)
			{
				// We only save if we are in editor mode, not if we are playing.
				bool saveChanges = !Application.isPlaying;
				
				if(saveChanges)
				{
					CubiquityDLL.AcceptOverrideBlocks(volumeHandle.Value);
				}
				CubiquityDLL.DiscardOverrideBlocks(volumeHandle.Value);
				
				CubiquityDLL.DeleteColoredCubesVolume(volumeHandle.Value);
				volumeHandle = null;
			}
		}
	}
}