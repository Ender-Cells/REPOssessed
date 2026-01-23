using HarmonyLib;
using REPOssessed.Cheats.Core;
using REPOssessed.Handler;
using REPOssessed.Manager;
using REPOssessed.Util;

namespace REPOssessed.Cheats.SelfTab
{
    [HarmonyPatch]
    internal class NoGunCooldown : ToggleCheat
    {
        private static ItemGun? currentGun;
        private static float OriginalCooldown = -1f;

        [HarmonyPatch(typeof(ItemGun), "Update"), HarmonyPrefix]
        public static void Update(ItemGun __instance)
        {
            if (!Instance<NoGunCooldown>().Enabled || __instance.Reflect()?.GetValue<PhysGrabObject>("physGrabObject") != GameObjectManager.LocalPlayer?.Handle()?.GetHeldPhysGrabObject())
            {
                if (currentGun != null)
                {
                    currentGun.shootCooldown = OriginalCooldown;
                    currentGun = null;
                    OriginalCooldown = -1f;
                }
                return;
            }
            currentGun = __instance;
            if (OriginalCooldown == -1f) OriginalCooldown = __instance.shootCooldown;
            __instance.shootCooldown = 0f;
        }
    }
}
