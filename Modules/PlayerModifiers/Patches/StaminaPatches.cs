using JehreeDevTools.Common;
using SPT.Reflection.Patching;
using System;
using System.Reflection;

namespace JehreeDevTools.Modules.PlayerModifiers
{
    internal class StaminaConsumePatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            FieldInfo staminaField = typeof(PlayerPhysicalClass).GetField("Stamina", BindingFlags.Instance | BindingFlags.Public);
            Type staminaType = staminaField.FieldType;

            return staminaType.GetMethod("Consume");
        }

        [PatchPrefix]
        public static bool Prefix()
        {
            if (Settings.DisableStaminaConsumption.Value) return false;

            return true;
        }
    }

    internal class StaminaProcessPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            FieldInfo staminaField = typeof(PlayerPhysicalClass).GetField("Stamina", BindingFlags.Instance | BindingFlags.Public);
            Type staminaType = staminaField.FieldType;

            return staminaType.GetMethod("Process");
        }

        [PatchPrefix]
        public static bool Prefix()
        {
            if (Settings.DisableStaminaConsumption.Value) return false;
            
            return true;
        }
    }
}
