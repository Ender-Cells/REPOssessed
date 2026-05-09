using REPOssessed.Cheats.Core;


namespace REPOssessed.Cheats.SettingsTab
{
    internal class Credits : ToggleCheat
    {
        public bool started = false;
        public bool Credit = true;

        public override void OnEnable()
        {
            Credit = true;
        }

        public override void OnDisable()
        {
            Credit = false;
        }
    }
}
