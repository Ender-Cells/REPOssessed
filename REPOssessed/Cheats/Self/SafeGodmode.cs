using HarmonyLib;
using REPOssessed.Cheats.Core;
using REPOssessed.Handler;
using REPOssessed.Manager;

namespace REPOssessed.Cheats
{
    [HarmonyPatch]
    internal class SafeGodmode : ToggleCheat
    {
        [HarmonyPatch(typeof(PlayerHealth), "Hurt"), HarmonyPrefix]
        public static bool Hurt(PlayerHealth __instance, int damage, bool savingGrace, int enemyIndex = -1)
        {
            if (Instance<SafeGodmode>().Enabled)
            {
                PlayerAvatar player = GameObjectManager.LocalPlayer;
                if (player != null && player.playerHealth == __instance && player.Handle().GetHealth() - damage <= 0) return false;
            }
            return true;
        }
    }
}