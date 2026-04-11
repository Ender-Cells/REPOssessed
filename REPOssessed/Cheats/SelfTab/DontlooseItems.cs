using HarmonyLib;
using REPOssessed.Cheats.Core;

namespace REPOssessed.Cheats.SelfTab
{
    [HarmonyPatch]
    internal class DontLooseItems : ToggleCheat
    {
        [HarmonyPatch(typeof(Inventory), nameof(Inventory.ForceUnequip))]
        public static class ForceUnequipPatch
        {
            static bool Prefix()
            {
                return !Cheat.Instance<DontLooseItems>().Enabled;
            }
        }
    }
}