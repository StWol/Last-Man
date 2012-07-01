using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using System.IO;

namespace LastMan.Audio
{
    public class Sound
    {
        public static Dictionary<string, SoundEffect> Sounds { get; private set; }

        public static void LoadSounds()
        {
            // Dic init
            Sounds = new Dictionary<string, SoundEffect>();

            // Configuration File öffnen
            TextReader tr = new StreamReader(Configuration.Get("sounds"));

            // Alle lines einlesen, bei = trennen und diese in das dic adden
            string input;
            while ((input = tr.ReadLine()) != null)
            {
                // falls erstes zeichen eine # ist, dann ist die zeile ein kommenatar
                if (input.Length < 1 || input.Substring(0, 1).Equals("#"))
                {
                    continue;
                }

                string[] split = input.Split(new char[] { ',' });

                if (split.Length == 2)
                {
                    // Sound laden
                    SoundEffect s;
                    s = Main.ContentManager.Load<SoundEffect>( Configuration.Get("soundDir") + split[1]);

                    // Sound adden
                    Sounds.Add(split[0], s);
                }
            }

            // File schließen
            tr.Close();
        }
    }
}
