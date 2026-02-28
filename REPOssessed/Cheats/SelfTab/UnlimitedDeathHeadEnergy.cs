using REPOssessed.Cheats.Core;
using REPOssessed.Util;

namespace REPOssessed.Cheats.SelfTab
{
    public class UnlimitedDeathHeadEnergy : ToggleCheat
    {
        public override void Update()
        {
            if (!Enabled) return;
            SpectateCamera.instance?.Reflect()?.SetValue("headEnergy", 1f);
        }
    }
}
