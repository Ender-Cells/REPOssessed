using HarmonyLib;
using REPOssessed.Cheats.Core;
using REPOssessed.Handler;
using REPOssessed.Manager;
using System.Linq;

namespace REPOssessed.Cheats.ServerTab
{
    [HarmonyPatch]
    internal class ForceThiefPunishment : ExecutableCheat
    {
        public override void Execute()
        {
            GameObjectManager.extractions.FirstOrDefault(e => e?.Handle()?.IsShop() == true)?.Handle()?.ThiefPunishment();
        }
    }
}
