# Camera Save System Guide

## Problem Fixed ✅
Camera positions were resetting to defaults each time you entered Play mode. Now your edited positions can be saved permanently!

## New Save/Load System Features

### 1. ScriptableObject Asset Storage
- Camera positions are saved to a `.asset` file
- Persists between Unity sessions
- Can be shared between team members
- Version control friendly

### 2. Enhanced Inspector Controls
- **Save Viewpoints** - Save current positions permanently
- **Load Viewpoints** - Load saved positions
- **Create Viewpoint Asset** - Create new save file
- **Select Asset** - Choose existing save file

## Step-by-Step Setup

### First Time Setup (One-Time Only)

1. **Select PlayerCamera** in Hierarchy
2. **Look at Inspector** → Camera View Editor section
3. **Click "Create Viewpoint Asset"**
4. **Choose save location** (e.g., `Assets/Data/CameraViewpoints.asset`)
5. **Click Save**

The asset is now assigned and ready to use!

### Using the Save System

#### Method 1: Edit and Save (Recommended)
1. **Enter Play mode**
2. **Select PlayerCamera** in Hierarchy
3. **Enable "Live Preview"** in Inspector
4. **Select a view** (e.g., Machine View)
5. **Adjust position/rotation** until perfect
6. **Click "Save Viewpoints"** - ✅ Permanently saved!
7. **Exit Play mode** and **Enter Play mode** again
8. **Your positions are preserved!** 🎉

#### Method 2: Save Current Camera Position
1. **Enter Play mode**
2. **Manually position camera** in Scene view
3. **Select PlayerCamera** in Inspector
4. **Click "Copy Current Camera"** to copy to selected view
5. **Click "Save Viewpoints"** to make permanent

## Inspector Layout (New Sections)

```
Camera View Editor
├── [Standard editing controls]
│
├── Save/Load
│   ├── [Save Viewpoints]     ← Save current positions permanently
│   ├── [Load Viewpoints]     ← Load saved positions
│   ├── [Create Viewpoint Asset] ← Create new save file
│   └── [Select Asset]        ← Choose existing save file
│
└── Asset: CameraViewpoints.asset ← Shows current save file
```

## File Management

### Asset File Location
- Saved as: `Assets/[YourPath]/CameraViewpoints.asset`
- Contains all 4 camera views (Overview, Machine, Grinder, Workspace)
- Can be moved/renamed like any Unity asset

### Loading Options
The CameraController has a checkbox:
- ✅ **Load Saved On Start** - Automatically loads saved positions on Play
- ❌ **Load Saved On Start** - Uses default positions instead

## Workflow Examples

### Scenario 1: Adjust Machine View
```
1. Play → Select PlayerCamera → Enable Live Preview
2. Choose "Machine View" from dropdown
3. Adjust Z position from 1.0 to 1.3 (move back)
4. Click "Save Viewpoints"
5. Stop Play → Play again → Position preserved! ✅
```

### Scenario 2: Share with Team
```
1. Edit camera positions to perfection
2. Save Viewpoints
3. Commit CameraViewpoints.asset to version control
4. Team members get your exact camera setup
```

### Scenario 3: Multiple Setups
```
1. Create "CameraViewpoints_Wide.asset" for wide views
2. Create "CameraViewpoints_Close.asset" for close views  
3. Switch between setups using "Select Asset"
```

## Troubleshooting

### "No asset assigned" message
**Fix**: Click "Create Viewpoint Asset" to create a save file

### Changes not saving
**Fix**: Ensure you click "Save Viewpoints" after editing

### Positions still reset
**Fix**: Check "Load Saved On Start" is enabled in Inspector

### Can't find saved positions
**Fix**: Click "Load Viewpoints" to restore from asset file

## Pro Tips 🎯

1. **Save frequently** - Click "Save Viewpoints" after each good adjustment
2. **Create backups** - Make multiple asset files for different setups  
3. **Use descriptive names** - "CameraViewpoints_Final.asset" vs "CameraViewpoints.asset"
4. **Test thoroughly** - Always test all 4 views before final save
5. **Share assets** - Include .asset files in version control for team consistency

## What Happens Now ✅

```
Before: Edit positions → Stop Play → Positions lost 😞
After:  Edit positions → Save Viewpoints → Stop Play → Positions preserved! 🎉
```

Your camera positions are now permanently saved and will persist between Unity sessions! 🎮