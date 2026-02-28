using REPOssessed.Cheats.Core;
using UnityEngine;

namespace REPOssessed.Cheats.Executable
{
    internal class UnloadMenu : ExecutableCheat
    {
        public UnloadMenu() : base(KeyCode.Pause) { }

        public override void Execute()
        {
            REPOssessed.Instance?.Unload();
        }
    }
}
