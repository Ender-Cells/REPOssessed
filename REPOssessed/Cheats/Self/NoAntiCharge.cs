using REPOssessed.Cheats.Core;
using REPOssessed.Manager;
using REPOssessed.Util;

namespace REPOssessed.Cheats
{
    internal class NoAntiCharge : ToggleCheat
    {
        public override void Update()
        {
            if (!Enabled) return;
            PlayerAvatar playerAvatar = GameObjectManager.LocalPlayer;
            if (playerAvatar == null || playerAvatar.physGrabber == null) return;
            playerAvatar.physGrabber.Reflect().SetValue("physGrabBeamOverChargeFloat", 0f);
        }
    }
}
