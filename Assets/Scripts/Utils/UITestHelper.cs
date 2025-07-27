using UnityEngine;
using UnityEngine.UI;
using BaristaSimulator.UI;

namespace BaristaSimulator.Utils
{
    public class UITestHelper : MonoBehaviour
    {
        [Header("UI Test Controls")]
        [SerializeField] private KeyCode testKey = KeyCode.T;
        [SerializeField] private bool showDebugInfo = true;
        
        void Update()
        {
            if (Input.GetKeyDown(testKey))
            {
                TestUIVisibility();
            }
            
            if (showDebugInfo && Input.GetKeyDown(KeyCode.F1))
            {
                DebugAllUIElements();
            }
        }
        
        void TestUIVisibility()
        {
            Debug.Log("=== UI VISIBILITY TEST ===");
            
            Canvas[] canvases = FindObjectsOfType<Canvas>();
            Debug.Log($"Found {canvases.Length} Canvas objects");
            
            foreach (Canvas canvas in canvases)
            {
                Debug.Log($"Canvas: {canvas.name} - Active: {canvas.gameObject.activeSelf} - Enabled: {canvas.enabled} - SortingOrder: {canvas.sortingOrder}");
                
                Button[] buttons = canvas.GetComponentsInChildren<Button>(true);
                Debug.Log($"  Found {buttons.Length} buttons in this canvas");
                
                foreach (Button button in buttons)
                {
                    Debug.Log($"    Button: {button.name} - Active: {button.gameObject.activeSelf} - Enabled: {button.enabled}");
                    Debug.Log($"      Position: {button.transform.position}");
                    
                    RectTransform rect = button.GetComponent<RectTransform>();
                    if (rect != null)
                    {
                        Debug.Log($"      RectTransform - AnchoredPos: {rect.anchoredPosition}, Size: {rect.sizeDelta}");
                    }
                    
                    Image img = button.GetComponent<Image>();
                    if (img != null)
                    {
                        Debug.Log($"      Image - Sprite: {img.sprite != null}, Color: {img.color}");
                    }
                }
            }
            
            // Force UI Manager activation
            UIManager uiManager = FindObjectOfType<UIManager>();
            if (uiManager != null)
            {
                Debug.Log("Found UIManager, forcing activation...");
                uiManager.ForceActivateAllUI();
            }
            else
            {
                Debug.LogError("UIManager not found!");
            }
        }
        
        void DebugAllUIElements()
        {
            Debug.Log("=== FULL UI DEBUG ===");
            
            // Find all UI elements
            Canvas[] canvases = Resources.FindObjectsOfTypeAll<Canvas>();
            Button[] buttons = Resources.FindObjectsOfTypeAll<Button>();
            Image[] images = Resources.FindObjectsOfTypeAll<Image>();
            Text[] texts = Resources.FindObjectsOfTypeAll<Text>();
            
            Debug.Log($"Total UI Elements Found:");
            Debug.Log($"  Canvases: {canvases.Length}");
            Debug.Log($"  Buttons: {buttons.Length}");
            Debug.Log($"  Images: {images.Length}");
            Debug.Log($"  Texts: {texts.Length}");
            
            // Check screen dimensions
            Debug.Log($"Screen: {Screen.width}x{Screen.height}");
            Debug.Log($"Camera: {Camera.main?.name}");
        }
        
        [ContextMenu("Create Test Button")]
        void CreateTestButton()
        {
            // Create a simple test button to verify UI system works
            Canvas canvas = FindObjectOfType<Canvas>();
            if (canvas == null)
            {
                GameObject canvasGO = new GameObject("TestCanvas");
                canvas = canvasGO.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                canvasGO.AddComponent<CanvasScaler>();
                canvasGO.AddComponent<GraphicRaycaster>();
            }
            
            GameObject buttonGO = new GameObject("TestButton");
            buttonGO.transform.SetParent(canvas.transform, false);
            
            RectTransform rect = buttonGO.AddComponent<RectTransform>();
            rect.sizeDelta = new Vector2(200, 50);
            rect.anchoredPosition = Vector2.zero; // Center of screen
            
            Image img = buttonGO.AddComponent<Image>();
            img.color = Color.red;
            
            Button btn = buttonGO.AddComponent<Button>();
            btn.onClick.AddListener(() => Debug.Log("Test button clicked!"));
            
            GameObject textGO = new GameObject("Text");
            textGO.transform.SetParent(buttonGO.transform, false);
            
            RectTransform textRect = textGO.AddComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.sizeDelta = Vector2.zero;
            textRect.anchoredPosition = Vector2.zero;
            
            Text text = textGO.AddComponent<Text>();
            text.text = "TEST BUTTON";
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.fontSize = 16;
            text.color = Color.black;
            text.alignment = TextAnchor.MiddleCenter;
            
            Debug.Log("Test button created in center of screen");
        }
        
        void OnGUI()
        {
            if (showDebugInfo)
            {
                GUI.Label(new Rect(10, 10, 300, 20), $"Press {testKey} for UI test, F1 for debug info");
                GUI.Label(new Rect(10, 30, 300, 20), $"Screen: {Screen.width}x{Screen.height}");
                
                Canvas[] canvases = FindObjectsOfType<Canvas>();
                GUI.Label(new Rect(10, 50, 300, 20), $"Active Canvases: {canvases.Length}");
                
                Button[] buttons = FindObjectsOfType<Button>();
                GUI.Label(new Rect(10, 70, 300, 20), $"Active Buttons: {buttons.Length}");
            }
        }
    }
}