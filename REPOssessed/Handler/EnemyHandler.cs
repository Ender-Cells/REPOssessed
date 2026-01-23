using REPOssessed.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace REPOssessed.Handler
{
    public class EnemyHandler
    {
        public static List<Enemy> PermaKilledEnemies = new List<Enemy>();
        public static List<Enemy> NoEnemyOrbEnemies = new List<Enemy>();
        public Enemy enemy;
        public EnemyHealth? enemyHealth;
        public EnemyParent? enemyParent;
        public EnemyNavMeshAgent? enemyNavMeshAgent;
        public EnemyRigidbody? enemyRigidbody;

        public EnemyHandler(Enemy enemy)
        {
            this.enemy = enemy;
            this.enemyHealth = enemy.GetComponent<EnemyHealth>();
            this.enemyParent = enemy.GetComponentInParent<EnemyParent>();
            this.enemyNavMeshAgent = enemy.GetComponent<EnemyNavMeshAgent>();
            this.enemyRigidbody = enemy.GetComponent<EnemyRigidbody>();
        }

        public void Heal(int heal)
        {
            if (!GameUtil.IsMasterClient()) return;
            enemyHealth?.Heal(heal);
        }
        public void Hurt(int damage)
        {
            if (!GameUtil.IsMasterClient()) return;
            enemyHealth?.Hurt(damage, Vector3.zero);
        }
        public void Kill(bool noEnemyOrb)
        {
            bool spawnValuable = enemyHealth?.spawnValuable ?? false;
            if (enemy != null && !NoEnemyOrbEnemies.Contains(enemy) && noEnemyOrb && spawnValuable)
            {
                enemy.Handle()?.enemyHealth?.spawnValuable = false;
                NoEnemyOrbEnemies.Add(enemy);
            }
            Hurt(GetHealth());
        }
        public void Freeze(float time)
        {
            if (!GameUtil.IsMasterClient()) return;
            enemy.Freeze(time);
        }
        public void PermaKill()
        {
            if (!GameUtil.IsMasterClient()) return;
            Kill(false);
            if (!PermaKilledEnemies.Contains(enemy)) PermaKilledEnemies.Add(enemy);
        }
        public void Lure(Vector3 position)
        {
            if (!GameUtil.IsMasterClient()) return;
            enemyNavMeshAgent?.SetDestination(position);
        }
        public void Teleport(Vector3 position)
        {
            if (!GameUtil.IsMasterClient()) return;
            enemy.EnemyTeleported(position);
        }
        public bool IsDisabled() => (!enemyParent?.EnableObject?.activeSelf ?? false) || (!enemyParent.Reflect()?.GetValue<bool>("Spawned") ?? false);
        public bool IsDead() => enemyHealth?.Reflect().GetValue<bool>("dead") ?? false;
        public string GetName() => enemyParent?.enemyName ?? "Unknown";
        public int GetHealth() => enemyHealth?.Reflect().GetValue<int>("healthCurrent") ?? 0;
        public int GetMaxHealth() => enemyHealth?.health ?? 0;
        public PlayerAvatar? GetEnemyTarget() => enemy.Reflect()?.GetValue<PlayerAvatar>("TargetPlayerAvatar");
        public PlayerAvatar? GetPlayerGrabbing() => enemyRigidbody?.Reflect().GetValue<PlayerAvatar>("onGrabbedPlayerAvatar");
    }

    public static class EnemyHandlerExtensions
    {
        public static Dictionary<int, EnemyHandler> EnemyHandlers = new Dictionary<int, EnemyHandler>(); 
        
        public static EnemyHandler? Handle(this Enemy enemy)
        {
            if (enemy == null) return null; 
            int id = enemy.GetInstanceID(); 
            if (!EnemyHandlers.TryGetValue(id, out EnemyHandler enemyHandler))
            {
                enemyHandler = new EnemyHandler(enemy);
                EnemyHandlers[id] = enemyHandler; }
            return enemyHandler; 
        }

        public static string GetName(this EnemySetup enemySetup)
        {
            if (enemySetup == null) return "Unknown";
            PrefabRef? prefabRef = enemySetup.spawnObjects?.FirstOrDefault(p => p?.IsValid() == true && p.Prefab?.GetComponent<EnemyParent>() != null);
            EnemyParent? enemyParent = prefabRef?.Prefab?.GetComponent<EnemyParent>();
            if (enemyParent == null) return enemySetup.name ?? "Unknown";
            string name = enemyParent.enemyName ?? "Unknown";
            string? prefabName = prefabRef?.PrefabName?.Replace("Enemy -", "").Trim();
            return !string.IsNullOrEmpty(prefabName) && !string.Equals(prefabName, name, StringComparison.OrdinalIgnoreCase) ? $"{name} ({prefabName})" : name;
        }
    }
}