using Comfort.Common;
using EFT;
using EFT.Console.Core;
using EFT.UI;
using JehreeDevTools.Common;
using System.IO;
using UnityEngine;

namespace JehreeDevTools.Modules.Teleport
{
    internal class TeleportController : JDTComponentBase
    {
        public TeleportMapData MapData;

        private void Update()
        {
            for (int i = 0; i < Settings.AllTpHotkeys.Length; i++)
            {
                var key = Settings.AllTpHotkeys[i];
                if (key.Value.MainKey == KeyCode.None) continue;
                if (!key.Value.IsDown()) continue;
                if (MapData.HotkeyedTeleportKeys[i] == null) continue;

                string teleportPointName = MapData.HotkeyedTeleportKeys[i];
                Vector3 position = MapData.GetTeleportPoint(teleportPointName).Value;

                Teleport(position);
            }
        }

        public override void OnGameStarted()
        {
            string filePath = JsonUtils.GetPath($"{GameWorld.LocationId}.json", TeleportMapData.SUB_FOLDER_NAME);
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                MapData = JsonUtils.GetDataFromJson<TeleportMapData>(json);
            }
            else
            {
                MapData = new TeleportMapData(GameWorld.LocationId);
            }

            foreach (var kvp in MapData.SavedTeleports)
            {
                TeleportPointInteractable.Create(kvp.Key, kvp.Value);
            }

            MapData.BirthDeathEvent += HandleBirthEvent;
        }

        public void HandleBirthEvent(bool born, string name)
        {
            if (born == false) return;
            TeleportPointInteractable.Create(name, MapData.GetTeleportPoint(name).Value);
        }

        public void Teleport(Vector3 position)
        {
            if (Settings.DisableFallDamage.Value == false)
            {
                ConsoleScreen.LogWarning("Teleporting with fall damage enabled is dangerous! Fall damage has been disabled.");
                Settings.DisableFallDamage.Value = true;
            }

            Player.gameObject.transform.position = position;
            GUISounds.PlayUISound(EUISoundType.ButtonClick);
        }

        internal class CommandGroup
        {
            [ConsoleCommand("tp", "", null, "Teleport to saved position")]
            public static void TeleportToSavedPoint([ConsoleArgument("", "Name of saved teleport point")] string name)
            {
                var controller = GetPlayerComponent<TeleportController>();

                Vector3? position = controller.MapData.GetTeleportPoint(name);
                if (!position.HasValue) return;
                
                controller.Teleport(position.Value);
            }

            [ConsoleCommand("tp:save", "", null, "Save teleport point")]
            public static void SaveTeleportPoint([ConsoleArgument("", "Name of teleport point to save")] string name)
            {
                Vector3 pos = Singleton<GameWorld>.Instance.MainPlayer.gameObject.transform.position;
                GetPlayerComponent<TeleportController>().MapData.SaveTeleport(name, pos);
            }

            [ConsoleCommand("tp:delete", "", null, "Delete teleport point")]
            public static void DeleteTeleportPoint([ConsoleArgument("", "Name of teleport point to delete")] string name)
            {
                GetPlayerComponent<TeleportController>().MapData.DeleteTeleport(name);
            }

            [ConsoleCommand("tp:delete_all", "", null, "Delete ALL teleport points")]
            public static void DeleteAllTeleportPoints([ConsoleArgument("", "Are you sure? (type true if yes)")] bool iAmSure)
            {
                if (!iAmSure) return;
                GetPlayerComponent<TeleportController>().MapData.ClearAllTeleports();
            }

            [ConsoleCommand("tp:hotkey", "", null, "Hotkey a saved teleport point")]
            public static void HotkeyTeleportPoint([ConsoleArgument("", "Name of teleport point to hotkey")] string name, [ConsoleArgument(0, "Key to hotkey teleport to (0 thru 9)")] int key)
            {
                GetPlayerComponent<TeleportController>().MapData.HotkeyTeleport(name, key);
            }

            [ConsoleCommand("tp:unhotkey_name", "", null, "Unhotkey a saved teleport point by name")]
            public static void UnHotkeyTeleportPointByName([ConsoleArgument("", "Name of teleport point to unhotkey")] string name)
            {
                GetPlayerComponent<TeleportController>().MapData.UnHotkeyTeleport(name);
            }

            [ConsoleCommand("tp:unhotkey_key", "", null, "Unhotkey a saved teleport point by key")]
            public static void UnHotkeyTeleportPointByKey([ConsoleArgument(0, "Key to unhotkey (0 thru 9)")] int key)
            {
                GetPlayerComponent<TeleportController>().MapData.UnHotkeyTeleport(key);
            }

            [ConsoleCommand("tp:hotkey_clear", "", null, "Clear all hotkeyed teleport points")]
            public static void ClearHotkeyedPoints()
            {
                GetPlayerComponent<TeleportController>().MapData.ClearHotkeys();
                ConsoleScreen.LogWarning("ALL hotkeys been cleared!!");
            }

            [ConsoleCommand("tp:coord", "", null, "Teleport to a specific X, Y, Z coordinate")]
            public static void TeleportToCoordinate([ConsoleArgument(0, "X Coordinate")] float x, [ConsoleArgument(0, "Y Coordinate")] float y, [ConsoleArgument(0, "Z Coordinate")] float z)
            {
                GetPlayerComponent<TeleportController>().Teleport(new Vector3(x, y, z));
            }

            [ConsoleCommand("tp:list", "", null, "List all saved teleport points")]
            public static void ListTeleportPoints()
            {
                if (!Singleton<GameWorld>.Instantiated)
                {
                    ConsoleScreen.LogError("Can't list teleport points while not in a raid");
                    Singleton<GUISounds>.Instance.PlayUISound(EUISoundType.ErrorMessage);
                    return;
                }

                string allTeleportNames = "";

                foreach (var kvp in GetPlayerComponent<TeleportController>().MapData.SavedTeleports)
                {
                    allTeleportNames += $"{kvp.Key}, ";
                }

                ConsoleScreen.Log($"All teleport point names: {allTeleportNames}");
            }
        }
    }
}
