using REPOssessed.Util;

namespace REPOssessed.Menu.Core
{
    internal class MenuTab : MenuFragment
    {
        public string Name;
        
        public MenuTab(string name)
        {
            this.Name = TranslationUtil.Translate(name);
        }

        public virtual void Draw() { }
    }
}
