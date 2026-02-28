using HarmonyLib;
using REPOssessed.Cheats.Core;
using Steamworks;

namespace REPOssessed.Cheats.SelfTab
{
    [HarmonyPatch]
    internal class SteamIDSpoofer : ToggleCheat, IVariableCheat<string>
    {
        public static string Value = SteamClient.SteamId.Value.ToString();

        [HarmonyPatch(typeof(SteamClient), "get_SteamId"), HarmonyPrefix]
        public static bool get_SteamId(ref SteamId __result)
        {
            if (Instance<SteamIDSpoofer>().Enabled)
            {
                __result = ulong.Parse(Value);
                return false;
            }
            return true;
        }
    }
}
