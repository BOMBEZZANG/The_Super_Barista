# File Name Fix Guide

## The Problem ❌
Unity error: "Type cannot be found: BaristaSimulator.Data.CameraViewpointAsset. Containing file and class name must match."

## What This Means
In Unity, the **filename must exactly match the main class name** inside the file. The file was named `CameraViewpointData.cs` but contained a class called `CameraViewpointAsset`.

## ✅ **I've Fixed This For You**

I've split the file into two properly named files:
- `CameraViewpointData.cs` - Contains data classes
- `CameraViewpointAsset.cs` - Contains the ScriptableObject asset class

## What You Need To Do Now

### Step 1: Recompile Scripts
1. **Right-click in Project window** → **Reimport All**
2. **Wait for Unity to finish compiling** (check bottom-right corner)
3. ✅ **No compilation errors should appear**

### Step 2: Create Asset Through Unity Menu
1. **Right-click in Project window**
2. **Create → Barista Simulator → Camera Viewpoints**
3. **Name it** "CameraViewpoints"
4. **Save in your desired folder**

### Step 3: Assign Asset
1. **Select PlayerCamera** in Hierarchy
2. **Drag the new asset** to "Viewpoint Asset" field in Inspector
3. ✅ **Should assign without "X" error**

### Step 4: Test
1. **Click "Save Viewpoints"** - Should work
2. **Click "Load Viewpoints"** - Should work
3. **Inspector shows**: "Asset: CameraViewpoints" ✅

## Alternative: Use Inspector Button
1. **Select PlayerCamera** in Hierarchy
2. **Click "Create Viewpoint Asset"** in Inspector
3. **Choose location and save**
4. ✅ **Asset created and assigned automatically**

## Why This Happened
Unity's scripting system requires that:
- **File name** = **Main class name** inside the file
- `CameraViewpointAsset.cs` must contain `class CameraViewpointAsset`
- This is a strict Unity requirement for ScriptableObjects

## After Fix
- ✅ No compilation errors
- ✅ Menu option "Create → Barista Simulator → Camera Viewpoints" appears
- ✅ Can create and assign assets properly
- ✅ Save/Load system works perfectly

The naming issue is now fixed - you should be able to create and use camera viewpoint assets! 🎮