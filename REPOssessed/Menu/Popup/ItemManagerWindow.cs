using Photon.Pun;
using REPOssessed.Handler;
using REPOssessed.Menu.Core;
using REPOssessed.Util;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace REPOssessed.Menu.Popup
{
    internal class ItemManagerWindow : PopupMenu
    {
        public ItemManagerWindow(int id) : base("ItemManager.Title", new Rect(50f, 50f, 600f, 300f), id) { }

        private Vector2 scrollPos = Vector2.zero;
        private string search = "";
        private string amount = "1";
        private string value = "1000";

        private Dictionary<GameObject, string>? Items;

        public override void DrawContent(int windowID)
        { 
            UI.VerticalGroup(ref scrollPos, () =>
            {
                if (!GameUtil.IsMasterClient())
                {
                    UI.Label("General.HostRequired", null, false, -1, false, Settings.c_error);
                    GUI.DragWindow();
                    return;
                }
                if (!GameUtil.IsInGame())
                {
                    UI.Label("General.MustBeIngame", null, false, -1, false, Settings.c_error);
                    GUI.DragWindow();
                    return;
                }

                GUILayout.BeginHorizontal();
                UI.Textbox("General.Search", ref search, "", 50);
                UI.Textbox("ItemManager.Amount", ref amount, @"[^0-9]", 3);
                UI.Textbox("ItemManager.Value", ref value, @"[^0-9]", 6);
                GUILayout.EndHorizontal();
                GUILayout.Space(20);
                if (Items == null) Items = GameUtil.GetAllItems();
                UI.ButtonGrid(Items.Where(i => i.Key != null && !string.IsNullOrEmpty(i.Value)).ToList(), (i) => i.Key?.GetComponent<PhysGrabObject>()?.Handle()?.GetName() ?? "Unknown", search, (i) =>
                {
                    Transform? cameraTransform = SemiFunc.MainCamera()?.transform;
                    if (cameraTransform == null) return;
                    string path = i.Value;
                    GameObject item = i.Key;
                    string resourcePath = ValuableDirector.instance.Reflect().GetValue<string>("resourcePath")?.Replace("/", "") ?? "";
                    if (string.IsNullOrEmpty(resourcePath)) return;
                    for (int j = 0; j < int.Parse(amount); j++)
                    {
                        GameObject? spawnedItem = null;
                        if (GameManager.Multiplayer())
                        {
                            string spawnPath = path switch
                            {
                                "shop" => $"Items/{item.name}",
                                "surplus" => $"{resourcePath}/{item.name}",
                                "enemy" => $"{resourcePath}/{item.name}",
                                _ => $"{resourcePath}/{path}/{item.name}"
                            };
                            if (!string.IsNullOrEmpty(spawnPath)) spawnedItem = PhotonNetwork.InstantiateRoomObject(spawnPath, cameraTransform.position, default);
                        }
                        else spawnedItem = Object.Instantiate(item, cameraTransform.position, default);
                        spawnedItem?.GetComponent<PhysGrabObject>()?.Handle()?.SetValue(int.Parse(value));
                    }
                }, 3);
            });
            GUI.DragWindow();
        }
    }
}