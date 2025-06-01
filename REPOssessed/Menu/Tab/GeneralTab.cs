using REPOssessed.Menu.Core;
using REPOssessed.Util;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace REPOssessed.Menu.Tab
{
    internal class GeneralTab : MenuTab
    {
        Vector2 scrollPos = Vector2.zero;
        public GeneralTab() : base("GeneralTab.Title") { }

        public override void Draw()
        {
            UI.VerticalSpace(ref scrollPos, () =>
            {
                UI.Header(Settings.c_primary.AsString("Welcome to REPOssessed!"), 30);
                GUILayout.Space(20);
                UI.Label("Developed by Dustin, receiving constant updates to better the menu!");
                GUILayout.Space(20);
                List<IGrouping<string, Settings.Changelog.Entry>> versions = Settings.Changelog.entries.GroupBy(e => e.Version).ToList();
                for (int i = 0; i < versions.Count; i++)
                {
                    UI.Label($"v{versions[i].Key}", bold: i == 0);
                    versions[i].ToList().ForEach(e => UI.Label($"> {e.Type} {e.Name} - {e.Description}"));
                }
            });
        }
    }
}
