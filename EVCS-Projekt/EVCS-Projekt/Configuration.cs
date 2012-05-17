using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EVCS_Projekt
{
    class Configuration
    {
        public static Dictionary<string, string> configurationDic;

        public static void LoadConfig()
        {
            // Dictionary initialisieren
            configurationDic = new Dictionary<string, string>();

            // Dummy werte
            configurationDic.Add("resolutionWidth", "800");
            configurationDic.Add("resolutionHeight", "600");
            configurationDic.Add("isFullscreen", "false");
        }

        public static void SaveConfig()
        {
            // Speicher die aktuelle ConfigurationDic

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
