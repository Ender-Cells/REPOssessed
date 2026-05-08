using Photon.Pun;
using REPOssessed.Handler;
using REPOssessed.Manager;
using REPOssessed.Menu.Core;
using REPOssessed.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace REPOssessed.Menu.Popup
{
    internal class UpgradeManagerWindow : PopupMenu
    {
        public UpgradeManagerWindow(int id) : base("UpgradeManager.Title", new Rect(50f, 50f, 600f, 300f), id) { }

        private string search = "";
        private Vector2 scrollPos = Vector2.zero;

        public override void DrawContent(int windowID)
        {
            if (!GameUtil.IsInGame())
            {
                UI.Label("General.MustBeIngame", null, false, -1, false, Settings.c_error);
                GUI.DragWindow();
                return;
            }

            List<PhysGrabObject> items = GameObjectManager.items?.Where(i => i != null && i.Handle() is ObjectHandler h && !h.CurrentlyHeld() && (h.IsUpgrade())).ToList() ?? new List<PhysGrabObject>();
            Dictionary<string, List<PhysGrabObject>> groupedItems = items.GroupBy(i => i.Handle()?.GetName() ?? "Unknown").ToDictionary(g => g.Key, g => g.ToList());
            UI.VerticalGroup(ref scrollPos, () =>
            {
                GUILayout.BeginHorizontal();
                UI.Textbox("General.Search", ref search, "", 50);
                UI.Button("UpgradeManager.UseAllUpgrades", () => items?.Where(i => i != null).ToList().ForEach(p => Use(p)));
                GUILayout.EndHorizontal();
                GUILayout.Space(20);
                UI.ButtonGrid(groupedItems.Keys.ToList(), name => name + " " + groupedItems[name].Count + "x", search, name =>
                {
                    List<PhysGrabObject> matchingItems = groupedItems[name];
                    if (matchingItems.Count == 0) return;
                    Use(matchingItems[Random.Range(0, matchingItems.Count)]);
                }, 3);
            });
            GUI.DragWindow();
        }

        private static void Use(PhysGrabObject physGrabObject)
        {
            try
            {
                if (physGrabObject == null) return;
                
                ItemUpgrade item = physGrabObject.GetComponent<ItemUpgrade>();
                if (item == null)
                {
                    item = physGrabObject.GetComponentInParent<ItemUpgrade>();
                }
                if (item == null) return;
                
                ItemToggle toggle = item.GetComponent<ItemToggle>();
                toggle.enabled = true;
                if (toggle == null)
                {
                    toggle = item.GetComponentInParent<ItemToggle>();
                }
                if (toggle == null) return;
                toggle.enabled = true;
                PhotonView photon = item.GetComponent<PhotonView>();

                int player = SemiFunc.PhotonViewIDPlayerAvatarLocal();
                if (toggle.enabled)
                {
                    if (!SemiFunc.IsMultiplayer())
                    {
                        toggle?.ToggleItem(true, player);
                    }
                    else
                    {
                        photon?.RPC("ToggleItemRPC", RpcTarget.All, true, player);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}
