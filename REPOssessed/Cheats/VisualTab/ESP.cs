using REPOssessed.Cheats.Core;
using REPOssessed.Handler;
using REPOssessed.Manager;
using REPOssessed.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace REPOssessed.Cheats.VisualTab
{
    internal class ESP : ToggleCheat, IVariableCheat<float>
    {
        public static float Value = 5000f;

        public override void OnGui()
        {
            if (!Enabled) return;
            try
            {
                if (Settings.b_PlayerESP) DisplayPlayers();
                if (Settings.b_ItemESP) DisplayItems();
                if (Settings.b_EnemyESP) DisplayEnemies();
                if (Settings.b_CartESP) DisplayCarts();
                if (Settings.b_ExtractionESP) DisplayExtractions();
                if (Settings.b_DeathHeadESP) DisplayDeathHeads();
                if (Settings.b_TruckESP) DisplayTruck();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        public static void ToggleAll()
        {
            Settings.b_PlayerESP = !Settings.b_PlayerESP;
            Settings.b_EnemyESP = !Settings.b_EnemyESP;
            Settings.b_ItemESP = !Settings.b_ItemESP;
            Settings.b_CartESP = !Settings.b_CartESP;
            Settings.b_ExtractionESP = !Settings.b_ExtractionESP;
            Settings.b_DeathHeadESP = !Settings.b_DeathHeadESP;
            Settings.b_TruckESP = !Settings.b_TruckESP;
        }

        private void DisplayObjects<T>(IEnumerable<T> objects, Func<T, string> labelSelector, Func<T, RGBAColor?> colorSelector) where T : Component
        {
            if (objects == null) return;
            foreach (T obj in objects.Where(o => o != null && o.gameObject.activeSelf))
            {
                if (obj.transform == null) continue;
                float distance = GetDistanceToPos(obj.transform.position);
                if (distance == 0f || distance > Value || !WorldToScreen(obj.transform.position, out Vector3 screen)) continue;
                VisualUtil.DrawString(screen, $"{labelSelector(obj)}\n{distance}m", colorSelector(obj) ?? RGBAColor.Default, true, true, false);
            }
        }

        private void DisplayPlayers()
        {
            DisplayObjects(
                GameObjectManager.players.Where(p => p?.Handle() is PlayerHandler h && !h.IsLocalPlayer() && !h.IsDead()),
                player => player.Handle() is PlayerHandler h ? $"{(h.IsTalking() ? "[VC] " : "")}{h.GetName()} ({h.GetHealth()}/{h.GetMaxHealth()})" : "Unknown",
                player => Settings.c_espPlayer
            );
        }

        private void DisplayItems()
        {
            DisplayObjects(
                GameObjectManager.items.Where(i => i != null && i.Handle() is ObjectHandler h && (!h.IsCart() && h.IsShopItem() || h.IsValuable())),
                item => item.Handle() is ObjectHandler h ? $"{h.GetName()}{(h.IsValuable() ? $" ( {h.GetValue()} )" : "")}{(h.IsTrap() ? " ( Trap )" : "")}" : "Unknown",
                item =>
                {
                    if (!Settings.b_useValuableTiers) return Settings.c_espItem;
                    int index = Array.FindLastIndex(Settings.i_valuableValueThresholds, x => x <= item.Handle()?.GetValue());
                    return index > -1 ? Settings.c_valuableValueColors[index] : Settings.c_espItem;
                }
            );
        }

        private void DisplayEnemies()
        {
            DisplayObjects(
                GameObjectManager.enemies.Where(e => e != null && e.Handle() is EnemyHandler h && !h.IsDead() && !h.IsDisabled()),
                enemy => enemy.Handle() is EnemyHandler h ? $"{h.GetName()} ({h.GetHealth()}/{h.GetMaxHealth()})" : "Unknown",
                enemy => Settings.c_espEnemy
            );
        }

        private void DisplayDeathHeads()
        {
            DisplayObjects(
                GameObjectManager.players.Select(p => p?.Handle()?.GetPlayerDeathHead()).Where(d => d != null).Cast<PlayerDeathHead>(),
                deathHead => deathHead.playerAvatar?.Handle() is PlayerHandler h ? $"{h.GetName()}'s Death Head" : "Unknown",
                deathHead => Settings.c_espDeathHead
            );
        }

        private void DisplayExtractions()
        {
            DisplayObjects(
                GameObjectManager.extractions.Where(e => e?.Handle()?.IsCompleted() ?? false),
                extraction => extraction.Handle() is ExtractionPointHandler h ? $"{(h.IsShop() ? "Shop" : $"Extraction")}" : "Unknown",
                extraction => Settings.c_espExtraction
            );           
        }

        private void DisplayCarts()
        {
            DisplayObjects(
                GameObjectManager.carts.Where(c => c != null),
                cart => cart.isSmallCart ? "Small Cart" : "Cart",
                cart => Settings.c_espCart
            );
        }

        private void DisplayTruck()
        {
            TruckScreenText? truck = GameObjectManager.truck;
            if (truck == null) return;
            DisplayObjects(
                [truck],          
                truck => "Truck",
                truck => Settings.c_espTruck
            );
        }
    }
}
