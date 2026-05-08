using REPOssessed.Cheats.Core;
using REPOssessed.Manager;
using REPOssessed.Util;

namespace REPOssessed.Cheats.SelfTab
{
    internal class AntiPhysDisable : ToggleCheat
    {
        public override void Update()
        {
            if (GameObjectManager.LocalPlayer?.physGrabber?.Reflect().GetValue<float>("grabDisableTimer") >= 0)
            {
                GameObjectManager.LocalPlayer?.physGrabber?.Reflect().SetValue("grabDisableTimer", 0f);
            }
        }
    }
}
