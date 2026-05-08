//using REPOssessed.Cheats.Core;
//using REPOssessed.Cheats.SelfTab;
//using REPOssessed.Manager;
//using REPOssessed.Util;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using UnityEngine;

//namespace REPOssessed.Cheats.PlayersTab
//{
//    internal class Revive : ExecutableCheat
//    {
//        public override void Execute()
//        {
//            try
//            {
//                PlayerAvatar? localPlayer = GameObjectManager.LocalPlayer;

//                if (!localPlayer?.playerDeathHead)
//                {
//                    Debug.LogError("Tried to revive without death head...");
//                    return;
//                }

//                TutorialDirector.instance.Reflect().SetValue("playerRevived", true);
//                //if (_revivedByTruck)
//                //{
//                //    TruckHealer.instance.Heal(this);
//                //}

//                Vector3 position = localPlayer!.playerDeathHead.Reflect().GetValue<PhysGrabObject>("physGrabObject")!.centerPoint - Vector3.up * 0.25f;
//                Vector3 eulerAngles = localPlayer.playerDeathHead.Reflect().GetValue<PhysGrabObject>("physGrabObject")!.transform.eulerAngles;
//                if (SemiFunc.RunIsTutorial())
//                {
//                    position = Vector3.zero + Vector3.up * 2f - Vector3.right * 5f;
//                    localPlayer.playerDeathHead.transform.position = position;
//                }

//                //if (SemiFunc.IsMasterClientOrSingleplayer())
//                //{
//                //    localPlayer.tumble.Reflect().GetValue<PhysGrabObject>("physGrabObject").Teleport(position, base.transform.rotation);
//                //}

//                localPlayer.Reflect().SetValue("clientPositionCurrent", position);
//                localPlayer.Reflect().SetValue("clientPosition", position);
//                localPlayer.Reflect().SetValue("clientPhysRiding", false);
//                base.gameObject.SetActive(value: true);
//                localPlayer.playerAvatarVisuals.gameObject.SetActive(value: true);
//                localPlayer.playerAvatarVisuals.transform.position = position;
//                localPlayer.playerAvatarVisuals.Reflect().SetValue("visualPosition", position);
//                localPlayer.playerAvatarVisuals.Revive();
//                localPlayer.Reflect().SetValue("isDisabled", false);
//                localPlayer.playerDeathHead.Reset();
//                localPlayer.playerDeathEffects.Reset();
//                localPlayer.playerReviveEffects.Trigger();
//                localPlayer.Reflect().SetValue("deadSet", false);
//                localPlayer.Reflect().SetValue("deadTimer", localPlayer.Reflect().GetValue<float>("deadTime"));
//                //if ((bool)voiceChat)
//                //{
//                //    voiceChat.ToggleMixer(_lobby: false);
//                //}

//                localPlayer.Reflect().GetValue<PlayerAvatarCollision>("playerAvatarCollision")?.SetCrouch();
//                localPlayer.playerHealth.SetMaterialGreen();
//                if (localPlayer.Reflect().GetValue<bool>("isLocal"))
//                {
//                    localPlayer.playerHealth.HealOther(1, effect: true);
//                    localPlayer.playerTransform.position = position;
//                    localPlayer.playerTransform.parent.gameObject.SetActive(value: true);
//                    if (!SpectateCamera.instance || !SpectateCamera.instance.CheckState(SpectateCamera.State.Head))
//                    {
//                        CameraAim.Instance.SetPlayerAim(Quaternion.Euler(0f, eulerAngles.y, 0f), _setRotation: true);
//                    }

//                    CameraPosition.instance.transform.position = position;
//                    CameraAim.Instance.OverrideNoSmooth(0.25f);
//                    GameDirector.instance.Revive();
//                    SpectateCamera.instance.StopSpectate();
//                    PlayerController.instance.Revive(eulerAngles);
//                    CameraGlitch.Instance.PlayLongHeal();
//                }
//            }
//            catch (Exception ex)
//            {
//                Debug.LogWarning($"Failed to revive: {ex}");
//            }
//        }
//    }
//}
