using REPOssessed.Cheats;
using REPOssessed.Cheats.Core;
using REPOssessed.Language;
using REPOssessed.Menu.Core;
using REPOssessed.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace REPOssessed.Menu.Tab
{
    internal class SettingsTab : MenuTab
    {
        public SettingsTab() : base("SettingsTab.Title") { }

        private Vector2 scrollPos = Vector2.zero;
        private Vector2 scrollPos2 = Vector2.zero;
        private Vector2 scrollPos3 = Vector2.zero;

        private int i_languageIndex = -1;
        private int i_themeIndex = -1;

        private string s_kbSearch = "";

        private string s_primaryColor = Settings.c_primary.GetHexCode();
        private string s_menuText = Settings.c_menuText.GetHexCode();
        private string s_espPlayer = Settings.c_espPlayer.GetHexCode();
        private string s_espItem = Settings.c_espItem.GetHexCode();
        private string s_espEnemy = Settings.c_espEnemy.GetHexCode();
        private string s_espCart = Settings.c_espCart.GetHexCode();
        private string s_espExtraction = Settings.c_espExtraction.GetHexCode();
        private string s_espDeathHead = Settings.c_espDeathHead.GetHexCode();
        private string s_espTruck = Settings.c_espTruck.GetHexCode();

        private string s_lootTierColors = string.Join(",", Array.ConvertAll(Settings.c_valuableValueColors, x => x.GetHexCode()));
        private string s_lootTiers = string.Join(",", Settings.i_valuableValueThresholds);

        public override void Draw()
        {
            if (i_languageIndex == -1) i_languageIndex = Array.IndexOf(LanguageUtil.GetLanguages(), LanguageUtil.Language.Name);
            if (i_themeIndex == -1) i_themeIndex = Array.IndexOf(ThemeUtil.GetThemes(), ThemeUtil.name);
            MenuContent();
            KeybindContent();
        }

        private void MenuContent()
        {
            UI.VerticalSpace(ref scrollPos, () =>
            {
                UI.Header("SettingsTab.Title");

                UI.Actions(
                    new UIButton("SettingsTab.ResetSettings", () => Settings.Config.RegenerateConfig()),
                    new UIButton("SettingsTab.SaveSettings", () => Settings.Config.SaveConfig()),
                    new UIButton("SettingsTab.ReloadSettings", () => Settings.Config.LoadConfig())
                );

                UI.Actions(
                    new UIButton("SettingsTab.OpenSettings", () => Settings.Config.OpenConfig())
                );

                UI.Header("SettingsTab.GeneralSettings");

                UI.Select("SettingsTab.Theme", ref i_themeIndex, ThemeUtil.GetThemes().Select(x => new UIOption(x, () => ThemeUtil.SetTheme(x))).ToArray());
                UI.Select("SettingsTab.Language", ref i_languageIndex, LanguageUtil.GetLanguages().Select(x => new UIOption(x, () => LanguageUtil.SetLanguage(x))).ToArray());

                UI.Checkbox("SettingsTab.FPSCounter", Cheat.Instance<FPSCounter>());
                UI.Checkbox("SettingsTab.DisplayREPOssessedUsers", Cheat.Instance<DisplayREPOssessedUsers>());
                UI.Toggle("SettingsTab.DebugMode", ref Settings.b_DebugMode, "General.Enable", "General.Disable", HackMenu.Instance.ToggleDebugTab);

                UI.Header("SettingsTab.Colors");
                UI.TextboxAction("SettingsTab.MenuText", ref s_menuText, @"[^0-9A-Za-z]", 8, new UIButton("General.Set", () => SetColor(ref Settings.c_menuText, s_menuText)));
                UI.TextboxAction("SettingsTab.Primary", ref s_primaryColor, @"[^0-9A-Za-z]", 8, new UIButton("General.Set", () => SetColor(ref Settings.c_primary, s_primaryColor)));
                UI.TextboxAction("SettingsTab.PlayerColor", ref s_espPlayer, @"[^0-9A-Za-z]", 8, new UIButton("General.Set", () => SetColor(ref Settings.c_espPlayer, s_espPlayer)));
                UI.TextboxAction("SettingsTab.ItemColor", ref s_espItem, @"[^0-9A-Za-z]", 8, new UIButton("General.Set", () => SetColor(ref Settings.c_espItem, s_espItem)));
                UI.TextboxAction("SettingsTab.EnemyColor", ref s_espEnemy, @"[^0-9A-Za-z]", 8, new UIButton("General.Set", () => SetColor(ref Settings.c_espEnemy, s_espEnemy)));
                UI.TextboxAction("SettingsTab.CartColor", ref s_espCart, @"[^0-9A-Za-z]", 8, new UIButton("General.Set", () => SetColor(ref Settings.c_espCart, s_espCart)));
                UI.TextboxAction("SettingsTab.ExtractionColor", ref s_espExtraction, @"[^0-9A-Za-z]", 8, new UIButton("General.Set", () => SetColor(ref Settings.c_espExtraction, s_espExtraction)));
                UI.TextboxAction("SettingsTab.DeathHeadColor", ref s_espDeathHead, @"[^0-9A-Za-z]", 8, new UIButton("General.Set", () => SetColor(ref Settings.c_espDeathHead, s_espDeathHead)));
                UI.TextboxAction("SettingsTab.TruckColor", ref s_espTruck, @"[^0-9A-Za-z]", 8, new UIButton("General.Set", () => SetColor(ref Settings.c_espTruck, s_espTruck)));
                UI.Button(["SettingsTab.TieredLootColors", $"({GetTiersColored()})"], () => EditTierColors(), "General.Set");
                UI.Textbox("SettingsTab.Tiers", ref s_lootTiers, @"[^0-9,]");
                UI.Textbox("SettingsTab.TierColors", ref s_lootTierColors, @"[^0-9A-Za-z,]");

            }, GUILayout.Width(HackMenu.Instance.contentWidth * 0.55f - HackMenu.Instance.spaceFromLeft));
        }

        private void KeybindContent()
        {
            UI.VerticalSpace(ref scrollPos3, () =>
            {
                UI.Header("SettingsTab.Keybinds");
                UI.Textbox("SettingsTab.Search", ref s_kbSearch, big: false);
                List<Cheat> cheats = Cheat.instances.FindAll(c => !c.Hidden);
                foreach (Cheat cheat in cheats)
                {
                    if (!cheat.GetType().Name.ToLower().Contains(s_kbSearch.ToLower())) continue;
                    GUILayout.BeginHorizontal();
                    UI.Label(cheat.GetType().Name);
                    GUILayout.FlexibleSpace();
                    if (cheat.HasKeybind && GUILayout.Button("-")) cheat.keybind = KeyCode.None;
                    string btnText = cheat.WaitingForKeybind ? "General.Waiting" : cheat.HasKeybind ? cheat.keybind.ToString() : "General.None".Localize();
                    if (GUILayout.Button(btnText, GUILayout.Width(85)))
                    {
                        GUI.FocusControl(null);
                        KBUtil.BeginChangeKeybind(cheat);
                    }
                    GUILayout.EndHorizontal();
                }
            }, GUILayout.Width(HackMenu.Instance.contentWidth * 0.45f - HackMenu.Instance.spaceFromLeft));
        }

        private void SetColor(ref RGBAColor color, string hexCode)
        {
            color = new RGBAColor(hexCode.PadRight(6, '0'));
        }

        private void EditTierColors()
        {
            int[] thresholds = Array.ConvertAll(s_lootTiers.Split(','), x => int.TryParse(x, out int i) ? i : 0);
            RGBAColor[] rgbaColors = Array.ConvertAll(s_lootTierColors.Split(','), x => new RGBAColor(x));
            if (thresholds.Length != rgbaColors.Length) return;
            Settings.i_valuableValueThresholds = thresholds;
            Settings.c_valuableValueColors = rgbaColors;
        }

        private string GetTiersColored()
        {
            return string.Join(",", Settings.i_valuableValueThresholds.Zip(Settings.c_valuableValueColors, (t, c) => c.AsString(t.ToString())));
        }
    }
}
