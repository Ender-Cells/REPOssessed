using REPOssessed.Cheats.Core;
using REPOssessed.Handler;
using REPOssessed.Manager;
using System.Collections.Generic;
using System.Linq;

namespace REPOssessed.Cheats.PlayersTab
{
    internal class DemiGod : ToggleCheat
    {
        private static readonly HashSet<string> DemiGodPlayers = new();

        public static bool IsPlayerDemiGod(PlayerHandler? handler)
        {
            string? steam = handler?.GetSteamID();
            return !string.IsNullOrEmpty(steam) && DemiGodPlayers.Contains(steam);
        }

        public static void SetPlayerDemiGod(PlayerHandler? handler, bool enable)
        {
            string? steam = handler?.GetSteamID();
            if (string.IsNullOrEmpty(steam)) return;

            if (enable) DemiGodPlayers.Add(steam);
            else DemiGodPlayers.Remove(steam);
        }

        public override void Update()
        {
            if (DemiGodPlayers.Count == 0) return;

            // Удаляем игроков которых уже нет в игре
            var activeSteamIds = GameObjectManager.players.Select(p => p?.Handle()?.GetSteamID()).Where(s => !string.IsNullOrEmpty(s)).ToHashSet();
            DemiGodPlayers.RemoveWhere(s => !activeSteamIds.Contains(s));

            // Лечим всех DemiGod игроков
            foreach (var p in GameObjectManager.players)
            {
                var handler = p?.Handle();
                if (!IsPlayerDemiGod(handler)) continue;

                int missing = handler!.GetMaxHealth() - handler.GetHealth();
                if (missing > 0) handler.Heal(missing);
            }
        }
    }
}