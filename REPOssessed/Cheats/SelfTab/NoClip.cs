using REPOssessed.Cheats.Components;
using REPOssessed.Cheats.Core;
using System.Linq;
using UnityEngine;

namespace REPOssessed.Cheats.SelfTab
{
    internal class NoClip : ToggleCheat, IVariableCheat<float>
    {
        private KBInput? movement;

        public static float Value = 20f;

        public override void OnEnable()
        {
            if (movement == null) movement = PlayerController.instance?.gameObject?.AddComponent<KBInput>();
        }

        public override void OnDisable()
        {
            if (movement != null)
            {
                Destroy(movement);
                movement = null;
            }
            PlayerController.instance?.GetComponentsInChildren<Collider>()?.ToList()?.ForEach(c => c.enabled = true);
        }

        public override void Update()
        {
            if (!Enabled) return;
            movement?.speed = Value;
        }

        public override void FixedUpdate()
        {
            if (!Enabled) return;
            PlayerController player = PlayerController.instance;
            if (player == null) return;
            if (movement == null) movement = player.gameObject.AddComponent<KBInput>();
            player.transform.position = movement.transform.position;
            player.GetComponentsInChildren<Collider>().Where(c => c != null && c.enabled != true).ToList().ForEach(c => c.enabled = false);
            Rigidbody rb = player.rb;
            if (rb == null) return;
            rb.isKinematic = true;
            rb.useGravity = false;
        }
    }
}