using UnityEngine;
using System.Collections.Generic;

namespace BaristaSimulator.Data
{
    [System.Serializable]
    public class CameraViewpointData
    {
        public string viewName;
        public Vector3 position;
        public Vector3 rotation;
        public float fieldOfView;
        
        public CameraViewpointData(string name, Vector3 pos, Vector3 rot, float fov)
        {
            viewName = name;
            position = pos;
            rotation = rot;
            fieldOfView = fov;
        }
    }
    
    [System.Serializable]
    public class CameraViewpointCollection
    {
        public List<CameraViewpointData> viewpoints = new List<CameraViewpointData>();
        
        public CameraViewpointCollection()
        {
            viewpoints = new List<CameraViewpointData>();
        }
    }
}