using REPOssessed.Cheats.Core;
using REPOssessed.Extensions;
using REPOssessed.Handler;

namespace REPOssessed.Cheats
{
    internal class UnlimitedBattery : ToggleCheat
    {
        public override void Update()
        {
            if (!Enabled) return;
            PlayerAvatar player = PlayerAvatar.instance.GetLocalPlayer();
            if (player == null || player.Handle() == null || !player.Handle().IsMasterClient()) return;
            ItemEquippable itemEquippable = player.Handle().itemEquippable;
            if (itemEquippable == null) return;
            ItemBattery itemBattery = itemEquippable.GetComponentHierarchy<ItemBattery>();
            if (itemBattery == null) return;
            itemBattery.SetBatteryLife(100);
        }
    }
}
