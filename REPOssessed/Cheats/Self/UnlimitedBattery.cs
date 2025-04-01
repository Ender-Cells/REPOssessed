using HarmonyLib;
using REPOssessed.Cheats.Core;
using REPOssessed.Handler;
using REPOssessed.Manager;
using REPOssessed.Util;

namespace REPOssessed.Cheats
{
    [HarmonyPatch]
    internal class UnlimitedBattery : ToggleCheat
    {
        [HarmonyPatch(typeof(ItemBattery), "Update"), HarmonyPrefix]
        public static void Update(ItemBattery __instance)
        {
            if (Cheat.Instance<UnlimitedBattery>().Enabled)
            {
                PhysGrabObject physGrabObject = __instance.Reflect().GetValue<PhysGrabObject>("physGrabObject");
                if (physGrabObject != null)
                {
                    PlayerAvatar player = GameObjectManager.LocalPlayer;
                    if (player != null && player.Handle().physGrabObject != null && player.Handle().physGrabObject == physGrabObject) __instance.SetBatteryLife(100);
                }
            }
        }
    }
}
