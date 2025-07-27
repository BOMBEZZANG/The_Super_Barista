using UnityEngine;

namespace BaristaSimulator.Core
{
    public static class GameConstants
    {
        public static class Dimensions
        {
            public static readonly Vector3 CoffeeBar = new Vector3(2.2f, 0.9f, 0.75f);
            public static readonly Vector3 Sink = new Vector3(0.4f, 0.2f, 0.4f);
            public static readonly Vector3 EspressoMachine = new Vector3(0.75f, 0.5f, 0.55f);
            public static readonly Vector3 CoffeeGrinder = new Vector3(0.2f, 0.6f, 0.35f);
            public static readonly Vector3 KnockBox = new Vector3(0.15f, 0.15f, 0.15f);
            public static readonly Vector3 Portafilter = new Vector3(0.07f, 0.08f, 0.25f);
            public static readonly Vector3 Tamper = new Vector3(0.06f, 0.09f, 0.06f);
            public static readonly Vector3 ShotGlass = new Vector3(0.05f, 0.07f, 0.05f);
            public static readonly Vector3 DemitasseCup = new Vector3(0.065f, 0.06f, 0.065f);
            public static readonly Vector3 CappuccinoCup = new Vector3(0.1f, 0.07f, 0.1f);
            public static readonly Vector3 Saucer = new Vector3(0.15f, 0.02f, 0.15f);
            public static readonly Vector3 SteamPitcher = new Vector3(0.12f, 0.11f, 0.09f);
            public static readonly Vector3 ServingTray = new Vector3(0.35f, 0.03f, 0.25f);
            public static readonly Vector3 SteamClothTray = new Vector3(0.12f, 0.02f, 0.12f);
        }

        public static class ViewPoints
        {
            public const string OVERVIEW = "Overview";
            public const string MACHINE_VIEW = "Machine View";
            public const string GRINDER_VIEW = "Grinder View";
            public const string WORKSPACE_VIEW = "Workspace View";
        }

        public static class Layers
        {
            public const string INTERACTABLE = "Interactable";
            public const string ENVIRONMENT = "Environment";
            public const string UI = "UI";
        }

        public static class Tags
        {
            public const string PLAYER = "Player";
            public const string GREYBOX = "Greybox";
            public const string EQUIPMENT = "Equipment";
            public const string TOOL = "Tool";
            public const string CUP = "Cup";
        }
    }
}