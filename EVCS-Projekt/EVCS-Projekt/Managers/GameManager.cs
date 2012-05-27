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
using EVCS_Projekt.UI;
using EVCS_Projekt.Objects.Items;

namespace EVCS_Projekt.Managers
{
    public class GameManager : Manager
    {
        private List<SpawnPoint> spawnPoints;
        public GameState GameState { get; set; }

        // Player Blickrichtung und PlayerDrehpunkt
        float playerRotation;

        // Keybelegung
        private Keys keyMoveUp;
        private Keys keyMoveDown;
        private Keys keyMoveLeft;
        private Keys keyMoveRight;

        // Debug
        private float updateObjects = 0;

        // Tests
        private Texture2D test;
        private SpriteFont testFont;
        Texture2D monster3;
        Texture2D arrow, shot;
        Enemy testEnemy;
        float accu = 0.1F;
        float mausrad = 0F;

        // ***************************************************************************
        // Läd den ganzen Stuff, den der GameManager benötigt
        public void Load()
        {
            Debug.WriteLine("Läd das eigentliche Spiel");

            // Variablen initialisiwerung
            spawnPoints = new List<SpawnPoint>();
            GameState = new GameState();


            // GameState initialisieren
            GameState.MapSize = new Vector2(3000, 2000); // TODO: Mapgröße hier mitgeben
            GameState.QuadTreeEnemies = new QuadTree<Enemy>(0, 0, (int)GameState.MapSize.X, (int)GameState.MapSize.Y);

            GameState.ShotList = new List<Shot>(); // Shots als Liste, da diese nur eine kurze Lebenszeit haben

            // Player
            MapLocation playerPosition = new MapLocation(new Rectangle(0, 0, 30, 30));
            GameState.Player = new Player(playerPosition, 100, 100, 1);

            // Keybelegung
            keyMoveUp = Keys.W;
            keyMoveDown = Keys.S;
            keyMoveLeft = Keys.A;
            keyMoveRight = Keys.D;

            // ################################################################################
            // TEST
            // Test für ladebilschirm
            Thread.Sleep(500);

            monster3 = Main.ContentManager.Load<Texture2D>("test/red_monster_angry");
            arrow = Main.ContentManager.Load<Texture2D>("test/arrow");
            shot = Main.ContentManager.Load<Texture2D>("test/shot");

            GameState.Player.Renderer = new StaticRenderer(arrow);
            GameState.Player.LocationSizing();

            MouseCursor.CurrentCursor = MouseCursor.DefaultCursor;

            // Testtextur
            test = Main.ContentManager.Load<Texture2D>("images/pixelWhite");
            testFont = Main.ContentManager.Load<SpriteFont>("fonts/arialSmall");

            // Test für quadtree
            Random random = new Random();

            Texture2D monster = Main.ContentManager.Load<Texture2D>("test/red_monster_small");
            Texture2D monster2 = Main.ContentManager.Load<Texture2D>("test/red_monster_happy");


            Texture2D[] ani = new Texture2D[] { monster, monster2, monster3 };

            for (int i = 0; i < 100; i++)
            {
                // 20 % chance, dass gegner ne richtige textur bekommt..
                IRenderBehavior render;
                if (random.Next(0, 100) < 50)
                {
                    render = new AnimationRenderer(ani, random.Next(1, 9));
                }
                else if (random.Next(0, 100) < 30)
                {
                    render = new StaticRenderer(ani[random.Next(0, 3)]);
                }
                else
                {
                    render = new SimpleRenderer(new Color(random.Next(0, 255), random.Next(0, 255), random.Next(0, 200)));
                }

                MapLocation m = new MapLocation(new Rectangle(random.Next(0, (int)GameState.MapSize.X), random.Next(0, (int)GameState.MapSize.Y), random.Next(10, 30), random.Next(10, 30)));

                Enemy x = new Enemy(m, render, 0, 0, 0, 0, 0, 0, 0);
                x.LocationSizing();

                GameState.QuadTreeEnemies.Add(x);
            }


            AnimationRenderer ar = new AnimationRenderer(ani, 1);
            // Enemy mit Animation
            Enemy ea = new Enemy(new MapLocation(new Rectangle(150, 150, 0, 0)), ar, 0, 0, 0, 0, 0, 0, 0);
            ea.LocationSizing();
            GameState.QuadTreeEnemies.Add(ea);

            ar = new AnimationRenderer(ani, 2);
            // Enemy mit Animation
            ea = new Enemy(new MapLocation(new Rectangle(200, 150, 0, 0)), ar, 0, 0, 0, 0, 0, 0, 0);
            ea.LocationSizing();
            GameState.QuadTreeEnemies.Add(ea);

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
            // Bildschirm Rectangle + 10 % tolleranz in jede richtung
            Rectangle screenRect = new Rectangle((int)(GameState.MapOffset.X - Configuration.GetInt("resolutionWidth") * 0.1), (int)(GameState.MapOffset.Y - Configuration.GetInt("resolutionHeight") * 0.1), (int)(Configuration.GetInt("resolutionWidth") * 1.2), (int)(Configuration.GetInt("resolutionHeight") * 1.2));

            // Enemies in UdpateRect
            List<Enemy> enemies = GameState.QuadTreeEnemies.GetObjects(screenRect);

            updateObjects = enemies.Count;

            // Input prüfen
            CheckInput();


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

            float mr = Mouse.GetState().ScrollWheelValue - mausrad;
            if (mr > 0)
            {
                accu += 0.05F;
            }
            else if ( mr < 0 )
            {
                accu -= 0.05F;
            }
            mausrad = Mouse.GetState().ScrollWheelValue;

            Random r = new Random();
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                Vector2 accuracy = new Vector2((float)(r.NextDouble() * accu - accu / 2), (float)(r.NextDouble() * accu - accu / 2));

                Shot s = new Shot(0, 0, 1000, -GameState.Player.LocationBehavior.Direction + accuracy, 0, "", 0, "", 0, new MapLocation(GameState.Player.LocationBehavior.Position));
                s.Renderer = new StaticRenderer(shot);
                s.SetDirection(-GameState.Player.LocationBehavior.Direction + accuracy);
                s.LocationSizing();

                GameState.ShotList.Add(s);
            }

            List<Shot> shotListTemp = new List<Shot>(GameState.ShotList);
            foreach (Shot s in shotListTemp)
            {
                s.UpdatePosition();
            }

            GameState.Player.RelativeLookAt(new Vector2(Mouse.GetState().X, Mouse.GetState().Y));

            foreach (Enemy e in enemies)
            {
                e.Renderer.Update();

                e.LookAt(GameState.Player.LocationBehavior.Position);
            }

            // TEST-ENDE
            // ###############################################################################
        }

        // ***************************************************************************
        // Lässt einen Gegner spawnen
        public void CheckInput()
        {
            // KeyState holen
            KeyboardState keyState = Keyboard.GetState();

            bool hasMoved = false;

            Vector2 newPlayerPosition = GameState.Player.LocationBehavior.Position;
            if (keyState.IsKeyDown(keyMoveUp))
            {
                hasMoved = true;
                newPlayerPosition.Y -= 1;
            }
            if (keyState.IsKeyDown(keyMoveDown))
            {
                hasMoved = true;
                newPlayerPosition.Y += 1;
            }
            if (keyState.IsKeyDown(keyMoveLeft))
            {
                hasMoved = true;
                newPlayerPosition.X -= 1;
            }
            if (keyState.IsKeyDown(keyMoveRight))
            {
                hasMoved = true;
                newPlayerPosition.X += 1;
            }

            if (hasMoved)
            {
                GameState.Player.LocationBehavior.Position = newPlayerPosition;
                CalculateMapOffset();
            }
        }

        public void CalculateMapOffset()
        {
            GameState.MapOffset = new Vector2(MathHelper.Max(GameState.Player.LocationBehavior.Position.X - Configuration.GetInt("resolutionWidth") / 2, 0), MathHelper.Max(GameState.Player.LocationBehavior.Position.Y - Configuration.GetInt("resolutionHeight") / 2, 0));
        }

        // ***************************************************************************
        // Draw
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

            // Komplett weiß
            spriteBatch.Draw(test, new Rectangle(0, 0, Configuration.GetInt("resolutionWidth"), Configuration.GetInt("resolutionHeight")), Color.White);

            // Bildschirm Rectangle
            Rectangle screenRect = new Rectangle((int)GameState.MapOffset.X, (int)GameState.MapOffset.Y, Configuration.GetInt("resolutionWidth"), Configuration.GetInt("resolutionHeight"));

            // Alle gegner im Bilschirm
            List<Enemy> enemiesOnScreen = GameState.QuadTreeEnemies.GetObjects(screenRect);

            // Player zeichnen
            GameState.Player.Renderer.Draw(spriteBatch, GameState.Player.LocationBehavior);

            // ################################################################################
            // TEST

            // player

            foreach (Shot s in GameState.ShotList)
            {
                s.Renderer.Draw(spriteBatch, s.LocationBehavior);
            }
            

            //Debug.WriteLine("enemies: " + enemies.Count);
            foreach (Enemy e in enemiesOnScreen)
            {
                //Debug.WriteLine((int)e.LocationBehavior.Position.X + " " + (int)e.LocationBehavior.Position.Y);
                //spriteBatch.Draw(test, new Rectangle((int)e.LocationBehavior.Position.X, (int)e.LocationBehavior.Position.Y, 10, 10), Color.Red);
                e.Renderer.Draw(spriteBatch, e.LocationBehavior);
            }

            spriteBatch.DrawString(testFont, "Enemies: " + GameState.QuadTreeEnemies.Count + " Draws: " + enemiesOnScreen.Count + " Updates: " + updateObjects + " FPS: " + (1 / Main.GameTimeDraw.ElapsedGameTime.TotalSeconds), new Vector2(0, 0), Color.Black);
            spriteBatch.DrawString(testFont, "MapOffset: " + GameState.MapOffset + " PlayerPos: " + GameState.Player.LocationBehavior.Position + " PlayerRel: " + GameState.Player.LocationBehavior.RelativePosition, new Vector2(0, 30), Color.Red);
            spriteBatch.DrawString(testFont, "Player: " + GameState.Player.LocationBehavior.RelativeBoundingBox + " Shots: " + GameState.ShotList.Count, new Vector2(0, 60), Color.Red);
            spriteBatch.DrawString(testFont, "PlayerDirection: " + GameState.Player.LocationBehavior.Direction + " Accu (Mausrad): " + accu, new Vector2(0, 90), Color.Blue);

            // TEST-ENDE
            // ################################################################################

            // CUrsor zeichnen
            MouseCursor.DrawMouse(spriteBatch);

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
