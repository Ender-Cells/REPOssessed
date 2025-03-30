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
    internal class LootManagerWindow : PopupMenu
    {
        public LootManagerWindow(int id) : base("LootManager.Title", new Rect(50f, 50f, 600f, 300f), id) { }

        private string s_search = "";
        private Vector2 scrollPos = Vector2.zero;
        private bool b_ignoreExtractionItems = false;
        private bool b_ignoreCartItems = false;

        public override void DrawContent(int windowID)
        {
            if (!REPOssessed.Instance.IsIngame)
            {
                UI.Label("General.MustBeIngame", Settings.c_error);
                GUI.DragWindow();
                return;
            }
            List<GroupedPhysGrabObject> groupedPhysGrabObject = GameObjectManager.items?.Where(i => i != null && i.Handle() != null && !i.Handle().CurrentlyHeld() && (i.Handle().IsValuable() || i.Handle().IsShopItem()) && (!b_ignoreExtractionItems || !i.Handle().IsInExtraction()) && (!b_ignoreCartItems || !i.Handle().IsInCart())).GroupBy(i => i.Handle()?.GetName()).Select(g => new GroupedPhysGrabObject { physGrabObject = g.FirstOrDefault(), Count = g.Count() }).ToList() ?? new List<GroupedPhysGrabObject>();
            UI.VerticalSpace(ref scrollPos, () =>
            {
                GUILayout.BeginHorizontal();
                UI.Textbox("General.Search", ref s_search);
                UI.Button("LootManager.TeleportAllItems", () => TeleportAll(groupedPhysGrabObject));
                UI.Checkbox("LootManager.IgnoreExtractionItems", ref b_ignoreExtractionItems);
                UI.Checkbox("LootManager.IgnoreCartsItems", ref b_ignoreCartItems);
                GUILayout.EndHorizontal();
                GUILayout.Space(20);
                UI.ButtonGrid(groupedPhysGrabObject, p => $"{p?.physGrabObject?.Handle()?.GetName()} {p.Count}x", s_search, p =>
                {
                    List<PhysGrabObject> items = GameObjectManager.items?.Where(i => i != null && i.Handle() != null && i.Handle().GetName() == p.physGrabObject.Handle().GetName()).ToList();
                    Teleport(items[Random.Range(0, items.Count)]);
                }, 3);        
            });
            GUI.DragWindow();
        }

        public static void TeleportAll(List<GroupedPhysGrabObject> groupedPhysGrabObject)
        {
            if (SemiFunc.MainCamera() == null || SemiFunc.MainCamera().transform == null) return;
            groupedPhysGrabObject.Where(i => i != null && i.physGrabObject != null).ToList().ForEach(i => Teleport(i.physGrabObject));
        }

        private static void Teleport(PhysGrabObject physGrabObject)
        {
            if (SemiFunc.MainCamera() == null || SemiFunc.MainCamera().transform == null || physGrabObject == null || physGrabObject.Handle() == null) return;
            physGrabObject.Handle().Teleport(SemiFunc.MainCamera().transform.position, SemiFunc.MainCamera().transform.rotation);
        }

        public class GroupedPhysGrabObject
        {
            public PhysGrabObject physGrabObject { get; set; }
            public int Count { get; set; }
        }
    }
}
