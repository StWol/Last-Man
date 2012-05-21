using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System.Diagnostics;
using EVCS_Projekt.Objects;
using System.Threading;
using EVCS_Projekt.Tree;
using EVCS_Projekt.Location;
using Microsoft.Xna.Framework.Input;

namespace EVCS_Projekt.Managers
{
    public class GameManager : Manager
    {
        private List<SpawnPoint> spawnPoints;
        private GameState gameState;

        private Texture2D test;
        private SpriteFont testFont;

        // ***************************************************************************
        // Läd den ganzen Stuff, den der GameManager benötigt
        public void Load()
        {
            Debug.WriteLine("Läd das eigentliche Spiel");

            // Variablen initialisiwerung
            spawnPoints = new List<SpawnPoint>();
            gameState = new GameState();

            // GameState initialisieren
            gameState.QuadTreeEnemies = new QuadTree<Enemy>(0, 0, 1000, 1000); // TODO: Mapgröße hier mitgeben

            // ################################################################################
            // TEST
            // Test für ladebilschirm
            Thread.Sleep(500);

            // Testtextur
            test = Main.ContentManager.Load<Texture2D>("images/pixelWhite");
            testFont = Main.ContentManager.Load<SpriteFont>("fonts/arialSmall");

            // Test für quadtree
            Random random = new Random();

            for (int i = 0; i < 1000; i++)
            {
                MapLocation m = new MapLocation(new Rectangle(random.Next(0, 1000), random.Next(0, 1000), 10, 10));
                Enemy x = new Enemy(m, 0, 0, 0, 0, 0, 0, 0);
                gameState.QuadTreeEnemies.Add(x);
            }

            // TEST-ENDE
            // ################################################################################

            // Wenn ladevorgang fertig => Switch von MenuManager auf GameManager
            Main.MainObject.CurrentManager = this;
        }

        // ***************************************************************************
        // Update
        public override void Update()
        {

            // ################################################################################
            // TEST
            foreach ( Enemy e in gameState.QuadTreeEnemies )
            {
                int x = (int)e.LocationBehavior.Position.X + 1;
                if ( x > 1000)
                    x = 0;
                e.LocationBehavior.Position = new Vector2(x ,  e.LocationBehavior.Position.Y);
                e.HasMoved = true;

                gameState.QuadTreeEnemies.Move(e);
            }
            // TEST-ENDE
            // ################################################################################

        }

        // ***************************************************************************
        // Draw
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

            // Komplett weiß
            spriteBatch.Draw(test, new Rectangle(0, 0, Configuration.GetInt("resolutionWidth"), Configuration.GetInt("resolutionHeight")), Color.White);

            // ################################################################################
            // TEST
            List<Enemy> enemies = gameState.QuadTreeEnemies.GetObjects(new Rectangle(Mouse.GetState().X - 100, Mouse.GetState().Y - 100, 200, 200));
            
            //Debug.WriteLine("enemies: " + enemies.Count);
            foreach (Enemy e in enemies)
            {
                //Debug.WriteLine((int)e.LocationBehavior.Position.X + " " + (int)e.LocationBehavior.Position.Y);
                spriteBatch.Draw(test, new Rectangle((int)e.LocationBehavior.Position.X, (int)e.LocationBehavior.Position.Y, 10, 10), Color.Red);
            }

            spriteBatch.Draw(test, new Rectangle(Mouse.GetState().X - 100, Mouse.GetState().Y - 100, 200, 200), Color.Blue * 0.5F);

            spriteBatch.DrawString(testFont, "Enemies: "+gameState.QuadTreeEnemies.Count+" Draws: " + enemies.Count + " FPS: " + (1/Main.GameTimeDraw.ElapsedGameTime.TotalSeconds), new Vector2(0,0), Color.Black);

            // TEST-ENDE
            // ################################################################################

            spriteBatch.End();
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
