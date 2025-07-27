using UnityEngine;
using System.Collections.Generic;
using BaristaSimulator.Core;
using BaristaSimulator.UI;

namespace BaristaSimulator.Modules
{
    [System.Serializable]
    public class CameraViewpoint
    {
        public string viewName;
        public Vector3 position;
        public Vector3 rotation;
        public float fieldOfView = 60f;
    }
    
    public class CameraController : MonoBehaviour
    {
        [Header("Camera Settings")]
        [SerializeField] private Camera playerCamera;
        [SerializeField] private float transitionDuration = 0.5f;
        [SerializeField] private AnimationCurve transitionCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        
        [Header("Viewpoints")]
        [SerializeField] private List<CameraViewpoint> viewpoints = new List<CameraViewpoint>();
        
        private CameraViewpoint currentViewpoint;
        private int currentViewIndex = 0;
        private bool isTransitioning = false;
        
        private static CameraController instance;
        public static CameraController Instance
        {
            get
            {
                if (instance == null)
                    instance = FindObjectOfType<CameraController>();
                return instance;
            }
        }
        
        void Awake()
        {
            if (instance == null)
                instance = this;
            else if (instance != this)
                Destroy(gameObject);
                
            if (playerCamera == null)
                playerCamera = GetComponent<Camera>();
                
            InitializeDefaultViewpoints();
        }
        
        void Start()
        {
            if (viewpoints.Count > 0)
                SetViewpoint(0);
        }
        
        void InitializeDefaultViewpoints()
        {
            if (viewpoints.Count == 0)
            {
                viewpoints.Add(new CameraViewpoint
                {
                    viewName = GameConstants.ViewPoints.OVERVIEW,
                    position = new Vector3(0, 1.8f, -1.5f),
                    rotation = new Vector3(20, 0, 0),
                    fieldOfView = 60f
                });
                
                viewpoints.Add(new CameraViewpoint
                {
                    viewName = GameConstants.ViewPoints.MACHINE_VIEW,
                    position = new Vector3(-0.2f, 1.6f, -0.8f),
                    rotation = new Vector3(10, 0, 0),
                    fieldOfView = 50f
                });
                
                viewpoints.Add(new CameraViewpoint
                {
                    viewName = GameConstants.ViewPoints.GRINDER_VIEW,
                    position = new Vector3(-0.8f, 1.6f, -0.8f),
                    rotation = new Vector3(10, 0, 0),
                    fieldOfView = 50f
                });
                
                viewpoints.Add(new CameraViewpoint
                {
                    viewName = GameConstants.ViewPoints.WORKSPACE_VIEW,
                    position = new Vector3(0.3f, 1.6f, -0.8f),
                    rotation = new Vector3(15, -10, 0),
                    fieldOfView = 55f
                });
            }
        }
        
        public void SwitchToView(string viewName)
        {
            for (int i = 0; i < viewpoints.Count; i++)
            {
                if (viewpoints[i].viewName == viewName)
                {
                    SetViewpoint(i);
                    return;
                }
            }
            
            Debug.LogWarning($"Viewpoint '{viewName}' not found!");
        }
        
        public void SwitchToView(int index)
        {
            if (index >= 0 && index < viewpoints.Count)
            {
                SetViewpoint(index);
            }
        }
        
        public void NextView()
        {
            currentViewIndex = (currentViewIndex + 1) % viewpoints.Count;
            SetViewpoint(currentViewIndex);
        }
        
        public void PreviousView()
        {
            currentViewIndex--;
            if (currentViewIndex < 0)
                currentViewIndex = viewpoints.Count - 1;
            SetViewpoint(currentViewIndex);
        }
        
        private void SetViewpoint(int index)
        {
            if (isTransitioning || index < 0 || index >= viewpoints.Count)
                return;
                
            currentViewIndex = index;
            currentViewpoint = viewpoints[index];
            
            if (Application.isPlaying)
            {
                StartCoroutine(TransitionToViewpoint(currentViewpoint));
            }
            else
            {
                playerCamera.transform.position = currentViewpoint.position;
                playerCamera.transform.rotation = Quaternion.Euler(currentViewpoint.rotation);
                playerCamera.fieldOfView = currentViewpoint.fieldOfView;
            }
            
            UIManager.Instance?.UpdateViewButtonHighlight(currentViewpoint.viewName);
        }
        
        private System.Collections.IEnumerator TransitionToViewpoint(CameraViewpoint targetViewpoint)
        {
            isTransitioning = true;
            
            Vector3 startPosition = playerCamera.transform.position;
            Quaternion startRotation = playerCamera.transform.rotation;
            float startFOV = playerCamera.fieldOfView;
            
            Vector3 targetPosition = targetViewpoint.position;
            Quaternion targetRotation = Quaternion.Euler(targetViewpoint.rotation);
            float targetFOV = targetViewpoint.fieldOfView;
            
            float elapsedTime = 0f;
            
            while (elapsedTime < transitionDuration)
            {
                float t = transitionCurve.Evaluate(elapsedTime / transitionDuration);
                
                playerCamera.transform.position = Vector3.Lerp(startPosition, targetPosition, t);
                playerCamera.transform.rotation = Quaternion.Lerp(startRotation, targetRotation, t);
                playerCamera.fieldOfView = Mathf.Lerp(startFOV, targetFOV, t);
                
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            
            playerCamera.transform.position = targetPosition;
            playerCamera.transform.rotation = targetRotation;
            playerCamera.fieldOfView = targetFOV;
            
            isTransitioning = false;
        }
        
        public string GetCurrentViewName()
        {
            return currentViewpoint?.viewName ?? "";
        }
        
        public List<string> GetAllViewNames()
        {
            List<string> names = new List<string>();
            foreach (var viewpoint in viewpoints)
            {
                names.Add(viewpoint.viewName);
            }
            return names;
        }
        
        public bool IsTransitioning => isTransitioning;
    }
}