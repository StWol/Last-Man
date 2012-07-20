using System.Collections.Generic;
using LastMan.Tree;
using Microsoft.Xna.Framework;
using LastMan.Objects.Items;
using LastMan.Objects;
using LastMan.Map;

namespace LastMan
{
    public class GameState
    {
        /// <summary>
        /// QuadTree in dem die Enemies gespeichert sind
        /// </summary>
        public QuadTree<Enemy> QuadTreeEnemies { get; set; }

        /// <summary>
        /// QuadTree in dem die SpawnPoints gespeichert sind
        /// </summary>
        public QuadTree<SpawnPoint> QuadTreeSpawnPoints { get; set; }

        /// <summary>
        /// QuadTree in dem die Objekte der Map gespeichert sind
        /// </summary>
        public QuadTree<StaticObject> QuadTreeStaticObjects { get; set; }

        /// <summary>
        /// QuadTree in dem die Items gespeichert sind, welche auf der Map liegen
        /// </summary>
        public QuadTree<Item> QuadTreeItems { get; set; }

        /// <summary>
        /// List der Schüsse, die mit Enemies kollidieren können
        /// </summary>
        public List<Shot> ShotListVsEnemies { get; set; }

        /// <summary>
        /// List der Schüsse, die mit dem Player kollidieren können
        /// </summary>
        public List<Shot> ShotListVsPlayer { get; set; }
        
        /// <summary>
        /// Das Playerobjekt
        /// </summary>
        public Player Player { get; set; }

        /// <summary>
        /// Der Versatz der Map relativ zum Spieler - so dass Spieler in der Mitte von Screen ist
        /// </summary>
        public Vector2 MapOffset { get; set; }
        
        /// <summary>
        ///  Größe der Map in Pixel
        /// </summary>
        public Vector2 MapSize { get; set; }
        
        /// <summary>
        /// Zeit bis die nächsten Gegner spawnen
        /// </summary>
        public float NextSpawn { get; set; }
        
        /// <summary>
        /// Das Mapobjekt
        /// </summary>
        public Karte Karte { get; set; }

        /// <summary>
        /// Statistic des aktuellen Spiels
        /// </summary>
        public GameStatistic GameStatistic { get; set; }

        // Variablen für Runden etc
        public float TimeToRoundStart { get; set; }
        public float RoundDelay { get; set; }
        public bool RoundIsRunning { get; set; }
        public int MonsterSpawnCount { get; set; }
        public int KillsToEndRound { get; set; }
        

    }
}
