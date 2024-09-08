using BepInEx;
using BepInEx.Logging;
using EFT.UI;
using System.Reflection;
using JehreeDevTools.Modules.PlayerModifiers;
using JehreeDevTools.Modules.Teleport;
using JehreeDevTools.Common;
using JehreeDevTools.Common.Patches;

namespace JehreeDevTools
{
    [BepInPlugin("Jehree.JehreeDevTools", "JehreeDevTools", "1.0.0")]

    public class Plugin : BaseUnityPlugin
    {
        public static string AssemblyPath { get; private set; } = Assembly.GetExecutingAssembly().Location;

        public static ManualLogSource LogSource;

        private void Awake()
        {
            LogSource = Logger;
            Settings.Init(Config);
            
            new GameStartedPatch().Enable();
            new HandleFallPatch().Enable();
            new StaminaConsumePatch().Enable();
            new StaminaProcessPatch().Enable();
            new GetAvailableActionsPatch().Enable();

            ConsoleScreen.Processor.RegisterCommandGroup<TeleportController.CommandGroup>();
        }
    }
}
