using UnityEngine;
using UnityEngine.EventSystems;
using BaristaSimulator.UI;
using BaristaSimulator.Modules;

namespace BaristaSimulator.Core
{
    public class GameManager : MonoBehaviour
    {
        [Header("Game Settings")]
        [SerializeField] private bool autoInitialize = true;
        [SerializeField] private int targetFrameRate = 60;
        
        private SceneManager sceneManager;
        private CameraController cameraController;
        private UIManager uiManager;
        
        private static GameManager instance;
        public static GameManager Instance
        {
            get
            {
                if (instance == null)
                    instance = FindObjectOfType<GameManager>();
                return instance;
            }
        }
        
        void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (instance != this)
            {
                Destroy(gameObject);
                return;
            }
            
            ConfigureApplication();
            
            if (autoInitialize)
                InitializeGame();
        }
        
        void ConfigureApplication()
        {
            Application.targetFrameRate = targetFrameRate;
            
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            
            QualitySettings.vSyncCount = 0;
            
            if (EventSystem.current == null)
            {
                GameObject eventSystem = new GameObject("EventSystem");
                eventSystem.AddComponent<EventSystem>();
                eventSystem.AddComponent<StandaloneInputModule>();
            }
        }
        
        void InitializeGame()
        {
            InitializeManagers();
            SetupScene();
        }
        
        void InitializeManagers()
        {
            if (sceneManager == null)
            {
                sceneManager = FindObjectOfType<SceneManager>();
                if (sceneManager == null)
                {
                    GameObject sceneManagerObj = new GameObject("SceneManager");
                    sceneManager = sceneManagerObj.AddComponent<SceneManager>();
                }
            }
            
            if (cameraController == null)
            {
                cameraController = FindObjectOfType<CameraController>();
                if (cameraController == null)
                {
                    GameObject cameraObj = GameObject.Find("Main Camera");
                    if (cameraObj == null)
                    {
                        cameraObj = new GameObject("Main Camera");
                        cameraObj.AddComponent<Camera>();
                        cameraObj.AddComponent<AudioListener>();
                    }
                    cameraController = cameraObj.AddComponent<CameraController>();
                }
            }
            
            if (uiManager == null)
            {
                uiManager = FindObjectOfType<UIManager>();
                if (uiManager == null)
                {
                    GameObject uiManagerObj = new GameObject("UIManager");
                    uiManager = uiManagerObj.AddComponent<UIManager>();
                }
            }
        }
        
        void SetupScene()
        {
            SetupLighting();
        }
        
        void SetupLighting()
        {
            RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Trilight;
            RenderSettings.ambientSkyColor = new Color(0.5f, 0.7f, 0.9f, 1);
            RenderSettings.ambientEquatorColor = new Color(0.4f, 0.5f, 0.6f, 1);
            RenderSettings.ambientGroundColor = new Color(0.2f, 0.2f, 0.2f, 1);
            
            GameObject mainLight = GameObject.Find("Main Light");
            if (mainLight == null)
            {
                mainLight = new GameObject("Main Light");
                Light light = mainLight.AddComponent<Light>();
                light.type = LightType.Directional;
                light.intensity = 1.2f;
                light.color = new Color(1f, 0.95f, 0.8f, 1);
                light.shadows = LightShadows.Soft;
                mainLight.transform.rotation = Quaternion.Euler(45f, -30f, 0);
            }
            
            GameObject fillLight = GameObject.Find("Fill Light");
            if (fillLight == null)
            {
                fillLight = new GameObject("Fill Light");
                Light light = fillLight.AddComponent<Light>();
                light.type = LightType.Directional;
                light.intensity = 0.5f;
                light.color = new Color(0.8f, 0.85f, 1f, 1);
                light.shadows = LightShadows.None;
                fillLight.transform.rotation = Quaternion.Euler(30f, 150f, 0);
            }
        }
        
        public void ResetScene()
        {
            if (sceneManager != null)
            {
                foreach (Transform child in sceneManager.transform)
                {
                    Destroy(child.gameObject);
                }
                InitializeGame();
            }
        }
        
        void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                Debug.Log("Application paused");
            }
            else
            {
                Debug.Log("Application resumed");
            }
        }
        
        void OnApplicationFocus(bool hasFocus)
        {
            if (!hasFocus)
            {
                Debug.Log("Application lost focus");
            }
            else
            {
                Debug.Log("Application gained focus");
            }
        }
    }
}