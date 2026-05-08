using REPOssessed.Handler;
using REPOssessed.Manager;
using REPOssessed.Menu.Core;
using REPOssessed.Util;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace REPOssessed.Menu.Popup
{
    internal class BatteryManagerWindow : PopupMenu
    {
        public BatteryManagerWindow(int id) : base("BatteryManager.Title", new Rect(50f, 50f, 600f, 300f), id) { }

        private string search = "";
        private Vector2 scrollPos = Vector2.zero;
        public static Inventory? instance;

        public override void DrawContent(int windowID)
        {
            if (!GameUtil.IsInGame())
            {
                UI.Label("General.MustBeIngame", null, false, -1, false, Settings.c_error);
                GUI.DragWindow();
                return;
            }

            List<PhysGrabObject> items = GameObjectManager.items?.Where(i => i != null && i.Handle() is ObjectHandler h && (h.HasBattery())).ToList() ?? new List<PhysGrabObject>();
            Dictionary<string, List<PhysGrabObject>> groupedItems = items.GroupBy(i => i.Handle()?.GetName() ?? "Unknown").ToDictionary(g => g.Key, g => g.ToList());
            UI.VerticalGroup(ref scrollPos, () =>
            {
                GUILayout.BeginHorizontal();
                UI.Textbox("General.Search", ref search, "", 50);
                UI.Button("BatteryManager.ChargeAllitems", () => items?.Where(i => i != null).ToList().ForEach(p => Charge(p)));
                GUILayout.EndHorizontal();
                GUILayout.Space(20);
                UI.ButtonGrid(groupedItems.Keys.ToList(), name => name + " " + groupedItems[name].Count + "x", search, name =>
                {
                    List<PhysGrabObject> matchingItems = groupedItems[name];
                    if (matchingItems.Count == 0) return;
                    Charge(matchingItems[Random.Range(0, matchingItems.Count)]);
                }, 3);
            });
            GUI.DragWindow();
        }

        private static void Charge(PhysGrabObject phys)
        {
            ItemBattery? battery = phys?.GetComponent<ItemBattery>();
            phys?.Handle()?.ChargeBattery(100);
        }
    }
}
