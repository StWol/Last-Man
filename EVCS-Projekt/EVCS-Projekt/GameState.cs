using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EVCS_Projekt.Tree;
using Microsoft.Xna.Framework;
using EVCS_Projekt.Objects.Items;
using EVCS_Projekt.Objects;
using EVCS_Projekt.Map;

namespace EVCS_Projekt
{
    public class GameState
    {
        // QT für die Gegner
        public QuadTree<Enemy> QuadTreeEnemies { get; set; }
        // QT für die SpawnPoints
        public QuadTree<SpawnPoint> QuadTreeSpawnPoints { get; set; }
        // QT für statische Objekte
        public QuadTree<StaticObject> QuadTreeStaticObjects { get; set; }
        // QT für Items
        public QuadTree<Item> QuadTreeItems { get; set; }
        // List für alle Schüsse von Spieler
        public List<Shot> ShotListVsEnemies { get; set; }
        public List<Shot> ShotListVsPlayer { get; set; }
        // Der Spieler
        public Player Player { get; set; }
        // Der Versatz der Map relativ zum Spieler - so dass Spieler in der Mitte von Screen ist
        public Vector2 MapOffset { get; set; }
        // Größe der Map in Pixel
        public Vector2 MapSize { get; set; }
        // Zeit bis zum nächsten Gegner spawn
        public float NextSpawn { get; set; }
        // Die Map selbst
        public Karte Karte { get; set; }

        // Getötete Monster
        public int TotalKilledMonsters
        {
            get
            {
                int s = 0;
                foreach (int i in KilledMonsters.Values)
                    s += i;
                return s;
            }
        }

        public Dictionary<EEnemyType, int> KilledMonsters { get; set; }

        // Highscore
        public long Highscore { get; set; }
        public long Shots { get; set; }
        public float DamageGiven { get; set; }
        public float DamageTaken { get; set; }

        // Variablen für Runden etc
        public int Round { get; set; }
        public float TimeToRoundStart { get; set; }
        public float RoundDelay { get; set; }
        public bool RoundIsRunning { get; set; }
        public int MonsterSpawnCount { get; set; }
        public int KillsToEndRound { get; set; }
        public Dictionary<int, double> RoundStartTime { get; set; }
        public Dictionary<int, double> RoundEndTime { get; set; }

    }
}
