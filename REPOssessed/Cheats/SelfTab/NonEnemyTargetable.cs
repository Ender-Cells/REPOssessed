using HarmonyLib;
using REPOssessed.Cheats.Core;
using REPOssessed.Handler;
using REPOssessed.Util;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace REPOssessed.Cheats.SelfTab
{
    [HarmonyPatch]
    internal class NonEnemyTargetable : ToggleCheat
    {
        private bool patchesApplied = false;

        public override void Update()
        {
            if (!Enabled || !GameUtil.IsMasterClient()) return;
            EnemyDirector.instance?.Reflect()?.SetValue("debugNoVision", true);
        }

        public override void OnEnable()
        {
            if (patchesApplied) return;
            patchesApplied = true;
            typeof(PlayerAvatar).Assembly.GetTypes().SelectMany(t => t.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public).Where(m => m.Name == "UpdatePlayerTarget" && m.GetParameters().Length == 1 && m.GetParameters()[0].ParameterType == typeof(PlayerAvatar))).ToList().ForEach(m => REPOssessed.Instance?.harmony?.Patch(m, new HarmonyMethod(typeof(NonEnemyTargetable).GetMethod(nameof(UpdatePlayerTarget)))));
        }

        public override void OnDisable()
        {
            if (!GameUtil.IsMasterClient()) return;
            EnemyDirector.instance?.Reflect()?.SetValue("debugNoVision", false);
        }

        public static bool UpdatePlayerTarget(object __0)
        {
            PlayerAvatar? player = __0 as PlayerAvatar;
            if (Instance<NonEnemyTargetable>().Enabled && (player?.Handle()?.IsLocalPlayer() ?? false) && GameUtil.IsMasterClient()) return false;
            return true;
        }

        [HarmonyPatch(typeof(EnemyTricycle), "IsPlayerBlockingNavmeshPath"), HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> IsPlayerBlockingNavmeshPathTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            foreach (CodeInstruction instruction in instructions)
            {
                if (instruction.opcode == OpCodes.Ldloc_S && instruction.operand is LocalBuilder localBuilder && localBuilder.LocalType == typeof(PlayerAvatar))
                {
                    yield return instruction;
                    yield return new CodeInstruction(OpCodes.Call, typeof(NonEnemyTargetable).GetMethod(nameof(newPlayerAvatar)));
                }
                else yield return instruction;
            }
        }

        [HarmonyPatch(typeof(BirthdayBoyBalloon), "OnTriggerEnter"), HarmonyPostfix]
        public static void OnTriggerEnter(BirthdayBoyBalloon __instance)
        {
            PlayerHandler? playerHandler = __instance.popper?.Handle();
            if (Instance<NonEnemyTargetable>().Enabled && playerHandler != null && playerHandler.IsLocalPlayer() && GameUtil.IsMasterClient()) __instance.popper = null;
        }

        public static PlayerAvatar? newPlayerAvatar(PlayerAvatar player)
        {
            PlayerHandler? playerHandler = player?.Handle();
            if (Instance<NonEnemyTargetable>().Enabled && playerHandler != null && playerHandler.IsLocalPlayer() && GameUtil.IsMasterClient()) return null;
            return player; 
        }

        [HarmonyPatch(typeof(SemiFunc), "PlayerGetNearestPlayerAvatarWithinRange"), HarmonyPrefix]
        public static bool PlayerGetNearestPlayerAvatarWithinRange(ref PlayerAvatar __result)
        {
            if (Instance<NonEnemyTargetable>().Enabled && (__result.Handle()?.IsLocalPlayer() ?? false))
            {
                return false;
            }
            return true;
        }
    }
}
