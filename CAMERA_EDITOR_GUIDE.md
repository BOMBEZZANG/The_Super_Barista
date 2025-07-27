# Camera Position Editor Guide

## New Camera Editor Features ✨

I've created a custom inspector editor that allows you to easily adjust camera positions in real-time!

## How to Use the Camera Editor

### 1. Access the Editor
1. **Select the PlayerCamera** (or GameManager with CameraController) in Hierarchy
2. **Look at the Inspector** - you'll see the new "Camera View Editor" section
3. **Enter Play mode** for live preview features

### 2. Editor Features

#### View Selector
- **Dropdown menu** to select which camera view to edit:
  - Overview
  - Machine View  
  - Grinder View
  - Workspace View

#### Position & Rotation Controls
- **Vector3 fields** for precise position/rotation input
- **FOV slider** (20° to 120°)
- **Live Preview toggle** - see changes in real-time while playing

#### Quick Adjustment Buttons
- **Move Back/Forward** - Adjust distance (0.2 units)
- **Move Left/Right** - Side positioning (0.1 units)  
- **Move Up/Down** - Height adjustment (0.1 units)

#### Preview Controls
- **"Preview This View"** - Jump camera to selected view
- **"Live Preview" toggle** - Real-time updates as you adjust values
- **Works only in Play mode**

#### Quick Actions
- **"Reset to Defaults"** - Restore original camera positions
- **"Copy Current Camera"** - Copy current camera position to selected view
- **"Save Current as Viewpoint"** - Log current position to console
- **"Log All Positions"** - Print all viewpoint data to console

## Step-by-Step Camera Adjustment

### Method 1: Real-Time Adjustment (Recommended)
1. **Enter Play mode**
2. **Select PlayerCamera** in Hierarchy
3. **Turn ON "Live Preview"** in inspector
4. **Select a view** from dropdown (e.g., "Machine View")
5. **Adjust Position/Rotation values** - camera updates instantly!
6. **Use Quick Adjustment buttons** for fine-tuning

### Method 2: Manual Positioning
1. **Enter Play mode** 
2. **Manually move camera** in Scene view to desired position
3. **Click "Copy Current Camera"** to save position to selected view
4. **Test with "Preview This View"** button

### Method 3: Direct Inspector Editing
1. **Select view** from dropdown
2. **Edit Position/Rotation values** directly
3. **Click "Preview This View"** to test
4. **Repeat until satisfied**

## Fixed Camera Distances

I've also updated the default distances to be less "too close":

- **Overview**: Moved back to `Z = 1.8` (was 1.2)
- **Machine View**: Moved back to `Z = 1.0` (was 0.6)  
- **Grinder View**: Moved back to `Z = 1.1` (was 0.7)
- **Workspace View**: Moved back to `Z = 1.2` (was 0.8)

Plus increased FOV for wider views.

## Pro Tips 🎯

### For Better Positioning:
1. **Start with Overview** - get the general feel
2. **Use Scene view** alongside Game view for reference
3. **Test all views** to ensure consistency
4. **Save positions** to console if you want to hardcode them later

### Common Adjustments:
- **Too close?** → Increase Z position (move back)
- **Can't see equipment?** → Decrease Y rotation or adjust X rotation
- **View too narrow?** → Increase FOV
- **Wrong height?** → Adjust Y position (1.6-1.8 is typical human eye level)

### Workflow:
1. Play the scene
2. Enable Live Preview
3. Select a view
4. Adjust until it looks good
5. Test with UI buttons
6. Repeat for all views

## Inspector Layout
```
Camera View Editor
├── Select View: [Dropdown: Overview/Machine/Grinder/Workspace]
├── Editing: [Current View Name]
├── Position: [X, Y, Z fields]
├── Rotation: [X, Y, Z fields]  
├── Field of View: [Slider 20-120]
├── Quick Adjustments: [Move buttons]
├── Preview Controls: [Preview button, Live Preview toggle]
└── Quick Actions: [Reset, Copy, Save, Log buttons]
```

Now you can easily fine-tune camera positions without diving into code! 🎮