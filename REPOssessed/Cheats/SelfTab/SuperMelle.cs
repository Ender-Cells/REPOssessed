using REPOssessed.Cheats.Core;
using REPOssessed.Handler;
using REPOssessed.Manager;
using REPOssessed.Util;
using UnityEngine;

namespace REPOssessed.Cheats.SelfTab
{
    internal class SuperMelle : ToggleCheat
    {
        public override void Updade()
        {
            if (!Enabled) return;
            PhysGrabObject? phys = GameObjectManager.LocalPlayer?.Handle()?.GetHeldPhysGrabObject();
            if (phys == null) return;
            if (phys.Reflect().GetValue<bool>("isMelee") == false) return;
            ItemMelee melee = phys.GetComponent<ItemMelee>();
            melee.Reflect().SetValue("isSwinging", true);
            melee.Reflect().SetValue("newSwing", true);
        }
    }
}
