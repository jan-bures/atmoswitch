using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using SpaceWarp;
using SpaceWarp.API.Mods;

namespace Atmoswitch
{
    [BepInPlugin(ModGuid, ModName, ModVer)]
    [BepInDependency(SpaceWarpPlugin.ModGuid, SpaceWarpPlugin.ModVer)]
    public class AtmoswitchMod: BaseSpaceWarpPlugin
    {
        public const string ModGuid = "com.munix.atmoswitch";
        public const string ModName = "AtmoSwitch";
        public const string ModVer = "0.3.0";

        internal new static ManualLogSource Logger;

        public override void OnInitialized()
        {
            Logger = base.Logger;
            Logger.LogInfo("Atmoswitch is initialized");
            Harmony.CreateAndPatchAll(typeof(AtmoswitchMod).Assembly, ModGuid);
        }
    }
}