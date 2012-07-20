using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.IO;
using System.Xml.Serialization;

namespace LastMan.Helper
{
    public struct Score
    {
        public string Name;
        public long Points;
        public int Round;
    }

    class HighscoreHelper
    {
        private static List<Score> highscore;
        private static bool loaded = false;

        public static void LoadHighscore()
        {
            if (!loaded)
            {
                loaded = true;

                LoadOutFile();

                highscore.Sort(CompareScore);
            }
        }

        private static void LoadOutFile()
        {
            try
            {
                FileStream fs = new FileStream("highscore.xml", FileMode.Open);
                XmlSerializer serializer = new XmlSerializer(typeof(List<Score>));
                highscore = (List<Score>)serializer.Deserialize(fs);
                fs.Close();
            }
            catch
            {
                highscore = new List<Score>();
            }
        }

        private static void SaveInFile()
        {
            FileStream fs = new FileStream("highscore.xml", FileMode.OpenOrCreate);
            XmlSerializer serializer = new XmlSerializer(typeof(List<Score>));
            serializer.Serialize(fs, highscore);
            fs.Flush();
            fs.Close();
        }

        public static void Add(Score s)
        {
            highscore.Add(s);

            highscore.Sort(CompareScore);

            SaveInFile();
        }

        public static int GetPosition(long score)
        {
            int p = 0;

            List<Score> temp = new List<Score>(highscore);
            temp.Reverse();

            while (p < HighscoreCount)
            {
                if (temp[p].Points < score)
                {
                    break;
                }

                p++;
            }

            p += 1;

            return p;
        }

        public static int HighscoreCount
        {
            get { return highscore.Count; }
        }

        public static List<Score> Top5
        {
            get
            {
                int count = (int)MathHelper.Clamp(highscore.Count, 0, 5);

                List<Score> l = new List<Score>(highscore);
                l.Reverse();

                return l.GetRange(0, count);
            }
        }


        // **************************************************************************
        // Vergleicht zwei Scores
        private static int CompareScore(Score x, Score y)
        {
            // ansonsten vergleichen
            if (x.Points > y.Points)
            {
                return 1;
            }
            else if (x.Points < y.Points)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
    }
}
