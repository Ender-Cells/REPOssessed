//using HarmonyLib;
//using Photon.Pun;
//using Photon.Realtime;
//using REPOssessed.Cheats.Core;
//using REPOssessed.Handler;
//using REPOssessed.Manager;
//using REPOssessed.Util;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using UnityEngine;
////using PlayerAvater = REPOssessed.Handler.PlayerHandler;

//namespace REPOssessed.Cheats.SelfTab
//{
//    [HarmonyPatch]
//    internal class Invis : ToggleCheat
//    {
//        public override void Update()
//        {
//            if (!Instance<Invis>().Enabled) 
//            { 
//                Invi = false;
//            }
//            else
//            {
//                Invi = true;
//            }
//        }
        
//        [HarmonyPatch(typeof(PlayerAvatar), "OnPhotonSerializeView"), HarmonyPrefix]
//        private static bool OnPhotonSerializeView_Prefix(PlayerAvatar Player, PhotonStream stream)
//        {
//            //bool loc = (bool)Player.Reflect()?.GetValue("isLocal");
//            bool loc = GameObjectManager.LocalPlayer;//?.Handle() == Player.Handle();
//            if (!loc)
//            {
//                Debug.Log("Not local player, skipping serialization.");
//                return true;
//            }
//            else
//            {
//                if (stream.IsWriting)
//                {
//                    stream.SendNext(Player.Reflect()?.GetValue("isCrouching"));
//                    stream.SendNext(Player.Reflect()?.GetValue("isSprinting"));
//                    stream.SendNext(Player.Reflect()?.GetValue("isCrawling"));
//                    stream.SendNext(Player.Reflect()?.GetValue("isSliding"));
//                    stream.SendNext(Player.Reflect()?.GetValue("isMoving"));
//                    stream.SendNext(Player.Reflect()?.GetValue("isGrounded"));
//                    stream.SendNext(Player.Reflect()?.GetValue("Interact"));
//                    stream.SendNext(Player.Reflect()?.GetValue("InputDirection"));
//                    stream.SendNext(PlayerController.instance.VelocityRelative);
//                    stream.SendNext(Player.Reflect()?.GetValue("rbVelocityRaw"));
//                    if (!Invi) // проверка на то,что инвиз вкл
//                    {
//                        Debug.Log("Invis ON");
//                        stream.SendNext(PlayerController.instance.transform.position);
//                        stream.SendNext(PlayerController.instance.transform.rotation);
//                        stream.SendNext(Player.Reflect()?.GetValue("localCameraPosition"));
//                        stream.SendNext(Player.Reflect()?.GetValue("localCameraRotation"));
//                    }
//                    else
//                    {
//                        Debug.Log("Invis OFF");
//                        stream.SendNext(new Vector3(0f, 100000f, 0f));
//                        stream.SendNext(default(Quaternion));
//                        stream.SendNext(new Vector3(0f, 100000f, 0f));
//                        stream.SendNext(default(Quaternion));
//                    }
//                    stream.SendNext(PlayerController.instance.CollisionGrounded.physRiding);
//                    stream.SendNext(PlayerController.instance.CollisionGrounded.physRidingID);
//                    stream.SendNext(PlayerController.instance.CollisionGrounded.physRidingPosition);
//                    stream.SendNext(Player.flashlightLightAim.clientAimPoint);
//                    stream.SendNext(Player.Reflect()?.GetValue("playerPing"));
//                    return false;
//                }
//                else
//                {
//                    Debug.Log("stream.IsWriting isnt writing");
//                    return true;
//                }
//            }
//        return true;
//        }
       

//        static bool Invi = false;
//    }
//}
