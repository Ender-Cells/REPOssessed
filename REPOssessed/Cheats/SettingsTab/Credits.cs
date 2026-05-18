using REPOssessed.Cheats.Core;


namespace REPOssessed.Cheats.SettingsTab
{
    internal class Credits : ToggleCheat
    {

        public override void OnEnable()
        {
            REPOssessed.showCR = false;
        }

        public override void OnDisable()
        {
            REPOssessed.showCR = true;
        }
    }
}
