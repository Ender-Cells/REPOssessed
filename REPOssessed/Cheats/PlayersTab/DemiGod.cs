using REPOssessed.Cheats.Core;
using REPOssessed.Handler;
using REPOssessed.Manager;
using REPOssessed.Util;
using REPOssessed.Menu.Tab;
using System.Collections.Generic;
using System.Linq;

namespace REPOssessed.Cheats.PlayersTab
{
    internal class DemiGod : ToggleCheat
    {
        // Список steamID игроков, для которых включён DemiGod (запоминаем между переключениями)
        private static HashSet<string> DemiGodPlayers = new HashSet<string>();

        public static bool IsPlayerDemiGod(PlayerHandler? handler)
        {
            if (handler == null) return false;
            string? steam = handler.GetSteamID();
            return !string.IsNullOrEmpty(steam) && DemiGodPlayers.Contains(steam);
        }

        public static void SetPlayerDemiGod(PlayerHandler? handler, bool enable)
        {
            if (handler == null) return;
            string? steam = handler.GetSteamID();
            if (string.IsNullOrEmpty(steam)) return;
            if (enable) DemiGodPlayers.Add(steam);
            else DemiGodPlayers.Remove(steam);
        }

        public static void TogglePlayerDemiGod(PlayerHandler? handler)
        {
            if (handler == null) return;
            string? steam = handler.GetSteamID();
            if (string.IsNullOrEmpty(steam)) return;
            if (DemiGodPlayers.Contains(steam)) DemiGodPlayers.Remove(steam);
            else DemiGodPlayers.Add(steam);
        }

        // Выполняется в основном игровом цикле — подлечиваем всех "запомненных" игроков
        public override void Update()
        {
            // если никто не в списке — не тратим цикл
            if (DemiGodPlayers.Count == 0) return;

            // Очистка списка от ушедших/исчезнувших игроков:
            var currentSteam = GameObjectManager.players
                .Where(p => p != null)
                .Select(p => p.Handle())
                .Where(h => h != null)
                .Select(h => h.GetSteamID())
                .Where(s => !string.IsNullOrEmpty(s))
                .ToHashSet();

            // Удаляем те steamID, которых уже нет в текущем списке игроков
            DemiGodPlayers.RemoveWhere(s => !currentSteam.Contains(s));

            // Если после очистки список опустел — выходим
            if (DemiGodPlayers.Count == 0) return;

            // Подлечиваем всех игроков, чей steamID в множестве
            foreach (PlayerAvatar? p in GameObjectManager.players.Where(x => x != null))
            {
                if (p == null) continue;
                PlayerHandler? handler = p.Handle();
                if (handler == null) continue;
                string? steam = handler.GetSteamID();
                if (string.IsNullOrEmpty(steam)) continue;
                if (!DemiGodPlayers.Contains(steam)) continue;

                int missing = handler.GetMaxHealth() - handler.GetHealth();
                if (missing > 0) handler.Heal(missing);
            }
        }
    }
}
