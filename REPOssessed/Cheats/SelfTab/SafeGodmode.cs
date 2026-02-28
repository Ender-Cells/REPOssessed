using HarmonyLib;
using REPOssessed.Cheats.Core;
using REPOssessed.Handler;
using REPOssessed.Manager;

namespace REPOssessed.Cheats.SelfTab
{
    [HarmonyPatch]
    internal class SafeGodmode : ToggleCheat
    {
        [HarmonyPatch(typeof(PlayerHealth), "Hurt"), HarmonyPrefix]
        public static bool Hurt(PlayerHealth __instance, int damage, bool savingGrace, int enemyIndex = -1)
        {
            if (Instance<SafeGodmode>().Enabled)
            {
                PlayerAvatar? localPlayer = GameObjectManager.LocalPlayer;
                if (localPlayer?.playerHealth == __instance && localPlayer?.Handle()?.GetHealth() - damage <= 0) return false;
            }
            return true;
        }
    }
}