using REPOssessed.Cheats.Core;
//using REPOssessed.Cheats.REPOGambling;
using REPOssessed.Menu.Core;
using REPOssessed.Util;
using System.Linq;
using UnityEngine;

namespace REPOssessed.Menu.Tab
{
    internal class REPOGabling : MenuTab
    {
        public REPOGabling() : base("Gambling.Title") { }
        private Vector2 scrollPos = Vector2.zero;
        private Vector2 scrollPos2 = Vector2.zero;

        public override void Draw()
        {
            WheelContent();
            SlotContent();
        }

        public void WheelContent()
        {
            if (HackMenu.Instance == null) return;
            UI.VerticalGroup(ref scrollPos, () =>
            {
                //UI.Label("Gambling.Wheel", null, true, -1, true);
                //UI.Checkbox("Gambling.Jackpot_auto", Cheat.Instance<Jackpot_auto>());
                //UI.Textbox("Gambling.User", ref Jackpot_auto.Id_string, @"[^0-9]", 100);
                //UI.Select("Gambling.JackpotPrize", ref SettingsTab.i_prizeIndex, Jackpot_auto.prizes.Select(x => new UIOption(x, () => Jackpot_auto.prize = x)).ToArray());


            }, GUILayout.Width(HackMenu.Instance.contentWidth * 0.5f - HackMenu.Instance.spaceFromLeft));
        }

        public void SlotContent()
        {
            if (HackMenu.Instance == null) return;
            UI.VerticalGroup(ref scrollPos2, () =>
            {
                //UI.Label("Gambling.Roulette", null, true, -1, true);
                //UI.Textbox("Gambling.User", ref SlotMachine.Id_string, @"[^0-9]", 100);
                //UI.Button("Gambling.Use", () => Cheat.Instance<SlotMachine>().Execute());

            }, GUILayout.Width(HackMenu.Instance.contentWidth * 0.5f - HackMenu.Instance.spaceFromLeft));
        }
    }
}
