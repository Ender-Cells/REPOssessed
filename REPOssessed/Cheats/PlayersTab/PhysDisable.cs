using REPOssessed.Cheats.Core;
using REPOssessed.Handler;
using REPOssessed.Manager;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace REPOssessed.Cheats.PlayersTab
{
    internal class PhysDisable : ToggleCheat
    {
        private static readonly HashSet<string> PhysDisablePlayers = new();
        private static readonly Dictionary<string, float> cooldowns = new();
        private const float COOLDOWN = 0.9f;

        public static bool IsPlayerPhys(PlayerHandler? handler)
        {
            string? steam = handler?.GetSteamID();
            return !string.IsNullOrEmpty(steam) && PhysDisablePlayers.Contains(steam);
        }

        public static void SetPlayerPhys(PlayerHandler? handler, bool enable)
        {
            string? steam = handler?.GetSteamID();
            if (string.IsNullOrEmpty(steam)) return;
            if (enable) PhysDisablePlayers.Add(steam);
            else { PhysDisablePlayers.Remove(steam); cooldowns.Remove(steam); }
        }

        public override void Update()
        {
            if (PhysDisablePlayers.Count == 0) return;

            var activeSteamIds = GameObjectManager.players
                .Select(p => p?.Handle()?.GetSteamID())
                .Where(s => !string.IsNullOrEmpty(s))
                .ToHashSet();
            PhysDisablePlayers.RemoveWhere(s => !activeSteamIds.Contains(s));

            foreach (var p in GameObjectManager.players)
            {
                var handler = p?.Handle();
                if (!IsPlayerPhys(handler)) continue;

                if (handler!.IsDead()) continue;

                string steam = handler!.GetSteamID()!;
                if (cooldowns.TryGetValue(steam, out float next) && Time.time < next) continue;

                cooldowns[steam] = Time.time + COOLDOWN;
                handler.PhysDisable();
            }
        }
    }
}