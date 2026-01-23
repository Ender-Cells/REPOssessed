using REPOssessed.Menu.Popup;
using REPOssessed.Menu.Tab;
using REPOssessed.Util;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace REPOssessed.Menu.Core
{
    internal class HackMenu : MenuFragment
    {

        public Rect windowRect = new Rect(50f, 50f, 700f, 450f);

        public PopupMenu FirstSetupManagerWindow = new FirstSetupManagerWindow(1);
        public PopupMenu ItemManagerWindow = new ItemManagerWindow(2);
        public PopupMenu LootManagerWindow = new LootManagerWindow(3);
        public PopupMenu LevelManagerWindow = new LevelManagerWindow(4);

        private Vector2 scrollPos = Vector2.zero;
        private int selectedTab = 1;
        public List<MenuTab> tabs = new List<MenuTab>();
        public float contentWidth;
        public float contentHeight;
        public int spaceFromTop = 60;
        public int spaceFromLeft = 10;

        public static HackMenu? Instance;

        public HackMenu()
        {
            Instance = this;
            tabs.Add(new SettingsTab());
            tabs.Add(new GeneralTab());
            tabs.Add(new SelfTab());
            tabs.Add(new VisualTab());
            tabs.Add(new PlayersTab());
            tabs.Add(new EnemyTab());
            tabs.Add(new ServerTab());
        }

        public void Resize()
        {
            windowRect.width = Settings.i_menuWidth;
            windowRect.height = Settings.i_menuHeight;
            contentWidth = windowRect.width - (spaceFromLeft * 2);
            contentHeight = windowRect.height - spaceFromTop;
        }

        public void ResetMenuSize()
        {
            Settings.i_menuFontSize = 14;
            Settings.i_menuWidth = 810;
            Settings.i_menuHeight = 410;
            Settings.i_sliderWidth = 100;
            Settings.i_textboxWidth = 85;
        }

        public void Stylize()
        {
            if (ThemeUtil.Skin == null || GUI.skin == null)
            {
                Debug.LogError("ThemeUtil or GUI skin is null");
                return;
            }

            GUI.skin = ThemeUtil.Skin;
            GUI.color = Color.white;

            int fontSize = Settings.i_menuFontSize;
            new GUIStyle[] {
                GUI.skin.label,
                GUI.skin.button,
                GUI.skin.toggle,
                GUI.skin.box,
                GUI.skin.textField,
                GUI.skin.horizontalSlider,
                GUI.skin.horizontalSliderThumb,
                GUI.skin.verticalSlider,
                GUI.skin.verticalSliderThumb
            }.Where(x => x != null).ToList().ForEach(x => x.fontSize = fontSize);

            new string[]
            { 
                "TabBtn", 
                "SelectedTab"
            }.ToList().ForEach(name => GUI.skin.customStyles?.FirstOrDefault(x => x.name == name)?.fontSize = fontSize);

            Resize();
        }


        public void Draw()
        {
            if (Settings.b_IsFirstLaunch || Settings.b_isMenuOpen) Stylize(); else return;
            if (Settings.b_IsFirstLaunch) FirstSetupManagerWindow.Draw();
            else
            {
                GUI.color = new Color(1f, 1f, 1f, Settings.f_menuAlpha);
                windowRect = GUILayout.Window(0, windowRect, new GUI.WindowFunction(DrawContent), "REPOssessed");
                ItemManagerWindow.Draw();
                LootManagerWindow.Draw();
                LevelManagerWindow.Draw();
                GUI.color = Color.white;
            }
            if (!Cursor.visible) Cursor.visible = true;
            if (Cursor.lockState != CursorLockMode.None) Cursor.lockState = CursorLockMode.None;
        }

        private void DrawContent(int windowID)
        {
            GUI.color = new Color(1f, 1f, 1f, 0.1f);
            GUIStyle watermark = new GUIStyle(GUI.skin.label) { fontSize = 20, fontStyle = FontStyle.Bold };
            string text = "By Dustin";

            GUI.Label(new Rect(windowRect.width - watermark.CalcSize(new GUIContent(text)).x - 10, windowRect.height - watermark.CalcSize(new GUIContent(text)).y - 10, watermark.CalcSize(new GUIContent(text)).x, watermark.CalcSize(new GUIContent(text)).y), text, watermark);
            GUI.color = new Color(1f, 1f, 1f, Settings.f_menuAlpha);

            GUILayout.BeginVertical();

            GUILayout.BeginArea(new Rect(0, 25, windowRect.width, 25), style: "Toolbar");

            GUILayout.BeginHorizontal();
            selectedTab = GUILayout.Toolbar(selectedTab, tabs.Select(x => x.Name).ToArray(), style: "TabBtn");
            GUILayout.EndHorizontal();

            GUILayout.EndArea();

            GUILayout.Space(spaceFromTop);

            GUILayout.BeginArea(new Rect(spaceFromLeft, spaceFromTop, windowRect.width - spaceFromLeft, contentHeight - 15));

            scrollPos = GUILayout.BeginScrollView(scrollPos);

            GUILayout.BeginHorizontal();
            tabs[selectedTab].Draw();
            GUILayout.EndHorizontal();

            GUILayout.EndScrollView();

            GUILayout.EndArea();

            GUI.color = Color.white;

            GUI.DragWindow();
        }
    }
}
