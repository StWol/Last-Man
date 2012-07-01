using System;
using System.Collections.Generic;
using System.Linq;
using EVCS_Projekt.Objects;

namespace EVCS_Projekt
{
    public class GameStatistic
    {
        /// <summary>
        /// Initialisiert alle Variablen (Dicionaries)
        /// </summary>
        public GameStatistic()
        {
            // Getötete Gegner
            KilledMonsters = new Dictionary<EEnemyType, int>();

            // Für jeden Gegnertyp ein Eitnrag mit 0 adden
            foreach (EEnemyType e in Enum.GetValues(typeof(EEnemyType)))
            {
                KilledMonsters.Add(e, 0);
            }

            // Dicionary für Rundenzeiten
            RoundTimes = new Dictionary<int, double>();
        }

        /// <summary>
        ///   Anzahl der getöteten Monster
        /// </summary>
        public int TotalKilledMonsters
        {
            get { return KilledMonsters.Values.Sum(); }
        }

        /// <summary>
        ///   Anzahl der getöteten Gegner - für jeden Typ separat
        /// </summary>
        public Dictionary<EEnemyType, int> KilledMonsters { get; private set; }

        /// <summary>
        ///   Anzahl der abgegeben Schüsse
        /// </summary>
        public long Shots { get; set; }

        /// <summary>
        ///   Wieviel Schaden hat der Spieler vergeben
        /// </summary>
        public float DamageGiven { get; set; }

        /// <summary>
        ///   Wieviel Schaden hat der Spieler genommen
        /// </summary>
        public float DamageTaken { get; set; }

        /// <summary>
        ///   Zeit wann aktuelle Runde begonnen hat
        /// </summary>
        public float RoundStartTime { get; set; }
    
        /// <summary>
        ///   Wielange wurde für welche Runde benötigt
        /// </summary>
        public Dictionary<int, double> RoundTimes { get; private set; }

        /// <summary>
        /// In welcher Runde ist der Spieler gerade
        /// </summary>
        public int Round { get; set; }

        /// <summary>
        ///   Berechnung der aktuellen Punktezahl
        /// </summary>
        public long Highscore
        {
            get
            {
                long score = 0;

                foreach (EEnemyType e in Enum.GetValues(typeof(EEnemyType)))
                {
                    score += ((int)e + 1) * 50 * KilledMonsters[e];
                }

                foreach (KeyValuePair<int, double> pair in RoundTimes)
                {
                    double vorgabe = pair.Key * 20;
                    double faktor = vorgabe - pair.Value;

                    score += (long)Math.Max(0, vorgabe + faktor);
                }

                score += (long)(DamageGiven * 10);
                score = (long)Math.Max(0, score - DamageTaken);

                score = Math.Max(0, score - Shots);

                return score;
            }
        }
    }
}