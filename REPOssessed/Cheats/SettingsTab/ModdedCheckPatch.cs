using HarmonyLib;
using Photon.Pun;
using Photon.Realtime;
using REPOssessed.Cheats.Core;
using System;
using System.Threading;
using UnityEngine.InputSystem.Utilities;

namespace REPOssessed.Cheats.SelfTab
{
    [HarmonyPatch(typeof(ModdedCheck), "IsModded")]
    internal class ModdedCheckPatch : HarmonyPatch
    {
        [HarmonyPrefix]
        public static bool Prefix(ref bool __result)
        {
            AccessTools.Field(typeof(ModdedCheck), "moddedState").SetValue(null, Enum.Parse(AccessTools.Inner(typeof(ModdedCheck), "ModdedState"), "Vanilla"));
            __result = false;
            return false;
        }
    }
}