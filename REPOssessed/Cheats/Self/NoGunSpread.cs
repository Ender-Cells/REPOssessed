using HarmonyLib;
using REPOssessed.Cheats.Core;

namespace REPOssessed.Cheats
{
    [HarmonyPatch]
    internal class NoGunSpread : ToggleCheat
    {
        public static float OriginalGunRandomSpread = -1f;

        [HarmonyPatch(typeof(ItemGun), "Update"), HarmonyPrefix]
        public static void Update(ItemGun __instance)
        {
            if (!Cheat.Instance<NoGunSpread>().Enabled)
            {
                if (OriginalGunRandomSpread != -1f)
                {
                    OriginalGunRandomSpread = -1f;
                    __instance.gunRandomSpread = (int)OriginalGunRandomSpread;
                }
                return;
            }
            if (OriginalGunRandomSpread == -1f) OriginalGunRandomSpread = __instance.gunRandomSpread;
            __instance.gunRandomSpread = 0f;
        }
    }
}
