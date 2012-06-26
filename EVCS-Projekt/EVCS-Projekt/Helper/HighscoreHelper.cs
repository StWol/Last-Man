using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EVCS_Projekt.Objects;

namespace EVCS_Projekt.Helper
{
    class HighscoreHelper
    {
        public static long Highscore
        {
            get
            {
                GameState g = Main.MainObject.GameManager.GameState;
                long h = 0;

                foreach (EEnemyType e in Enum.GetValues(typeof(EEnemyType)))
                {
                    h += ((int)e + 1) * 50 * g.KilledMonsters[e];
                }

                foreach (KeyValuePair<int, double> pair in g.RoundEndTime)
                {
                    double vorgabe = pair.Key * 20;
                    double geschafft = g.RoundEndTime[pair.Key] * 5 - g.RoundStartTime[pair.Key] * 5;

                    double faktor = vorgabe - geschafft;

                    h += (long)Math.Max(0, vorgabe + faktor);
                }

                h = Math.Max(0, h - g.Shots);

                return h;
            }
        }
    }
}
