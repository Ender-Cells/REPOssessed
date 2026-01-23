using REPOssessed.Cheats.Core;

namespace REPOssessed.Cheats.SelfTab
{
    internal class SuperSpeed : ToggleCheat, IVariableCheat<float>
    {
        private static float OriginalValue = -1f;
        public static float Value = 5f;

        public override void Update()
        {
            if (!Enabled || OriginalValue == -1f) return;
            PlayerController.instance.MoveSpeed = Value;
        }

        public override void OnEnable()
        {
            OriginalValue = PlayerController.instance.MoveSpeed;
        }

        public override void OnDisable()
        {
            PlayerController.instance.MoveSpeed = OriginalValue;
            OriginalValue = -1f;
        }
    }
}
