using REPOssessed.Cheats.Core;
using System.Collections;
using UnityEngine;

namespace REPOssessed.Cheats.SettingsTab
{
    internal class FPSCounter : ToggleCheat
    {
        public Coroutine? fpsCoroutine;
        public int FPS = 0;

        public override void OnEnable()
        {
            if (fpsCoroutine == null) fpsCoroutine = REPOssessed.Instance?.StartCoroutine(FPSCounterCoroutine());
        }

        public override void OnDisable()
        {
            if (fpsCoroutine == null) return;
            REPOssessed.Instance?.StopCoroutine(fpsCoroutine);
            fpsCoroutine = null;
        }

        public IEnumerator FPSCounterCoroutine()
        {
            while (true)
            {
                FPS = (int)(1.0f / Time.deltaTime);
                yield return new WaitForSeconds(1f);
            }
        }
    }
}
