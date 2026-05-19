using Photon.Pun;
using REPOssessed.Cheats.Core;
using REPOssessed.Handler;
using REPOssessed.Manager;
using REPOssessed.Util;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace REPOssessed.Cheats.PlayersTab
{
    internal class TubmleUPG : ToggleCheat
    {
        public PlayerAvatar? playerUPG = null;
        public override void Update()
        {
            if (!Enabled)
            {
                playerUPG = null;
                return;
            }
            if (playerUPG != null)
            {
                Appy(playerUPG);
            }
        }
        public static void Appy(PlayerAvatar player)
        {
            int count = 0;
            int id = player.photonView.ViewID;
            List<ItemUpgrade>? upg = GameObjectManager.items?.Where(i => i?.Handle() is ObjectHandler h && h.IsUpgrade()).Select(i => i.GetComponentInParent<ItemUpgrade>()).Where(upg => upg != null && upg.GetComponentInParent<ItemUpgradePlayerTumbleLaunch>() != null).ToList();
            while (upg != null && upg.Count > 0 && count<=1000)
            {
                foreach (ItemUpgrade item in upg)
                {
                    item.GetComponent<PhotonView>().RPC("ToggleItemRPC", RpcTarget.All, true, id);
                    count++;
                }
            }
            if (count > 1000)
            {
                Cheat.Instance<TubmleUPG>().Enabled = false;
            }
        }
    }
}
