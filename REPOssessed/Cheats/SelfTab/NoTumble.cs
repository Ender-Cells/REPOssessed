using REPOssessed.Cheats.Core;
using REPOssessed.Util;

namespace REPOssessed.Cheats.SelfTab
{
    internal class NoTumble : ToggleCheat
    {
        public override void Update()
        {
            if (!Enabled) return;
            PlayerController.instance?.DebugNoTumble = true;
        }

        public override void OnDisable()
        {
            PlayerController.instance?.DebugNoTumble = false;
        }
    }
}
