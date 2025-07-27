using UnityEngine;
using System.Collections.Generic;
using BaristaSimulator.Modules;

namespace BaristaSimulator.Core
{
    public class SceneManager : MonoBehaviour
    {
        [Header("Scene References")]
        [SerializeField] private Transform coffeeBarRoot;
        [SerializeField] private Transform equipmentRoot;
        [SerializeField] private Transform toolsRoot;
        
        [Header("Scene Configuration")]
        [SerializeField] private bool generateGreyboxOnStart = true;
        
        private GreyboxModelGenerator greyboxGenerator;
        private Dictionary<string, GameObject> sceneObjects = new Dictionary<string, GameObject>();
        
        private static SceneManager instance;
        public static SceneManager Instance
        {
            get
            {
                if (instance == null)
                    instance = FindObjectOfType<SceneManager>();
                return instance;
            }
        }
        
        void Awake()
        {
            if (instance == null)
                instance = this;
            else if (instance != this)
                Destroy(gameObject);
                
            InitializeScene();
        }
        
        void InitializeScene()
        {
            greyboxGenerator = GetComponent<GreyboxModelGenerator>();
            if (greyboxGenerator == null)
                greyboxGenerator = gameObject.AddComponent<GreyboxModelGenerator>();
                
            if (generateGreyboxOnStart)
            {
                GenerateSceneLayout();
            }
        }
        
        void GenerateSceneLayout()
        {
            CreateSceneHierarchy();
            
            greyboxGenerator.GenerateAllModels();
            
            ArrangeLShapedLayout();
            
            CacheSceneObjects();
        }
        
        void CreateSceneHierarchy()
        {
            if (coffeeBarRoot == null)
            {
                GameObject coffeeBar = new GameObject("CoffeeBar_Root");
                coffeeBarRoot = coffeeBar.transform;
            }
            
            if (equipmentRoot == null)
            {
                GameObject equipment = new GameObject("Equipment_Root");
                equipmentRoot = equipment.transform;
            }
            
            if (toolsRoot == null)
            {
                GameObject tools = new GameObject("Tools_Root");
                toolsRoot = tools.transform;
            }
        }
        
        void ArrangeLShapedLayout()
        {
            GameObject[] allObjects = GameObject.FindGameObjectsWithTag(GameConstants.Tags.GREYBOX);
            
            foreach (GameObject obj in allObjects)
            {
                if (obj.name.Contains("Coffee_Bar") || obj.name.Contains("Sink"))
                {
                    obj.transform.SetParent(coffeeBarRoot);
                }
                else if (obj.name.Contains("Machine") || obj.name.Contains("Grinder"))
                {
                    obj.transform.SetParent(equipmentRoot);
                }
                else
                {
                    obj.transform.SetParent(toolsRoot);
                }
            }
            
            coffeeBarRoot.position = Vector3.zero;
            equipmentRoot.position = new Vector3(0, 0.45f, 0);
            toolsRoot.position = new Vector3(0, 0.45f, 0);
        }
        
        void CacheSceneObjects()
        {
            sceneObjects.Clear();
            GameObject[] allObjects = GameObject.FindGameObjectsWithTag(GameConstants.Tags.GREYBOX);
            
            foreach (GameObject obj in allObjects)
            {
                if (!sceneObjects.ContainsKey(obj.name))
                    sceneObjects.Add(obj.name, obj);
            }
        }
        
        public GameObject GetSceneObject(string objectName)
        {
            if (sceneObjects.ContainsKey(objectName))
                return sceneObjects[objectName];
            return null;
        }
        
        public Transform GetCoffeeBarRoot() => coffeeBarRoot;
        public Transform GetEquipmentRoot() => equipmentRoot;
        public Transform GetToolsRoot() => toolsRoot;
    }
}