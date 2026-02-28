using REPOssessed.Cheats.Core;
using REPOssessed.Manager;
using System.Collections;
using UnityEngine;

namespace REPOssessed.Cheats.SelfTab
{
    internal class RainbowSuit : ToggleCheat, IVariableCheat<float>
    {
        private Coroutine? rainbowCoroutine;
        public static float Value = 0.1f;

        public override void OnEnable()
        {
            if (rainbowCoroutine == null) rainbowCoroutine = REPOssessed.Instance?.StartCoroutine(RainbowSuitCoroutine());
        }

        public override void OnDisable()
        {
            if (rainbowCoroutine == null) return;
            REPOssessed.Instance?.StopCoroutine(rainbowCoroutine);
            rainbowCoroutine = null;
        }

        private IEnumerator RainbowSuitCoroutine()
        {
            int colors = AssetManager.instance.playerColors.Count;
            int index = 0;
            while (true) 
            {
                PlayerAvatar? localPlayer = GameObjectManager.LocalPlayer;
                if (localPlayer == null)
                {
                    yield return new WaitForSeconds(1f); 
                    continue; 
                }
                localPlayer.PlayerAvatarSetColor(index);
                index = (index + 1) % colors;
                yield return new WaitForSeconds(Value);
            }
        }
    }
}
