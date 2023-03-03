using HarmonyLib;
using KSP.Sim.impl;

namespace Atmoswitch;

[HarmonyPatch(typeof(TelemetryComponent), nameof(TelemetryComponent.OnUpdate))]
public class TelemetryComponent_OnUpdate
{
    public static void Prefix(TelemetryComponent __instance)
    {
        if (__instance.SimulationObject?.Vessel is null)
        {
            return;
        }
        
        try
        {
            var args = new object[] { null };
            var getRange = AccessTools.Method(typeof(TelemetryComponent), "GetMaxTransmitterDistanceFromParts");
            var maxRange = (double)getRange.Invoke(__instance, args);
            var isActive = (bool)args[0];
                
            var node = __instance.CommNetNode;

            if (node.IsActive != isActive || Math.Abs(node.MaxRange - maxRange) > 0.1)
            {
                var comManager = KSP.Game.GameManager.Instance.Game.SessionManager.CommNetManager;
                
                AtmoswitchMod.Logger.LogInfo($"Refreshed CommNet from {node.MaxRange}/{node.IsActive} to {maxRange}/{isActive}");
                
                node.MaxRange = maxRange;
                node.IsActive = isActive;
                comManager.UnregisterNode(node);
                comManager.RegisterNode(node);
            }
        }
        catch(Exception ex)
        {
            AtmoswitchMod.Logger.LogError($"Could not refresh: {ex.Message}");
        }
    }
}