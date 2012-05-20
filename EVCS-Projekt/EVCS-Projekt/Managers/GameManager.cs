using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System.Diagnostics;
using EVCS_Projekt.Objects;

namespace EVCS_Projekt.Managers
{
    public class GameManager : Manager
    {
        private List<SpawnPoint> spawnPoints;

        // ***************************************************************************
        // Läd den ganzen Stuff, den der GameManager benötigt
        public void Load()
        {
            Debug.WriteLine("Läd das eigentliche Spiel");

            // Variablen initialisiwerung
            spawnPoints = new List<SpawnPoint>();

            // Wenn ladevorgang fertig => Switch von MenuManager auf GameManager
            Main.MainObject.CurrentManager = this;
        }

        // ***************************************************************************
        // Update
        public override void Update()
        {

        }

        // ***************************************************************************
        // Draw
        public override void Draw(SpriteBatch spriteBatch)
        {

        }

        // ***************************************************************************
        // Lässt einen Gegner spawnen
        public void SpawnEnemy()
        {

        }

        // ***************************************************************************
        // Kollisionsprüfung
        public bool CheckCollision(GameObject x, GameObject y)
        {
            return false;
        }

        

    }
}
