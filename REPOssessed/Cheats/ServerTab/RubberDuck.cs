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
    internal class RubberDuck : ExecutableCheat
    {
        public override void Execute()
        {
            if (DuckHeld())
            {
                ItemRubberDuck? duck = GameObjectManager.LocalPlayer?.Handle()?.GetHeldPhysGrabObject()?.GetComponentInParent<ItemRubberDuck>();
                if (SemiFunc.IsMultiplayer())
                {
                    duck.Reflect()?.GetValue<PhotonView>("photonView")?.RPC("QuackRPC", RpcTarget.All);
                }
                else
                {
                    duck?.QuackRPC();
                }
                return;
            }
            IEnumerable<ItemRubberDuck> enumerable = from obj in Object.FindObjectsOfType<ItemRubberDuck>()
                                                          select obj;
            foreach (ItemRubberDuck duck in enumerable)
            {
                if (SemiFunc.IsMultiplayer())
                {
                    duck.Reflect()?.GetValue<PhotonView>("photonView")?.RPC("QuackRPC", RpcTarget.All);
                }
                else
                {
                    duck.QuackRPC();
                }
            }
        }
        public static bool DuckHeld() => GameObjectManager.LocalPlayer?.Handle()?.GetHeldPhysGrabObject()?.GetComponentInParent<ItemRubberDuck>() != null;
    }
}
