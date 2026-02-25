// ﻿using HarmonyLib;
// using REPOssessed.Cheats.Core;
// using REPOssessed.Handler;
// using REPOssessed.Manager;
// using REPOssessed.Util;

// namespace REPOssessed.Cheats.SelfTab
// {
//     [HarmonyPatch]
//     internal class UnlimitedBattery : ToggleCheat
//     {
//         [HarmonyPatch(typeof(ItemBattery), "Update"), HarmonyPrefix]
//         public static void Update(ItemBattery __instance)
//         {
//             PhysGrabObject? physGrabObject = __instance.Reflect()?.GetValue<PhysGrabObject>("physGrabObject");
//             if (!Instance<UnlimitedBattery>().Enabled || GameObjectManager.LocalPlayer?.Handle()?.GetHeldPhysGrabObject() != physGrabObject) return;
//             ObjectHandler? objectHandler = physGrabObject?.Handle();
//             objectHandler?.ChargeBattery(objectHandler.GetMaxBattery());
//         }
//     }
// }
