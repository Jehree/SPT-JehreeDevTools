using EFT.HealthSystem;
using JehreeDevTools.Common;
using SPT.Reflection.Patching;
using System.Reflection;

namespace JehreeDevTools.Modules.PlayerModifiers
{
    internal class HandleFallPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return typeof(ActiveHealthController).GetMethod(nameof(ActiveHealthController.HandleFall));
        }

        [PatchPrefix]
        public static bool PatchPrefix(ActiveHealthController __instance)
        {
            if (!__instance.Player.IsYourPlayer) return true;
            if (Settings.DisableFallDamage.Value) return false;
            return true;
        }
    }
}
