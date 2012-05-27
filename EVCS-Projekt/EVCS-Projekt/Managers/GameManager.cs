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
using EVCS_Projekt.Renderer;
using EVCS_Projekt.Audio;

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

            // Player
            MapLocation playerPosition = new MapLocation(new Rectangle(0, 0, 30, 30));
            gameState.Player = new Player(playerPosition, 100, 100, 1);

            // ################################################################################
            // TEST
            // Test für ladebilschirm
            Thread.Sleep(500);

            // Testtextur
            test = Main.ContentManager.Load<Texture2D>("images/pixelWhite");
            testFont = Main.ContentManager.Load<SpriteFont>("fonts/arialSmall");

            // Test für quadtree
            Random random = new Random();

            Texture2D monster = Main.ContentManager.Load<Texture2D>("test/red_monster_small");
            Texture2D monster2 = Main.ContentManager.Load<Texture2D>("test/red_monster_happy");
            Texture2D monster3 = Main.ContentManager.Load<Texture2D>("test/red_monster_angry");

            Texture2D[] ani = new Texture2D[] { monster, monster2, monster3 };

            for (int i = 0; i < 1000; i++)
            {
                // 20 % chance, dass gegner ne richtige textur bekommt..
                IRenderBehavior render;
                if (random.Next(0, 100) < 50)
                {
                    render = new AnimationRenderer(ani, random.Next(1,9));
                }
                else if (random.Next(0, 100) < 30)
                {
                    render = new StaticRenderer(ani[random.Next(0, 3)]);
                }
                else
                {
                    render = new SimpleRenderer(new Color(random.Next(0, 255), random.Next(0, 255), random.Next(0, 200)));
                }

                MapLocation m = new MapLocation(new Rectangle(random.Next(0, 1000), random.Next(0, 1000), random.Next(10, 30), random.Next(10, 30)));

                Enemy x = new Enemy(m, render, 0, 0, 0, 0, 0, 0, 0);
                //x.LocationSizing();

                gameState.QuadTreeEnemies.Add(x);
            }


            AnimationRenderer ar = new AnimationRenderer(ani, 1);
            // Enemy mit Animation
            Enemy ea = new Enemy(new MapLocation(new Rectangle(150, 150, 0, 0)), ar, 0, 0, 0, 0, 0, 0, 0);
            ea.LocationSizing();
            gameState.QuadTreeEnemies.Add(ea);

            ar = new AnimationRenderer(ani, 2);
            // Enemy mit Animation
            ea = new Enemy(new MapLocation(new Rectangle(200, 150, 0, 0)), ar, 0, 0, 0, 0, 0, 0, 0);
            ea.LocationSizing();
            gameState.QuadTreeEnemies.Add(ea);

            // TEST-ENDE
            // ################################################################################

            // Wenn ladevorgang fertig => Switch von MenuManager auf GameManager
            Main.MainObject.CurrentManager = this;
            MusicPlayer.Stop();
        }

        // ***************************************************************************
        // Update
        public override void Update()
        {

            // ################################################################################
            // TEST
            /*foreach ( Enemy e in gameState.QuadTreeEnemies )
            {
                int x = (int)e.LocationBehavior.Position.X + 1;
                if ( x > 1000)
                    x = 0;
                e.LocationBehavior.Position = new Vector2(x ,  e.LocationBehavior.Position.Y);
                e.HasMoved = true;

                gameState.QuadTreeEnemies.Move(e);
            }*/
            List<Enemy> enemies = gameState.QuadTreeEnemies.GetObjects(new Rectangle(Mouse.GetState().X - 100, Mouse.GetState().Y - 100, 200, 200));

            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                foreach (Enemy e in enemies)
                {
                    gameState.QuadTreeEnemies.Remove(e);
                }
            }


            foreach (Enemy e in enemies)
            {
                e.Renderer.Update();
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

            // Player zeichnen

            // ################################################################################
            // TEST
            List<Enemy> enemies = gameState.QuadTreeEnemies.GetObjects(new Rectangle(Mouse.GetState().X - 100, Mouse.GetState().Y - 100, 200, 200));

            //Debug.WriteLine("enemies: " + enemies.Count);
            foreach (Enemy e in enemies)
            {
                //Debug.WriteLine((int)e.LocationBehavior.Position.X + " " + (int)e.LocationBehavior.Position.Y);
                //spriteBatch.Draw(test, new Rectangle((int)e.LocationBehavior.Position.X, (int)e.LocationBehavior.Position.Y, 10, 10), Color.Red);
                e.Renderer.Draw(spriteBatch, e.LocationBehavior);
            }

            spriteBatch.Draw(test, new Rectangle(Mouse.GetState().X - 100, Mouse.GetState().Y - 100, 200, 200), Color.Blue * 0.5F);

            spriteBatch.DrawString(testFont, "Enemies: " + gameState.QuadTreeEnemies.Count + " Draws: " + enemies.Count + " FPS: " + (1 / Main.GameTimeDraw.ElapsedGameTime.TotalSeconds), new Vector2(0, 0), Color.Black);
            spriteBatch.DrawString(testFont, "Linksklick zum Enemies loeschen", new Vector2(0, 30), Color.Red);

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
