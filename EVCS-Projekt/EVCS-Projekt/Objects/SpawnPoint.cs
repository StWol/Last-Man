using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EVCS_Projekt.Location;
using Microsoft.Xna.Framework;

using System.Diagnostics;

namespace EVCS_Projekt.Objects
{
    class SpawnPoint
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

            MapLocation playerPosition = new MapLocation(new Rectangle(1000, 1000, 0, 0));

            float distanceX = MathHelper.Clamp(playerPosition.Position.X - Location.Position.X, 0, maxSpawnDistance + screenWidth / 2);
            float distanceY = MathHelper.Clamp(playerPosition.Position.Y - Location.Position.Y, 0, maxSpawnDistance + screenHeight / 2);

            // Im Bildschirm muss 0 sein
            float bildschirmX = MathHelper.Clamp(Math.Abs(distanceX) - screenWidth / 2, 0, 1);
            float bildschirmY = MathHelper.Clamp(Math.Abs(distanceY) - screenHeight / 2, 0, 1);

            //Debug.WriteLine("Bildschirm: " + bildschirmX + " " + bildschirmY);

            // Umso weiter er weg ist, desto ergebnis näher an 0
            float distX = (maxSpawnDistance - (distanceX - screenWidth / 2)) / maxSpawnDistance;
            float distY = (maxSpawnDistance - (distanceY - screenHeight / 2)) / maxSpawnDistance;

            //Debug.WriteLine("Dist: " + distX + " " + distY);

            float spawn = ((distX+distY)/2) * bildschirmX * bildschirmY * modifier;

            //Debug.WriteLine("Spawn: " + spawn);

            return spawn;
        }
    }
}
