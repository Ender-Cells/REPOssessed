using HarmonyLib;
using Photon.Pun;
using REPOssessed.Cheats.Core;
using Steamworks;
using UnityEngine;

namespace REPOssessed.Cheats.SelfTab
{
    [HarmonyPatch]
    internal class NameSpoofer : ToggleCheat, IVariableCheat<string>
    {
        public static string Value = SteamClient.Name;

        public override void Update()
        {
            if (!Enabled) return;
            PhotonNetwork.NickName = Value;
        }

        public override void OnDisable()
        {
            PhotonNetwork.NickName = SteamClient.Name;
        }

        [HarmonyPatch(typeof(SteamClient), "get_Name"), HarmonyPrefix]
        public static bool get_Name(ref string __result)
        {
            if (Instance<NameSpoofer>().Enabled)
            {
                __result = Value;
                return false;
            }
            return true;
        }
    }
}
