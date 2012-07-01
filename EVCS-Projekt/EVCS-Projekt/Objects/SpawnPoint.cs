using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LastMan.Location;
using LastMan.Objects;
using Microsoft.Xna.Framework;

using System.Diagnostics;
using LastMan.Tree;
using LastMan.Managers;
using LastMan.AI;

namespace LastMan.Objects
{
    public class SpawnPoint : IQuadStorable
    {
        public MapLocation Location { get; private set; }
        public EEnemyType EnemyType { get; private set; }
        private float modifier;

        // ***************************************************************************
        // Läd und erzeugt das eigentliche Spiel
        public SpawnPoint(MapLocation location, EEnemyType enemyType, float modifier)
        {
            Location = location;
            EnemyType = enemyType;
            this.modifier = modifier;
        }

        // ***************************************************************************
        // float gibt die wahrscheinlichkeit an ob ein Monster spawn. 0=0% 1=100%
        public float IsSpawning()
        {
            // RUnde begrenzen
            if (!Main.MainObject.GameManager.GameState.RoundIsRunning || Main.MainObject.GameManager.GameState.MonsterSpawnCount <= 0)
                return 0;

            // TODO: Auslagern
            int maxSpawnDistance = 1000;

            // Wieviele Mosnter gleichzeitig zugelassen sind
            int maxEnemies = 50;

            // + 10% Sicherheitsrahmen
            int screenWidth = (int)(Configuration.GetInt("resolutionWidth") * 1.1F);
            int screenHeight = (int)(Configuration.GetInt("resolutionHeight") * 1.1F);

            Vector2 playerPosition = Main.MainObject.GameManager.GameState.MapOffset;
            playerPosition.X += Configuration.GetInt("resolutionWidth") / 2;
            playerPosition.Y += Configuration.GetInt("resolutionHeight") / 2;

            float distanceX = MathHelper.Clamp(Math.Abs(playerPosition.X - Location.Position.X), 0, maxSpawnDistance + screenWidth / 2);
            float distanceY = MathHelper.Clamp(Math.Abs(playerPosition.Y - Location.Position.Y), 0, maxSpawnDistance + screenHeight / 2);

            //Debug.WriteLine("Distance: " + distanceX + " " + distanceY);

            // Im Bildschirm muss 0 sein
            float bildschirmX = MathHelper.Clamp(distanceX - screenWidth / 2, 0, 1);
            float bildschirmY = MathHelper.Clamp(distanceY - screenHeight / 2, 0, 1);

            //Debug.WriteLine("Bildschirm: " + bildschirmX + " " + bildschirmY);

            // Umso weiter er weg ist, desto ergebnis näher an 0
            float distX = (maxSpawnDistance - (distanceX - screenWidth / 2)) / maxSpawnDistance;
            float distY = (maxSpawnDistance - (distanceY - screenHeight / 2)) / maxSpawnDistance;

            float limit = -(1 / (maxEnemies / 4F)) * Main.MainObject.GameManager.GameState.QuadTreeEnemies.Count + 4F;

            //Debug.WriteLine("Dist: " + distX + " " + distY);
            //Debug.WriteLine("limit: " + limit);

            float spawn = ((distX + distY) / 2) * bildschirmX * bildschirmY * (float)modifier * limit;

            //Debug.WriteLine("Spawn: " + spawn);

            return spawn;
        }

        // ***************************************************************************
        // spawnt enemies von der liste in den quadtree
        public static void SpawnEnemies(GameState gameState)
        {
            // Randomwert für "Wahrscheinlichkei"
            Random rand = new Random();

            //Debug.WriteLine("Spawning..");
            int debugCount = 0;

            foreach (SpawnPoint s in gameState.QuadTreeSpawnPoints)
            {

                float random = (float)rand.NextDouble();

                //Debug.WriteLine( s.Location.Position + " - Enemie: " + s.IsSpawning() + " - " + random);

                // Darf er Spawnen ?
                if (s.IsSpawning() >= random)
                {
                    //Enemy newEnemie = Enemy.DefaultEnemies[s.EnemyType].Clone();
                    // Enemies nach runden bestimmen
                    EEnemyType spawnType = GetEType(gameState.GameStatistic.Round, rand);
                    Enemy newEnemie = Enemy.DefaultEnemies[spawnType].Clone();

                    //Debug.WriteLine(spawnType);

                    // Spawn ein gegner
                    newEnemie.LocationBehavior.Position = s.Location.Position;

                    // Darf er Spawnen und ist an der SpawnPosition überhaupt die Map
                    if (AllowToSpawn(gameState, newEnemie) && GameManager.CheckRectangleInMap(newEnemie.Rect))
                    {
                        // Richtung player laufen
                        newEnemie.Activity = new WalkToPlayer();

                        // Gegner in Baum adden
                        gameState.QuadTreeEnemies.Add(newEnemie);

                        // SpawnCount erniedrigen
                        Main.MainObject.GameManager.GameState.MonsterSpawnCount--;

                        debugCount++;
                    }
                }

            }

            //Debug.WriteLine("Spawned Enemies: " + debugCount);
        }

        private static EEnemyType GetEType(int round, Random r)
        {
            // Wahrscheinlichkeits matrix
            float[,] w = new float[,] { 
            { 1F, 0, 0, 0, 0 },
            { 0.9F, 1F, 0, 0, 0 },
            { 0.8F, 0.95F, 1F, 0, 0 },
            { 0.6F, 0.85F, 0.95F, 1F, 0 },
            { 0.45F, 0.7F, 0.85F, 0.95F, 1F },
            { 0.2F, 0.4F, 0.6F, 0.75F, 0.9F }};

            round = (int)MathHelper.Clamp(round - 1, 0, 5);

            double rand = r.NextDouble();

            // Gegner durchgehen
            if (rand < w[round, 0])
                return EEnemyType.E1;
            if (rand < w[round, 1])
                return EEnemyType.E2;
            if (rand < w[round, 2])
                return EEnemyType.E3;
            if (rand < w[round, 3])
                return EEnemyType.E4;
            if (rand < w[round, 4])
                return EEnemyType.E5;

            return EEnemyType.E6;
        }

        // ***************************************************************************
        // Prüft ob ein dem Punkt zum spawnen bereits ein gegner steht
        private static bool AllowToSpawn(GameState gameState, Enemy e)
        {
            if (gameState.QuadTreeEnemies.GetObjects(e.Rect).Count > 0)
            {
                return false;
            }
            return true;
        }


        // ***************************************************************************
        // Für Quadtree benötigt - Gibt Position als Rectangle zurück / BoundingBox
        public Rectangle Rect
        {
            get
            {
                // Korrektur, da die gerendeten Bilder ihre position zentriert haben und nicht in der linken oberen ecke
                Rectangle r = Location.BoundingBox;
                r.X -= r.Width / 2;
                r.Y -= r.Height / 2;
                return r;
            }
        }

        // ***************************************************************************
        // Für Quadtree benötigt - Muss auf True gesetzt werden, falls sich das Objekt bewegt hat
        public bool HasMoved { get; set; }
    }
}
