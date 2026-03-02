using REPOssessed.Cheats.Core;
using REPOssessed.Handler;
using REPOssessed.Manager;
using REPOssessed.Util;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace REPOssessed.Cheats.SelfTab
{
    internal class HearAllPlayer : ToggleCheat
    {
        public override void Update()
        {
            if (!Enabled)
            {
                return;
            }
            bool game = SemiFunc.RunIsLobbyMenu();
            if (game == true)
            {
                return;
            }

            if (GameObjectManager.players == null || GameObjectManager.players.Count == 0)
            {
                return;
            }

            foreach (PlayerAvatar player in GameObjectManager.players)
            {
                if (player == null)
                {
                    continue;
                }

                bool isDead = player.Handle()?.IsDead() ?? false;

                PlayerVoiceChat? voice = player.Reflect().GetValue<PlayerVoiceChat>("voiceChat");
                if (voice == null)
                {
                    continue;
                }
                voice.ToggleMixer(true);
            }
        }
        public override void OnDisable()
        {
            foreach (PlayerAvatar player in GameObjectManager.players)
            {
                if (player == null)
                {
                    continue;
                }
                PlayerVoiceChat? voice = player.Reflect().GetValue<PlayerVoiceChat>("voiceChat");
                if (voice == null)
                {
                    continue;
                }
                bool isDead = player.Handle()?.IsDead() ?? false;
                if (isDead)
                {
                    voice.ToggleMixer(true);
                }
                else
                {
                    voice.ToggleMixer(false);
                }
            }
        }
    }
}
