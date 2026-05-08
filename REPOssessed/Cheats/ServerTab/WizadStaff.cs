using HarmonyLib;
using Photon.Pun;
using REPOssessed.Cheats.Core;
using REPOssessed.Handler;
using REPOssessed.Manager;
using REPOssessed.Util;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace REPOssessed.Cheats.ServerTab
{
    //[HarmonyPatch]
    internal class WizardStaff : ExecutableCheat
    {
        public static float time = 5f;
        public override void Execute()
        {
            if (WizardHeld())
            {
                ValuableWizardStaff? Staff = GameObjectManager.LocalPlayer?.Handle()?.GetHeldPhysGrabObject()?.GetComponentInParent<ValuableWizardStaff>();
                if (SemiFunc.IsMultiplayer())
                {
                    Staff?.Reflect()?.GetValue<PhotonView>("photonView")?.RPC("StaffLaserRPC", RpcTarget.All, time);
                }
                else
                {
                    Staff?.StaffLaserRPC(time);
                }
                return;
            }
            IEnumerable<ValuableWizardStaff> enumerable = from obj in Object.FindObjectsOfType<ValuableWizardStaff>()
                                                 select obj;
            foreach (ValuableWizardStaff Staff in enumerable)
            {
                if (SemiFunc.IsMultiplayer())
                {
                    Staff.Reflect()?.GetValue<PhotonView>("photonView")?.RPC("StaffLaserRPC", RpcTarget.All, time);
                }
                else
                {
                    Staff.StaffLaserRPC(time);
                }
            }
        }
        public static bool WizardHeld() => GameObjectManager.LocalPlayer?.Handle()?.GetHeldPhysGrabObject()?.GetComponentInParent<ValuableWizardStaff>() != null;
    }
}
