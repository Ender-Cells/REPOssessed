using HarmonyLib;
using REPOssessed.Cheats.Core;

namespace REPOssessed.Cheats
{
    [HarmonyPatch]
    internal class GunBulletAmount : ToggleCheat
    {
        public static float OriginalNumberOfBullets = -1f;
        public static float Value = 1f;

        [HarmonyPatch(typeof(ItemGun), "Update"), HarmonyPrefix]
        public static void Update(ItemGun __instance)
        {
            if (!Cheat.Instance<GunBulletAmount>().Enabled)
            {
                if (OriginalNumberOfBullets != -1f)
                {
                    OriginalNumberOfBullets = -1f;
                    __instance.numberOfBullets = (int)OriginalNumberOfBullets;
                }
                return;
            }
            if (OriginalNumberOfBullets == -1f) OriginalNumberOfBullets = __instance.numberOfBullets;
            __instance.numberOfBullets = (int)Value;
        }
    }
}
