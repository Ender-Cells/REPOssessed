using Newtonsoft.Json.Linq;
using REPOssessed.Cheats.Core;
using REPOssessed.Cheats.SettingsTab;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace REPOssessed.Util
{
    public class TranslationUtil
    {
        public class Language
        {
            public int Rank { get; set; }
            public string Name { get; private set; }
            public string Translator { get; private set; }

            private Dictionary<string, string> Translation;

            public Language(string name, string translator, Dictionary<string, string> translation)
            {
                Name = name;
                Translator = translator;
                Translation = translation;
            }

            public string Localize(string key) => Translation.ContainsKey(key) ? Translation[key] : key;
            public bool Has(string key) => Translation.ContainsKey(key);
            public int Count() => Translation.Count;
        }

        public static Dictionary<string, Language> Languages = new Dictionary<string, Language>();

        public static Language SelectedLanguage => Languages.ContainsKey(Settings.Language) ? Languages[Settings.Language] : Languages["English"];

        public static void Initialize()
        {
            List<string> embedResources = Assembly.GetExecutingAssembly().GetManifestResourceNames().Where(m => m.Contains(".Languages.") && m.EndsWith(".json")).ToList();
            LoadLanguage(embedResources.FirstOrDefault(r => r.Contains("English.json")));
            embedResources.Where(r => !r.Contains("English.json")).ToList().ForEach(r => LoadLanguage(r));
            int englishCount = Languages["English"].Count();
            Languages.Values.Where(l => l.Name != "English").ToList().ForEach(l =>
            {
                if (l.Count() != englishCount)
                {
                    if (Cheat.Instance<DebugMode>().Enabled) Debug.LogWarning($"Language {l.Name} is missing {englishCount - l.Count()} keys");
                    if ((l.Count() / (double)englishCount) * 100 < 80.00)
                    {
                        Languages.Remove(l.Name);
                        Debug.LogError($"{l.Name} is too far behind. Unloading it.");
                    }
                }
            });
        }

        private static void LoadLanguage(string resource)
        {
            JObject json = JObject.Parse(new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream(resource)).ReadToEnd());
            if (!json.TryGetValue("LANGUAGE", out JToken? languageToken) || !json.TryGetValue("TRANSLATOR", out JToken? translatorToken))
            {
                Debug.LogError($"Failed to load Translation file => {resource}");
                return;
            }
            string language = languageToken.ToString();
            string translator = translatorToken.ToString();
            Dictionary<string, string> translation = json.Properties().ToDictionary(p => p.Name, p => p.Value.ToString());
            Languages[language] = new Language(language, translator, translation);
            Debug.Log($"Loaded Language {language} by {translator}");
        }

        public static string[] GetLanguages() => Languages.Values.Select(l => l.Name).OrderBy(n => n == Settings.Language ? 0 : 1).ThenBy(n => n).ToArray();

        public static string Translate(string key) => SelectedLanguage.Has(key) ? SelectedLanguage.Localize(key) : key;

        public static string Translate(params string[] keys) => string.Join(" ", keys.Select(k => SelectedLanguage.Has(k) ? SelectedLanguage.Localize(k) : k));

        public static string[] TranslateArray(params string[] keys) => keys.Select(k => SelectedLanguage.Has(k) ? SelectedLanguage.Localize(k) : k).ToArray();
    }
}