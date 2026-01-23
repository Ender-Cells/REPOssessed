using Photon.Pun;
using REPOssessed.Handler;
using REPOssessed.Manager;
using REPOssessed.Menu.Core;
using REPOssessed.Util;
using System.Linq;
using UnityEngine;

namespace REPOssessed.Menu.Tab
{
    internal class DebugTab : MenuTab
    {
        public DebugTab() : base("Debug") { }
        private Vector2 scrollPos = Vector2.zero;

        public override void Draw()
        {
            GUILayout.BeginVertical();
            MenuContent();
            GUILayout.EndVertical();
        }

        private void MenuContent()
        {
            UI.VerticalGroup(ref scrollPos, () =>
            {
                UI.Label("Is MasterClient:", GameUtil.IsMasterClient().ToString());
                UI.Button("Log RPCS", () => PhotonNetwork.PhotonServerSettings.RpcList.ToList().ForEach(r => Debug.Log(r)));

                UI.Button("Debug all prefabs", () =>
                {
                    Object[] prefabs = Resources.LoadAll("", typeof(GameObject));
                    if (prefabs.Length == 0) return;
                    Debug.Log($"Found {prefabs.Length} prefabs!");
                    prefabs.Where(p => p != null).ToList().ForEach(p => Debug.Log(p.name));
                });

                UI.Button("Raycast", () =>
                {
                    Transform? cameraAimTransform = PlayerController.instance.cameraAim?.transform;
                    if (cameraAimTransform == null) return;
                    foreach (RaycastHit hit in Physics.SphereCastAll(cameraAimTransform.position + (cameraAimTransform.forward * 2.75f), 1f, cameraAimTransform.forward, float.MaxValue) ?? [])
                    {
                        Collider collider = hit.collider;
                        Debug.Log($"Hit: {collider.GetType().Name} => {collider.gameObject.GetType().Name} => Layer {LayerMask.LayerToName(collider.gameObject.layer)} {collider.gameObject.layer}\n");
                    }
                });

                UI.Button("Test", () =>
                {
                    GameObjectManager.enemies.Select(e => e?.Handle()).ToList().ForEach(h =>
                    {
                        Debug.Log(h?.GetName());
                        Debug.Log(h?.IsDead());
                        Debug.Log(h?.IsDisabled());
                        Debug.Log(h == null);
                    });
                });
            });
        }
    }
}