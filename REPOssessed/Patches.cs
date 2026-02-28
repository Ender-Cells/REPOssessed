using ExitGames.Client.Photon;
using HarmonyLib;
using Photon.Pun;
using Photon.Realtime;
using REPOssessed.Handler;
using REPOssessed.Manager;
using REPOssessed.Util;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace REPOssessed
{
    [HarmonyPatch]
    internal class Patches
    {
        internal static readonly object keyByteZero = (object)(byte)0;
        internal static readonly object keyByteOne = (object)(byte)1;
        internal static readonly object keyByteTwo = (object)(byte)2;
        internal static readonly object keyByteThree = (object)(byte)3;
        internal static readonly object keyByteFour = (object)(byte)4;
        internal static readonly object keyByteFive = (object)(byte)5;
        internal static readonly object keyByteSix = (object)(byte)6;
        internal static readonly object keyByteSeven = (object)(byte)7;
        internal static readonly object keyByteEight = (object)(byte)8;

        public static List<string> IgnoredRPCDebugs = new List<string>
        {
            "IsTalkingRPC",
            "ReceiveSyncData",
            "SetColorRPC",
        };

        [HarmonyPatch(typeof(PhotonNetwork), "ExecuteRpc"), HarmonyPrefix]
        public static bool ExecuteRPC(Hashtable rpcData, Player sender)
        {
            try
            {
                if (sender == null) return true;
                PlayerHandler? senderHandler = sender.GamePlayer()?.Handle();
                if (senderHandler == null) return true;

                string rpc = rpcData.TryGetValue(keyByteFive, out object indexObj) && Convert.ToInt32(indexObj) is int i && i >= 0 && i < PhotonNetwork.PhotonServerSettings.RpcList.Count
                    ? PhotonNetwork.PhotonServerSettings.RpcList[i]
                    : rpcData[keyByteThree] as string ?? "";

                if (!IgnoredRPCDebugs.Contains(rpc)) Debug.LogWarning($"Processing RPC {rpc} from {sender.NickName}");

                if (!sender.IsLocal && senderHandler.IsRPCBlocked())
                {
                    Debug.LogError($"RPC {rpc} was blocked from {sender.NickName}.");
                    return false;
                }

                return senderHandler.OnReceivedRPC(rpc, rpcData);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return true;
            }
        }

        [HarmonyPatch(typeof(MenuPageEsc), "ButtonEventQuitToMenu"), HarmonyPrefix]
        public static void ButtonEventQuitToMenu()
        {
            GameObjectManager.ClearObjects();
            EnemyHandler.PermaKilledEnemies.Clear();
            EnemyHandler.NoEnemyOrbEnemies.Clear();
            EnemyHandlerExtensions.EnemyHandlers.Clear();
            PlayerHandlerExtensions.PlayerHandlers.Clear();
            ObjectHandlerExtensions.ObjectHandlers.Clear();
            ExtractionPointHandlerExtensions.ExtractionPointHandlers.Clear();
        }

        [HarmonyPatch(typeof(RunManager), "UpdateLevel"), HarmonyPrefix]
        public static void UpdateLevel()
        {
            GameObjectManager.CleanUpObjects();
        }

        [HarmonyPatch(typeof(EnemyParent), "SpawnRPC"), HarmonyPostfix]
        public static void SpawnRPC(EnemyParent __instance)
        {
            Enemy? enemy = __instance.Reflect()?.GetValue<Enemy>("Enemy");
            if (enemy != null && EnemyHandler.PermaKilledEnemies.Contains(enemy)) enemy.Handle()?.Kill(false);
        }

        public static bool forceThiefPunishment = false;

        [HarmonyPatch(typeof(SemiFunc), "ShopGetTotalCost"), HarmonyPrefix]
        public static bool ShopGetTotalCostPrefix(ref int __result)
        {
            if (!forceThiefPunishment) return true;
            forceThiefPunishment = false;
            __result = 1;
            return false;
        }

        [HarmonyPatch(typeof(EnemyParent), "Despawn"), HarmonyPostfix]
        public static void Despawn(EnemyParent __instance)
        {
            Enemy? enemy = __instance.Reflect()?.GetValue<Enemy>("Enemy");
            if (enemy != null && EnemyHandler.NoEnemyOrbEnemies.Contains(enemy))
            {
                EnemyHandler.NoEnemyOrbEnemies.Remove(enemy);
                enemy.Handle()?.enemyHealth?.spawnValuable = true;
            }         
        }
    }
}