using Newtonsoft.Json.Linq;
using Photon.Pun;
using REPOssessed.Cheats.Core;
using REPOssessed.Handler;
using REPOssessed.Manager;
using REPOssessed.Util;
using UnityEngine;

namespace REPOssessed.Cheats.SelfTab
{
    internal class GODitems : ToggleCheat
    {
        public override void Update()
        {
            if (!Enabled) return;
            PlayerAvatar? player = GameObjectManager.LocalPlayer;
            player?.Handle()?.IsLocalPlayer();
            PhysGrabObject? phys = GameObjectManager.LocalPlayer?.Handle()?.GetHeldPhysGrabObject();
            if (phys == null)
            {
                return;
            }
            PhysGrabObjectImpactDetector? imp = phys.Reflect()?.GetValue<PhysGrabObjectImpactDetector>("impactDetector");
            if (imp == null)
            {
                return;
            }
            ValuableObject? valu = imp.Reflect()?.GetValue<ValuableObject>("valuableObject");
            if (valu == null)
            {
                return;
            }
            PhysGrabber? grab = player?.physGrabber;
            if (grab == null)
            {
                return;
            }
            //if (SemiFunc.IsMasterClient())
            //{
            //    phys?.Reflect()?.SetValue("hasNeverBeenGrabbed", true);
            //    return;
            //}
            float heal = valu.Reflect().GetValue<float>("dollarValueOriginal");
            if (valu.Reflect().GetValue<float>("dollarValueOriginal") == valu.Reflect().GetValue<float>("dollarValueCurrent"))
            {
                return;
            }
            if (SemiFunc.IsMultiplayer())
            {
                imp.Reflect()?.GetValue<PhotonView>("photonView")?.RPC("HealRPC", RpcTarget.All, heal, Vector3.zero);
            }
            else
            {
                imp.Reflect().Invoke("HealLogic", heal, Vector3.zero);
            }
        }
    }
}
