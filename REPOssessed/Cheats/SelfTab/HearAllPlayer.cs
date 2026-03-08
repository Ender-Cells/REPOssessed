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
            if (SemiFunc.RunIsLobbyMenu())
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
                PlayerDeathHead head = player.playerDeathHead;
                bool isDead = player.Handle()?.IsDead() ?? false;
                if (isDead)
                {
                    if (head.Reflect().GetValue<bool>("spectated"))
                    {
                        voice.ToggleMixer(false);
                    }
                    else
                    {
                        voice.ToggleMixer(true);
                    }
                }
                else
                {
                    if (SemiFunc.RunIsLobbyMenu())
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
}
