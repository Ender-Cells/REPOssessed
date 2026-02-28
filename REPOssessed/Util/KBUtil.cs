using REPOssessed.Cheats.Core;
using System;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace REPOssessed.Util
{
    internal class KBUtil
    {
        internal class KBCallback
        {
            private Cheat cheat;

            public KBCallback(Cheat cheat)
            {
                this.cheat = cheat;
            }

            public async void Invoke(KeyCode key)
            {
                cheat.keybind = key;
                await Task.Delay(100);
                cheat.WaitingForKeybind = false;
                Settings.Config.SaveConfig();
            }

        }

        private static readonly KeyCode[] KeyCodeBlackList = new KeyCode[]
        {
            KeyCode.W,
            KeyCode.A,
            KeyCode.S,
            KeyCode.D,
            KeyCode.Space,
            KeyCode.LeftControl,
            KeyCode.LeftShift
        };

        public static async Task BeginChangeKeybind(Cheat cheat)
        {
            if (Cheat.instances.Where(c => c.WaitingForKeybind).Count() > 0) return;
            cheat.WaitingForKeybind = true;
            cheat.keybind = await WaitForKey();
            cheat.WaitingForKeybind = false;

        }
        private static async Task<KeyCode> WaitForKey()
        {
            float startTime = Time.time;
            KeyCode key = KeyCode.None;
            while (key == KeyCode.None && Time.time - startTime < 15f)
            {
                KeyCode pressed = GetPressedKey();
                if (pressed != KeyCode.None && !KeyCodeBlackList.Contains(pressed)) key = pressed;
                await Task.Yield(); 
            }
            return key;
        }

        private static KeyCode GetPressedKey()
        {
            return Enum.GetValues(typeof(KeyCode)).Cast<KeyCode>().FirstOrDefault(k => Input.GetKeyDown(k));
        }
    }
}
