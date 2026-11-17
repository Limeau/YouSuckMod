using BepInEx;
using HarmonyLib;
using UnityEngine;
using AmongUs.GameOptions;
using BepInEx.Configuration;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using BepInEx.Unity.IL2CPP.Utils.Collections;
using Il2CppInterop.Runtime;
using InnerNet;
using Unity.Services.Analytics.Internal;

namespace BeanNinja;

[BepInPlugin("com.Limeau.YouSuckMod", "YouSuckMod", "1.0.1")]
[BepInProcess("Among Us.exe")]
public class Main : BasePlugin
{
    public static Main Instance;
    public static ManualLogSource Logger;
    public Harmony harmony = new Harmony("com.Limeau.YouSuckMod");
        
    public override void Load()
    {
        Instance = this;
        Logger = BepInEx.Logging.Logger.CreateLogSource("YouSuckMod");
        dispatcher = AddComponent<Dispatcher>();
                
        harmony.PatchAll();

        ConsoleManager.DetachConsole();
        Logger.LogInfo("========= You Suck! =========");
    }
    
[HarmonyPatch(typeof(PlayerBanData), nameof(PlayerBanData.BanMinutesLeft), MethodType.Getter)]
public static class DisconnectPenaltyPatch
{
    public static bool Prefix(PlayerBanData __instance, ref int __result)
    {
        if (__instance.BanPoints != 0f)
        {
            __instance.BanPoints = 0f;
            __result = 0;
            return false;
        }
        return true;
    }
}
