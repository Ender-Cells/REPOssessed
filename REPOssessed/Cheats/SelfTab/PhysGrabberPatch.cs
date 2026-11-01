using HarmonyLib;
using REPOssessed.Cheats.Core;
using REPOssessed.Util;
using System;
using System.Reflection;
using UnityEngine;

namespace REPOssessed.Cheats.SelfTab
{
    [HarmonyPatch(typeof(PhysGrabber), "RayCheck")]
    internal class GrabThroughWallsPatch : ToggleCheat
    {

        private static FieldInfo maskField;
        private static FieldInfo currentlyLookingAtPhysGrabObjectField;

        private static LayerMask originalMask;
        private static bool hasStoredOriginalMask;

        static GrabThroughWallsPatch()
        {
            maskField = AccessTools.Field(typeof(PhysGrabber), "mask");
            currentlyLookingAtPhysGrabObjectField = AccessTools.Field(typeof(PhysGrabber), "currentlyLookingAtPhysGrabObject");
        }

        [HarmonyPrefix]
        public static void Prefix(PhysGrabber __instance, bool _grab)
        {
            //bool flag = __instance.playerAvatar.Reflect().GetValue<PlayerTumble>("tumble") ? __instance.playerAvatar.Reflect().GetValue<float>("upgradeTumbleClimb") > 0 : false;
            if (!Instance<GrabThroughWallsPatch>().Enabled || !__instance.isLocal || (__instance.playerAvatar.Reflect().GetValue<PlayerTumble>("tumble").Reflect().GetValue<bool>("isTumbling") ? __instance.playerAvatar.Reflect().GetValue<float>("upgradeTumbleClimb") > 0 : false) )
            {
                return;
            }

            try
            {
                if (maskField == null)
                {
                    return;
                }

                LayerMask currentMask = (LayerMask)maskField.GetValue(__instance);

                if (!hasStoredOriginalMask)
                {
                    originalMask = currentMask;
                    hasStoredOriginalMask = true;
                }

                if (_grab)
                {
                    // Remove "Default" layer from mask (walls/obstacles)
                    int defaultLayerMask = LayerMask.GetMask("Default");
                    int newMask = currentMask.value & ~defaultLayerMask;

                    // Add grabbing layers back in
                    int grabLayers = LayerMask.GetMask("PhysGrabObject", "PhysGrabObjectCart", "PhysGrabObjectHinge", "StaticGrabObject");
                    newMask |= grabLayers;

                    maskField.SetValue(__instance, (LayerMask)newMask);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"[GrabThroughWallsPatch] Error in Prefix: {ex.Message}");
            }
        }

        [HarmonyPostfix]
        public static void Postfix(PhysGrabber __instance)
        {
            if (!Instance<GrabThroughWallsPatch>().Enabled || !__instance.isLocal)
            {
                return;
            }

            try
            {
                if (hasStoredOriginalMask && maskField != null)
                {
                    maskField.SetValue(__instance, originalMask);
                    hasStoredOriginalMask = false;
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"[GrabThroughWallsPatch] Error in Postfix: {ex.Message}");
            }
        }
    }
}
