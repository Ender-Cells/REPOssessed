using REPOssessed.Cheats.Core;
using REPOssessed.Menu.Core;
using REPOssessed.Menu.Tab;

namespace REPOssessed.Cheats.SettingsTab
{
    public class DebugMode : ToggleCheat
    {
        public override void OnEnable()
        {
            HackMenu.Instance?.tabs?.Add(new DebugTab());
        }

        public override void OnDisable()
        {
            HackMenu.Instance?.tabs?.RemoveAll(t => t is DebugTab);
        }
    }
}
