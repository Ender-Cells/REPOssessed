using HarmonyLib;
using System;

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