using REPOssessed.Cheats.Core;
using REPOssessed.Cheats.SelfTab;
using REPOssessed.Handler;
using REPOssessed.Manager;
using REPOssessed.Menu.Core;
using REPOssessed.Menu.Tab;
using REPOssessed.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace REPOssessed.Menu.Popup
{
    internal class EquipManagerWindow : PopupMenu
    {
        public EquipManagerWindow(int id) : base("EquipManager.Title", new Rect(50f, 50f, 600f, 300f), id) { }

        private string search = "";
        private Vector2 scrollPos = Vector2.zero;
        public static PlayerAvatar[] players => GameObjectManager.players.ToArray();
        public static int user = -1;


        public override void DrawContent(int windowID)
        {
            if (!GameUtil.IsInGame())
            {
                UI.Label("General.MustBeIngame", null, false, -1, false, Settings.c_error);
                GUI.DragWindow();
                return;
            }

            List<PhysGrabObject> items = GameObjectManager.items?.Where(i => i != null && i.Handle() is ObjectHandler h && (h.IsEquippable() && !h.IsEquipedByMe())).ToList() ?? new List<PhysGrabObject>();
            Dictionary<string, List<PhysGrabObject>> groupedItems = items.GroupBy(i => i.Handle()?.GetName() ?? "Unknown").ToDictionary(g => g.Key, g => g.ToList());
            UI.VerticalGroup(ref scrollPos, () =>
            {
                GUILayout.BeginHorizontal();
                UI.Textbox("General.Search", ref search, "", 50);
                UI.Button("EquipManager.EquipAllItems", () => EquipAll(items));
                UI.Button("EquipManager.UnequipAllItems", () => Unequip());
                UI.Select("UpgradeManager.User", ref SettingsTab.i_userIndex, EquipManagerWindow.players.Select(x => new UIOption(x.Reflect()?.GetValue<string>("playerName"), () => EquipManagerWindow.user = x.photonView.ViewID)).ToArray());
                GUILayout.EndHorizontal();
                GUILayout.Space(20);
                UI.ButtonGrid(groupedItems.Keys.ToList(), name => name + " " + groupedItems[name].Count + "x", search, name =>
                {
                    List<PhysGrabObject> matchingItems = groupedItems[name];
                    if (matchingItems.Count == 0) return;
                    Equip(matchingItems[Random.Range(0, matchingItems.Count)]);
                }, 3);
            });
            GUI.DragWindow();
        }

        private static void Equip(PhysGrabObject physGrabObject)
        {
    
            if(user==-1)
                user=GameObjectManager.LocalPlayer.photonView.ViewID;
            ItemEquippable? equip = physGrabObject?.GetComponent<ItemEquippable>();
            int free_slot = 0;
            if (user == SemiFunc.PhotonViewIDPlayerAvatarLocal())
            {
                free_slot = Inventory.instance.GetFirstFreeInventorySpotIndex();
                if (free_slot == -1)
                {
                    return;
                }
            }
            else
            {
                List<int?> listobj = GameObjectManager.items?.Where(i => i?.Handle() is ObjectHandler h && h.IsEquiped()).Select(i => i.GetComponent<ItemEquippable>()).Where(eq => eq != null && eq.Reflect().GetValue<int>("ownerPlayerId") == user).Select(equip => equip.Reflect()?.GetValue<int>("inventorySpotIndex")).ToList();
                while (listobj.Contains(free_slot))
                {
                    free_slot++;
                }
                if (free_slot > 2)
                {
                    return;
                }
            }

            if (equip.Reflect().GetValue<bool>("isEquipped"))
            {
                // equip?.ForceUnequip(physGrabObject.centerPoint, SemiFunc.PhotonViewIDPlayerAvatarLocal());
                equip?.ForceUnequip(SemiFunc.PlayerAvatarGetFromPhotonID(equip.Reflect().GetValue<int>("ownerPlayerId")).transform.position, equip.Reflect().GetValue<int>("ownerPlayerId"));
            }
            equip?.RequestEquip(free_slot, user);
            if(!equip.Reflect().GetValue<bool>("isEquipped"))
            {
                for (int i = 0; i <= 3; i++)
                {
                    equip?.RequestEquip(free_slot, user);
                }
            }
            if(!equip.Reflect().GetValue<bool>("isEquipped"))
                Debug.LogError($"{SemiFunc.PlayerAvatarGetFromPhotonID(user).name} dont have free slot");
        }
        private static void Unequip()
        {
            if (user == SemiFunc.PhotonViewIDPlayerAvatarLocal() && !SemiFunc.RunIsShop())
            {
                bool loose = false;
                if (Cheat.Instance<DontLooseItems>().Enabled)
                {
                    Cheat.Instance<DontLooseItems>().Enabled = false;
                    loose = true;
                }
                Inventory.instance?.ForceUnequip();
                if (loose)
                {
                    Cheat.Instance<DontLooseItems>().Enabled = true;
                }
            }
            else
            {
                GameObjectManager.items?.Where(i => i?.Handle() is ObjectHandler h && h.IsEquiped()).Select(i => i.GetComponent<ItemEquippable>()).Where(eq => eq != null && eq.Reflect().GetValue<int>("ownerPlayerId") == user).ToList().ForEach(equip => equip.ForceUnequip(equip.GetOwnerPlayerAvatar().transform.position, equip.Reflect().GetValue<int>("ownerPlayerId")));
            }
        }
        private static void EquipAll(List<PhysGrabObject> items)
        {
            if (user == -1)
                user = GameObjectManager.LocalPlayer.photonView.ViewID;

            bool isLocal = user == SemiFunc.PhotonViewIDPlayerAvatarLocal();
            HashSet<int> usedSlots = new();
            int maxSlots;

            if (isLocal)
            {
                // локальный игрок: берём реальные слоты инвентаря
                var slots = Inventory.instance?.Reflect()?.GetValue<List<InventorySpot>>("inventorySpots");
                if (slots == null)
                    return;

                maxSlots = slots.Count;
                for (int i = 0; i < maxSlots; i++)
                {
                    if (Inventory.instance.IsSpotOccupied(i))
                        usedSlots.Add(i);
                }
            }
            else
            {
                maxSlots = 3;
                var occupied = GameObjectManager.items?
                    .Where(i => i?.Handle() is ObjectHandler h && h.IsEquiped())
                    .Select(i => i.GetComponent<ItemEquippable>())
                    .Where(eq => eq != null && eq.Reflect().GetValue<int>("ownerPlayerId") == user)
                    .Select(eq => eq.Reflect().GetValue<int>("inventorySpotIndex"));

                if (occupied != null)
                {
                    foreach (int s in occupied)
                        usedSlots.Add(s);
                }
            }

            foreach (var item in items)
            {
                try
                {
                    var equip = item?.GetComponent<ItemEquippable>();
                    if (equip == null) continue;

                    bool isEquipped = equip.Reflect().GetValue<bool>("isEquipped");
                    int ownerId = equip.Reflect().GetValue<int>("ownerPlayerId");

                    // предмет уже у нужного игрока - пропускаем
                    if (isEquipped && ownerId == user)
                        continue;

                    // ищем первый свободный слот
                    int slot = 0;
                    while (slot < maxSlots && usedSlots.Contains(slot))
                        slot++;

                    if (slot >= maxSlots)
                        break; // свободных слотов больше нет

                    if (isEquipped)
                    {
                        equip?.ForceUnequip(equip.GetOwnerPlayerAvatar().transform.position, ownerId);
                    }

                    equip?.RequestEquip(slot, user);
                    usedSlots.Add(slot);
                }
                catch (Exception ex )
                {
                    Debug.LogException(ex);
                }
                
            }
        }
    }
}
