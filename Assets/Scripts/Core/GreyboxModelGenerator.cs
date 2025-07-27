using UnityEngine;
using BaristaSimulator.Core;

namespace BaristaSimulator.Core
{
    public class GreyboxModelGenerator : MonoBehaviour
    {
        [SerializeField] private Material greyboxMaterial;
        
        private void CreateCoffeeBar()
        {
            GameObject coffeeBar = CreateCube("Coffee_Bar", GameConstants.Dimensions.CoffeeBar);
            coffeeBar.transform.position = Vector3.zero;
            
            GameObject sink = CreateCube("Sink", GameConstants.Dimensions.Sink);
            sink.transform.position = new Vector3(0.5f, 0.55f, 0);
            sink.transform.parent = coffeeBar.transform;
        }

        private void CreateEspressoMachine()
        {
            GameObject machine = CreateCube("Espresso_Machine", GameConstants.Dimensions.EspressoMachine);
            machine.transform.position = new Vector3(-0.2f, 0.7f, 0);
            
            GameObject groupHead1 = CreateCylinder("GroupHead_1", 0.03f, 0.05f);
            groupHead1.transform.parent = machine.transform;
            groupHead1.transform.localPosition = new Vector3(-0.15f, -0.2f, 0.2f);
            
            GameObject groupHead2 = CreateCylinder("GroupHead_2", 0.03f, 0.05f);
            groupHead2.transform.parent = machine.transform;
            groupHead2.transform.localPosition = new Vector3(0.15f, -0.2f, 0.2f);
            
            GameObject steamWand = CreateCylinder("Steam_Wand", 0.015f, 0.3f);
            steamWand.transform.parent = machine.transform;
            steamWand.transform.localPosition = new Vector3(0.3f, -0.1f, 0.2f);
            steamWand.transform.rotation = Quaternion.Euler(0, 0, -30);
        }

        private void CreateGrinder()
        {
            GameObject grinder = CreateCube("Coffee_Grinder", GameConstants.Dimensions.CoffeeGrinder);
            grinder.transform.position = new Vector3(-0.8f, 0.75f, 0);
            
            GameObject hopper = CreateCylinder("Hopper", 0.08f, 0.2f);
            hopper.transform.parent = grinder.transform;
            hopper.transform.localPosition = new Vector3(0, 0.4f, 0);
            
            GameObject dosingFork = CreateCube("Dosing_Fork", new Vector3(0.1f, 0.02f, 0.1f));
            dosingFork.transform.parent = grinder.transform;
            dosingFork.transform.localPosition = new Vector3(0, -0.25f, 0.1f);
        }

        private void CreateTools()
        {
            GameObject portafilter = CreatePortafilter();
            portafilter.transform.position = new Vector3(0.3f, 0.5f, 0.2f);
            
            GameObject tamper = CreateTamper();
            tamper.transform.position = new Vector3(0.4f, 0.5f, 0.2f);
            
            GameObject knockBox = CreateCube("Knock_Box", GameConstants.Dimensions.KnockBox);
            knockBox.transform.position = new Vector3(0, 0.52f, 0.2f);
        }

        private void CreateCups()
        {
            GameObject shotGlass = CreateCylinder("Shot_Glass", 0.025f, 0.07f);
            shotGlass.transform.position = new Vector3(0.6f, 0.5f, 0.1f);
            
            GameObject demitasse = CreateCylinder("Demitasse_Cup", 0.0325f, 0.06f);
            demitasse.transform.position = new Vector3(0.6f, 0.5f, 0.2f);
            
            GameObject cappuccino = CreateCylinder("Cappuccino_Cup", 0.05f, 0.07f);
            cappuccino.transform.position = new Vector3(0.6f, 0.5f, 0.3f);
            
            GameObject saucer = CreateCylinder("Saucer", 0.075f, 0.02f);
            saucer.transform.position = new Vector3(0.6f, 0.5f, 0.4f);
            
            GameObject steamPitcher = CreateSteamPitcher();
            steamPitcher.transform.position = new Vector3(0.7f, 0.5f, 0.2f);
        }

        private GameObject CreatePortafilter()
        {
            GameObject portafilter = new GameObject("Portafilter");
            
            GameObject handle = CreateCube("Handle", new Vector3(0.02f, 0.02f, 0.15f));
            handle.transform.parent = portafilter.transform;
            handle.transform.localPosition = new Vector3(0, 0, -0.075f);
            
            GameObject basket = CreateCylinder("Basket", 0.035f, 0.05f);
            basket.transform.parent = portafilter.transform;
            basket.transform.localPosition = new Vector3(0, 0, 0.05f);
            
            GameObject spouts = CreateCube("Spouts", new Vector3(0.04f, 0.03f, 0.06f));
            spouts.transform.parent = portafilter.transform;
            spouts.transform.localPosition = new Vector3(0, -0.04f, 0.05f);
            
            return portafilter;
        }

        private GameObject CreateTamper()
        {
            GameObject tamper = new GameObject("Tamper");
            
            GameObject handle = CreateCylinder("Handle", 0.025f, 0.06f);
            handle.transform.parent = tamper.transform;
            handle.transform.localPosition = new Vector3(0, 0.03f, 0);
            
            GameObject tamperBase = CreateCylinder("Base", 0.029f, 0.03f);
            tamperBase.transform.parent = tamper.transform;
            tamperBase.transform.localPosition = new Vector3(0, -0.015f, 0);
            
            return tamper;
        }

        private GameObject CreateSteamPitcher()
        {
            GameObject pitcher = new GameObject("Steam_Pitcher");
            
            GameObject body = CreateCylinder("Body", 0.045f, 0.11f);
            body.transform.parent = pitcher.transform;
            
            GameObject handle = CreateCube("Handle", new Vector3(0.02f, 0.06f, 0.02f));
            handle.transform.parent = pitcher.transform;
            handle.transform.localPosition = new Vector3(0.055f, 0, 0);
            
            GameObject spout = CreateCube("Spout", new Vector3(0.03f, 0.02f, 0.04f));
            spout.transform.parent = pitcher.transform;
            spout.transform.localPosition = new Vector3(-0.045f, 0.045f, 0);
            spout.transform.rotation = Quaternion.Euler(0, 45, 0);
            
            return pitcher;
        }

        private GameObject CreateCube(string name, Vector3 dimensions)
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.name = name;
            cube.transform.localScale = dimensions;
            if (greyboxMaterial != null)
                cube.GetComponent<Renderer>().material = greyboxMaterial;
            cube.tag = GameConstants.Tags.GREYBOX;
            return cube;
        }

        private GameObject CreateCylinder(string name, float radius, float height)
        {
            GameObject cylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            cylinder.name = name;
            cylinder.transform.localScale = new Vector3(radius * 2, height / 2, radius * 2);
            if (greyboxMaterial != null)
                cylinder.GetComponent<Renderer>().material = greyboxMaterial;
            cylinder.tag = GameConstants.Tags.GREYBOX;
            return cylinder;
        }

        public void GenerateAllModels()
        {
            CreateCoffeeBar();
            CreateEspressoMachine();
            CreateGrinder();
            CreateTools();
            CreateCups();
            
            GameObject servingTray = CreateCube("Serving_Tray", GameConstants.Dimensions.ServingTray);
            servingTray.transform.position = new Vector3(0.8f, 0.5f, 0);
            
            GameObject steamClothTray = CreateCube("Steam_Cloth_Tray", GameConstants.Dimensions.SteamClothTray);
            steamClothTray.transform.position = new Vector3(0.2f, 0.5f, -0.2f);
        }

        [ContextMenu("Generate Greybox Models")]
        void GenerateInEditor()
        {
            GenerateAllModels();
        }
    }
}