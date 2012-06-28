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
        private UpdateDel updateGamePlayDelegator;

        // Für Gui
        private Texture2D gui_overlay, health_bar, gui_backlayer;
        private float health_bar_height { get; set; }
        private SpriteFont defaultFont;
        private SpriteFont defaultFontBig;

        private int buffIconPulse = 0;

        // Fader
        private float _fadeToBlack = 0;

        private bool gameover = false;
        private Texture2D gameoverTexture;

        // Tests
        private Texture2D test;
        private SpriteFont testFont;
        Texture2D monster3;
        Texture2D shot, blood, shot_01;

        SoundEffect headshot;
        private InventarPanel inventarPanel;
        private Constructor constructorPanel;
        bool showWaypoints = false;


        // ***************************************************************************
        // Läd den ganzen Stuff, den der GameManager benötigt
        public void Load()
        {
            Debug.WriteLine("Läd das eigentliche Spiel");

            // Update Delegator setzen
            updateDelegater = UpdateGame;
            updateGamePlayDelegator = UpdateGamePlay;

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

            GameState.KilledMonsters = new Dictionary<EEnemyType, int>();
            foreach (EEnemyType e in Enum.GetValues(typeof(EEnemyType)))
                GameState.KilledMonsters.Add(e, 0);

            // Werte für Runde
            GameState.RoundDelay = 2;
            GameState.Round = 1;
            GameState.RoundIsRunning = false;
            GameState.RoundStartTime = new Dictionary<int, double>();
            GameState.RoundEndTime = new Dictionary<int, double>();

            GameState.TimeToRoundStart = GameState.RoundDelay;

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

            // GUI elemente
            gui_overlay = Main.ContentManager.Load<Texture2D>("images/gui/gui_overlay");
            health_bar = Main.ContentManager.Load<Texture2D>("images/gui/health_bar");
            gui_backlayer = Main.ContentManager.Load<Texture2D>("images/gui/gui_backlayer");

            gameoverTexture = Main.ContentManager.Load<Texture2D>("images/gameover");

            defaultFont = Main.ContentManager.Load<SpriteFont>(Configuration.Get("defaultFont"));
            defaultFontBig = Main.ContentManager.Load<SpriteFont>(Configuration.Get("defaultFontBig"));

            // Posi für Gui
            DrawHelper.AddDimension("HealthBar_Position", 184, 425);
            DrawHelper.AddDimension("HealthBar_Size", 33, 153);

            DrawHelper.AddDimension("Munition_Position", 235, 535);

            DrawHelper.AddDimension("BuffIcon_Position", 16, 365);
            DrawHelper.AddDimension("BuffIcon_Size", 32, 32);

            DrawHelper.AddDimension("Minimap_Size", 181, 170);
            DrawHelper.AddDimension("Minimap_Position", 0, 407);


            // AI Thread starten
            new Thread(new ThreadStart(AIThread.UpdateAI)).Start();

            //User Interface erstellen
            InitGui();

            // Enemies laden
            Enemy.Load();

            // Maus auf fadenkreuz setzten
            MouseCursor.CurrentCursor = MouseCursor.Cross_01;

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

            // Testtextur
            test = Main.ContentManager.Load<Texture2D>("images/pixelWhite");
            testFont = Main.ContentManager.Load<SpriteFont>("fonts/arialSmall");


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
            GameState.Player.AddItemToInventar(Item.AllItems[101]);
            GameState.Player.AddItemToInventar(Item.AllItems[102]);
            GameState.Player.AddItemToInventar(Item.AllItems[103]);


            GameState.Player.AddItemToInventar(Item.AllItems[200]);
            GameState.Player.AddItemToInventar(Item.AllItems[201]);


            GameState.Player.AddItemToInventar(Item.AllItems[300]);
            GameState.Player.AddItemToInventar(Item.AllItems[301]);
            GameState.Player.AddItemToInventar(Item.AllItems[302]);
            GameState.Player.AddItemToInventar(Item.AllItems[303]);
            GameState.Player.AddItemToInventar(Item.AllItems[304]);
            GameState.Player.AddItemToInventar(Item.AllItems[305]);

            GameState.Player.AddItemToInventar(Item.AllItems[400]);
            GameState.Player.AddItemToInventar(Item.AllItems[401]);
            GameState.Player.AddItemToInventar(Item.AllItems[402]);
            GameState.Player.AddItemToInventar(Item.AllItems[403]);
            GameState.Player.AddItemToInventar(Item.AllItems[404]);
            GameState.Player.AddItemToInventar(Item.AllItems[405]);
            GameState.Player.AddItemToInventar(Item.AllItems[406]);

            GameState.Player.AddItemToInventar(Item.AllItems[500]);
            GameState.Player.AddItemToInventar(Item.AllItems[501]);
            GameState.Player.AddItemToInventar(Item.AllItems[502]);
            GameState.Player.AddItemToInventar(Item.AllItems[503]);
            GameState.Player.AddItemToInventar(Item.AllItems[504]);
            GameState.Player.AddItemToInventar(Item.AllItems[505]);
            GameState.Player.AddItemToInventar(Item.AllItems[506]);

            // TEST ENDE
            // ################################################################################

            inventarPanel = new InventarPanel(760, 400, new Vector2(1024 / 2 - 760 / 2, 576 / 2 - 400 / 2));
            constructorPanel = new Constructor(760, 400, new Vector2(1024 / 2 - 760 / 2, 576 / 2 - 400 / 2));
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
            if (inventarPanel.Visible)
                inventarPanel.Update();
            if (constructorPanel.Visible)
                constructorPanel.Update();
        }

        public void UpdateRoundLogic()
        {
            if (GameState.RoundIsRunning)
            {
                // Wenn alle Monster getötet sind TImer wieder starten
                if (GameState.KillsToEndRound <= 0)
                {
                    // Runde beenden
                    GameState.RoundIsRunning = false;

                    // Roundenendzeit speichern
                    GameState.RoundEndTime.Add(GameState.Round, Main.GameTimeUpdate.TotalGameTime.TotalSeconds);

                    // Runde erhöhen
                    GameState.Round++;

                    // Timer setzten
                    GameState.TimeToRoundStart = GameState.RoundDelay;


                }
            }
            else
            {
                // Timer rutnerzählen
                if (GameState.TimeToRoundStart > 0)
                {
                    GameState.TimeToRoundStart -= (float)Main.GameTimeUpdate.ElapsedGameTime.TotalSeconds;
                }
                else
                {
                    // Runde starten
                    GameState.RoundIsRunning = true;

                    // Monstercount für die runden
                    GameState.MonsterSpawnCount = GameState.Round * 10;

                    // KillsToEndRound setzten
                    GameState.KillsToEndRound = GameState.MonsterSpawnCount;

                    // Roundenstartzeit speichern
                    GameState.RoundStartTime.Add(GameState.Round, Main.GameTimeUpdate.TotalGameTime.TotalSeconds);
                }
            }
        }

        public void UpdateGamePlay()
        {
            // Bildschirm Rectangle + 200 % in jede richtung
            UpdateRectangle = new Rectangle((int)(GameState.MapOffset.X - Configuration.GetInt("resolutionWidth") * 1), (int)(GameState.MapOffset.Y - Configuration.GetInt("resolutionHeight") * 1), (int)(Configuration.GetInt("resolutionWidth") * 3), (int)(Configuration.GetInt("resolutionHeight") * 3));


            // Alle Gegner im SPiel. damit diese auch zum spieler laufen (wir begrenzen die gegner anzahl,
            // anstatt die ein rect des quadtrees
            List<Enemy> enemies = GameState.QuadTreeEnemies.GetAllObjects();

            // SO in UdpateRect
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
                // Items rotieren lassen
                if (i.GetType() != typeof(Liquid))
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

            // healthbar_rect berechnen
            health_bar_height = DrawHelper.Get("HealthBar_Size").Y / GameState.Player.MaxHealth * GameState.Player.Health;

            // Pulls für bufficons
            buffIconPulse = (int)((Main.GameTimeUpdate.TotalGameTime.TotalSeconds % 0.8F) / 0.2F);

            // logic der round updaten
            UpdateRoundLogic();

            // Spieler stirbt
            if (!GameState.Player.IsAlive)
            {
                PlayerDied();
            }

            // Anfang das Bild einfaden
            _fadeToBlack = MathHelper.Clamp(_fadeToBlack + (float)Main.GameTimeUpdate.ElapsedGameTime.TotalSeconds, 0, 1);

            // ################################################################################
            // ################################################################################
            // ################################################################################
            // TEST

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

            if (newState.IsKeyDown(Keys.M))
            {
                if (GameState.QuadTreeEnemies.GetAllObjects().Count > 0)
                {
                    Enemy e = GameState.QuadTreeEnemies.GetAllObjects()[0];

                    KillEnemie(e);

                }
            }

            if (newState.IsKeyDown(Keys.B))
            {
                GameState.Player.Health = 0;
            }


            

            // TEST-ENDE
            // ###############################################################################
        }

        // ***************************************************************************
        // Player stirbt
        private void PlayerDied()
        {
            // Update umbiegen
            updateDelegater = UpdatePlayerIsDead;

            // AiThread beenden
            AIThread.Running = false;

            // maus verstecken
            MouseCursor.CurrentCursor = MouseCursor.NoCursor;

            // player füße verstecken
            GameState.Player.IsMoving = false;

            // Gameover setzten
            gameover = true;
        }

        // timer bis zum managerswitch
        private float timeUntilHighscoreManager = 5;

        // ***************************************************************************
        // Update sobald der spieler gestorben ist
        public void UpdatePlayerIsDead()
        {
            // Schüsse weiter fliegen lassen
            UpdateShots();

            // Enemies updaten (nur Renderer)
            foreach (Enemy e in GameState.QuadTreeEnemies.GetAllObjects())
            {
                e.HasMoved = false;
                e.Renderer.Update();
            }

            GameState.Player.Renderer.Update();

            timeUntilHighscoreManager -= (float)Main.GameTimeUpdate.ElapsedGameTime.TotalSeconds;

            // Rendererswitch
            if (timeUntilHighscoreManager < 0)
            {

                // gamestate mitgeben
                Main.MainObject.Highscoremanager = new HighscoreManager(GameState);

                Main.MainObject.CurrentManager = Main.MainObject.Highscoremanager;
            }

            // Bild ausfaden
            _fadeToBlack = MathHelper.Clamp(timeUntilHighscoreManager, 0, 1);
        }

        // ***************************************************************************
        // Update für das eigentliche spiel
        public void UpdateGame()
        {
            updateGamePlayDelegator();

            var newState = Keyboard.GetState();

            if ( newState.IsKeyDown( Keys.I ) && !oldKeyState.IsKeyDown( Keys.I ) && !constructorPanel.NameIsActive )
            {
                if (constructorPanel.Visible)
                {
                    constructorPanel.Visible = false;
                }
                if (inventarPanel.Visible)
                {
                    updateGamePlayDelegator = UpdateGamePlay;
                    inventarPanel.Visible = false;
                }
                else
                {
                    updateGamePlayDelegator = UpdateGui;
                    inventarPanel.Visible = true;
                }
            }
            if (newState.IsKeyDown(Keys.K) && !oldKeyState.IsKeyDown(Keys.K) && !constructorPanel.NameIsActive)
            {
                if (inventarPanel.Visible)
                {
                    inventarPanel.Visible = false;
                }
                if (constructorPanel.Visible)
                {
                    updateGamePlayDelegator = UpdateGamePlay;
                    constructorPanel.Visible = false;
                }
                else
                {
                    updateGamePlayDelegator = UpdateGui;
                    constructorPanel.Visible = true;
                }
            }
            oldKeyState = newState;
        }

        // ***************************************************************************
        // Update
        public override void Update()
        {
            updateDelegater();
        }

        // Colision mit items
        private void ItemColission()
        {
            // Items die mit dem spieler kollidieren
            List<Item> itemsInPlayer = GameState.QuadTreeItems.GetObjects(GameState.Player.Rect);

            foreach (Item i in itemsInPlayer)
            {
                // Auf Liquid testen
                if (i.GetType() == typeof(Liquid))
                {
                    // Liquid adden
                    GameState.Player.AddLiquid(((Liquid)i).TypeOfLiquid, ((Liquid)i).Amount);

                    // Item aus quadtree löschen
                    GameState.QuadTreeItems.Remove(i);
                }
                else
                {
                    // Item in inventar zufügem
                    GameState.Player.AddItemToInventar(i);

                    // Item aus quadtree löschen
                    GameState.QuadTreeItems.Remove(i);
                }
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
                if (GameState.Player.MoveGameObject(moveVector, true, false))
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

            // Bildschirm Rectangle
            Rectangle screenRect = new Rectangle((int)GameState.MapOffset.X, (int)GameState.MapOffset.Y, Configuration.GetInt("resolutionWidth"), Configuration.GetInt("resolutionHeight"));

            // Screen karrieren
            int karoSize = 20;
            for (int i = 0; i <= Configuration.GetInt("resolutionWidth") / karoSize; i++)
            {
                Draw2D.DrawLine(spriteBatch, 1, Color.LightGray, new Vector2(i * karoSize - GameState.MapOffset.X % karoSize, 0), new Vector2(i * karoSize - GameState.MapOffset.X % karoSize, Configuration.GetInt("resolutionHeight")));
            }
            for (int i = 0; i <= Configuration.GetInt("resolutionHeight") / karoSize; i++)
            {
                Draw2D.DrawLine(spriteBatch, 1, Color.LightGray, new Vector2(0, i * karoSize - GameState.MapOffset.Y % karoSize), new Vector2(Configuration.GetInt("resolutionWidth"), i * karoSize - GameState.MapOffset.Y % karoSize));
            }

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
                ILocationBehavior temp = it.LocationBehavior.Clone();
                temp.Size = new Vector2(32, 32);
                it.Renderer.Draw(spriteBatch, temp);
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

            // Hud zeichnen
            DrawHUD(spriteBatch);

            // DrawRoundinfos
            DrawRoundinfos(spriteBatch);

            // inventar zeichnen
            if (inventarPanel.Visible)
            {
                spriteBatch.Draw(PixelWhite, new Rectangle(0, 0, Configuration.GetInt("resolutionWidth"), Configuration.GetInt("resolutionHeight")), new Color(0, 0, 0, 128));

                inventarPanel.Draw(spriteBatch);
            }







            // ################################################################################
            // ################################################################################
            // TEST
            //int x = (int)(GameState.Karte.Minimap.Width / GameState.MapSize.X * (GameState.MapOffset.X+295));
            //int y = (int)(GameState.Karte.Minimap.Height / GameState.MapSize.Y * (GameState.MapOffset.Y+1094));
            //spriteBatch.Draw( GameState.Karte.Minimap, new Rectangle(0,0,200,100), new Rectangle(x,y,200,100), Color.White );

            /*foreach (Enemy e in GameState.QuadTreeEnemies.GetAllObjects())
            {
                Draw2D.DrawLine(spriteBatch, 1, Color.Red, e.LocationBehavior.RelativePosition, GameState.Player.LocationBehavior.RelativePosition);
            }*/

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

            spriteBatch.DrawString(testFont, "Enemies: " + GameState.QuadTreeEnemies.Count + " FPS: " + (1 / Main.GameTimeDraw.ElapsedGameTime.TotalSeconds), new Vector2(0, 0), Color.Green);

            spriteBatch.DrawString(testFont, "Liquids: " + GameState.Player.Liquids + " Highscore: " + HighscoreHelper.Highscore, new Vector2(0, 30), Color.Red);
            spriteBatch.DrawString(testFont, "Health: " + GameState.Player.Health, new Vector2(0, 60), Color.Red);
            spriteBatch.DrawString(testFont, "TimeToRoundStart: " + GameState.TimeToRoundStart + " Kills: " + GameState.TotalKilledMonsters, new Vector2(0, 90), Color.Blue);

            // Mapinfos
            /*foreach (StaticObject so in GameState.Karte.QuadTreeWalkable.GetObjects(screenRect))
            {
                spriteBatch.DrawString(defaultFont, so.LocationBehavior.RelativeBoundingBox+"\n"+so.LocationBehavior.RelativePosition+"\n"+so.LocationBehavior.Position, so.LocationBehavior.RelativePosition, Color.Magenta);
            }*/

            if (constructorPanel.Visible)
            {
                spriteBatch.Draw(PixelWhite, new Rectangle(0, 0, Configuration.GetInt("resolutionWidth"), Configuration.GetInt("resolutionHeight")), new Color(0, 0, 0, 128));
                constructorPanel.Draw(spriteBatch);
            }


            // TEST-ENDE
            // ################################################################################
            // ################################################################################

            // Gameover
            if (gameover)
                spriteBatch.Draw(gameoverTexture, new Rectangle(0, 0, Configuration.GetInt("resolutionWidth"), Configuration.GetInt("resolutionHeight")), new Color(255, 255, 255, (int)(255 - 255 * MathHelper.Clamp(timeUntilHighscoreManager - 3, 0, 1))));

            // FadeToBlack
            spriteBatch.Draw(PixelWhite, new Rectangle(0, 0, Configuration.GetInt("resolutionWidth"), Configuration.GetInt("resolutionHeight")), new Color(0, 0, 0, (int)(255 - 255 * _fadeToBlack)));

            // Cursor zeichnen
            MouseCursor.DrawMouse(spriteBatch);

            spriteBatch.End();
        }

        //**********************************************************************************
        // Zeichne die Rundeninfos
        private void DrawRoundinfos(SpriteBatch spriteBatch)
        {

            if (!GameState.RoundIsRunning || Main.GameTimeUpdate.TotalGameTime.TotalSeconds - GameState.RoundStartTime[GameState.Round] < 3)
            {
                string info = "";

                if (GameState.RoundEndTime.ContainsKey(GameState.Round - 1) && Main.GameTimeUpdate.TotalGameTime.TotalSeconds - GameState.RoundEndTime[GameState.Round - 1] < 3)
                {
                    info = "Runde " + (GameState.Round - 1) + " beendet..";
                }
                else
                {

                    if (!GameState.RoundIsRunning)
                    {
                        if (GameState.TimeToRoundStart < 1)
                            info = "1";
                        else if (GameState.TimeToRoundStart < 2)
                            info = "2";
                        else if (GameState.TimeToRoundStart < 3)
                            info = "3";
                        else if (GameState.TimeToRoundStart < 4)
                            info = "4";
                        else if (GameState.TimeToRoundStart < 5)
                            info = "5";
                    }
                    else
                    {
                        info = "Runde " + GameState.Round + " beginnt..";
                    }
                }

                Vector2 size = defaultFontBig.MeasureString(info);
                Vector2 center = new Vector2(Configuration.GetInt("resolutionWidth") / 2, Configuration.GetInt("resolutionHeight") / 2);
                Vector2 origin = new Vector2(size.X / 2, size.Y / 2);

                spriteBatch.DrawString(defaultFontBig, info, center, Color.Black, 0, origin, 1.025F, SpriteEffects.None, 0);
                spriteBatch.DrawString(defaultFontBig, info, center, Color.Gray, 0, origin, 1, SpriteEffects.None, 0);
            }

        }


        //**********************************************************************************
        // Zeichne die Oberfläche
        private void DrawHUD(SpriteBatch spriteBatch)
        {
            // backlayer
            spriteBatch.Draw(gui_backlayer, new Rectangle(0, 0, Configuration.GetInt("resolutionWidth"), Configuration.GetInt("resolutionHeight")), Color.White);

            // GUI zeichnen
            Vector2 hb_position = DrawHelper.Get("HealthBar_Position");
            Vector2 hb_size = DrawHelper.Get("HealthBar_Size");

            // Minimap zeichnen - Korrektur der Map x:-7 y:34
            Vector2 mm_size = DrawHelper.Get("Minimap_Size");
            Vector2 mm_position = DrawHelper.Get("Minimap_Position");

            int x = (int)((GameState.MapOffset.X * 0.2) + 5 );
            int y = (int)(GameState.MapOffset.Y * 0.2 - 34);

            spriteBatch.Draw(GameState.Karte.Minimap, new Rectangle((int)mm_position.X, (int)mm_position.Y, (int)mm_size.X, (int)mm_size.Y), new Rectangle(x, y, 181, 170), Color.White);


            // healthbar
            spriteBatch.Draw(health_bar,
                new Rectangle((int)hb_position.X, (int)(hb_position.Y + (hb_size.Y - health_bar_height)), (int)hb_size.X, (int)hb_size.Y),
                //new Rectangle((int)hb_position.X, (int)(hb_position.Y + (hb_size.Y - health_bar_height)), (int)hb_size.X, (int)(health_bar_height)),
                //new Rectangle(0, (int)(hb_size.Y - health_bar_height), (int)hb_size.X, (int)(health_bar_height)), 
                Color.Red);

            // Munition
            string munCount = "--";
            if (GameState.Player.Weapon.Munition != null)
                munCount = GameState.Player.Weapon.Munition.Count + "";

            spriteBatch.DrawString(defaultFont, munCount, DrawHelper.Get("Munition_Position"), Color.Black);

            // Buff Icons zeichnen
            Vector2 bi_position = DrawHelper.Get("BuffIcon_Position");
            Vector2 bi_size = DrawHelper.Get("BuffIcon_Size");

            foreach (Buff b in GameState.Player.Buffs.Values)
            {
                // Transparentes Icon
                spriteBatch.Draw(Buff.BuffIcons[b.Type], new Rectangle((int)bi_position.X - buffIconPulse, (int)bi_position.Y - buffIconPulse, (int)bi_size.X + 2 * buffIconPulse, (int)bi_size.Y + 2 * buffIconPulse), new Color(128, 128, 128, 128));

                int height = (int)((bi_size.Y + 2 * buffIconPulse) / b.FullDuration * b.Duration);
                int height_ = (int)((bi_size.Y) / b.FullDuration * b.Duration);

                // Icon mit voller Farbe
                spriteBatch.Draw(Buff.BuffIcons[b.Type],
                    new Rectangle((int)bi_position.X - buffIconPulse, (int)bi_position.Y - buffIconPulse + (int)((bi_size.Y + 2 * buffIconPulse) - height), (int)bi_size.X + 2 * buffIconPulse, (int)height),
                    new Rectangle(0, (int)(bi_size.Y - height_), (int)bi_size.X, (int)height_),
                    Color.White);

                // Position weiterrücken
                bi_position.X = bi_position.X + bi_size.X * 1.5F;
            }

            // Overlay zeichnen
            spriteBatch.Draw(gui_overlay, new Rectangle(0, 0, Configuration.GetInt("resolutionWidth"), Configuration.GetInt("resolutionHeight")), Color.White);
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
            GameState.KilledMonsters[e.TypOfEnemy]++;

            //Test new StaticRenderer(blood)
            AnimationRenderer a = LoadedRenderer.GetAnimation("A_Splatter_01");
            a.PlayOnce();

            // Splatter erstellen und in quadtree einfügen
            StaticObject splatter = new StaticObject(new MapLocation(e.LocationBehavior.Position), a);
            GameState.QuadTreeStaticObjects.Add(splatter);

            // KillsToEndRound - 1
            GameState.KillsToEndRound--;

            // Liquid droppen
            ELiquid droppedLiquidType = ELiquid.Green;
            switch (e.TypOfEnemy)
            {
                //case EEnemyType.E1:
                //case EEnemyType.E2:
                //    droppedLiquidType = ELiquid.Green;
                //    break;
                case EEnemyType.E3:
                case EEnemyType.E4:
                    droppedLiquidType = ELiquid.Blue;
                    break;
                case EEnemyType.E5:
                case EEnemyType.E6:
                    droppedLiquidType = ELiquid.Red;
                    break;
            }

            Random r = new Random();
            Liquid droppedLiquid = Liquid.Get(droppedLiquidType, r.Next(1, 5));
            droppedLiquid.LocationBehavior = new MapLocation(e.LocationBehavior.Position);
            droppedLiquid.LocationSizing();

            // Liquid den items hinzufügen
            GameState.QuadTreeItems.Add(droppedLiquid);

            // Remove enemie
            if (!GameState.QuadTreeEnemies.Remove(e))
            {
                Debug.WriteLine("Fehler beim löschen eines Gegners");
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
