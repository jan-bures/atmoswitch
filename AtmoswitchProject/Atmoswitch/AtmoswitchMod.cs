using SpaceWarp.API.Logging;
using SpaceWarp.API.Mods;

namespace Atmoswitch
{
    [MainMod]
     public class AtmoswitchMod : Mod
     {
        public override void OnInitialized()
        {
            Logger.Info("Atmoswitch is initialized");
        }
    }
}