using System.Collections.Generic;
using System.IO;
using System.Diagnostics;

namespace LastMan
{
    class Configuration
    {
        public static Dictionary<string, string> configurationDic;

        private static void CreateConfig()
        {
            Debug.WriteLine("config.ini neu erstellen");

            // Dictionary initialisieren
            configurationDic = new Dictionary<string, string>();

            // Standardwerte adden
            configurationDic.Add("resolutionWidth", "1024");
            configurationDic.Add("resolutionHeight", "576");
            configurationDic.Add("isFullscreen", "false");
            configurationDic.Add("antiAliasing", "true");
            configurationDic.Add("mapDir", "Content/maps/");
            configurationDic.Add("itemDir", "Content/items/");
            configurationDic.Add("renderer", "Content/renderer.ini");
            configurationDic.Add("sounds", "Content/sounds.ini");
            configurationDic.Add("soundDir", "audio/sounds/");
            configurationDic.Add("iconDir", "images/itemIcons/");
            configurationDic.Add("buffs", "Content/buffs.xml");
            configurationDic.Add("buffIcons", "images/buffIcons/");

            configurationDic.Add("defaultFontBig", "fonts/defaultBig");
            configurationDic.Add("defaultFontSmall", "fonts/defaultSmall");

            configurationDic.Add("keyMoveUp", "W");
            configurationDic.Add("keyMoveDown", "S");
            configurationDic.Add("keyMoveLeft", "A");
            configurationDic.Add("keyMoveRight", "D");

            // Speichern
            SaveConfig();
        }

        public static void LoadConfig()
        {
            Debug.WriteLine("Versuche config.ini zuladen..");

            // prüfen ob config.ini existiert
            if (!File.Exists("Content/config.ini"))
            {
                // Falls nicht existiert erstellen und Methode beenden
                CreateConfig();

                return;
            }

            // Configuration File öffnen
            TextReader tr = new StreamReader("Content/config.ini");

            // Dictionary initialisieren
            configurationDic = new Dictionary<string, string>();

            // Alle lines einlesen, bei = trennen und diese in das dic adden
            string input;
            while ((input = tr.ReadLine()) != null)
            {
                // falls erstes zeichen eine # ist, dann ist die zeile ein kommenatar
                if ( input.Length < 1 || input.Substring(0,1).Equals("#") ) {
                    continue;
                }

                string[] split = input.Split(new char[]{'='});

                Debug.WriteLine(split[0] + "=" + split[1]);

                configurationDic.Add(split[0], split[1]);
            }

            // File schließen
            tr.Close();
        }

        public static void SaveConfig()
        {
            // Speicher die aktuelle ConfigurationDic

            // Configuration File öffnen
            TextWriter tw = new StreamWriter("Content/config.ini");

            tw.WriteLine("# Config - Last Man");

            // Alle werte aus dem dic in das file schreiben. = trennt key und value
            foreach (var key in configurationDic)
            {
                tw.WriteLine(key.Key + "=" + key.Value);
            }

            // File schließen
            tw.Close();
        }

        public static void Set(string key, string value)
        {
            configurationDic[key] = value;
        }

        public static string Get(string key)
        {
            return configurationDic[key];
        }

        public static int GetInt(string key)
        {
            return int.Parse(configurationDic[key]);
        }

        public static long GetLong(string key)
        {
            return long.Parse(configurationDic[key]);
        }

        public static bool GetBool(string key)
        {
            return bool.Parse(configurationDic[key]);
        }
    }
}
