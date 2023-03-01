using KSP.Sim.impl;
using HarmonyLib;
using KSP.Dev;
using KSP.Sim.Definitions;

namespace Atmoswitch;

[HarmonyPatch(typeof(ViewController), nameof(ViewController.CanObserverLeaveTheActiveVessel))]
public class ViewController_SetNextVessel
{
    public static bool Prefix(ref bool __result, ref bool ____canObserverLeaveActiveVessel)
    {
        __result = true;
        ____canObserverLeaveActiveVessel = true;
        return false;
    }
}