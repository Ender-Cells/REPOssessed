using REPOssessed.Cheats.Core;

namespace REPOssessed.Cheats.SelfTab
{
    internal class NoOverCharge : ToggleCheat
    {
        public override void Update()
        {
            if (!Enabled) return;
            PlayerController.instance.DebugDisableOvercharge = true;
        }

        public override void OnDisable()
        {
            PlayerController.instance.DebugDisableOvercharge = false;
        }
    }
}
