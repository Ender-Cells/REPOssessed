using HarmonyLib;
using REPOssessed.Cheats.Core;
using REPOssessed.Util;
using UnityEngine;

namespace REPOssessed.Cheats
{
    [HarmonyPatch]
    internal class HearAllDeadPlayers : ToggleCheat
    {
        [HarmonyPatch(typeof(PlayerVoiceChat), "ToggleLobby"), HarmonyPostfix]
        public static void ToggleLobby(PlayerVoiceChat __instance, bool _toggle)
        {
            if (Cheat.Instance<HearAllDeadPlayers>().Enabled && _toggle) __instance.Reflect().GetValue<AudioSource>("audioSource").outputAudioMixerGroup = __instance.mixerMicrophoneSound;
        }
    }
}
