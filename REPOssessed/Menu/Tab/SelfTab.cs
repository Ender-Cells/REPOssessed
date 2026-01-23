using REPOssessed.Cheats.Core;
using REPOssessed.Cheats.SelfTab;
using REPOssessed.Handler;
using REPOssessed.Manager;
using REPOssessed.Menu.Core;
using REPOssessed.Util;
using System.Linq;
using UnityEngine;

namespace REPOssessed.Menu.Tab
{
    internal class SelfTab : MenuTab
    {
        public SelfTab() : base("SelfTab.Title") { }
        private Vector2 scrollPos = Vector2.zero;
        private Vector2 scrollPos2 = Vector2.zero;

        public override void Draw()
        {
            SelfContent();
            TeleportContent();
        }

        public void SelfContent()
        {
            if (HackMenu.Instance == null) return;
            UI.VerticalGroup(ref scrollPos, () =>
            {
                UI.Label("SelfTab.Title", null, true, -1, true);
                UI.Checkbox("SelfTab.UnlimitedEnergy", Cheat.Instance<UnlimitedStamina>());
                UI.Checkbox("SelfTab.Godmode", Cheat.Instance<Godmode>());
                UI.Checkbox("SelfTab.SafeGodmode", Cheat.Instance<SafeGodmode>());
                UI.Checkbox("SelfTab.NoTumble", Cheat.Instance<NoTumble>());
                UI.Checkbox("SelfTab.InfiniteJump", Cheat.Instance<InfiniteJump>());
                UI.Checkbox(["SelfTab.UnlimitedBattery", "General.HostTag"], Cheat.Instance<UnlimitedBattery>());
                UI.Checkbox("SelfTab.NoGunSpread", Cheat.Instance<NoGunSpread>());
                UI.Checkbox("SelfTab.NoGunCooldown", Cheat.Instance<NoGunCooldown>());
                UI.ToggleSlider(Cheat.Instance<GunBulletAmount>(), "SelfTab.GunBulletAmount", GunBulletAmount.Value.ToString("F1"), ref GunBulletAmount.Value, 1, 50);
                UI.Checkbox(["SelfTab.NonEnemyTargetable", "General.HostTag"], Cheat.Instance<NonEnemyTargetable>());
                UI.Checkbox("SelfTab.AlwaysShowLevel", Cheat.Instance<AlwaysShowLevel>());
                UI.Checkbox("SelfTab.NoOverCharge", Cheat.Instance<NoOverCharge>());
                UI.ToggleSlider(Cheat.Instance<RainbowSuit>(), "SelfTab.RainbowSuit", RainbowSuit.Value.ToString("F1"), ref RainbowSuit.Value, 0.1f, 1f);
                UI.ToggleSlider(Cheat.Instance<NoClip>(), "SelfTab.NoClip", NoClip.Value.ToString("F1"), ref NoClip.Value, 1f, 50f);
                UI.ToggleSlider(Cheat.Instance<SuperSpeed>(), "SelfTab.SuperSpeed", SuperSpeed.Value.ToString("F1"), ref SuperSpeed.Value, 5f, 100f);
                UI.Checkbox("SelfTab.UnlimitedDeathHeadEnergy", Cheat.Instance<UnlimitedDeathHeadEnergy>());
                UI.Checkbox(["SelfTab.NameSpoofer", "General.UseBeforeJoinTag"], Cheat.Instance<NameSpoofer>());
                UI.Textbox("SelfTab.SpoofedName", ref NameSpoofer.Value, "", 100);
                UI.Checkbox(["SelfTab.SteamIDSpoofer", "General.UseBeforeJoinTag"], Cheat.Instance<SteamIDSpoofer>());
                UI.Textbox("SelfTab.SpoofedSteamID", ref SteamIDSpoofer.Value, @"[^0-9]", 100);
            }, GUILayout.Width(HackMenu.Instance.contentWidth * 0.5f - HackMenu.Instance.spaceFromLeft));
        }

        public void TeleportContent()
        {
            if (HackMenu.Instance == null) return;
            UI.VerticalGroup(ref scrollPos2, () =>
            {
                UI.Label("SelfTab.TeleportTitle", null, true, -1, true);
                UI.Button("SelfTab.Truck", () =>
                {
                    Transform? transform = Object.FindObjectsOfType<SpawnPoint>()?.FirstOrDefault(s => s != null)?.transform;
                    if (transform != null) GameObjectManager.LocalPlayer?.Handle()?.Teleport(transform.position, transform.rotation);
                }, "SelfTab.Teleport");
                CartsTeleportContent();
                ExtractionsTeleportContent();

            }, GUILayout.Width(HackMenu.Instance.contentWidth * 0.5f - HackMenu.Instance.spaceFromLeft));
        }

        private void CartsTeleportContent()
        {
            int Index = 1;
            string cartTranslation = TranslationUtil.Translate("SelfTab.Cart");
            GameObjectManager.carts.Where(c => c != null).ToList().ForEach(c =>
            {
                Transform transform = c.transform;
                if (transform != null) UI.Button($"{cartTranslation} {Index++}", () => GameObjectManager.LocalPlayer?.Handle()?.Teleport(transform.position, transform.rotation), "SelfTab.Teleport");
            });
        }

        private void ExtractionsTeleportContent()
        {
            int Index = 1;
            string extractionTranslation = TranslationUtil.Translate("General.Extraction");
            GameObjectManager.extractions.Where(e => e != null && e?.Handle() is ExtractionPointHandler h && !h.IsShop() && !h.IsCompleted()).ToList().ForEach(e =>
            {
                Transform transform = e.transform;
                if (transform != null) UI.Button($"{extractionTranslation} {Index++}", () => GameObjectManager.LocalPlayer?.Handle()?.Teleport(transform.position, transform.rotation), "SelfTab.Teleport");
            });
        }
    }
}
