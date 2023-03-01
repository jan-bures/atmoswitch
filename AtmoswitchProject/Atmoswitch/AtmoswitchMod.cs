using SpaceWarp.API.Logging;
using SpaceWarp.API.Mods;

namespace Atmoswitch
{
    [MainMod]
    public class AtmoswitchMod : Mod
    {
        public static BaseModLogger sLogger;

        public override void OnInitialized()
        {
            sLogger = Logger;
            Logger.Info("Atmoswitch is initialized");
        }
    }
}