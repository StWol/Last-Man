using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EVCS_Projekt.GUI;
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
using Microsoft.Xna.Framework.Audio;
using System.Xml;
using EVCS_Projekt.Helper.XMLManager;
using System.Xml.Serialization;
using System.IO;
using EVCS_Projekt.AI;
using EVCS_Projekt.Helper;

namespace EVCS_Projekt.Managers
{
    public class GameManager : Manager
    {
        public GameState GameState { get; set; }


        // Cachen von dem KeyboardState. Ist fur das Ein- und Ausschalten von GUI noetig 
        KeyboardState oldKeyState;

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
        SoundEffect peng, headshot;
        private Inventar inventar;

        // ***************************************************************************
        // Läd den ganzen Stuff, den der GameManager benötigt
        public void Load()
        {
            Debug.WriteLine("Läd das eigentliche Spiel");

            // Variablen initialisiwerung
            GameState = new GameState();

            // Renderer laden
            LoadRenderer();

            // Sounds laden
            Sound.LoadSounds();

            // GameState initialisieren
            GameState.MapSize = new Vector2(10000, 10000); // TODO: Mapgröße hier mitgeben
            GameState.QuadTreeEnemies = new QuadTree<Enemy>(0, 0, (int)GameState.MapSize.X, (int)GameState.MapSize.Y);
            GameState.QuadTreeSpawnPoints = new QuadTree<SpawnPoint>(0, 0, (int)GameState.MapSize.X, (int)GameState.MapSize.Y);
            GameState.QuadTreeStaticObjects = new QuadTree<StaticObject>(0, 0, (int)GameState.MapSize.X, (int)GameState.MapSize.Y);

            GameState.ShotListVsEnemies = new List<Shot>(); // Shots als Liste, da diese nur eine kurze Lebenszeit haben
            GameState.ShotListVsPlayer = new List<Shot>(); // Shots als Liste, da diese nur eine kurze Lebenszeit haben

            GameState.Karte = new Karte();
            GameState.Karte.LoadMap(GameState, "testmap");

            // Items laden
            Item.LoadItems();

            // Player
            MapLocation playerPosition = new MapLocation(GameState.Karte.PlayerStart);
            GameState.Player = new Player(playerPosition, 100, 100, 200);

            CalculateMapOffset();

            // Keybelegung
            keyMoveUp = (Keys)Enum.Parse(typeof(Keys), Configuration.Get("keyMoveUp"));
            keyMoveDown = (Keys)Enum.Parse(typeof(Keys), Configuration.Get("keyMoveDown"));
            keyMoveLeft = (Keys)Enum.Parse(typeof(Keys), Configuration.Get("keyMoveLeft"));
            keyMoveRight = (Keys)Enum.Parse(typeof(Keys), Configuration.Get("keyMoveRight"));

            // Background laden
            background = Main.ContentManager.Load<Texture2D>("images/background");




            // ################################################################################
            // ################################################################################
            // ################################################################################
            // TEST
            // Test für ladebilschirm

            MapLocation mmm = new MapLocation(new Rectangle(1, 2, 3, 4));
            Shot s = new Shot(1, EGroup.FeuerGross, 2, new Vector2(1, 2), 3, "name", 4, "desc", 5, mmm);
            s.Renderer = LoadedRenderer.GetStatic("S_Shot_Normal");

            Debug.WriteLine(">" + s.Renderer.Name);


            GameState.Player.Inventar.Add(Item.AllItems[2]);
            GameState.Player.Inventar.Add(Item.AllItems[3]);
            GameState.Player.Inventar.Add(Item.AllItems[4]);
            GameState.Player.Inventar.Add(Item.AllItems[6]);
            GameState.Player.Inventar.Add(Item.AllItems[7]);
            GameState.Player.Inventar.Add(Item.AllItems[7]);
            GameState.Player.Inventar.Add(Item.AllItems[7]);
            //User Interface erstellen
            InitGui();

            GameState.Player.Weapon = Item.DefaultWeapon[8].Clone();

            GameState.Player.Weapon.Munition = Item.DefaultMunition[3].Clone();

            gun_cd = 0.10F;

            gun_fire = Main.ContentManager.Load<Texture2D>("images/effects/guns/gun_fire");
            gun = new StaticRenderer(gun_fire);

            //peng = Main.ContentManager.Load<SoundEffect>("test/Skorpion-Kibblesbob-1109158827");
            headshot = Main.ContentManager.Load<SoundEffect>("test/headshot2");

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

            Enemy d1 = new Enemy(new MapLocation(new Vector2(0, 0)), LoadedRenderer.Get("A_RoterDrache_Move"), 1, 0, 0, 100, 100, 100, 0);
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

            
            // TEST-ENDE
            // ################################################################################


            // Wenn ladevorgang fertig => Switch von MenuManager auf GameManager
            Main.MainObject.CurrentManager = this;
            MusicPlayer.Stop();
        }

        //***************************************************************************
        // User Interface laden
        private void InitGui()
        {

            // ################################################################################
            // TEST 


            GameState.Player.Inventar.Add(Item.AllItems[2]);
            GameState.Player.Inventar.Add(Item.AllItems[3]);
            GameState.Player.Inventar.Add(Item.AllItems[4]);
            GameState.Player.Inventar.Add(Item.AllItems[6]);
            GameState.Player.Inventar.Add(Item.AllItems[7]);

            // TEST ENDE
            // ################################################################################

            inventar = new Inventar(600, 400, new Vector2(100, 100));
        }


        // ***************************************************************************
        // Renderer laden
        public void LoadRenderer()
        {
            //
            Debug.WriteLine("Renderer laden");

            // Renderer initialisieren
            LoadedRenderer.DefaultRenderer = new Dictionary<string, IRenderBehavior>();
            LoadedRenderer.DefaultRenderer.Add("NoRenderer", new NoRenderer());
            LoadedRenderer.DefaultRenderer.Add("SimpleRenderer", new SimpleRenderer(Color.White));

            // Configuration File öffnen
            TextReader tr = new StreamReader(Configuration.Get("renderer"));

            // Alle lines einlesen, bei = trennen und diese in das dic adden
            string input;
            while ((input = tr.ReadLine()) != null)
            {
                // falls erstes zeichen eine # ist, dann ist die zeile ein kommenatar
                if (input.Length < 1 || input.Substring(0, 1).Equals("#"))
                {
                    continue;
                }

                string[] split = input.Split(new char[] { ',' });

                if (split[0].Substring(0, 1).Equals("A"))
                {
                    int frames = int.Parse(split[2]);
                    float fps = float.Parse(split[3]);

                    Debug.WriteLine("AR: " + split[0] + "=" + split[1] + " " + frames + " frames mit " + fps + " fps");

                    // AnimationRenderer laden
                    AnimationRenderer.Load(split[0], split[1], frames, fps);
                }
                else if (split[0].Substring(0, 1).Equals("S"))
                {
                    Debug.WriteLine("SR: " + split[0] + "=" + split[1]);

                    // StaticRenderer laden
                    StaticRenderer.Load(split[0], split[1]);
                }
            }

            // File schließen
            tr.Close();
        }

        // ***************************************************************************
        // Update
        public override void Update()
        {
            // Bildschirm Rectangle + 200 % in jede richtung
            Rectangle screenRect = new Rectangle((int)(GameState.MapOffset.X - Configuration.GetInt("resolutionWidth") * 1), (int)(GameState.MapOffset.Y - Configuration.GetInt("resolutionHeight") * 1), (int)(Configuration.GetInt("resolutionWidth") * 3), (int)(Configuration.GetInt("resolutionHeight") * 3));

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
            GameState.Player.Update();

            // StaticObjects Renderer updaten
            foreach (StaticObject s in staticObjects)
            {
                s.Renderer.Update();
            }

            // Linke Maustaste gedrückt
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                // Player schießt
                GameState.Player.Shoot();
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
            Debug.WriteLineIf(!CheckRectangleInMap(GameState.Player.LittleBoundingBox), "Player in Map!!!!!!!!!");

            foreach (Enemy e in enemies)
            {
                e.DoActivity();
            }

            if (Mouse.GetState().RightButton == ButtonState.Pressed && GameState.Player.Reloading <= 0)
            {
                GameState.Player.Weapon = Item.DefaultWeapon[15].Clone();

                GameState.Player.Weapon.Munition = Item.DefaultMunition[12].Clone();

                Sound.Sounds["Weapon_Reload"].Play();
                GameState.Player.Reloading = (float)Sound.Sounds["Weapon_Reload"].Duration.TotalSeconds;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.M) && GameState.Player.Reloading <= 0)
            {
                GameState.Player.Weapon = Item.DefaultWeapon[8].Clone();

                GameState.Player.Weapon.Munition = Item.DefaultMunition[3].Clone();

                Sound.Sounds["Weapon_Reload"].Play();
                GameState.Player.Reloading = (float)Sound.Sounds["Weapon_Reload"].Duration.TotalSeconds;
            }

            var newState = Keyboard.GetState();

            if (Keyboard.GetState().IsKeyDown(Keys.D0))
            {
                foreach (Enemy e in GameState.QuadTreeEnemies)
                {
                    e.Activity = new RandomWalk();
                }
            }



            if (Keyboard.GetState().IsKeyDown(Keys.D1))

            {
                foreach (Enemy e in GameState.QuadTreeEnemies)
                {
                    e.Renderer = LoadedRenderer.Get("A_Krabbler_Move");
                }
            }
            else if (newState.IsKeyDown(Keys.D2))
            {
                foreach (Enemy e in GameState.QuadTreeEnemies)
                {
                    e.Renderer = LoadedRenderer.Get("A_Schleimer_Move");
                }
            }
            else if (newState.IsKeyDown(Keys.D3))
            {
                foreach (Enemy e in GameState.QuadTreeEnemies)
                {
                    e.Renderer = LoadedRenderer.Get("A_Hellboy_Move");
                }
            }
            else if (newState.IsKeyDown(Keys.D4))
            {
                foreach (Enemy e in GameState.QuadTreeEnemies)
                {
                    e.Renderer = LoadedRenderer.Get("A_RoterDrache_Move");
                }
            }
            else if (newState.IsKeyDown(Keys.D5))
            {
                foreach (Enemy e in GameState.QuadTreeEnemies)
                {
                    e.Renderer = LoadedRenderer.Get("A_StachelKrabbe_Move");
                }
            }
            else if ( newState.IsKeyDown( Keys.I ) && !oldKeyState.IsKeyDown( Keys.I ) )
            {

                inventar.Visible = !inventar.Visible;

            }

            oldKeyState = newState;

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

                /*Shot s = new Shot(0, 0, 1000, -GameState.Player.LocationBehavior.Direction + accuracy, 10, "", 0, "", 0, new MapLocation(GameState.Player.LocationBehavior.Position));
                s.Renderer = LoadedRenderer.DefaultRenderer["S_Shot_Normal"];
                s.SetDirection(-GameState.Player.LocationBehavior.Direction + accuracy);
                s.LocationSizing();


                peng.Play(0.8F, -0.5F, 0);

                GameState.ShotListVsEnemies.Add(s);
                shoting = true;
                gun_cd = 0.05F;*/
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

                /*if (e.DistanceLessThan(GameState.Player, 300) && PointSeePoint(e.LocationBehavior.Position, GameState.Player.LocationBehavior.Position))
                {
                    e.LookAt(GameState.Player.LocationBehavior.Position);
                    e.Attack(GameState);
                }*/
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
            var keyState = Keyboard.GetState();

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

            // Player bewegen
            if (moveVector.X != 0 || moveVector.Y != 0)
            {
                moveVector.Normalize();

                // Neue Position
                //Vector2 newPosition = new Vector2();

                moveVector = moveVector * GameState.Player.Speed * (float)Main.GameTimeUpdate.ElapsedGameTime.TotalSeconds;

                // Prüfen ob man laufen kann, wenn ja bewegen
                //if (CheckRectCanMove( new FRectangle(GameState.Player.LittleBoundingBox), moveVector, out mov))
                //if (CheckPlayerCanMove(moveVector, out newPosition))
                if ( GameState.Player.MoveGameObject(moveVector) )
                {
                    GameState.Player.FootRotation = GetMoveRotation();

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
        // "Berechne" rotation der bewegung
        private float GetMoveRotation()
        {
            // KeyState holen
            KeyboardState keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(keyMoveLeft) && keyState.IsKeyDown(keyMoveUp))
            {
                return (float)Math.PI * 1 / 4;
            }
            if (keyState.IsKeyDown(keyMoveUp) && keyState.IsKeyDown(keyMoveRight))
            {
                return (float)Math.PI * 3 / 4;
            }
            if (keyState.IsKeyDown(keyMoveRight) && keyState.IsKeyDown(keyMoveDown))
            {
                return (float)Math.PI * 5 / 4;
            }
            if (keyState.IsKeyDown(keyMoveDown) && keyState.IsKeyDown(keyMoveLeft))
            {
                return (float)Math.PI * 7 / 4;
            }
            if (keyState.IsKeyDown(keyMoveLeft))
            {
                return 0;
            }
            if (keyState.IsKeyDown(keyMoveUp))
            {
                return (float)Math.PI * 2 / 4;
            }
            if (keyState.IsKeyDown(keyMoveRight))
            {
                return (float)Math.PI;
            }
            if (keyState.IsKeyDown(keyMoveDown))
            {
                return (float)Math.PI * 6 / 4;
            }

            return 0;
        }

        // ***************************************************************************
        // Prüft ob die neue postion blockiert ist
        private bool CheckPlayerCanMove(Vector2 moveVector, out Vector2 newPosition)
        {
            float movement = (float)Main.GameTimeUpdate.ElapsedGameTime.TotalSeconds * GameState.Player.Speed;

            float xPos = (GameState.Player.LocationBehavior.Position.X + moveVector.X * movement);
            float yPos = (GameState.Player.LocationBehavior.Position.Y + moveVector.Y * movement);

            // CurrentPosition
            Vector2 currentPosition = GameState.Player.LocationBehavior.Position;

            // out setzten
            newPosition = currentPosition;

            bool canMove = false;

            // Positon setzten
            GameState.Player.LocationBehavior.Position = new Vector2(xPos, yPos);

            // Prüfen ob man an neuer Position gehen kann
            if (CheckRectangleInMap(GameState.Player.LittleBoundingBox))
            {
                canMove = true;

                // new Positon ausgeben
                newPosition = new Vector2(xPos, yPos);
            }

            if (!canMove && moveVector.Y != 0)
            {
                // Prüfen ob man an neuer Position in Y richtung gehen kann
                GameState.Player.LocationBehavior.Position = new Vector2(currentPosition.X, yPos);

                if (CheckRectangleInMap(GameState.Player.LittleBoundingBox))
                {
                    canMove = true;

                    // new Positon ausgeben
                    newPosition = new Vector2(currentPosition.X, yPos);
                }
            }

            if (!canMove && moveVector.X != 0)
            {
                // Prüfen ob man an neuer Position in X richtung gehen kann
                GameState.Player.LocationBehavior.Position = new Vector2(xPos, currentPosition.Y);

                if (CheckRectangleInMap(GameState.Player.LittleBoundingBox))
                {
                    canMove = true;

                    // new Positon ausgeben
                    newPosition = new Vector2(xPos, currentPosition.Y);
                }
            }

            if (canMove)
            {
                // Prüft ob ein Gegner im Weg steht
                List<Enemy> enemies = GameState.QuadTreeEnemies.GetObjects(GameState.Player.LittleBoundingBox);

                foreach (Enemy e in enemies)
                {
                    if (e.PPCollisionWith(GameState.Player))
                    {
                        canMove = false;
                        break;
                    }
                }
            }

            // Player zurücksetzten
            GameState.Player.LocationBehavior.Position = currentPosition;

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
            /*for (int x = 0; x <= 1; x++)
                for (int y = 0; y <= 1; y++)
                    spriteBatch.Draw(background, new Rectangle((int)(Configuration.GetInt("resolutionWidth") * x - GameState.MapOffset.X % Configuration.GetInt("resolutionWidth")), (int)(Configuration.GetInt("resolutionHeight") * y - GameState.MapOffset.Y % Configuration.GetInt("resolutionHeight")), Configuration.GetInt("resolutionWidth"), Configuration.GetInt("resolutionHeight")), Color.White);
            */

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
             * - Map (nicht durchschießbar)
             * - Statische Objecte (nur walkkollision)
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
                so.Renderer.Draw(spriteBatch, so.LocationBehavior, Color.Black);
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

            // Player zeichnen mit verschiedenen Renderern (deswegen hat er ne eigene methode)
            GameState.Player.Draw(spriteBatch);

           
            // ################################################################################
            // ################################################################################
            // TEST

            foreach ( WayPoint w in GameState.Karte.WayPoints.Values)
            {
                WayPoint.Renderer.Draw(spriteBatch, w.Location);
                spriteBatch.DrawString(testFont, ""+w.ID, w.Location.RelativePosition, Color.Black);

                foreach ( WayPoint wDest in w.connectedPoints )
                {
                   Draw2D.DrawLine(spriteBatch, 1, Color.Red, w.Location.RelativePosition, wDest.Location.RelativePosition);
                }
            }

            if (shoting)
            {
                gun.Draw(spriteBatch, GameState.Player.LocationBehavior);
            }

            spriteBatch.DrawString(testFont, "Enemies: " + GameState.QuadTreeEnemies.Count + " Draws: " + enemiesOnScreen.Count + " Updates: " + updateObjects + " FPS: " + (1 / Main.GameTimeDraw.ElapsedGameTime.TotalSeconds), new Vector2(0, 0), Color.Green);
            spriteBatch.DrawString(testFont, "Munition: " + GameState.Player.Weapon.Munition.Count + " PlayerPos: " + GameState.Player.LocationBehavior.Position + " PlayerRel: " + GameState.Player.LocationBehavior.RelativePosition, new Vector2(0, 30), Color.Red);
            spriteBatch.DrawString(testFont, "Player: " + GameState.Player.LocationBehavior.RelativeBoundingBox + " Shots: " + GameState.ShotListVsEnemies.Count, new Vector2(0, 60), Color.Red);
            spriteBatch.DrawString(testFont, "PlayerDirection: " + GameState.Player.LocationBehavior.Direction + " Accu (Mausrad): " + accu, new Vector2(0, 90), Color.Blue);

            if (inventar.Visible)
                inventar.Draw(spriteBatch);

            // TEST-ENDE
            // ################################################################################
            // ################################################################################


            // Cursor zeichnen
            MouseCursor.DrawMouse( spriteBatch );

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
        // Prüfe schüsse gegen Spieler
        public void CheckShotsVsPlayer()
        {
            // Kopiere die lsite, um getroffene schüsse gleich entfernen zu können
            List<Shot> tempList = new List<Shot>(GameState.ShotListVsPlayer);

            foreach (Shot s in tempList)
            {
                // Bei collision
                if (s.PPCollisionWith(GameState.Player))
                {
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
                    AnimationRenderer a = LoadedRenderer.GetAnimation("A_Splatter_01");
                    a.PlayOnce();

                    StaticObject splatter = new StaticObject(new MapLocation(e.LocationBehavior.Position), a);

                    //headshot.Play();

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
        public static bool CheckRectangleInMap(Rectangle newPosition)
        {
            List<StaticObject> objects = Main.MainObject.GameManager.GameState.Karte.QuadTreeWalkable.GetObjects(newPosition);

            if (objects.Count <= 0 )
                return true;
            else
                return false;
        }

        // ***************************************************************************
        // Prüfe ob zwei Punkte sichtkontakt haben (schießt sozusagen von Vector A nach Vector B un schaut ob er kollidiert.
        public static bool PointSeePoint(Vector2 start, Vector2 target)
        {
            Vector2 direction = new Vector2(target.X - start.X, target.Y - start.Y);

            int steps = (int)Math.Sqrt(Math.Pow(direction.X, 2) + Math.Pow(direction.Y, 2));

            direction.Normalize();

            for (int i = 0; i < steps; i++)
            {
                start = start + direction;

                if (!CheckRectangleInMap(new Rectangle((int)start.X, (int)start.Y, 1, 1)))
                {
                    return false;
                }
            }

            return true;
        }

        // ***************************************************************************
        // Prüft ob die neue postion blockiert ist und falls bewegt werden kann gibt es die bewegung zurück
        public static bool CheckRectCanMove(FRectangle rect, Vector2 moveVector, out Vector2 outVector)
        {

            // CurrentPosition
            Vector2 initPosition = new Vector2(rect.X, rect.Y);

            // out setzten
            outVector = new Vector2(0, 0);

            rect.X += moveVector.X;
            rect.Y += moveVector.Y;
            
            // Prüfen ob man an neuer Position gehen kann
            if (CheckRectangleInMap(rect.Rect()))
            {
                // das rect kann wie gewünscht fahren
                outVector = moveVector;
                return true;
            }

            if (moveVector.Y != 0)
            {
                // Prüfen ob man an neuer Position in Y richtung gehen kann
                rect.X = initPosition.X;

                if (CheckRectangleInMap(rect.Rect()))
                {
                    // das rect kann nur in Y richtung fahren
                    outVector = new Vector2(0, moveVector.Y);
                    return true;
                }
            }

            if (moveVector.X != 0)
            {
                // Prüfen ob man an neuer Position in X richtung gehen kann
                rect.X += moveVector.X;
                rect.Y = initPosition.Y;

                if (CheckRectangleInMap(rect.Rect()))
                {
                    // das rect kann nur in X richtung fahren
                    outVector = new Vector2(moveVector.X, 0);
                    return true;
                }
            }


            return false;
        }
    }
}
