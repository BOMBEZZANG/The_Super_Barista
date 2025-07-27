# 3D Barista Simulator - Phase 1

## Project Overview
This is Phase 1 of the 3D Barista Simulator mobile game. The project creates a grey-box prototype with basic scene setup and camera navigation.

## Project Structure
```
Assets/
├── Scripts/
│   ├── Core/               # Core game systems
│   │   ├── GameManager.cs
│   │   ├── SceneManager.cs
│   │   ├── GameConstants.cs
│   │   └── GreyboxModelGenerator.cs
│   ├── Modules/            # Feature modules
│   │   └── CameraController.cs
│   └── UI/                 # UI components
│       └── UIManager.cs
├── Materials/              # Materials and shaders
├── Scenes/                 # Unity scenes
└── Prefabs/               # Reusable prefabs
```

## Phase 1 Features
- Grey-box 3D models for all coffee equipment based on KCA specifications
- L-shaped coffee bar layout
- Fixed viewpoint camera system with smooth transitions
- Mobile-optimized UI for camera switching
- Basic lighting setup for mobile rendering

## Setup Instructions
1. Open the project in Unity 2020.3 or later
2. Open the MainScene from Assets/Scenes/
3. The scene will automatically generate grey-box models on start
4. Use the view buttons at the bottom of the screen to switch camera views

## Camera Views
- **Overview**: Wide view of the entire coffee bar
- **Machine View**: Focused on the espresso machine
- **Grinder View**: Focused on the coffee grinder
- **Workspace View**: Focused on the working area

## Build Settings
- Platform: Android/iOS
- Orientation: Landscape
- Target Frame Rate: 60 FPS
- Minimum Android API: 21 (Android 5.0)

## Future Phases
- Phase 2: Core interaction system
- Phase 3: Smart object modules
- Phase 4: Evaluation system and UI integration