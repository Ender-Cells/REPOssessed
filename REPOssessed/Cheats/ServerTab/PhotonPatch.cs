//using HarmonyLib;
//using Photon.Pun;
//using Photon.Realtime;
//using REPOssessed.Cheats.Core;

//namespace REPOssessed.Cheats.SelfTab
//{
//    internal class PhotonPatch : ToggleCheat
//    {
//        [HarmonyPatch(typeof(PhotonMessageInfo), MethodType.Constructor,
//            new[] { typeof(Player), typeof(int), typeof(PhotonView) })]
//        public static class PhotonMessageInfoPatch
//        {
//            static void Prefix(ref Player player, ref int timestamp, ref PhotonView view)
//            {
//                if (!Cheat.Instance<PhotonPatch>().Enabled) return;

//                // подменяем sender на мастер-клиента
//                Player? master = PhotonNetwork.MasterClient;
//                if (master != null) player = master;
//            }
//        }
//    }
//}