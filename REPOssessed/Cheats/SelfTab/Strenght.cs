using HarmonyLib;
using Photon.Realtime;
using REPOssessed.Cheats.Core;
using REPOssessed.Handler;
using REPOssessed.Manager;
using REPOssessed.Util;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

namespace REPOssessed.Cheats.SelfTab
{
    internal class Strength : ToggleCheat
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
            PhysGrabber? grab = player?.physGrabber;   
            if (grab == null)
            {
                return;
            }
            Vector3 pos = grab.physGrabPointPullerPosition;
            phys?.Teleport(pos, Quaternion.identity);
        }
    }
}
