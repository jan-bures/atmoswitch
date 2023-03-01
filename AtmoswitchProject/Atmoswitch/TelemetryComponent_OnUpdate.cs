using HarmonyLib;
using KSP.Sim.impl;

namespace Atmoswitch;

[HarmonyPatch(typeof(TelemetryComponent), nameof(TelemetryComponent.OnUpdate))]
public class TelemetryComponent_OnUpdate
{
    public static void Prefix(TelemetryComponent __instance)
    {
        if (__instance.SimulationObject is null || __instance.SimulationObject.Vessel is null)
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
                
                AtmoswitchMod.sLogger.Info($"Refreshed CommNet from {node.MaxRange}/{node.IsActive} to {maxRange}/{isActive}");
                
                node.MaxRange = maxRange;
                node.IsActive = isActive;
                comManager.UnregisterNode(node);
                comManager.RegisterNode(node);
            }
        }
        catch(Exception ex)
        {
            AtmoswitchMod.sLogger.Error($"Could not refresh: {ex.Message}");
        }
    }
}

/*
 try
{
    var vessel = KSP.Game.GameManager.Instance.Game.ViewController.GetActiveSimVessel();
    var telemetry = Traverse.Create(vessel).Field("_telemetryComponent").GetValue<TelemetryComponent>();
    telemetry.RefreshCommNetNode();
    AtmoswitchMod.sLogger.Info("Refreshed CommNet!");
}
catch
{
    AtmoswitchMod.sLogger.Info("Not loaded yet");
}
 


var vessel = KSP.Game.GameManager.Instance.Game.ViewController.GetActiveSimVessel();
var owner = vessel.SimulationObject.FindComponent<KSP.Sim.impl.PartOwnerComponent>();
var globalId = owner.SimulationObject.GlobalId;
var modules = new List<KSP.Sim.impl.PartComponentModule_Command>();
owner.GetPartModules<KSP.Sim.impl.PartComponentModule_Command>(modules);
var comManager = KSP.Game.GameManager.Instance.Game.SessionManager.CommNetManager;
for (int i = 0; i < modules.Count; i++)
{
    KSP.Sim.impl.PartComponentModule_Command partComponentModule_Command = modules[i];
    KSP.Modules.Data_Command data_Command;
    if (partComponentModule_Command.DataModules.TryGetByType<KSP.Modules.Data_Command>(out data_Command))
    {
        KSP.Sim.Definitions.CommandControlState value = data_Command.controlStatus.GetValue();
        Log($"{i} - GlobalID: {globalId}");
        
        Log($"{i} - {Enum.GetName(typeof(KSP.Sim.Definitions.CommandControlState), value)}");

        Log($"{i} - {Enum.GetName(typeof(KSP.Sim.ConnectionNodeStatus), owner.SimulationObject.Telemetry.CommNetConnectionStatus)}");

        Log($"{i} - {Enum.GetName(typeof(KSP.Sim.ConnectionNodeStatus), comManager.GetConnectionStatus(globalId))}");

        var node = comManager._connectionGraph._allNodes.Find(n => n.Owner == globalId);
        Log($"Node1: MaxRange={node.MaxRange}, IsActive={node.IsActive}, IsControlSource={node.IsControlSource}");

        var node2 = vessel._telemetryComponent._commNetNode;
        Log($"Node2: MaxRange={node2.MaxRange}, IsActive={node2.IsActive}, IsControlSource={node2.IsControlSource}");
        

        var maxLength = vessel._telemetryComponent.GetMaxTransmitterDistanceFromParts(out bool hasActiveNode);
        node2.MaxRange = maxLength;
        node2.IsActive = hasActiveNode;
        comManager.UnregisterNode(node2);
        comManager.RegisterNode(node2);
        Log($"hasActiveNode: {hasActiveNode}");
        Log($"maxDistance: {maxLength}");
    }
}
*/