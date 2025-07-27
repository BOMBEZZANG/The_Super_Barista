using UnityEngine;
using UnityEditor;
using BaristaSimulator.Modules;
using BaristaSimulator.Data;

namespace BaristaSimulator.Editor
{
    [CustomEditor(typeof(CameraController))]
    public class CameraControllerEditor : UnityEditor.Editor
    {
        private CameraController cameraController;
        private bool showPreview = true;
        private int selectedViewIndex = 0;
        private bool isLivePreview = false;
        private Vector3 tempPosition;
        private Vector3 tempRotation;
        private float tempFOV;
        
        private void OnEnable()
        {
            cameraController = (CameraController)target;
        }
        
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Camera View Editor", EditorStyles.boldLabel);
            
            // Show asset assignment field
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Asset Assignment", EditorStyles.boldLabel);
            
            var currentAsset = cameraController.GetSavedViewpointsAsset();
            var newAsset = EditorGUILayout.ObjectField("Viewpoint Asset", currentAsset, typeof(CameraViewpointAsset), false) as CameraViewpointAsset;
            
            if (newAsset != currentAsset)
            {
                cameraController.SetSavedViewpointsAsset(newAsset);
                EditorUtility.SetDirty(cameraController);
            }
            
            EditorGUILayout.Space();
            
            // Check if we have viewpoints
            if (cameraController.GetAllViewNames().Count == 0)
            {
                EditorGUILayout.HelpBox("No viewpoints found. Enter Play mode to initialize default viewpoints, or add them manually.", MessageType.Info);
                return;
            }
            
            DrawViewSelector();
            DrawViewEditor();
            DrawPreviewControls();
            DrawQuickActions();
        }
        
        void DrawViewSelector()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Select View:", GUILayout.Width(80));
            
            var viewNames = cameraController.GetAllViewNames();
            string[] viewNameArray = viewNames.ToArray();
            
            selectedViewIndex = EditorGUILayout.Popup(selectedViewIndex, viewNameArray);
            selectedViewIndex = Mathf.Clamp(selectedViewIndex, 0, viewNames.Count - 1);
            
            EditorGUILayout.EndHorizontal();
        }
        
        void DrawViewEditor()
        {
            if (selectedViewIndex >= cameraController.GetAllViewNames().Count) return;
            
            var viewNames = cameraController.GetAllViewNames();
            string currentViewName = viewNames[selectedViewIndex];
            
            EditorGUILayout.Space();
            EditorGUILayout.LabelField($"Editing: {currentViewName}", EditorStyles.boldLabel);
            
            // Get current viewpoint data through reflection or public access
            var viewpoints = GetViewpoints();
            if (viewpoints != null && selectedViewIndex < viewpoints.Count)
            {
                var currentViewpoint = viewpoints[selectedViewIndex];
                
                EditorGUI.BeginChangeCheck();
                
                // Position controls
                EditorGUILayout.LabelField("Position", EditorStyles.boldLabel);
                Vector3 newPosition = EditorGUILayout.Vector3Field("Position", currentViewpoint.position);
                
                // Rotation controls
                EditorGUILayout.LabelField("Rotation", EditorStyles.boldLabel);
                Vector3 newRotation = EditorGUILayout.Vector3Field("Rotation", currentViewpoint.rotation);
                
                // FOV control
                EditorGUILayout.LabelField("Field of View", EditorStyles.boldLabel);
                float newFOV = EditorGUILayout.Slider("FOV", currentViewpoint.fieldOfView, 20f, 120f);
                
                if (EditorGUI.EndChangeCheck())
                {
                    currentViewpoint.position = newPosition;
                    currentViewpoint.rotation = newRotation;
                    currentViewpoint.fieldOfView = newFOV;
                    
                    if (isLivePreview && Application.isPlaying)
                    {
                        ApplyToCamera(currentViewpoint);
                    }
                }
                
                // Quick adjustment buttons
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Quick Adjustments", EditorStyles.boldLabel);
                
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Move Back"))
                {
                    currentViewpoint.position += Vector3.forward * 0.2f;
                    if (isLivePreview && Application.isPlaying) ApplyToCamera(currentViewpoint);
                }
                if (GUILayout.Button("Move Forward"))
                {
                    currentViewpoint.position -= Vector3.forward * 0.2f;
                    if (isLivePreview && Application.isPlaying) ApplyToCamera(currentViewpoint);
                }
                EditorGUILayout.EndHorizontal();
                
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Move Left"))
                {
                    currentViewpoint.position += Vector3.left * 0.1f;
                    if (isLivePreview && Application.isPlaying) ApplyToCamera(currentViewpoint);
                }
                if (GUILayout.Button("Move Right"))
                {
                    currentViewpoint.position += Vector3.right * 0.1f;
                    if (isLivePreview && Application.isPlaying) ApplyToCamera(currentViewpoint);
                }
                EditorGUILayout.EndHorizontal();
                
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Move Up"))
                {
                    currentViewpoint.position += Vector3.up * 0.1f;
                    if (isLivePreview && Application.isPlaying) ApplyToCamera(currentViewpoint);
                }
                if (GUILayout.Button("Move Down"))
                {
                    currentViewpoint.position += Vector3.down * 0.1f;
                    if (isLivePreview && Application.isPlaying) ApplyToCamera(currentViewpoint);
                }
                EditorGUILayout.EndHorizontal();
            }
        }
        
        void DrawPreviewControls()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Preview Controls", EditorStyles.boldLabel);
            
            if (Application.isPlaying)
            {
                EditorGUILayout.BeginHorizontal();
                
                if (GUILayout.Button("Preview This View"))
                {
                    cameraController.SwitchToView(selectedViewIndex);
                }
                
                isLivePreview = EditorGUILayout.Toggle("Live Preview", isLivePreview);
                
                EditorGUILayout.EndHorizontal();
                
                if (isLivePreview)
                {
                    EditorGUILayout.HelpBox("Live Preview is ON. Camera will update in real-time as you adjust values.", MessageType.Info);
                }
            }
            else
            {
                EditorGUILayout.HelpBox("Enter Play mode to preview camera positions.", MessageType.Info);
            }
        }
        
        void DrawQuickActions()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Quick Actions", EditorStyles.boldLabel);
            
            EditorGUILayout.BeginHorizontal();
            
            if (GUILayout.Button("Reset to Defaults"))
            {
                if (EditorUtility.DisplayDialog("Reset Viewpoints", 
                    "This will reset all viewpoints to default values. Continue?", "Yes", "Cancel"))
                {
                    cameraController.ResetToDefaults();
                }
            }
            
            if (GUILayout.Button("Copy Current Camera"))
            {
                CopyFromCurrentCamera();
            }
            
            EditorGUILayout.EndHorizontal();
            
            // Persistence controls
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Save/Load", EditorStyles.boldLabel);
            
            EditorGUILayout.BeginHorizontal();
            
            if (GUILayout.Button("Save Viewpoints"))
            {
                SaveViewpoints();
            }
            
            if (GUILayout.Button("Load Viewpoints"))
            {
                LoadViewpoints();
            }
            
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            
            if (GUILayout.Button("Create Viewpoint Asset"))
            {
                CreateViewpointAsset();
            }
            
            if (GUILayout.Button("Select Asset"))
            {
                SelectViewpointAsset();
            }
            
            if (GUILayout.Button("Auto-Find Asset"))
            {
                AutoFindAsset();
            }
            
            EditorGUILayout.EndHorizontal();
            
            // Show current asset
            var currentAsset = cameraController.GetSavedViewpointsAsset();
            if (currentAsset != null)
            {
                EditorGUILayout.LabelField($"Asset: {currentAsset.name}", EditorStyles.miniLabel);
            }
            else
            {
                EditorGUILayout.LabelField("No asset assigned", EditorStyles.miniLabel);
            }
            
            if (Application.isPlaying)
            {
                EditorGUILayout.BeginHorizontal();
                
                if (GUILayout.Button("Save Current as Viewpoint"))
                {
                    SaveCurrentCameraPosition();
                }
                
                if (GUILayout.Button("Log All Positions"))
                {
                    LogAllViewpoints();
                }
                
                EditorGUILayout.EndHorizontal();
                
                EditorGUILayout.BeginHorizontal();
                
                if (GUILayout.Button("Verify Asset Contents"))
                {
                    VerifyAssetContents();
                }
                
                if (GUILayout.Button("Force Save Asset"))
                {
                    ForceSaveAsset();
                }
                
                EditorGUILayout.EndHorizontal();
            }
        }
        
        System.Collections.Generic.List<CameraViewpoint> GetViewpoints()
        {
            return cameraController.Viewpoints;
        }
        
        void ApplyToCamera(CameraViewpoint viewpoint)
        {
            if (cameraController.GetComponent<Camera>() != null)
            {
                var camera = cameraController.GetComponent<Camera>();
                camera.transform.position = viewpoint.position;
                camera.transform.rotation = Quaternion.Euler(viewpoint.rotation);
                camera.fieldOfView = viewpoint.fieldOfView;
            }
        }
        
        void SaveViewpoints()
        {
            // Log current viewpoint values before saving
            var viewpoints = GetViewpoints();
            Debug.Log("=== SAVING CAMERA VIEWPOINTS ===");
            foreach (var vp in viewpoints)
            {
                Debug.Log($"Saving {vp.viewName}: Pos({vp.position.x:F2}, {vp.position.y:F2}, {vp.position.z:F2}) Rot({vp.rotation.x:F1}, {vp.rotation.y:F1}, {vp.rotation.z:F1}) FOV({vp.fieldOfView:F1})");
            }
            
            cameraController.SaveViewpoints();
            EditorUtility.SetDirty(cameraController);
            
            // Verify the asset was updated
            var asset = cameraController.GetSavedViewpointsAsset();
            if (asset != null)
            {
                Debug.Log($"Asset '{asset.name}' updated with {asset.viewpoints.Count} viewpoints");
            }
        }
        
        void LoadViewpoints()
        {
            cameraController.LoadViewpoints();
            EditorUtility.SetDirty(cameraController);
        }
        
        void CreateViewpointAsset()
        {
            string path = EditorUtility.SaveFilePanelInProject(
                "Create Camera Viewpoint Asset",
                "CameraViewpoints",
                "asset",
                "Choose location to save camera viewpoints");
                
            if (!string.IsNullOrEmpty(path))
            {
                CameraViewpointAsset asset = ScriptableObject.CreateInstance<CameraViewpointAsset>();
                AssetDatabase.CreateAsset(asset, path);
                AssetDatabase.SaveAssets();
                
                cameraController.SetSavedViewpointsAsset(asset);
                EditorUtility.SetDirty(cameraController);
                
                Debug.Log($"Created camera viewpoint asset at: {path}");
            }
        }
        
        void SelectViewpointAsset()
        {
            string path = EditorUtility.OpenFilePanel(
                "Select Camera Viewpoint Asset",
                "Assets",
                "asset");
                
            if (!string.IsNullOrEmpty(path))
            {
                // Convert absolute path to relative
                if (path.StartsWith(Application.dataPath))
                {
                    path = "Assets" + path.Substring(Application.dataPath.Length);
                }
                
                CameraViewpointAsset asset = AssetDatabase.LoadAssetAtPath<CameraViewpointAsset>(path);
                if (asset != null)
                {
                    cameraController.SetSavedViewpointsAsset(asset);
                    EditorUtility.SetDirty(cameraController);
                    Debug.Log($"Selected camera viewpoint asset: {asset.name}");
                }
                else
                {
                    Debug.LogError("Selected file is not a valid CameraViewpointAsset");
                }
            }
        }
        
        void AutoFindAsset()
        {
            // Search for CameraViewpointAsset files in the project
            string[] guids = AssetDatabase.FindAssets("t:CameraViewpointAsset");
            
            if (guids.Length == 0)
            {
                Debug.LogWarning("No CameraViewpointAsset files found in project");
                return;
            }
            
            // If multiple assets found, prioritize by name and modification time
            CameraViewpointAsset bestAsset = null;
            System.DateTime latestTime = System.DateTime.MinValue;
            
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                CameraViewpointAsset asset = AssetDatabase.LoadAssetAtPath<CameraViewpointAsset>(path);
                
                if (asset != null)
                {
                    // Get file modification time
                    var fileInfo = new System.IO.FileInfo(path);
                    
                    // Prioritize files named "CameraViewpoints" and newer files
                    bool isPreferredName = asset.name.Contains("CameraViewpoints");
                    System.DateTime fileTime = fileInfo.LastWriteTime;
                    
                    if (bestAsset == null || 
                        (isPreferredName && !bestAsset.name.Contains("CameraViewpoints")) ||
                        (isPreferredName == bestAsset.name.Contains("CameraViewpoints") && fileTime > latestTime))
                    {
                        bestAsset = asset;
                        latestTime = fileTime;
                    }
                }
            }
            
            if (bestAsset != null)
            {
                cameraController.SetSavedViewpointsAsset(bestAsset);
                EditorUtility.SetDirty(cameraController);
                Debug.Log($"Auto-found and assigned camera viewpoint asset: {bestAsset.name}");
            }
        }
        
        void CopyFromCurrentCamera()
        {
            var camera = cameraController.GetComponent<Camera>();
            if (camera != null && selectedViewIndex < GetViewpoints().Count)
            {
                var viewpoints = GetViewpoints();
                var currentViewpoint = viewpoints[selectedViewIndex];
                currentViewpoint.position = camera.transform.position;
                currentViewpoint.rotation = camera.transform.rotation.eulerAngles;
                currentViewpoint.fieldOfView = camera.fieldOfView;
                
                Debug.Log($"Copied current camera position to {currentViewpoint.viewName}");
            }
        }
        
        void SaveCurrentCameraPosition()
        {
            var camera = cameraController.GetComponent<Camera>();
            if (camera != null)
            {
                Debug.Log($"Current Camera Position: {camera.transform.position}");
                Debug.Log($"Current Camera Rotation: {camera.transform.rotation.eulerAngles}");
                Debug.Log($"Current Camera FOV: {camera.fieldOfView}");
            }
        }
        
        void LogAllViewpoints()
        {
            var viewpoints = GetViewpoints();
            if (viewpoints != null)
            {
                Debug.Log("=== All Viewpoints ===");
                for (int i = 0; i < viewpoints.Count; i++)
                {
                    var vp = viewpoints[i];
                    Debug.Log($"{vp.viewName}: Pos({vp.position.x:F2}, {vp.position.y:F2}, {vp.position.z:F2}) " +
                             $"Rot({vp.rotation.x:F1}, {vp.rotation.y:F1}, {vp.rotation.z:F1}) FOV({vp.fieldOfView:F1})");
                }
            }
        }
        
        void VerifyAssetContents()
        {
            var asset = cameraController.GetSavedViewpointsAsset();
            if (asset != null)
            {
                Debug.Log($"=== ASSET CONTENTS: {asset.name} ===");
                Debug.Log($"Asset has {asset.viewpoints.Count} saved viewpoints");
                
                foreach (var vp in asset.viewpoints)
                {
                    Debug.Log($"Asset {vp.viewName}: Pos({vp.position.x:F2}, {vp.position.y:F2}, {vp.position.z:F2}) " +
                             $"Rot({vp.rotation.x:F1}, {vp.rotation.y:F1}, {vp.rotation.z:F1}) FOV({vp.fieldOfView:F1})");
                }
            }
            else
            {
                Debug.LogWarning("No asset assigned to verify");
            }
        }
        
        void ForceSaveAsset()
        {
            var asset = cameraController.GetSavedViewpointsAsset();
            if (asset != null)
            {
                Debug.Log("=== FORCE SAVING ASSET ===");
                
                // Save current viewpoints to asset
                cameraController.SaveViewpoints();
                
                // Force Unity to save everything
                EditorUtility.SetDirty(asset);
                EditorUtility.SetDirty(cameraController);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                
                Debug.Log("Force save completed - check asset in Inspector");
            }
            else
            {
                Debug.LogError("No asset assigned to save to");
            }
        }
    }
}