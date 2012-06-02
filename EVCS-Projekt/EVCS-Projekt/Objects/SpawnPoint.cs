using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EVCS_Projekt.Location;
using Microsoft.Xna.Framework;

using System.Diagnostics;
using EVCS_Projekt.Tree;

namespace EVCS_Projekt.Objects
{
    public class SpawnPoint : IQuadStorable
    {
        public MapLocation Location { get; private set; }
        public int EnemyType { get; private set; }
        private float modifier;

        // ***************************************************************************
        // Läd und erzeugt das eigentliche Spiel
        public SpawnPoint(MapLocation location, int enemyType, float modifier)
        {
            Location = location;
            EnemyType = enemyType;
            this.modifier = modifier;
        }

        // ***************************************************************************
        // float gibt die wahrscheinlichkeit an ob ein Monster spawn. 0=0% 1=100%
        public float IsSpawning()
        {
            int maxSpawnDistance = 1000;

            // + 10% Sicherheitsrahmen
            int screenWidth = (int)(Configuration.GetInt("resolutionWidth") * 1.1F);
            int screenHeight = (int)(Configuration.GetInt("resolutionHeight") * 1.1F);

            Vector2 playerPosition = Main.MainObject.GameManager.GameState.Player.LocationBehavior.Position;

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

            //Debug.WriteLine("Dist: " + distX + " " + distY);

            float spawn = ((distX+distY)/2) * bildschirmX * bildschirmY * (float)modifier;

            //Debug.WriteLine("Spawn: " + spawn);

            return spawn;
        }

        // ***************************************************************************
        // spawnt enemies von der liste in den quadtree
        public static void SpawnEnemies(GameState gameState )
        {
            // Randomwert für "Wahrscheinlichkei"
            Random rand = new Random();

            //Debug.WriteLine("Spawning..");
            int debugCount = 0;

            foreach ( SpawnPoint s in gameState.QuadTreeSpawnPoints ) {

                float random = (float)rand.NextDouble();

                //Debug.WriteLine( s.Location.Position + " - Enemie: " + s.IsSpawning() + " - " + random);

                // Darf er Spawnen ?
                if (s.IsSpawning() >= random)
                {
                    Enemy d = Enemy.DefaultEnemies[s.EnemyType];

                    // Spawn ein gegner
                    Enemy newEnemie = new Enemy(new MapLocation(s.Location.Position), d.Renderer,0, 0, 0, 100, d.Speed, d.Health, d.TypOfEnemy);
                    newEnemie.LocationSizing();

                    // Gegner in Baum adden
                    gameState.QuadTreeEnemies.Add(newEnemie);

                    debugCount++;
                }

            }

            Debug.WriteLine("Spawned Enemies: " + debugCount);

        }

        // ***************************************************************************
        // Für Quadtree benötigt - Gibt Position als Rectangle zurück / BoundingBox
        public Rectangle Rect
        {
            get
            {
                // Korrektur, da die gerendeten Bilder ihre position zentriert haben und nicht in der linken oberen ecke
                Rectangle r = Location.BoundingBox;
                return r;
            }
        }

        // ***************************************************************************
        // Für Quadtree benötigt - Muss auf True gesetzt werden, falls sich das Objekt bewegt hat
        public bool HasMoved { get; set; }
    }
}
