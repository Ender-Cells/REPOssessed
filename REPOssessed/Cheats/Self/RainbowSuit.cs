using REPOssessed.Cheats.Core;
using REPOssessed.Handler;
using REPOssessed.Manager;
using System.Collections;
using UnityEngine;

namespace REPOssessed.Cheats
{
    internal class RainbowSuit : ToggleCheat, IVariableCheat<float>
    {
        public static float Value = 0.1f;

        public override void OnEnable()
        {
            REPOssessed.Instance.StartCoroutine(RainbowSuitStart());
        }

        private IEnumerator RainbowSuitStart()
        {
            int colors = AssetManager.instance.playerColors.Count;
            int index = 0;
            while (Enabled) 
            {
                PlayerAvatar player = GameObjectManager.LocalPlayer;
                if (player == null) yield return new WaitForSeconds(0.5f);
                player.PlayerAvatarSetColor(index);
                index = (index + 1) % colors;
                yield return new WaitForSeconds(Value);
            }
        }
    }
}
