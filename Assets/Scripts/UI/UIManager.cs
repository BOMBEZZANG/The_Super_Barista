using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using BaristaSimulator.Core;
using BaristaSimulator.Modules;
using BaristaSimulator.Utils;
using UnityEngine.EventSystems;

namespace BaristaSimulator.UI
{
    public class UIManager : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Canvas mainCanvas;
        [SerializeField] private GameObject viewButtonPrefab;
        [SerializeField] private Transform viewButtonContainer;
        [SerializeField] private float buttonSpacing = 10f;
        
        [Header("Button Styling")]
        [SerializeField] private Color normalButtonColor = new Color(0.8f, 0.8f, 0.8f, 0.9f);
        [SerializeField] private Color highlightedButtonColor = new Color(0.2f, 0.6f, 1f, 1f);
        
        private Dictionary<string, Button> viewButtons = new Dictionary<string, Button>();
        private CameraController cameraController;
        
        private static UIManager instance;
        public static UIManager Instance
        {
            get
            {
                if (instance == null)
                    instance = FindObjectOfType<UIManager>();
                return instance;
            }
        }
        
        void Awake()
        {
            if (instance == null)
                instance = this;
            else if (instance != this)
                Destroy(gameObject);
                
            DebugLogger.Initialize();
            DebugLogger.Log("UIManager Awake called", "UI");
            
            InitializeUI();
        }
        
        void Start()
        {
            cameraController = CameraController.Instance;
            
            // Ensure EventSystem exists
            if (UnityEngine.EventSystems.EventSystem.current == null)
            {
                GameObject eventSystem = new GameObject("EventSystem");
                eventSystem.AddComponent<UnityEngine.EventSystems.EventSystem>();
                eventSystem.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
            }
            
            CreateViewButtons();
        }
        
        void InitializeUI()
        {
            if (mainCanvas == null)
            {
                GameObject canvasObj = new GameObject("MainCanvas");
                mainCanvas = canvasObj.AddComponent<Canvas>();
                mainCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
                
                CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
                scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                scaler.referenceResolution = new Vector2(1920, 1080);
                scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
                scaler.matchWidthOrHeight = 0.5f;
                
                mainCanvas.sortingOrder = 100; // Ensure UI renders on top
                
                canvasObj.AddComponent<GraphicRaycaster>();
            }
            
            if (viewButtonContainer == null)
            {
                GameObject buttonContainer = new GameObject("ViewButtonContainer");
                buttonContainer.transform.SetParent(mainCanvas.transform, false);
                
                RectTransform rect = buttonContainer.AddComponent<RectTransform>();
                rect.anchorMin = new Vector2(0, 0);
                rect.anchorMax = new Vector2(1, 0);
                rect.pivot = new Vector2(0.5f, 0);
                rect.anchoredPosition = new Vector2(0, 80);
                rect.sizeDelta = new Vector2(-40, 80); // Add margins
                
                HorizontalLayoutGroup layoutGroup = buttonContainer.AddComponent<HorizontalLayoutGroup>();
                layoutGroup.spacing = buttonSpacing;
                layoutGroup.childAlignment = TextAnchor.MiddleCenter;
                layoutGroup.childControlWidth = false;
                layoutGroup.childControlHeight = false;
                layoutGroup.childForceExpandWidth = true;
                layoutGroup.childForceExpandHeight = true;
                layoutGroup.padding = new RectOffset(10, 10, 10, 10);
                
                viewButtonContainer = rect;
            }
            
            if (viewButtonPrefab == null)
            {
                CreateViewButtonPrefab();
            }
        }
        
        void CreateViewButtonPrefab()
        {
            DebugLogger.Log("Creating view button prefab", "UI");
            
            viewButtonPrefab = new GameObject("ViewButtonPrefab");
            
            RectTransform rect = viewButtonPrefab.AddComponent<RectTransform>();
            rect.sizeDelta = new Vector2(150, 60);
            
            Image buttonImage = viewButtonPrefab.AddComponent<Image>();
            buttonImage.color = normalButtonColor;
            // Create a simple white sprite for button background
            buttonImage.sprite = CreateWhiteSprite();
            buttonImage.type = Image.Type.Sliced;
            
            Button button = viewButtonPrefab.AddComponent<Button>();
            button.targetGraphic = buttonImage;
            
            GameObject textObj = new GameObject("Text");
            textObj.transform.SetParent(viewButtonPrefab.transform, false);
            
            RectTransform textRect = textObj.AddComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.sizeDelta = Vector2.zero;
            textRect.anchoredPosition = Vector2.zero;
            
            Text buttonText = textObj.AddComponent<Text>();
            buttonText.text = "View";
            
            // Try different font loading methods
            buttonText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            if (buttonText.font == null)
            {
                buttonText.font = Font.CreateDynamicFontFromOSFont(new string[] { "Arial", "Helvetica", "sans-serif" }, 16);
            }
            if (buttonText.font == null)
            {
                buttonText.font = Resources.Load<Font>("Fonts/Arial");
            }
            
            buttonText.fontSize = 16;
            buttonText.color = Color.black;
            buttonText.alignment = TextAnchor.MiddleCenter;
            
            viewButtonPrefab.SetActive(false);
            
            // Ensure the canvas is active
            if (mainCanvas != null)
                mainCanvas.gameObject.SetActive(true);
                
            DebugLogger.Log($"View button prefab created. Active: {viewButtonPrefab.activeSelf}", "UI");
        }
        
        private Sprite CreateWhiteSprite()
        {
            // Create a simple 1x1 white texture
            Texture2D texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, Color.white);
            texture.Apply();
            
            // Create sprite from texture
            return Sprite.Create(texture, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f));
        }
        
        void CreateViewButtons()
        {
            DebugLogger.Log("CreateViewButtons called", "UI");
            
            if (cameraController == null)
            {
                DebugLogger.LogError("CameraController is null, cannot create view buttons", "UI");
                return;
            }
            
            List<string> viewNames = cameraController.GetAllViewNames();
            DebugLogger.Log($"Found {viewNames.Count} view names", "UI");
            
            foreach (string viewName in viewNames)
            {
                DebugLogger.Log($"Creating button for view: {viewName}", "UI");
                
                GameObject buttonObj = Instantiate(viewButtonPrefab, viewButtonContainer);
                buttonObj.SetActive(true);
                buttonObj.name = $"ViewButton_{viewName}";
                
                // Ensure all components are active
                RectTransform buttonRect = buttonObj.GetComponent<RectTransform>();
                if (buttonRect != null)
                {
                    buttonRect.gameObject.SetActive(true);
                    LayoutRebuilder.ForceRebuildLayoutImmediate(buttonRect);
                }
                
                Text buttonText = buttonObj.GetComponentInChildren<Text>();
                if (buttonText != null)
                {
                    buttonText.text = viewName;
                    buttonText.gameObject.SetActive(true);
                    DebugLogger.Log($"Button text set to: {viewName}", "UI");
                }
                else
                {
                    DebugLogger.LogError($"No Text component found for button: {viewName}", "UI");
                }
                
                Button button = buttonObj.GetComponent<Button>();
                if (button != null)
                {
                    string capturedViewName = viewName;
                    button.onClick.AddListener(() => OnViewButtonClicked(capturedViewName));
                    viewButtons[viewName] = button;
                    DebugLogger.Log($"Button listener added for: {viewName}", "UI");
                }
                
                DebugLogger.Log($"Button created - Name: {buttonObj.name}, Active: {buttonObj.activeSelf}, Position: {buttonObj.transform.position}", "UI");
            }
            
            // Force canvas to refresh
            Canvas.ForceUpdateCanvases();
            LayoutRebuilder.ForceRebuildLayoutImmediate(viewButtonContainer as RectTransform);
            
            DebugLogger.Log($"Total buttons created: {viewButtons.Count}", "UI");
            DebugLogger.LogSeparator();
        }
        
        void OnViewButtonClicked(string viewName)
        {
            if (cameraController != null && !cameraController.IsTransitioning)
            {
                cameraController.SwitchToView(viewName);
            }
        }
        
        public void UpdateViewButtonHighlight(string activeViewName)
        {
            foreach (var kvp in viewButtons)
            {
                Image buttonImage = kvp.Value.GetComponent<Image>();
                if (buttonImage != null)
                {
                    buttonImage.color = kvp.Key == activeViewName ? highlightedButtonColor : normalButtonColor;
                }
            }
        }
        
        public void ShowUI()
        {
            if (mainCanvas != null)
                mainCanvas.gameObject.SetActive(true);
        }
        
        public void HideUI()
        {
            if (mainCanvas != null)
                mainCanvas.gameObject.SetActive(false);
        }
        
        [ContextMenu("Debug UI Status")]
        public void DebugUIStatus()
        {
            DebugLogger.LogSeparator();
            DebugLogger.Log("=== UI DEBUG STATUS ===", "UI-Debug");
            DebugLogger.Log($"MainCanvas exists: {mainCanvas != null}", "UI-Debug");
            DebugLogger.Log($"MainCanvas active: {mainCanvas?.gameObject.activeSelf}", "UI-Debug");
            DebugLogger.Log($"ViewButtonContainer exists: {viewButtonContainer != null}", "UI-Debug");
            DebugLogger.Log($"ViewButtonPrefab exists: {viewButtonPrefab != null}", "UI-Debug");
            DebugLogger.Log($"Number of buttons created: {viewButtons.Count}", "UI-Debug");
            DebugLogger.Log($"EventSystem exists: {EventSystem.current != null}", "UI-Debug");
            
            if (mainCanvas != null)
            {
                DebugLogger.Log($"Canvas RenderMode: {mainCanvas.renderMode}", "UI-Debug");
                DebugLogger.Log($"Canvas SortingOrder: {mainCanvas.sortingOrder}", "UI-Debug");
                
                CanvasScaler scaler = mainCanvas.GetComponent<CanvasScaler>();
                if (scaler != null)
                {
                    DebugLogger.Log($"Canvas Scaler Mode: {scaler.uiScaleMode}", "UI-Debug");
                    DebugLogger.Log($"Reference Resolution: {scaler.referenceResolution}", "UI-Debug");
                }
            }
            
            if (viewButtonContainer != null)
            {
                int childCount = viewButtonContainer.childCount;
                DebugLogger.Log($"Button container child count: {childCount}", "UI-Debug");
                RectTransform containerRect = viewButtonContainer as RectTransform;
                if (containerRect != null)
                {
                    DebugLogger.Log($"Container position: {containerRect.position}", "UI-Debug");
                    DebugLogger.Log($"Container anchored position: {containerRect.anchoredPosition}", "UI-Debug");
                    DebugLogger.Log($"Container size: {containerRect.sizeDelta}", "UI-Debug");
                }
                
                for (int i = 0; i < childCount; i++)
                {
                    Transform child = viewButtonContainer.GetChild(i);
                    RectTransform childRect = child as RectTransform;
                    DebugLogger.Log($"Child {i}: {child.name}", "UI-Debug");
                    DebugLogger.Log($"  - Active: {child.gameObject.activeSelf}", "UI-Debug");
                    DebugLogger.Log($"  - Position: {child.position}", "UI-Debug");
                    if (childRect != null)
                    {
                        DebugLogger.Log($"  - Anchored Position: {childRect.anchoredPosition}", "UI-Debug");
                        DebugLogger.Log($"  - Size: {childRect.sizeDelta}", "UI-Debug");
                    }
                    
                    Button btn = child.GetComponent<Button>();
                    if (btn != null)
                    {
                        DebugLogger.Log($"  - Button interactable: {btn.interactable}", "UI-Debug");
                    }
                }
            }
            
            DebugLogger.Log($"Log file saved to: {DebugLogger.GetLogFilePath()}", "UI-Debug");
            DebugLogger.LogSeparator();
            
            Debug.Log($"Debug log saved to: {DebugLogger.GetLogFilePath()}");
        }
        
        [ContextMenu("Force Activate All UI")]
        public void ForceActivateAllUI()
        {
            DebugLogger.Log("Force activating all UI elements", "UI-Fix");
            
            // Ensure canvas is active and properly configured
            if (mainCanvas != null)
            {
                mainCanvas.gameObject.SetActive(true);
                mainCanvas.enabled = true;
                mainCanvas.sortingOrder = 100;
                
                // Ensure GraphicRaycaster is active
                GraphicRaycaster raycaster = mainCanvas.GetComponent<GraphicRaycaster>();
                if (raycaster != null)
                    raycaster.enabled = true;
                    
                DebugLogger.Log("Canvas activated and configured", "UI-Fix");
            }
            
            // Ensure container is active
            if (viewButtonContainer != null)
            {
                viewButtonContainer.gameObject.SetActive(true);
                
                // Force activate all children
                for (int i = 0; i < viewButtonContainer.childCount; i++)
                {
                    Transform child = viewButtonContainer.GetChild(i);
                    child.gameObject.SetActive(true);
                    
                    // Ensure all child components are active
                    foreach (Transform subChild in child)
                    {
                        subChild.gameObject.SetActive(true);
                    }
                    
                    DebugLogger.Log($"Activated button: {child.name}", "UI-Fix");
                }
                
                // Force layout update
                Canvas.ForceUpdateCanvases();
                LayoutRebuilder.ForceRebuildLayoutImmediate(viewButtonContainer as RectTransform);
            }
            
            DebugLogger.Log("Force activation complete", "UI-Fix");
        }
        
        void OnEnable()
        {
            // Ensure UI is visible when enabled
            StartCoroutine(DelayedUICheck());
        }
        
        System.Collections.IEnumerator DelayedUICheck()
        {
            yield return new WaitForSeconds(0.5f);
            
            if (viewButtons.Count == 0 && cameraController != null)
            {
                DebugLogger.Log("No buttons found after delay, recreating...", "UI");
                CreateViewButtons();
            }
            
            ForceActivateAllUI();
        }
    }
}