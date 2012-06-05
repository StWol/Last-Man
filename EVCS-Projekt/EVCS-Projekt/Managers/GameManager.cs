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
using EVCS_Projekt.Map;

namespace EVCS_Projekt.Managers
{
    public class GameManager : Manager
    {
        private List<SpawnPoint> spawnPoints;
        public GameState GameState { get; set; }


        // Keybelegung
        private Keys keyMoveUp;
        private Keys keyMoveDown;
        private Keys keyMoveLeft;
        private Keys keyMoveRight;

        private float spawnInterval = 2F;

        // Debug
        private float updateObjects = 0;

        // ## Sonstiges
        Texture2D background;

        // Tests
        private Texture2D test;
        private SpriteFont testFont;
        Texture2D monster3;
        Texture2D arrow, shot, blood, gun_fire, shot_01;
        Enemy testEnemy;
        float accu = 0.1F;
        float mausrad = 0F;
        Texture2D[] bloodA;
        bool shoting = false;
        StaticRenderer gun;
        private float gun_cd;

        // ***************************************************************************
        // Läd den ganzen Stuff, den der GameManager benötigt
        public void Load()
        {
            Debug.WriteLine("Läd das eigentliche Spiel");

            // Variablen initialisiwerung
            spawnPoints = new List<SpawnPoint>();
            GameState = new GameState();

            // AnimationRenderer laden
            AnimationRenderer.Load(EAnimationRenderer.Splatter_01, "blood", 14, 35F);

            // StaticRenderer laden
            StaticRenderer.Load(EStaticRenderer.Shot_Normal, "shots/shot_01");
            StaticRenderer.Load(EStaticRenderer.Shot_Monster_01, "shots/shot_monster_01");

            // GameState initialisieren
            GameState.MapSize = new Vector2(2000, 2000); // TODO: Mapgröße hier mitgeben
            GameState.QuadTreeEnemies = new QuadTree<Enemy>(0, 0, (int)GameState.MapSize.X, (int)GameState.MapSize.Y);
            GameState.QuadTreeSpawnPoints = new QuadTree<SpawnPoint>(0, 0, (int)GameState.MapSize.X, (int)GameState.MapSize.Y);
            GameState.QuadTreeStaticObjects = new QuadTree<StaticObject>(0, 0, (int)GameState.MapSize.X, (int)GameState.MapSize.Y);

            GameState.ShotListVsEnemies = new List<Shot>(); // Shots als Liste, da diese nur eine kurze Lebenszeit haben
            GameState.ShotListVsPlayer = new List<Shot>(); // Shots als Liste, da diese nur eine kurze Lebenszeit haben

            GameState.Karte = new Karte();
            GameState.Karte.LoadMap(GameState, "");

            // Player
            MapLocation playerPosition = new MapLocation(GameState.Karte.PlayerStart);
            GameState.Player = new Player(playerPosition, 100, 100, 200);

            CalculateMapOffset();

            // Keybelegung
            keyMoveUp = Keys.W;
            keyMoveDown = Keys.S;
            keyMoveLeft = Keys.A;
            keyMoveRight = Keys.D;

            // Background laden
            background = Main.ContentManager.Load<Texture2D>("images/background");

            // ################################################################################
            // TEST
            // Test für ladebilschirm
            Thread.Sleep(100);


            foreach (EEnemyType e in Enum.GetValues(typeof(EEnemyType)))
            {
                Debug.WriteLine(e.ToString());
            }

            gun_cd = 0.10F;

            gun_fire = Main.ContentManager.Load<Texture2D>("images/effects/guns/gun_fire");
            gun = new StaticRenderer(gun_fire);

            monster3 = Main.ContentManager.Load<Texture2D>("test/red_monster_angry");
            shot = Main.ContentManager.Load<Texture2D>("test/shot");
            blood = Main.ContentManager.Load<Texture2D>("test/blood");
            shot_01 = Main.ContentManager.Load<Texture2D>("images/shots/shot_01");


            MouseCursor.CurrentCursor = MouseCursor.DefaultCursor;

            // Testtextur
            test = Main.ContentManager.Load<Texture2D>("images/pixelWhite");
            testFont = Main.ContentManager.Load<SpriteFont>("fonts/arialSmall");

            // Test für quadtree
            Random random = new Random();

            Texture2D monster = Main.ContentManager.Load<Texture2D>("test/red_monster_small");
            Texture2D monster2 = Main.ContentManager.Load<Texture2D>("test/red_monster_happy");


            // DefaultEnemies laden
            Enemy.DefaultEnemies = new Dictionary<EEnemyType, Enemy>();

            Enemy d1 = new Enemy(new MapLocation(new Vector2(0, 0)), new StaticRenderer(monster2), 1, 0, 0, 0, 0, 100, 0);
            d1.LocationSizing();

            Enemy.DefaultEnemies.Add(EEnemyType.E1, d1);

            Texture2D[] ani = new Texture2D[] { monster, monster2, monster3 };

            for (int i = 0; i < 00; i++)
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

                Enemy x = new Enemy(m, render, 0, 0, 0, 0, 0, 150, 0);
                x.LocationSizing();

                GameState.QuadTreeEnemies.Add(x);
            }

            for (int i = 0; i < 100; i++)
            {
                MapLocation m = new MapLocation(new Rectangle(random.Next(40, (int)GameState.MapSize.X - 40), random.Next(40, (int)GameState.MapSize.Y - 40), 1, 1));

                SpawnPoint s = new SpawnPoint(m, 0, 1);
                GameState.QuadTreeSpawnPoints.Add(s);

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

            // Enemies, SO in UdpateRect
            List<Enemy> enemies = GameState.QuadTreeEnemies.GetObjects(screenRect);
            List<StaticObject> staticObjects = GameState.QuadTreeStaticObjects.GetObjects(screenRect);

            updateObjects = enemies.Count;

            // Input prüfen
            CheckInput();

            // Gegner spawnen
            SpawnEnemy();

            // Schüsse fliegen lassen
            UpdateShots();

            // Kollisionen der SChüsse prüfen
            CheckShotsVsEnemies(enemies);
            CheckShotsVsPlayer();

            // Renderer des Players
            GameState.Player.Renderer.Update();

            // StaticObjects Renderer updaten
            foreach (StaticObject s in staticObjects)
            {
                s.Renderer.Update();
            }

            




            // ################################################################################
            // ################################################################################
            // ################################################################################
            // TEST
            /*
            foreach ( Enemy e in enemies )
            {
                int x = (int)e.LocationBehavior.Position.X + 1;
                if ( x > GameState.MapSize.X)
                    x = 0;
                e.LocationBehavior.Position = new Vector2(x ,  e.LocationBehavior.Position.Y);
                e.HasMoved = true;

                GameState.QuadTreeEnemies.Move(e);
            }*/

            float mr = Mouse.GetState().ScrollWheelValue - mausrad;
            if (mr > 0)
            {
                accu += 0.05F;
            }
            else if (mr < 0)
            {
                accu -= 0.05F;
            }
            mausrad = Mouse.GetState().ScrollWheelValue;

            Random r = new Random();
            if (gun_cd <= 0 && Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                Vector2 accuracy = new Vector2((float)(r.NextDouble() * accu - accu / 2), (float)(r.NextDouble() * accu - accu / 2));

                if (GameState.Player.IsMoving)
                {
                    accuracy = accuracy * 3F;
                }

                Shot s = new Shot(0, 0, 1000, -GameState.Player.LocationBehavior.Direction + accuracy, 10, "", 0, "", 0, new MapLocation(GameState.Player.LocationBehavior.Position));
                s.Renderer = StaticRenderer.DefaultRenderer[EStaticRenderer.Shot_Normal];
                s.SetDirection(-GameState.Player.LocationBehavior.Direction + accuracy);
                s.LocationSizing();

                GameState.ShotListVsEnemies.Add(s);
                shoting = true;
                gun_cd = 0.05F;
            }
            else
            {
                gun_cd -= (float)Main.GameTimeUpdate.ElapsedGameTime.TotalSeconds;
                shoting = false;
            }

            

            GameState.Player.RelativeLookAt(new Vector2(Mouse.GetState().X, Mouse.GetState().Y));

            foreach (Enemy e in enemies)
            {
                e.Renderer.Update();

                if (e.DistanceLessThan(GameState.Player, 300))
                {
                    e.LookAt(GameState.Player.LocationBehavior.Position);
                    e.Attack(GameState);
                }
            }

            // TEST-ENDE
            // ###############################################################################
        }

        // ***************************************************************************
        // Prüft die EIngabe
        public void UpdateShots()
        {
            // Schüsse gegen Gegner fliegen lassen - bei Kollision entfernen
            List<Shot> shotListTemp = new List<Shot>(GameState.ShotListVsEnemies);
            foreach (Shot s in shotListTemp)
            {
                s.UpdatePosition();

                if (!CheckRectangleInMap(s.Rect))
                {
                    // Wenn der Schuss das erste mal aus der Map fliegt, wird er korrigiert, dass er noch drinn ist und dann erst gelöscht
                    if (!s.Delete)
                    {
                        s.Delete = true;
                        for (int i = 0; i < 100; i++)
                        {
                            // Schuss in die Map schieben
                            s.AdjustShot();
                            if (CheckRectangleInMap(s.Rect))
                                break;
                        }
                    }
                    else
                        //Schuss entgültig löschen
                        GameState.ShotListVsEnemies.Remove(s);
                }
            }

            // Schüsse gegen Player fliegen lassen - bei Kollision entfernen
            shotListTemp = new List<Shot>(GameState.ShotListVsPlayer);
            foreach (Shot s in shotListTemp)
            {
                s.UpdatePosition();

                if (!CheckRectangleInMap(s.Rect))
                {
                    // Wenn der Schuss das erste mal aus der Map fliegt, wird er korrigiert, dass er noch drinn ist und dann erst gelöscht
                    if (!s.Delete)
                    {
                        s.Delete = true;
                        for (int i = 0; i < 100; i++)
                        {
                            // Schuss in die Map schieben
                            s.AdjustShot();
                            if (CheckRectangleInMap(s.Rect))
                                break;
                        }
                    }
                    else
                        //Schuss entgültig löschen
                        GameState.ShotListVsPlayer.Remove(s);
                }
            }
        }

        // ***************************************************************************
        // Prüft die EIngabe
        public void CheckInput()
        {
            // KeyState holen
            KeyboardState keyState = Keyboard.GetState();

            Vector2 moveVector = new Vector2(0, 0);
            if (keyState.IsKeyDown(keyMoveUp))
            {
                moveVector.Y -= 1;
            }
            if (keyState.IsKeyDown(keyMoveDown))
            {
                moveVector.Y += 1;
            }
            if (keyState.IsKeyDown(keyMoveLeft))
            {
                moveVector.X -= 1;
            }
            if (keyState.IsKeyDown(keyMoveRight))
            {
                moveVector.X += 1;
            }

            if (moveVector.X != 0 || moveVector.Y != 0)
            {
                moveVector.Normalize();

                if (CheckPlayerCanMove(moveVector))
                {
                    GameState.Player.IsMoving = true;
                    CalculateMapOffset();
                }
                else
                {
                    GameState.Player.IsMoving = false;
                }
            }
            else
            {
                GameState.Player.IsMoving = false;
            }
        }

        // ***************************************************************************
        // 
        private bool CheckPlayerCanMove(Vector2 moveVector)
        {
            float movement = (float)Main.GameTimeUpdate.ElapsedGameTime.TotalSeconds * GameState.Player.Speed;

            float xPos = GameState.Player.LocationBehavior.Position.X + moveVector.X * movement;
            float yPos = GameState.Player.LocationBehavior.Position.Y + moveVector.Y * movement;

            Vector2 newPlayerPosition = new Vector2(xPos, yPos);

            Vector2 currentPosition = GameState.Player.LocationBehavior.Position;
            GameState.Player.LocationBehavior.Position = newPlayerPosition;

            bool canMove = false;

            // Prüfen ob man an neuer Position gehen kann
            if (CheckRectangleInMap(GameState.Player.LittleBoundingBox))
            {
                canMove = true;
            }

            if (!canMove)
            {
                newPlayerPosition.X -= moveVector.X * movement;
                GameState.Player.LocationBehavior.Position = newPlayerPosition;
                // Prüfen ob man an neuer Position in X richtung gehen kann
                if (CheckRectangleInMap(GameState.Player.LittleBoundingBox))
                {
                    canMove = true;
                }
            }

            if (!canMove)
            {
                newPlayerPosition.X += moveVector.X * movement;
                newPlayerPosition.Y -= moveVector.Y * movement;
                GameState.Player.LocationBehavior.Position = newPlayerPosition;
                // Prüfen ob man an neuer Position in Y richtung gehen kann
                if (CheckRectangleInMap(GameState.Player.LittleBoundingBox))
                {
                    canMove = true;
                }
            }

            if (canMove)
            {
                // Prüft ob ein Gegner im Weg steht
                List<Enemy> enemies = GameState.QuadTreeEnemies.GetObjects(GameState.Player.Rect);

                foreach (Enemy e in enemies)
                {
                    if (e.PPCollisionWith(GameState.Player))
                    {
                        GameState.Player.LocationBehavior.Position = currentPosition;
                        canMove = false;
                        break;
                    }
                }
            }

            return canMove;
        }

        // ***************************************************************************
        // Berechnung des MapOffset
        public void CalculateMapOffset()
        {
            float x = MathHelper.Min(MathHelper.Max(GameState.Player.LocationBehavior.Position.X - Configuration.GetInt("resolutionWidth") / 2, 0), GameState.MapSize.X - Configuration.GetInt("resolutionWidth"));
            float y = MathHelper.Min(MathHelper.Max(GameState.Player.LocationBehavior.Position.Y - Configuration.GetInt("resolutionHeight") / 2, 0), GameState.MapSize.Y - Configuration.GetInt("resolutionHeight"));
            GameState.MapOffset = new Vector2(x, y);
        }

        // ***************************************************************************
        // Draw
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

            // Hintergrund zeichnen
            for (int x = 0; x <= 1; x++)
                for (int y = 0; y <= 1; y++)
                    spriteBatch.Draw(background, new Rectangle((int)(Configuration.GetInt("resolutionWidth") * x - GameState.MapOffset.X % Configuration.GetInt("resolutionWidth")), (int)(Configuration.GetInt("resolutionHeight") * y - GameState.MapOffset.Y % Configuration.GetInt("resolutionHeight")), Configuration.GetInt("resolutionWidth"), Configuration.GetInt("resolutionHeight")), Color.White);

            // Bildschirm Rectangle
            Rectangle screenRect = new Rectangle((int)GameState.MapOffset.X, (int)GameState.MapOffset.Y, Configuration.GetInt("resolutionWidth"), Configuration.GetInt("resolutionHeight"));

            //Static Objects im Bild
            List<StaticObject> staticOnScreen = GameState.QuadTreeStaticObjects.GetObjects(screenRect);

            // Alle gegner im Bilschirm
            List<Enemy> enemiesOnScreen = GameState.QuadTreeEnemies.GetObjects(screenRect);

            /* 
             * ## DRAWS
             * 
             * Reihenfolge:
             * - Map (fehlt)
             * - Statische Objecte
             * - Items etc. (fehlt)
             * - Schüsse
             * - Gegner
             * - Player
             * - Objekte die an der Decke sind
             * - Cursor
             * 
             */

            // Map zeichnen
            foreach (StaticObject so in GameState.Karte.QuadTreeWalkable.GetObjects(screenRect))
            {
                //Debug.WriteLine(so.LocationBehavior.Position + " " + so.LocationBehavior.Size);
                so.Renderer.Draw(spriteBatch, so.LocationBehavior);
            }

            // Statische Objekte zeichnen
            foreach (StaticObject so in staticOnScreen)
            {
                so.Renderer.Draw(spriteBatch, so.LocationBehavior);
            }

            // Shots werden alle gezeichnet, da es von ihnen nicht viele gibt und diese normalerweise alle innerhalb des bildschirms liegen
            foreach (Shot s in GameState.ShotListVsEnemies)
            {
                s.Renderer.Draw(spriteBatch, s.LocationBehavior);
            }
            foreach (Shot s in GameState.ShotListVsPlayer)
            {
                s.Renderer.Draw(spriteBatch, s.LocationBehavior);
            }

            // Enemies zeichnen
            foreach (Enemy e in enemiesOnScreen)
            {
                e.Renderer.Draw(spriteBatch, e.LocationBehavior);
            }

            // Player zeichnen
            GameState.Player.Renderer.Draw(spriteBatch, GameState.Player.LocationBehavior);


            // Cursor zeichnen
            MouseCursor.DrawMouse(spriteBatch);

            // ################################################################################
            // ################################################################################
            // TEST

            if (shoting)
            {
                gun.Draw(spriteBatch, GameState.Player.LocationBehavior);
            }

            spriteBatch.DrawString(testFont, "Enemies: " + GameState.QuadTreeEnemies.Count + " Draws: " + enemiesOnScreen.Count + " Updates: " + updateObjects + " FPS: " + (1 / Main.GameTimeDraw.ElapsedGameTime.TotalSeconds), new Vector2(0, 0), Color.Black);
            spriteBatch.DrawString(testFont, "MapOffset: " + GameState.MapOffset + " PlayerPos: " + GameState.Player.LocationBehavior.Position + " PlayerRel: " + GameState.Player.LocationBehavior.RelativePosition, new Vector2(0, 30), Color.Red);
            spriteBatch.DrawString(testFont, "Player: " + GameState.Player.LocationBehavior.RelativeBoundingBox + " Shots: " + GameState.ShotListVsEnemies.Count, new Vector2(0, 60), Color.Red);
            spriteBatch.DrawString(testFont, "PlayerDirection: " + GameState.Player.LocationBehavior.Direction + " Accu (Mausrad): " + accu, new Vector2(0, 90), Color.Blue);

            // TEST-ENDE
            // ################################################################################
            // ################################################################################

            spriteBatch.End();
        }

        // ***************************************************************************
        // Lässt einen Gegner spawnen, vlt
        public void SpawnEnemy()
        {
            // NextSpawn verringern
            GameState.NextSpawn -= (float)Main.GameTimeUpdate.ElapsedGameTime.TotalSeconds;

            if (GameState.NextSpawn <= 0)
            {
                // NextSpawn resetten
                GameState.NextSpawn = spawnInterval;

                // Spawn starten
                SpawnPoint.SpawnEnemies(GameState);
            }
        }

        // ***************************************************************************
        // Kollisionsprüfung
        public bool CheckCollision(GameObject x, GameObject y)
        {
            return false;
        }

        // ***************************************************************************
        // Prüfe schüsse gegen Spieler
        public void CheckShotsVsPlayer()
        {
            // Kopiere die lsite, um getroffene schüsse gleich entfernen zu können
            List<Shot> tempList = new List<Shot>(GameState.ShotListVsPlayer);

            foreach ( Shot s in tempList ) {
                // Bei collision
                if ( s.PPCollisionWith(GameState.Player) ) {
                    // Schuss dem Player gegebn
                    GameState.Player.TakeDamage(s);

                    // Schuss entfernen
                    GameState.ShotListVsPlayer.Remove(s);
                }
            }
        }

        // ***************************************************************************
        // Prüfe schüsse gegen gegner
        public void CheckShotsVsEnemies(List<Enemy> enemies)
        {

            foreach (Enemy e in enemies)
            {
                // Kopiere die lsite, um getroffene schüsse gleich entfernen zu können
                List<Shot> tempList = new List<Shot>(GameState.ShotListVsEnemies);

                if (e.IsDead)
                {
                    // FEHLER BEIM LÖSCHEN!!!!!!!!!!!!!!!! dieser gegner kann nicht gelöscht werden, falls es hier rein geht
                    foreach (Enemy x in GameState.QuadTreeEnemies)
                    {
                        if (x == e)
                            Debug.WriteLine("found");
                    }

                }

                // Prüfe kollision mit jedem schuss
                foreach (Shot s in tempList)
                {
                    // Shuss triff Gegner
                    if (s.PPCollisionWith(e))
                    {
                        // Gegner schaden zufügen
                        e.TakeDamage(s);

                        // Buffs des Schusses auf den Gegner übertragen
                        e.AddBuffs(s.BuffList);

                        //Debug.WriteLine("Health nach Schuss: " + e.Health);

                        GameState.ShotListVsEnemies.Remove(s);

                        continue;
                    }
                }

                // Töte Enemie - am besten in eigene Methode auslagern
                if (e.IsDead)
                {
                    //Test new StaticRenderer(blood)
                    AnimationRenderer a = AnimationRenderer.Get(EAnimationRenderer.Splatter_01);
                    a.PlayOnce();

                    StaticObject splatter = new StaticObject(new MapLocation(e.LocationBehavior.Position), a);

                    GameState.QuadTreeStaticObjects.Add(splatter);
                    //Debug.WriteLine("Entferne Gegner"); 381 1986

                    // FEHLER BEIM LÖSCHEN!!!!!!!!!!!!!!!!
                    if (!GameState.QuadTreeEnemies.Remove(e))
                    {
                        Debug.WriteLine("Fehler beim löschen");
                    }
                }
            }

        }

        // ***************************************************************************
        // Prüfe ob Player an neue Position laufen darf
        private bool CheckRectangleInMap(Rectangle newPosition)
        {
            List<StaticObject> objects = GameState.Karte.QuadTreeWalkable.GetObjects(newPosition);

            if (objects.Count <= 0)
                return false;

            float area = 0;

            // Fläche aller überschnitte berechnen
            foreach (StaticObject so in objects)
            {
                Rectangle inter = Rectangle.Intersect(newPosition, so.Rect);
                area += inter.Width * inter.Height;

                if (area >= newPosition.Width * newPosition.Height)
                    return true;
            }

            //Debug.WriteLine(area);

            if (area >= newPosition.Width * newPosition.Height)
                return true;
            else
                return false;
        }

    }
}
