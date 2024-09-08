using Comfort.Common;
using EFT;
using JehreeDevTools.Modules.Teleport;
using SPT.Reflection.Patching;
using System.Reflection;

namespace JehreeDevTools.Common
{
    internal class GameStartedPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return typeof(GameWorld).GetMethod(nameof(GameWorld.OnGameStarted));
        }

        [PatchPostfix]
        public static void PatchPostfix()
        {
            Player player = Singleton<GameWorld>.Instance.MainPlayer;

            player.gameObject.AddComponent<TeleportController>().RunGameStartedLogic();
        }
    }
}
