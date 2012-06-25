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

        // Update Rectangle
        public Rectangle UpdateRectangle { get; set; }

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
        public Texture2D PixelWhite { get; private set; }
        public Texture2D PixelTransparent { get; private set; }

        // Update Delegates
        private delegate void UpdateDel();
        private UpdateDel updateDelegater;


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
        private UIInventarPanel uiInventarPanel;
        bool showWaypoints = false;

        // ***************************************************************************
        // Läd den ganzen Stuff, den der GameManager benötigt
        public void Load()
        {
            Debug.WriteLine("Läd das eigentliche Spiel");

            // Update Delegator setzen
            updateDelegater = UpdateGame;

            // Variablen initialisiwerung
            GameState = new GameState();

            // Renderer laden
            Main.MainObject.MenuManager.LoadingText = "Loading images..";
            LoadRenderer();

            // Sounds laden
            Main.MainObject.MenuManager.LoadingText = "Loading sounds..";
            Sound.LoadSounds();

            // GameState initialisieren
            Main.MainObject.MenuManager.LoadingText = "Initialize objects..";

            GameState.MapSize = new Vector2(10000, 10000); // TODO: Mapgröße hier mitgeben
            GameState.QuadTreeEnemies = new QuadTree<Enemy>(0, 0, (int)GameState.MapSize.X, (int)GameState.MapSize.Y);
            GameState.QuadTreeSpawnPoints = new QuadTree<SpawnPoint>(0, 0, (int)GameState.MapSize.X, (int)GameState.MapSize.Y);
            GameState.QuadTreeStaticObjects = new QuadTree<StaticObject>(0, 0, (int)GameState.MapSize.X, (int)GameState.MapSize.Y);
            GameState.QuadTreeItems = new QuadTree<Item>(0, 0, (int)GameState.MapSize.X, (int)GameState.MapSize.Y);

            GameState.ShotListVsEnemies = new List<Shot>(); // Shots als Liste, da diese nur eine kurze Lebenszeit haben
            GameState.ShotListVsPlayer = new List<Shot>(); // Shots als Liste, da diese nur eine kurze Lebenszeit haben

            Main.MainObject.MenuManager.LoadingText = "Loading map..";
            GameState.Karte = new Karte();
            GameState.Karte.LoadMap(GameState, "testmap");

            // Buffs laden
            Buff.Load();

            // Items laden
            Main.MainObject.MenuManager.LoadingText = "Loading items..";
            Item.LoadItems();

            // Player
            MapLocation playerPosition = new MapLocation(GameState.Karte.PlayerStart);

            GameState.Player = new Player(playerPosition, 100, 100, 200);
            Weapon w1 = Item.DefaultWeapon[8].Clone();
            w1.Munition = Item.DefaultMunition[200].Clone();
            //GameState.Player.AddItemToInventar(w1);
            GameState.Player.AddWeaponToShortcutList(1, w1);


            Weapon w2 = Item.DefaultWeapon[15].Clone();
            w2.Munition = Item.DefaultMunition[201].Clone();
            //GameState.Player.AddItemToInventar(w2);
            GameState.Player.AddWeaponToShortcutList(2, w2);

            CalculateMapOffset();

            // Keybelegung
            keyMoveUp = (Keys)Enum.Parse(typeof(Keys), Configuration.Get("keyMoveUp"));
            keyMoveDown = (Keys)Enum.Parse(typeof(Keys), Configuration.Get("keyMoveDown"));
            keyMoveLeft = (Keys)Enum.Parse(typeof(Keys), Configuration.Get("keyMoveLeft"));
            keyMoveRight = (Keys)Enum.Parse(typeof(Keys), Configuration.Get("keyMoveRight"));

            // Background laden
            background = Main.ContentManager.Load<Texture2D>("images/background");
            PixelWhite = Main.ContentManager.Load<Texture2D>("images/pixelWhite");
            PixelTransparent = Main.ContentManager.Load<Texture2D>("images/pixelTransparent");

            // AI Thread starten
            new Thread(new ThreadStart(AIThread.UpdateAI)).Start();

            //User Interface erstellen
            InitGui();

            // ################################################################################
            // ################################################################################
            // ################################################################################
            // TEST

            Item it1 = Item.Get(100);
            it1.LocationBehavior.Position = new Vector2(1100, 4150);
            it1.LocationSizing();

            Item it2 = Item.Get(200);
            it2.LocationBehavior.Position = new Vector2(1200, 4150);
            it2.LocationSizing();

            Item it3 = Item.Get(300);
            it3.LocationBehavior.Position = new Vector2(1300, 4150);
            it3.LocationSizing();

            Item it4 = Item.Get(400);
            it4.LocationBehavior.Position = new Vector2(1400, 4150);
            it4.LocationSizing();

            Item it5 = Item.Get(700);
            it5.LocationBehavior.Position = new Vector2(1500, 4150);
            it5.LocationSizing();


            GameState.QuadTreeItems.Add(it1);
            GameState.QuadTreeItems.Add(it2);
            GameState.QuadTreeItems.Add(it3);
            GameState.QuadTreeItems.Add(it4);
            GameState.QuadTreeItems.Add(it5);


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



            // DefaultEnemies laden
            Enemy.DefaultEnemies = new Dictionary<EEnemyType, Enemy>();

            Enemy d1 = new Enemy(new MapLocation(new Vector2(0, 0)), LoadedRenderer.Get("A_Hellboy_Move"), 1, 300, 1000, 100, 100, 100, 0);
            d1.Damage = 5F;
            d1.LocationSizing();

            Enemy.DefaultEnemies.Add(EEnemyType.E1, d1);

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

            GameState.Player.AddItemToInventar(Item.AllItems[100]);
            GameState.Player.AddItemToInventar(Item.AllItems[200]);
            GameState.Player.AddItemToInventar(Item.AllItems[300]);
            GameState.Player.AddItemToInventar(Item.AllItems[400]);
            GameState.Player.AddItemToInventar(Item.AllItems[500]);
            GameState.Player.AddItemToInventar(Item.AllItems[100]);
            GameState.Player.AddItemToInventar(Item.AllItems[200]);
            GameState.Player.AddItemToInventar(Item.AllItems[300]);
            GameState.Player.AddItemToInventar(Item.AllItems[400]);
            GameState.Player.AddItemToInventar(Item.AllItems[500]);

            // TEST ENDE
            // ################################################################################

            DrawHelper.AddDimension("Inventar",350, 200);
            //574

            int x = (int) (Configuration.GetInt("resolutionWidth") / 2 - DrawHelper.Get("Inventar").X);
            int y = (int) (Configuration.GetInt("resolutionHeight") / 2 - DrawHelper.Get("Inventar").Y);
            uiInventarPanel = new UIInventarPanel(700, 400, new Vector2(x, y));
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

                    //Debug.WriteLine("AR: " + split[0] + "=" + split[1] + " " + frames + " frames mit " + fps + " fps");

                    // AnimationRenderer laden
                    AnimationRenderer.Load(split[0], split[1], frames, fps);
                }
                else if (split[0].Substring(0, 1).Equals("S"))
                {
                    //Debug.WriteLine("SR: " + split[0] + "=" + split[1]);

                    // StaticRenderer laden
                    StaticRenderer.Load(split[0], split[1]);
                }
            }

            // File schließen
            tr.Close();
        }

        public void UpdateGui()
        {
            uiInventarPanel.Update();
        }
        public void UpdateGame()
        {
            // Bildschirm Rectangle + 200 % in jede richtung
            UpdateRectangle = new Rectangle((int)(GameState.MapOffset.X - Configuration.GetInt("resolutionWidth") * 1), (int)(GameState.MapOffset.Y - Configuration.GetInt("resolutionHeight") * 1), (int)(Configuration.GetInt("resolutionWidth") * 3), (int)(Configuration.GetInt("resolutionHeight") * 3));

            // Enemies, SO in UdpateRect
            List<Enemy> enemies = GameState.QuadTreeEnemies.GetObjects(UpdateRectangle);
            List<StaticObject> staticObjects = GameState.QuadTreeStaticObjects.GetObjects(UpdateRectangle);
            List<Item> itemsOnScreen = GameState.QuadTreeItems.GetObjects(UpdateRectangle);

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

            // Itemkollision prüfen
            ItemColission();

            // Renderer des Players
            GameState.Player.Update();

            // StaticObjects Renderer updaten
            foreach (StaticObject s in staticObjects)
            {
                s.Renderer.Update();
            }

            // Items updaten
            foreach (Item i in itemsOnScreen)
            {
                i.LocationBehavior.Rotation = (float)(Main.GameTimeUpdate.TotalGameTime.TotalSeconds * 2) % MathHelper.TwoPi;
            }

            // Enemies updaten
            foreach (Enemy e in enemies)
            {
                e.Update();
            }

            // Linke Maustaste gedrückt
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                // Player schießt
                GameState.Player.Shoot();
            }

            // Player richtung des Maustzeigers schauen lassen
            GameState.Player.RelativeLookAt(new Vector2(Mouse.GetState().X, Mouse.GetState().Y));

            // AI can be updated
            AIThread.IsUpdating = true;

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

            if (Keyboard.GetState().IsKeyDown(Keys.D1) && GameState.Player.Reloading <= 0)
            {
                GameState.Player.ActiveShortcut = 1;

                Sound.Sounds["Weapon_Reload"].Play();
                GameState.Player.Reloading = (float)Sound.Sounds["Weapon_Reload"].Duration.TotalSeconds;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.D2) && GameState.Player.Reloading <= 0)
            {
                GameState.Player.ActiveShortcut = 2;

                Sound.Sounds["Weapon_Reload"].Play();
                GameState.Player.Reloading = (float)Sound.Sounds["Weapon_Reload"].Duration.TotalSeconds;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.R) && GameState.Player.Reloading <= 0)
            {
                GameState.Player.Weapon.Reload();

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



            if (newState.IsKeyDown(Keys.N))
            {
                if (showWaypoints)
                    showWaypoints = false;
                else
                    showWaypoints = true;
            }

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

            // TEST-ENDE
            // ###############################################################################
        }

        // ***************************************************************************
        // Update
        public override void Update()
        {
            updateDelegater();

            var newState = Keyboard.GetState();

            if (newState.IsKeyDown(Keys.I) && !oldKeyState.IsKeyDown(Keys.I))
            {
                if (uiInventarPanel.Visible)
                {
                    updateDelegater = UpdateGame;
                    uiInventarPanel.Visible = false;
                }
                else
                {
                    updateDelegater = UpdateGui;
                    uiInventarPanel.Visible = true;
                }


            }


            oldKeyState = newState;
        }

        // Colision mit items
        private void ItemColission()
        {
            // Items die mit dem spieler kollidieren
            List<Item> itemsInPlayer = GameState.QuadTreeItems.GetObjects(GameState.Player.Rect);

            foreach (Item i in itemsInPlayer)
            {
                if (i.GetType() == typeof(Powerup))
                {
                    GameState.Player.UsePowerup((Powerup)i);
                    GameState.QuadTreeItems.Remove(i);
                    continue;
                }

                // Item in inventar zufügem
                GameState.Player.AddItemToInventar(i);

                // Item aus quadtree löschen
                GameState.QuadTreeItems.Remove(i);
            }
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
                if (GameState.Player.MoveGameObject(moveVector))
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

            //Static Objects im Bild
            List<Item> itemsOnScreen = GameState.QuadTreeItems.GetObjects(screenRect);

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

            // Items zeichnen
            foreach (Item it in itemsOnScreen)
            {
                it.Renderer.Draw(spriteBatch, it.LocationBehavior);
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
                e.DrawHealthBar(spriteBatch);
            }

            // Player zeichnen mit verschiedenen Renderern (deswegen hat er ne eigene methode)
            GameState.Player.Draw(spriteBatch);


            // ################################################################################
            // ################################################################################
            // TEST

            if (showWaypoints)
                foreach (WayPoint w in GameState.Karte.WayPoints.Values)
                {
                    WayPoint.Renderer.Draw(spriteBatch, w.Location);
                    spriteBatch.DrawString(testFont, "" + w.ID, w.Location.RelativePosition, Color.Black);

                    foreach (WayPoint wDest in w.connectedPoints)
                    {
                        Draw2D.DrawLine(spriteBatch, 1, Color.Red, w.Location.RelativePosition, wDest.Location.RelativePosition);
                    }
                }

            if (shoting)
            {
                gun.Draw(spriteBatch, GameState.Player.LocationBehavior);
            }

            string munCount = "0";
            spriteBatch.DrawString(testFont, "Enemies: " + GameState.QuadTreeEnemies.Count + " FPS: " + (1 / Main.GameTimeDraw.ElapsedGameTime.TotalSeconds), new Vector2(0, 0), Color.Green);

            if (GameState.Player.Weapon.Munition != null)
                munCount = GameState.Player.Weapon.Munition.Count + "";

            spriteBatch.DrawString(testFont, "Munition: " + munCount, new Vector2(0, 30), Color.Red);
            spriteBatch.DrawString(testFont, "Health: " + GameState.Player.Health, new Vector2(0, 60), Color.Red);
            spriteBatch.DrawString(testFont, "Accu: " + GameState.Player.Weapon.Accuracy + " Kills: " + GameState.KilledMonsters, new Vector2(0, 90), Color.Blue);

            if (uiInventarPanel.Visible)
                uiInventarPanel.Draw(spriteBatch);

            // TEST-ENDE
            // ################################################################################
            // ################################################################################


            // Cursor zeichnen
            MouseCursor.DrawMouse(spriteBatch);

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
        // Prüfe ob Player an neue Position laufen darf
        public void KillEnemie(Enemy e)
        {
            // Killcounter erhöhen
            GameState.KilledMonsters = GameState.KilledMonsters + 1;

            //Test new StaticRenderer(blood)
            AnimationRenderer a = LoadedRenderer.GetAnimation("A_Splatter_01");
            a.PlayOnce();

            StaticObject splatter = new StaticObject(new MapLocation(e.LocationBehavior.Position), a);

            GameState.QuadTreeStaticObjects.Add(splatter);

            // FEHLER BEIM LÖSCHEN!!!!!!!!!!!!!!!!
            if (!GameState.QuadTreeEnemies.Remove(e))
            {
                Debug.WriteLine("Fehler beim löschen");
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

                // Prüfe kollision mit jedem schuss
                foreach (Shot s in tempList)
                {
                    // Shuss triff Gegner
                    if (s.PPCollisionWith(e))
                    {
                        // Gegner schaden zufügen
                        e.TakeDamage(s);

                        // Buffs des Schusses auf den Gegner übertragen
                        e.AddBuffs(s.Buffs);

                        //Debug.WriteLine("Health nach Schuss: " + e.Health);

                        GameState.ShotListVsEnemies.Remove(s);

                        continue;
                    }
                }

                // Töte Enemie - am besten in eigene Methode auslagern
                if (e.IsDead)
                {
                    KillEnemie(e);
                }
            }

        }

        // ***************************************************************************
        // Prüfe ob Player an neue Position laufen darf
        public static bool CheckRectangleInMap(Rectangle newPosition)
        {
            List<StaticObject> objects = Main.MainObject.GameManager.GameState.Karte.QuadTreeWalkable.GetObjects(newPosition);

            if (objects.Count <= 0)
                return true;
            else
                return false;
        }

        // ***************************************************************************
        // Prüfe ob zwei Punkte sichtkontakt haben (schießt sozusagen von Vector A nach Vector B un schaut ob er kollidiert.
        public static bool PointSeePoint(Vector2 start, Vector2 target)
        {
            return PointSeePoint(start, target, new Vector2(1, 1));
        }

        public static bool PointSeePoint(Vector2 start, Vector2 target, Vector2 size)
        {
            Vector2 direction = new Vector2(target.X - start.X, target.Y - start.Y);

            int steps = (int)Math.Sqrt(Math.Pow(direction.X, 2) + Math.Pow(direction.Y, 2));

            direction.Normalize();

            for (int i = 0; i < steps; i++)
            {
                start = start + direction;

                if (!CheckRectangleInMap(new Rectangle((int)(start.X - size.X / 2), (int)(start.Y - size.Y / 2), (int)size.X, (int)size.Y)))
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
