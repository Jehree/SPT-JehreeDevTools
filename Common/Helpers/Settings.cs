using BepInEx.Configuration;
using System;
using UnityEngine;

namespace JehreeDevTools.Common
{
    internal class Settings
    {
        private static ConfigFile _config;

        public static ConfigEntry<bool> DisableFallDamage;
        public static ConfigEntry<bool> DisableStaminaConsumption;

        public static ConfigEntry<KeyboardShortcut> TPHotkey0;
        public static ConfigEntry<KeyboardShortcut> TPHotkey1;
        public static ConfigEntry<KeyboardShortcut> TPHotkey2;
        public static ConfigEntry<KeyboardShortcut> TPHotkey3;
        public static ConfigEntry<KeyboardShortcut> TPHotkey4;
        public static ConfigEntry<KeyboardShortcut> TPHotkey5;
        public static ConfigEntry<KeyboardShortcut> TPHotkey6;
        public static ConfigEntry<KeyboardShortcut> TPHotkey7;
        public static ConfigEntry<KeyboardShortcut> TPHotkey8;
        public static ConfigEntry<KeyboardShortcut> TPHotkey9;

        public static ConfigEntry<KeyboardShortcut>[] AllTpHotkeys;

        public static void Init(ConfigFile config)
        {
            _config = config;

            DisableFallDamage = config.Bind(
                "Player Settings",
                "Disable Fall Damage",
                false
            );
            DisableStaminaConsumption = config.Bind(
                "Player Settings",
                "Disable Stamina Consumption",
                false
            );
            TPHotkey0 = config.Bind(
                "Teleport Hotkeys",
                "TP Hotkey 0",
                new KeyboardShortcut(KeyCode.Keypad0)
            );
            TPHotkey1 = config.Bind(
                "Teleport Hotkeys",
                "TP Hotkey 1",
                new KeyboardShortcut(KeyCode.Keypad1)
            );
            TPHotkey2 = config.Bind(
                "Teleport Hotkeys",
                "TP Hotkey 2",
                new KeyboardShortcut(KeyCode.Keypad2)
            );
            TPHotkey3 = config.Bind(
                "Teleport Hotkeys",
                "TP Hotkey 3",
                new KeyboardShortcut(KeyCode.Keypad3)
            );
            TPHotkey4 = config.Bind(
                "Teleport Hotkeys",
                "TP Hotkey 4",
                new KeyboardShortcut(KeyCode.Keypad4)
            );
            TPHotkey5 = config.Bind(
                "Teleport Hotkeys",
                "TP Hotkey 5",
                new KeyboardShortcut(KeyCode.Keypad5)
            );
            TPHotkey6 = config.Bind(
                "Teleport Hotkeys",
                "TP Hotkey 6",
                new KeyboardShortcut(KeyCode.Keypad6)
            );
            TPHotkey7 = config.Bind(
                "Teleport Hotkeys",
                "TP Hotkey 7",
                new KeyboardShortcut(KeyCode.Keypad7)
            );
            TPHotkey8 = config.Bind(
                "Teleport Hotkeys",
                "TP Hotkey 8",
                new KeyboardShortcut(KeyCode.Keypad8)
            );
            TPHotkey9 = config.Bind(
                "Teleport Hotkeys",
                "TP Hotkey 9",
                new KeyboardShortcut(KeyCode.Keypad9)
            );

            AllTpHotkeys = new ConfigEntry<KeyboardShortcut>[10];
            AllTpHotkeys[0] = TPHotkey0;
            AllTpHotkeys[1] = TPHotkey1;
            AllTpHotkeys[2] = TPHotkey2;
            AllTpHotkeys[3] = TPHotkey3;
            AllTpHotkeys[4] = TPHotkey4;
            AllTpHotkeys[5] = TPHotkey5;
            AllTpHotkeys[6] = TPHotkey6;
            AllTpHotkeys[7] = TPHotkey7;
            AllTpHotkeys[8] = TPHotkey8;
            AllTpHotkeys[9] = TPHotkey9;
        }


        private static void CreateSimpleButton(string configSection, string configEntryName, string buttonName, string description, Action onButtonPressedCallable)
        {
            Action<ConfigEntryBase> drawer = (ConfigEntryBase entry) =>
            {
                if (GUILayout.Button(buttonName, GUILayout.ExpandWidth(true)))
                {
                    onButtonPressedCallable();
                }
            };

            ConfigDescription configDescription = new ConfigDescription(description, null, new ConfigurationManagerAttributes { CustomDrawer = drawer });

            _config.Bind(configSection, configEntryName, "", configDescription);
        }
    }
}
