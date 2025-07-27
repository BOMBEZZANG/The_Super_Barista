using UnityEngine;
using System.Collections.Generic;

namespace BaristaSimulator.Data
{
    [CreateAssetMenu(fileName = "CameraViewpoints", menuName = "Barista Simulator/Camera Viewpoints")]
    public class CameraViewpointAsset : ScriptableObject
    {
        [Header("Saved Camera Viewpoints")]
        public List<CameraViewpointData> viewpoints = new List<CameraViewpointData>();
        
        public void SaveViewpoints(List<BaristaSimulator.Modules.CameraViewpoint> sourceViewpoints)
        {
            viewpoints.Clear();
            foreach (var vp in sourceViewpoints)
            {
                viewpoints.Add(new CameraViewpointData(vp.viewName, vp.position, vp.rotation, vp.fieldOfView));
            }
            
            #if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.AssetDatabase.SaveAssets();
            UnityEditor.AssetDatabase.Refresh();
            UnityEngine.Debug.Log($"Saved {viewpoints.Count} viewpoints to asset: {name}");
            
            // Log saved positions for verification
            foreach (var vp in viewpoints)
            {
                UnityEngine.Debug.Log($"Saved {vp.viewName}: Pos({vp.position.x:F2}, {vp.position.y:F2}, {vp.position.z:F2}) Rot({vp.rotation.x:F1}, {vp.rotation.y:F1}, {vp.rotation.z:F1}) FOV({vp.fieldOfView:F1})");
            }
            #endif
        }
        
        public void LoadViewpoints(List<BaristaSimulator.Modules.CameraViewpoint> targetViewpoints)
        {
            if (viewpoints.Count == 0) return;
            
            targetViewpoints.Clear();
            foreach (var data in viewpoints)
            {
                targetViewpoints.Add(new BaristaSimulator.Modules.CameraViewpoint
                {
                    viewName = data.viewName,
                    position = data.position,
                    rotation = data.rotation,
                    fieldOfView = data.fieldOfView
                });
            }
        }
        
        public bool HasSavedData()
        {
            return viewpoints.Count > 0;
        }
    }
}