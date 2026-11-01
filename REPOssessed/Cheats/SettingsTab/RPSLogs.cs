using REPOssessed.Cheats.Core;


namespace REPOssessed.Cheats.SettingsTab
{
    internal class RPCLogs : ToggleCheat
    {

        public override void OnEnable()
        {
            Settings.b_EnableRPCLogs = true;
        }

        public override void OnDisable()
        {
            Settings.b_EnableRPCLogs = false;
        }
    }
}
