using Comfort.Common;
using EFT;
using EFT.UI;
using UnityEngine;

namespace JehreeDevTools.Common
{
    internal abstract class JDTComponentBase : MonoBehaviour
    {
        public GameWorld GameWorld {  get; private set; }
        public EFT.Player Player {  get; private set; }
        public GUISounds GUISounds { get; private set; }

        public virtual void OnGameStarted() { }

        public void RunGameStartedLogic()
        {
            GameWorld = Singleton<GameWorld>.Instance;
            Player = GameWorld.MainPlayer;
            GUISounds = Singleton<GUISounds>.Instance;

            OnGameStarted();
        }

        public static T GetPlayerComponent<T>()
        {
            if (!Singleton<GameWorld>.Instantiated)
            {
                string err = "Tried to get Player component when GameWorld was not instantiated! This command does not work when not in raid.";
                ConsoleScreen.LogError(err);
                Singleton<GUISounds>.Instance.PlayUISound(EUISoundType.ErrorMessage);
                throw new System.Exception(err);
            }

            return Singleton<GameWorld>.Instance.MainPlayer.gameObject.GetComponent<T>();
        } 
    }
}
