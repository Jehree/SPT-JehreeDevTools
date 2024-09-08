using Comfort.Common;
using EFT.UI;
using JehreeDevTools.Common;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;
using static ChartAndGraph.ChartItemEvents;

namespace JehreeDevTools.Modules.Teleport
{
    internal class TeleportMapData
    {
        public const string SUB_FOLDER_NAME = "TeleportData";

        public string MapId;
        public string[] HotkeyedTeleportKeys = new string[10];
        public Dictionary<string, Vector3> SavedTeleports = new Dictionary<string, Vector3>();

        public delegate void BirthDeathEventHandler(bool born, string name);
        public event BirthDeathEventHandler BirthDeathEvent;

        public TeleportMapData(string mapId)
        {
            MapId = mapId;
        }

        private void SaveData()
        {
            string json = JsonUtils.CreateJsonFromData(this);
            string filePath = JsonUtils.GetPath($"{this.MapId}.json", SUB_FOLDER_NAME);
            File.WriteAllText(filePath, json);
        }

        public void SaveTeleport(string name, Vector3 position)
        {
            if (SavedTeleports.ContainsKey(name))
            {
                ConsoleScreen.LogError($"Telport name {name} already exists!");
                Singleton<GUISounds>.Instance.PlayUISound(EUISoundType.ErrorMessage);
                return;
            }

            SavedTeleports[name] = position;

            RaiseBirthDeathEvent(true, name);
            SaveData();

            ConsoleScreen.Log($"Teleport point {name} created for coordinates: {position.ToString()}");
        }

        public void DeleteTeleport(string name)
        {
            if (!SavedTeleports.ContainsKey(name))
            {
                ConsoleScreen.LogError($"Telport name {name} doesn't exist!");
                Singleton<GUISounds>.Instance.PlayUISound(EUISoundType.ErrorMessage);
                return;
            }

            UnHotkeyTeleport(name);
            SavedTeleports.Remove(name);
            RaiseBirthDeathEvent(false, name);
            SaveData();

            ConsoleScreen.Log($"Teleport point {name} deleted");
        }

        public void ClearAllTeleports()
        {
            foreach (var kvp in SavedTeleports)
            {
                RaiseBirthDeathEvent(false, kvp.Key);
            }
            SavedTeleports.Clear();
            ConsoleScreen.LogWarning("ALL teleport points have been deleted!!");
        }

        public void HotkeyTeleport(string name, int key)
        {
            if (!SavedTeleports.ContainsKey(name))
            {
                ConsoleScreen.LogError($"Telport name {name} doesn't exist!");
                Singleton<GUISounds>.Instance.PlayUISound(EUISoundType.ErrorMessage);
                return;
            }

            if (key < 0 || key > 9)
            {
                ConsoleScreen.LogError($"Telport key must be an integer from 0 to 9 (inclusive)");
                Singleton<GUISounds>.Instance.PlayUISound(EUISoundType.ErrorMessage);
                return;
            }

            if (HotkeyedTeleportKeys.Contains(name))
            {
                UnHotkeyTeleport(name);
            }

            HotkeyedTeleportKeys[key] = name;

            SaveData();

            ConsoleScreen.Log($"Teleport point {name} hotkeyed to key {key}");
        }

        public void UnHotkeyTeleport(string name)
        {
            for (int i = 0; i < HotkeyedTeleportKeys.Length; i++)
            {
                if (HotkeyedTeleportKeys[i] != name) continue;
                HotkeyedTeleportKeys[i] = null;
            }

            SaveData();

            ConsoleScreen.Log($"Teleport point {name} unhotkeyed");
        }

        public void UnHotkeyTeleport(int key)
        {
            HotkeyedTeleportKeys[key] = null;
            SaveData();

            ConsoleScreen.Log($"Teleport point for key {key} cleared");
        }

        public void ClearHotkeys()
        {
            HotkeyedTeleportKeys = new string[10];

            ConsoleScreen.Log($"All hotkeys cleared");
        }

        public Vector3? GetTeleportPoint(string name)
        {
            foreach (var kvp in SavedTeleports)
            {
                if (kvp.Key != name) continue;
                return kvp.Value;
            }

            ConsoleScreen.LogError($"Teleport point of the name: {name} not found!");
            Singleton<GUISounds>.Instance.PlayUISound(EUISoundType.ErrorMessage);
            return null;
        }

        private void RaiseBirthDeathEvent(bool born, string name)
        {
            BirthDeathEvent?.Invoke(born, name);
        }
    }
}
