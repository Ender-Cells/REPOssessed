using REPOssessed.Cheats.Core;
using REPOssessed.Cheats.Executable;
using REPOssessed.Menu.Core;
using REPOssessed.Util;
using System;
using System.Linq;
using UnityEngine;

namespace REPOssessed.Menu.Popup
{
    internal class FirstSetupManagerWindow : PopupMenu
    {
        private int selectedLanguage = -1;
        private string[] languages;
        private bool disableButtons = false;
        private Vector2 scrollPos;

        public FirstSetupManagerWindow(int id) : base("FirstSetupManager.Title", new Rect(Screen.width / 2 - 150, Screen.height / 2 - 125f, 300f, 275f), id)
        {
            languages = TranslationUtil.GetLanguages();
            isOpen = true;
        }

        public override void DrawContent(int windowID)
        {
            GUI.enabled = !disableButtons;
            windowRect.x = Screen.width / 2 - 150;
            windowRect.y = Screen.height / 2 - 100f;
            UI.VerticalGroup(() =>
            {
                UI.Label("FirstSetupManager.Welcome");
                UI.Label("FirstSetupManager.Keybind", Cheat.Instance<ToggleMenu>().HasKeybind ? Cheat.Instance<ToggleMenu>().keybind.ToString() : KeyCode.None.ToString());
                UI.Button(Cheat.Instance<ToggleMenu>().WaitingForKeybind ? "General.Waiting" : "FirstSetupManager.ClickToChange", async () =>
                {
                    disableButtons = true;
                    await KBUtil.BeginChangeKeybind(Cheat.Instance<ToggleMenu>());
                    disableButtons = false;
                }, "");
                if (selectedLanguage == -1) selectedLanguage = Array.FindIndex(languages, x => x == Settings.Language); Settings.Language = languages[selectedLanguage];
                UI.Label("FirstSetupManager.SelectLanguage", null, false, -1, true);
                scrollPos = GUILayout.BeginScrollView(scrollPos);
                languages.ToList().ForEach(l => UI.Button(l == Settings.Language ? Settings.c_primary.AsString(l) : l, () => Settings.Language = l, ""));
                GUILayout.EndScrollView();
                GUILayout.FlexibleSpace();
                UI.Button("FirstSetupManager.Complete", () =>
                {
                    if (Cheat.Instance<ToggleMenu>().keybind == KeyCode.None) return;
                    Settings.b_IsFirstLaunch = false;
                    Settings.Config.SaveConfig();
                }, "");
            });
            GUI.enabled = true;
            GUI.DragWindow();
        }
    }
}
