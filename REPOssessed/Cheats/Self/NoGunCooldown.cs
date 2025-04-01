using HarmonyLib;
using REPOssessed.Cheats.Core;

namespace REPOssessed.Cheats
{
    [HarmonyPatch]
    internal class NoGunCooldown : ToggleCheat
    {
        public static float OriginalShootCooldown = -1f;

        [HarmonyPatch(typeof(ItemGun), "Update"), HarmonyPrefix]
        public static void Update(ItemGun __instance)
        {
            if (!Cheat.Instance<NoGunCooldown>().Enabled)
            {
                if (OriginalShootCooldown != -1f)
                {
                    OriginalShootCooldown = -1f;
                    __instance.shootCooldown = (int)OriginalShootCooldown;
                }
                return;
            }
            if (OriginalShootCooldown == -1f) OriginalShootCooldown = __instance.shootCooldown;
            __instance.shootCooldown = 0f;
        }
    }
}
