using REPOssessed.Cheats.Core;
using REPOssessed.Util;

namespace REPOssessed.Cheats.VisualTab
{
    internal class FOV : ToggleCheat, IVariableCheat<float>
    {
        public static float Value = 70f;

        public override void Update()
        {
            if (!Enabled) return;
            CameraZoom cameraZoom = CameraZoom.Instance;
            if (cameraZoom == null) return;
            cameraZoom.Reflect().SetValue("zoomPrev", Value);
            cameraZoom.Reflect().SetValue("zoomNew", Value);
            cameraZoom.Reflect().SetValue("zoomCurrent", Value);
            cameraZoom.playerZoomDefault = Value;
        }

        public override void OnDisable()
        {
            CameraZoom cameraZoom = CameraZoom.Instance;
            if (cameraZoom == null) return;
            cameraZoom.Reflect().SetValue("zoomPrev", 70);
            cameraZoom.Reflect().SetValue("zoomNew", 70);
            cameraZoom.Reflect().SetValue("zoomCurrent", 70);
            cameraZoom.playerZoomDefault = 70;
        }
    }
}
