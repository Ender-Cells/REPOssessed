using Photon.Pun;
using REPOssessed.Cheats.Core;
using REPOssessed.Handler;
using REPOssessed.Manager;
using REPOssessed.Util;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace REPOssessed.Cheats.SelfTab
{
    internal class AllItemsGOD : ToggleCheat
    {
        public override void Update()
        {
            if (!Enabled) return;
            List<PhysGrabObject> items = GameObjectManager.items?.Where(i => i != null && i.Handle() is ObjectHandler h && (h.IsValuable())).ToList() ?? new List<PhysGrabObject>();
            Dictionary<string, List<PhysGrabObject>> groupedItems = items.GroupBy(i => i.Handle()?.GetName() ?? "Unknown").ToDictionary(g => g.Key, g => g.ToList());

            List<PhysGrabObject>? phys_list = items?.Where(i => i != null).ToList();
            items?.Where(i => i != null).ToList().ForEach(p => {
                var imp = p.Reflect().GetValue<PhysGrabObjectImpactDetector>("impactDetector");
                if (imp == null) return;
                float value = p.Handle().GetOriginalValue();
                if (value == p.Handle().GetValue())
                {
                    return;
                }
                if (SemiFunc.IsMultiplayer())
                {
                    imp.Reflect()?.GetValue<PhotonView>("photonView")?.RPC("HealRPC", RpcTarget.All, value, Vector3.zero);
                }
                else
                {
                    imp.Reflect().Invoke("HealLogic", value, Vector3.zero);
                }
            });
        }
    }
}
