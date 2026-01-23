using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace REPOssessed.Util
{
    public class ThemeUtil
    {
        public static string Name = "Default";
        public static GUISkin? Skin;
        public static AssetBundle? AssetBundle;

        public static string[] GetThemes()
        {
            return Assembly.GetExecutingAssembly().GetManifestResourceNames().Where(r => r.Contains(".Resources.Theme.") && r.EndsWith(".skin")).Select(r => r[(r.IndexOf(".Resources.Theme.") + ".Resources.Theme.".Length)..^".skin".Length]).OrderBy(n => n).ToArray();
        }

        public static void SetTheme(string themeName)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Stream stream = assembly.GetManifestResourceStream(assembly.GetManifestResourceNames().FirstOrDefault(r => r.EndsWith($"{themeName}.skin") && r.Contains(".Resources.Theme.")) ?? "");
            if (stream == null)
            {
                Debug.LogError($"[ERROR] Theme {themeName} doesn't exist");
                themeName = "Default";
            }
            if (Name == themeName && Skin != null && AssetBundle != null)
            {
                Debug.LogWarning($"[WARNING] Theme {themeName} already loaded");
                return;
            }
            AssetBundle?.Unload(true);
            AssetBundle = null;
            Skin = null;
            AssetBundle = AssetBundle.LoadFromStream(stream);
            if (AssetBundle == null) return;
            Skin = AssetBundle.LoadAsset<GUISkin>("assets/lethalmenu.guiskin");
            Name = themeName;
            Debug.Log($"Loaded Theme {themeName}");
        }
    }
}
