using HarmonyLib;
using REPOssessed.Cheats.Core;
using REPOssessed.Handler;
using REPOssessed.Manager;
using REPOssessed.Util;

namespace REPOssessed.Cheats.SelfTab
{
    [HarmonyPatch]
    internal class GunBulletAmount : ToggleCheat, IVariableCheat<float>
    {
        private static ItemGun? currentGun;
        private static float OriginalNumberOfBullets = -1f;
        public static float Value = 1f;

        [HarmonyPatch(typeof(ItemGun), "Update"), HarmonyPrefix]
        public static void Update(ItemGun __instance)
        {
            if (!Instance<GunBulletAmount>().Enabled || __instance.Reflect()?.GetValue<PhysGrabObject>("physGrabObject") != GameObjectManager.LocalPlayer?.Handle()?.GetHeldPhysGrabObject())
            {
                if (currentGun != null)
                {
                    currentGun.numberOfBullets = (int)OriginalNumberOfBullets;
                    currentGun = null;
                    OriginalNumberOfBullets = -1f;
                }
                return;
            }
            currentGun = __instance;
            if (OriginalNumberOfBullets == -1f) OriginalNumberOfBullets = __instance.numberOfBullets;
            __instance.numberOfBullets = (int)Value;
        }
    }
}
