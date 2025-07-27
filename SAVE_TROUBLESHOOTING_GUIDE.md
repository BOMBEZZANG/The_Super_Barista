# Save Troubleshooting Guide

## The Problem ❌
Camera positions are being edited but not saved to the asset file. The asset always shows default positions.

## Enhanced Debugging Tools ✅

I've added new debugging buttons to help identify the issue:

### New Inspector Buttons (in Play Mode):
- **"Verify Asset Contents"** - Shows what's actually saved in the asset file
- **"Force Save Asset"** - Forces Unity to save the asset to disk
- **"Log All Positions"** - Shows current viewpoint positions

## Step-by-Step Debugging Process

### Step 1: Verify Your Current Setup
1. **Enter Play mode**
2. **Select PlayerCamera** in Hierarchy
3. **Click "Log All Positions"** - Check Console
4. **Click "Verify Asset Contents"** - Check Console
5. **Compare the two outputs** - Are they different?

### Step 2: Test the Save Process
1. **Edit a camera position** (e.g., change Machine View Z from 1.0 to 1.5)
2. **Click "Save Viewpoints"**
3. **Check Console** for save messages:
   ```
   === SAVING CAMERA VIEWPOINTS ===
   Saving Machine View: Pos(-0.20, 1.60, 1.50) ...
   Saved 4 viewpoints to asset: CameraViewpoints
   ```
4. **Click "Verify Asset Contents"** - Has the asset been updated?

### Step 3: Force Save (If Normal Save Fails)
1. **Edit camera positions** to your desired values
2. **Click "Force Save Asset"** 
3. **Check Console** for "Force save completed"
4. **Exit Play mode**
5. **Select your asset in Project window** and check Inspector

### Step 4: Visual Verification
1. **Select your CameraViewpoints.asset** in Project window
2. **Look at Inspector** - You should see:
   ```
   Camera Viewpoint Asset
   ├── Saved Camera Viewpoints
   │   ├── Element 0: Overview
   │   │   ├── View Name: "Overview"
   │   │   ├── Position: (0, 1.7, 1.8)  ← Should match your edits
   │   │   ├── Rotation: (10, 180, 0)
   │   │   └── Field Of View: 75
   │   ├── Element 1: Machine View
   │   │   └── [Your edited values should appear here]
   ```

## Common Causes & Solutions

### Issue 1: Unity Not Saving to Disk
**Symptoms**: Console shows save messages but asset Inspector shows defaults
**Solution**: Use "Force Save Asset" button

### Issue 2: Wrong Asset Being Saved To
**Symptoms**: Values saved but to wrong file
**Solution**: Check "Asset: [filename]" shown in Inspector

### Issue 3: Play Mode Edits Not Persisting
**Symptoms**: Edits work in Play mode but reset when stopping
**Solution**: This is normal - you MUST click "Save Viewpoints" before exiting Play mode

### Issue 4: Asset Not Updating Visually
**Symptoms**: Asset file exists but shows empty/default data
**Solution**: 
1. Refresh Project window (Ctrl+R)
2. Use "Force Save Asset"
3. Check if asset is read-only

## Verification Checklist ✅

After editing and saving, verify:
- [ ] Console shows "Saved X viewpoints to asset"
- [ ] "Verify Asset Contents" shows your edited values
- [ ] Asset Inspector (when selected) shows edited values
- [ ] Exiting and re-entering Play mode loads your values

## Quick Fix Workflow

```
1. Enter Play mode
2. Edit camera position with Live Preview
3. Click "Save Viewpoints"
4. Click "Verify Asset Contents" (check Console)
5. If asset not updated → Click "Force Save Asset"
6. Exit Play mode
7. Re-enter Play mode → Your positions should load ✅
```

## Debug Console Output Example

When working correctly, you should see:
```
=== SAVING CAMERA VIEWPOINTS ===
Saving Overview: Pos(0.00, 1.70, 1.80) Rot(10.0, 180.0, 0.0) FOV(75.0)
Saving Machine View: Pos(-0.20, 1.60, 1.50) Rot(5.0, 180.0, 0.0) FOV(65.0)
...
Saved 4 viewpoints to asset: CameraViewpoints
Asset 'CameraViewpoints' updated with 4 viewpoints

=== ASSET CONTENTS: CameraViewpoints ===
Asset has 4 saved viewpoints
Asset Machine View: Pos(-0.20, 1.60, 1.50) Rot(5.0, 180.0, 0.0) FOV(65.0)
...
```

The enhanced debugging will help identify exactly where the save process is failing! 🔍