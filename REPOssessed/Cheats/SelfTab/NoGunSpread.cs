using HarmonyLib;
using REPOssessed.Cheats.Core;
using REPOssessed.Handler;
using REPOssessed.Manager;
using REPOssessed.Util;

namespace REPOssessed.Cheats.SelfTab
{
    [HarmonyPatch]
    internal class NoGunSpread : ToggleCheat, IVariableCheat<float>
    {
        private static ItemGun? currentGun;
        private static float OriginalGunRandomSpread = -1f;
        public static float Value = 1;

        [HarmonyPatch(typeof(ItemGun), "Update"), HarmonyPrefix]
        public static void Update(ItemGun __instance)
        {
            if (!Instance<NoGunSpread>().Enabled || __instance.Reflect()?.GetValue<PhysGrabObject>("physGrabObject") != GameObjectManager.LocalPlayer?.Handle()?.GetHeldPhysGrabObject())
            {
                if (currentGun != null)
                {
                    currentGun.gunRandomSpread = OriginalGunRandomSpread;
                    currentGun = null;
                    OriginalGunRandomSpread = -1f;
                }
                return;
            }
            currentGun = __instance;
            if (OriginalGunRandomSpread == -1f) OriginalGunRandomSpread = __instance.gunRandomSpread;
            __instance.gunRandomSpread = Value;
        }
    }
}
