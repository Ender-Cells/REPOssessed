using HarmonyLib;
using Photon.Pun;
using REPOGambling;
using REPOssessed.Cheats.Core;
using REPOssessed.Manager;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace REPOssessed.Cheats.REPOGambling
{
    //[HarmonyPatch]
    internal class SlotMachine : ExecutableCheat
    {
        public static string Id_string = "1";
        public static int Id_int = 0;
        public override void Execute()
        {
            if (string.IsNullOrEmpty(Id_string))
            {
                Id_int = Random.Range(1, 37);
            }
            else
            {
                if (int.Parse(Id_string) < 38)
                {
                    Id_int = int.Parse(Id_string);
                }
                else
                {
                    Id_int = Random.Range(1, 37);
                }
            }
            IEnumerable<GameObject> enumerable = from obj in Object.FindObjectsOfType<GameObject>()
                                                 where obj.name.Contains("Roulette")
                                                 select obj;
            foreach (GameObject Roulette in enumerable)
            {
                //Roulette.GetComponent<PhotonView>().RPC("SpinRpc", RpcTarget.All, Id_int);
                Roulette.GetPhotonView().RPC("SpinRpc", RpcTarget.All, Id_int);
            }
        }
    }
}
