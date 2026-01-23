using HarmonyLib;
using Photon.Pun;
using REPOssessed.Handler;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace REPOssessed.Manager
{
    [HarmonyPatch]
    public static class GameObjectManager
    {
        private static PlayerAvatar? localPlayer;

        public static PlayerAvatar? LocalPlayer
        {
            get
            {
                if (localPlayer == null) localPlayer = players.FirstOrDefault(p => p?.Handle()?.IsLocalPlayer() == true);
                return localPlayer;
            }
        }

        public static List<Enemy> enemies = new List<Enemy>();
        public static List<PlayerAvatar> players = new List<PlayerAvatar>();
        public static List<PhysGrabObject> items = new List<PhysGrabObject>();
        public static List<ExtractionPoint> extractions = new List<ExtractionPoint>();
        public static List<PhysGrabCart> carts = new List<PhysGrabCart>();
        public static TruckScreenText? truck;

        [HarmonyPatch(typeof(PhysGrabObject), "Awake"), HarmonyPrefix]
        public static void Awake(PhysGrabObject __instance) => items.Add(__instance);

        [HarmonyPatch(typeof(PlayerAvatar), "Awake"), HarmonyPrefix]
        public static void Awake(PlayerAvatar __instance) => players.Add(__instance);

        [HarmonyPatch(typeof(Enemy), "Awake"), HarmonyPrefix]
        public static void Awake(Enemy __instance) => enemies.Add(__instance);

        [HarmonyPatch(typeof(ExtractionPoint), "Start"), HarmonyPrefix]
        public static void Start(ExtractionPoint __instance) => extractions.Add(__instance);

        [HarmonyPatch(typeof(PhysGrabCart), "Start"), HarmonyPrefix]
        public static void Start(PhysGrabCart __instance) => carts.Add(__instance);

        [HarmonyPatch(typeof(TruckScreenText), "Start"), HarmonyPrefix]
        public static void Start(TruckScreenText __instance) => truck = __instance;

        [HarmonyPatch(typeof(PhotonNetwork), "RemoveInstantiatedGO"), HarmonyPrefix]
        public static void RemoveInstantiatedGO(GameObject go, bool localOnly)
        {
            if (go?.GetComponent<PhysGrabObject>() is { } physGrabObject && items.Contains(physGrabObject)) items.Remove(physGrabObject);
            if (go?.GetComponent<PlayerAvatar>() is { } playerAvatar && players.Contains(playerAvatar)) players.Remove(playerAvatar);
            if (go?.GetComponent<Enemy>() is { } enemy && enemies.Contains(enemy)) enemies.Remove(enemy);
            if (go?.GetComponent<ExtractionPoint>() is { } extractionPoint && extractions.Contains(extractionPoint)) extractions.Remove(extractionPoint);
            if (go?.GetComponent<PhysGrabCart>() is { } physGrabCart && carts.Contains(physGrabCart)) carts.Remove(physGrabCart);
        }

        public static void CollectObjects()
        {
            CollectObjects(enemies);
            CollectObjects(players);
            CollectObjects(items);
            CollectObjects(extractions);
            CollectObjects(carts);
            truck = Object.FindAnyObjectByType<TruckScreenText>();
        }

        public static void ClearObjects()
        {
            players.Clear();
            enemies.Clear();
            items.Clear();
            extractions.Clear();
            carts.Clear();
            truck = null;
        }

        public static void CleanUpObjects()
        {
            players.RemoveAll(p => p == null);
            enemies.RemoveAll(e => e == null);
            items.RemoveAll(i => i == null);
            extractions.RemoveAll(e => e == null);
            carts.RemoveAll(c => c == null);
        }

        private static void CollectObjects<T>(List<T> list, Func<T, bool>? filter = null) where T : MonoBehaviour
        {
            list.Clear();
            list.AddRange(filter == null ? Object.FindObjectsOfType<T>() : Object.FindObjectsOfType<T>().Where(filter));
        }
    }
}
