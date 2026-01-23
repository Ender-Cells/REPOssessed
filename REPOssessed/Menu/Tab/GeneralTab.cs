using REPOssessed.Menu.Core;
using REPOssessed.Util;
using System.Linq;
using UnityEngine;
using SVersion = System.Version;

namespace REPOssessed.Menu.Tab
{
    internal class GeneralTab : MenuTab
    {
        Vector2 scrollPos = Vector2.zero;
        public GeneralTab() : base("GeneralTab.Title") { }

        public override void Draw()
        {
            UI.VerticalGroup(ref scrollPos, () =>
            {
                UI.Label(Settings.c_primary.AsString("Welcome to REPOssessed!"), null, true, 30, true); 
                GUILayout.Space(20);
                UI.Label("Developed by Dustin, receiving constant updates to better the menu!");
                GUILayout.Space(20);
                Settings.Changelog.entries.GroupBy(e => e.Version).OrderByDescending(g => SVersion.Parse(g.Key)).ToList().ForEach(g =>
                {
                    UI.Label($"v{g.Key}", null, true);
                    g.ToList().ForEach(e =>
                    {
                        UI.Label(e.Type == "DevMessage" ? $"> {e.Name} - {e.Description}" : $"> {e.Type} {e.Name} - {e.Description}");
                    });
                });
            });
        }
    }
}
