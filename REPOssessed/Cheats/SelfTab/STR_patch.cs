//using HarmonyLib;
//using Photon.Pun;
//using Photon.Realtime;
//using REPOssessed.Cheats.Core;
//using REPOssessed.Handler;
//using REPOssessed.Manager;
//using REPOssessed.Util;
//using System.Threading.Tasks;
//using UnityEngine;
//using UnityEngine.InputSystem.LowLevel;

//namespace REPOssessed.Cheats.SelfTab
//{
//    [HarmonyPatch]
//    internal class STR_patch : ToggleCheat
//    {
//        private static STR_patch? _instance;

//        public override void OnEnable()
//        {
//            base.OnEnable();
//            _instance = this;
//        }

//        public override void OnDisable()
//        {
//            base.OnDisable();
//            _instance = null;
//        }

//        [HarmonyPatch(typeof(PhysGrabber), "OnPhotonSerializeView"), HarmonyPrefix]
//        private static void OnPhotonSerializeView_patch(PhysGrabber __instance, PhotonStream stream, PhotonMessageInfo info)
//        {
//            // Проверяем, активен ли чит
//            if (_instance == null || !_instance.Enabled)
//            {
//                return;
//            }

//            PlayerAvatar player = GameObjectManager.LocalPlayer;
//            if (player == null)
//            {
//                return;
//            }

//            player.Handle()?.IsLocalPlayer();
//            PhysGrabber grab = player.physGrabber;
            
//            // Проверяем null перед использованием
//            if (grab == null)
//            {
//                return;
//            }

//            Vector3 PGPP = grab.physGrabPointPullerPosition;
//            byte PGBOC = grab.Reflect().GetValue<byte>("physGrabBeamOverCharge");
//            Vector3 physpull = grab.physGrabPointPullerPosition;

//            if (SemiFunc.MasterAndOwnerOnlyRPC(info, info.photonView))
//            {
//                if (stream.IsWriting)
//                {
//                    if (_instance.Enabled)
//                    {
//                        Mathf.Clamp(physpull.x * 10, physpull.y * 10, physpull.z * 10);
//                    }
//                    stream.SendNext(physpull);
//                    stream.SendNext(grab.physGrabPointPlane.position);
//                    stream.SendNext(grab.mouseTurningVelocity);
//                    stream.SendNext(grab.isRotating);
//                    stream.SendNext(grab.colorState);
//                    stream.SendNext(PGBOC);
//                }
//                else
//                {
//                    physpull = (Vector3)stream.ReceiveNext();
//                    grab.physGrabPointPuller.position = PGPP;
//                    grab.physGrabPointPlane.position = (Vector3)stream.ReceiveNext();
//                    grab.mouseTurningVelocity = (Vector3)stream.ReceiveNext();
//                    grab.isRotating = (bool)stream.ReceiveNext();
//                    grab.colorState = (int)stream.ReceiveNext();
//                    PGBOC = (byte)stream.ReceiveNext();
//                }
//            }
//        }
//    }
//}
