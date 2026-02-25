using REPOssessed.Handler;
using REPOssessed.Manager;
using REPOssessed.Menu.Core;
using REPOssessed.Util;
using System.Linq;
using UnityEngine;
using REPOssessed.Cheats.PlayersTab;

namespace REPOssessed.Menu.Tab
{
    internal class PlayersTab : MenuTab
    {
        public PlayersTab() : base("PlayersTab.Title") { }

        private Vector2 scrollPos = Vector2.zero;
        private Vector2 scrollPos2 = Vector2.zero;
        public static PlayerAvatar? selectedPlayer;
        private string message = "=)!";
        private string heal = "100";
        private string damage = "50";
        private string objectDamage = "1000";

        public override void Draw()
        {
            if (HackMenu.Instance == null) return;
            UI.VerticalGroup(ref scrollPos, () =>
            {
                PlayersList();
            }, GUILayout.Width(HackMenu.Instance.contentWidth * 0.3f - HackMenu.Instance.spaceFromLeft));
            UI.VerticalGroup(ref scrollPos2, () =>
            {
                GeneralActions();
                PlayerActions();
            }, GUILayout.Width(HackMenu.Instance.contentWidth * 0.7f - HackMenu.Instance.spaceFromLeft));
        }

        private void GeneralActions()
        {
            if (HackMenu.Instance == null) return;
            UI.Label("PlayersTab.GeneralActions", null, true, -1, true);

            UI.Button(["PlayersTab.ReviveAll", "General.HostTag"], () => GameObjectManager.players.Select(p => p?.Handle()).Where(h => h != null && h.IsDead()).ToList().ForEach(h => h?.RevivePlayer()));
            UI.Button(["PlayersTab.ReviveOthers", "General.HostTag"], () => GameObjectManager.players.Select(p => p?.Handle()).Where(h => h != null && h.IsDead() && !h.IsLocalPlayer()).ToList().ForEach(h => h?.RevivePlayer()));
            UI.Button(["PlayersTab.KillAll", "General.HostTag"], () => GameObjectManager.players.Select(p => p?.Handle()).Where(h => h != null).ToList().ForEach(h => h?.Kill()));
            UI.Button(["PlayersTab.KillOthers", "General.HostTag"], () => GameObjectManager.players.Select(p => p?.Handle()).Where(h => h != null && !h.IsLocalPlayer()).ToList().ForEach(h => h?.Kill()));
            UI.Textbox(["PlayersTab.ChatMessageAll", "General.HostTag"], ref message, "", 100, new UIButton("PlayersTab.Send", () => GameObjectManager.players.Select(p => p?.Handle()).Where(h => h != null).ToList().ForEach(h => h?.SendMessage(message))));
            UI.Textbox(["PlayersTab.ChatMessageOthers", "General.HostTag"], ref message, "", 100, new UIButton("PlayersTab.Send", () => GameObjectManager.players.Select(p => p?.Handle()).Where(h => h != null && !h.IsLocalPlayer()).ToList().ForEach(h => h?.SendMessage(message))));
        }

        private void PlayerActions()
        {
            if (selectedPlayer == null) return;
            PlayerHandler? selectedPlayerHandler = selectedPlayer.Handle();
            if (selectedPlayerHandler == null) return;

            ObjectHandler? objectHandler = selectedPlayerHandler.GetHeldPhysGrabObject()?.Handle();

            UI.Label("PlayersTab.PlayerActions", null, true, -1, true);

            UI.Label("PlayersTab.SteamId", selectedPlayerHandler.GetSteamID()?.ToString() ?? "0");
            UI.Label("PlayersTab.Status", selectedPlayerHandler.IsDead() ? "Dead" : "Alive");
            UI.Label("PlayersTab.Health", selectedPlayerHandler.GetHealth().ToString());
            UI.Label("PlayersTab.MaxHealth", selectedPlayerHandler.GetMaxHealth().ToString());
            UI.Label("PlayersTab.HoldingItem", objectHandler?.GetName() ?? TranslationUtil.Translate("General.None"));
            UI.Label("PlayersTab.IsMasterClient", selectedPlayerHandler.IsMasterClient().ToString());
            UI.Label("PlayersTab.Crowned", selectedPlayerHandler.IsCrowned().ToString());
            UI.Button("PlayersTab.OpenProfile", () => Application.OpenURL($"https://steamcommunity.com/profiles/{selectedPlayerHandler.GetSteamID()}"));
            UI.Button("PlayersTab.Heal", () => selectedPlayerHandler.Heal(selectedPlayerHandler.GetMaxHealth()));

            bool isDemi = DemiGod.IsPlayerDemiGod(selectedPlayerHandler);
            bool newVal = isDemi;
            UI.Checkbox("DemiGod", ref newVal);
            if (newVal != isDemi)
            {
                DemiGod.SetPlayerDemiGod(selectedPlayerHandler, newVal);
            }

            UI.Button(["PlayersTab.Crown", "General.HostTag"], () => selectedPlayerHandler.Crown());
            UI.Button(["PlayersTab.Kill", "General.HostOrLocalTag"], () => selectedPlayerHandler.Kill());
            UI.Button(["PlayersTab.Revive", "General.HostTag"], () => selectedPlayerHandler.RevivePlayer());
            UI.Button("PlayersTab.ForceTumble", () => selectedPlayerHandler.ForceTumble());
            UI.Textbox("PlayersTab.Heal", ref heal, @"[^0-9]", 3, new UIButton("General.Set", () => selectedPlayerHandler.Heal(int.Parse(heal))));
            UI.Textbox(["PlayersTab.Damage", "General.HostOrLocalTag"], ref damage, @"[^0-9]", 3, new UIButton("General.Set", () => selectedPlayerHandler.Hurt(int.Parse(damage))));
            UI.Button(["PlayersTab.BreakHeldObject"], () => objectHandler?.Break(false));
            UI.Textbox(["PlayersTab.DamageHeldObject", "General.HostTag"], ref objectDamage, @"[^0-9]", 5, new UIButton("General.Set", () => objectHandler?.Damage(int.Parse(objectDamage))));
            UI.Textbox(["PlayersTab.ChatMessage", "General.HostOrLocalTag"], ref message, "", 100, new UIButton("PlayersTab.Send", () => selectedPlayerHandler.SendMessage(message)));
            if (!selectedPlayerHandler.IsLocalPlayer())
            {
                UI.Button("PlayersTab.TeleportToPlayer", () =>
                {
                    Transform? transform = selectedPlayer.transform;
                    if (transform != null) GameObjectManager.LocalPlayer?.Handle()?.Teleport(transform.position, transform.rotation);
                }, "SelfTab.Teleport");
                UI.Button(["PlayersTab.TeleportPlayerToYou"], () =>
                {
                    Transform? transform = SemiFunc.MainCamera()?.transform;
                    if (transform != null) selectedPlayerHandler.Teleport(transform.position, transform.rotation);
                }, "SelfTab.Teleport");
                UI.Button("PlayersTab.BlockRPCs", () => selectedPlayerHandler.ToggleRPCBlock(), selectedPlayerHandler.IsRPCBlocked() ? "PlayersTab.Unblock" : "PlayersTab.Block");
                UI.Button("PlayersTab.Glitch", () => selectedPlayerHandler.glitch());
            }
        }

        private void PlayersList()
        {
            if (HackMenu.Instance == null) return;
            float width = HackMenu.Instance.contentWidth * 0.3f - HackMenu.Instance.spaceFromLeft * 2;
            float height = HackMenu.Instance.contentHeight - 20;
            GUI.Box(new Rect(0, 0, width, height), TranslationUtil.Translate("PlayersTab.PlayerList"));
            GUILayout.BeginVertical(GUILayout.Width(width), GUILayout.Height(height));
            GUILayout.Space(25);
            foreach (PlayerAvatar player in GameObjectManager.players.Where(p => p != null))
            {
                if (player == null) continue;
                PlayerHandler? playerHandler = player.Handle();
                if (playerHandler == null) continue;
                selectedPlayer ??= player;
                GUI.contentColor = selectedPlayer.GetInstanceID() == player.GetInstanceID() ? Settings.c_success.GetColor() : Settings.c_menuText.GetColor();
                if (GUILayout.Button(playerHandler.GetName(), GUI.skin.label)) selectedPlayer = player;
                GUI.contentColor = Settings.c_menuText.GetColor();
            }
            GUILayout.EndVertical();
        }
    }
}
