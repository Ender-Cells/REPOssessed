using HarmonyLib;
using Photon.Pun;
using Photon.Realtime;
using REPOssessed.Cheats.Core;
using System.Threading;
using UnityEngine.InputSystem.Utilities;

namespace REPOssessed.Cheats.SelfTab
{
    internal class PhotonPatch : ToggleCheat
    {

        [HarmonyPatch(typeof(PhotonMessageInfo), MethodType.Constructor,
            new[] { typeof(Player), typeof(int), typeof(PhotonView) })]
        public static class PhotonMessageInfoPatch
        {
            private static Player? Sender;
            private static PhotonView? photonView;

            static void Prefix(ref Player player, ref int timestamp, ref PhotonView view)
            {
                if (!Cheat.Instance<PhotonPatch>().Enabled) return;

                Player? master = PhotonNetwork.MasterClient;
                if (master != null)
                Sender = master;
                int timeInt = timestamp;
                photonView = view;
            }
        }
    }
}