using Photon.Pun;
using REPOGambling;
using REPOssessed.Cheats.Core;
using REPOssessed.Manager;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace REPOssessed.Cheats.REPOGambling
{
    internal class Jackpot_auto : ToggleCheat
    {
        public static string prize = "Jackpot";
        public static string Id_string = "1";
        public static int Id_int = 0;
        public static string[] prizes = ["Jackpot", "Mystery Prize", "Lose", "Bankruptcy", "Death", "Upgrade"];
        public override void Update()
        {
            if (!Enabled) return;
            if (string.IsNullOrEmpty(Id_string))
            {
                Id_int = GameObjectManager.LocalPlayer.photonView.OwnerActorNr;
            }
            else
            {
                Id_int = int.Parse(Id_string);
            }
            IEnumerable<GameObject> enumerable = from obj in Object.FindObjectsOfType<GameObject>()
                where obj.name.Contains("Wheel Machine")
                select obj;
            foreach (GameObject wheel in enumerable)
            {

                WheelMachineState component = wheel.GetComponent<WheelMachineState>();
                PhotonView photon = wheel.GetComponent<PhotonView>();
                photon.RPC("RPC_SpinWheel", RpcTarget.All, new object[3]
                {
                    photon.ViewID,
                    prize,
                    GameObjectManager.LocalPlayer.photonView.OwnerActorNr
                });
            }
        }
    }
}
