using System;
using System.Diagnostics;
using EVCS_Projekt.AI;
using EVCS_Projekt.Audio;
using EVCS_Projekt.Helper;
using EVCS_Projekt.Managers;
using EVCS_Projekt.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace EVCS_Projekt
{
    public class Main : Game
    {
        // Attribute
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        /// <summary>
        ///   Aktueller GameManager
        /// </summary>
        public GameManager GameManager { get; set; }

        /// <summary>
        ///   Aktueller MenuManager
        /// </summary>
        public MenuManager MenuManager { get; set; }

        /// <summary>
        ///   Aktueller HighscoreManager
        /// </summary>
        public HighscoreManager Highscoremanager { get; set; }

        /// <summary>
        ///   Der aktive Manager, dessen Update- und Drawmethoden ausgeführt werden
        /// </summary>
        public Manager CurrentManager { private get; set; }

        // ############### Statische Attribute ###############

        /// <summary>
        ///   GameTime des letzten Update Durchlaufs
        /// </summary>
        public static GameTime GameTimeUpdate { get; private set; }

        /// <summary>
        ///   GameTime des letzten Draw Durchlaufs
        /// </summary>
        public static GameTime GameTimeDraw { get; private set; }

        /// <summary>
        ///   Referenz zu dem aktuellen Main-Objekt
        /// </summary>
        public static Main MainObject { get; private set; }

        /// <summary>
        ///   Referenz zu dem ContentManager
        /// </summary>
        public static ContentManager ContentManager { get; private set; }


        /// <summary>
        ///   Main-Methode. Hier startet das Spiel
        /// </summary>
        public Main()
        {
            // Das ist ein Test :-)
            Debug.WriteLine("Start Game..");

            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }


        /// <summary>
        ///   Initialisierung wichtiger Variablen und setzten der Auflösung
        /// </summary>
        protected override void Initialize()
        {
            // Load Config
            Configuration.LoadConfig();

            // Auflösung einstellen
            int resWidht = Configuration.GetInt("resolutionWidth");
            int resHeight = Configuration.GetInt("resolutionHeight");

            _graphics.PreferredBackBufferWidth = resWidht;
            _graphics.PreferredBackBufferHeight = resHeight;

            _graphics.IsFullScreen = Configuration.GetBool("isFullscreen");

            _graphics.PreferMultiSampling = Configuration.GetBool("antiAliasing");

            _graphics.ApplyChanges();

            // Helper updaten
            DrawHelper.Callculate();

            base.Initialize();
        }


        /// <summary>
        ///   Laden der ManagerKlassen
        /// </summary>
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Main + ContentManager für static Zugriff
            MainObject = this;
            ContentManager = Content;

            // Mauszeiger laden
            MouseCursor.Load();

            // Music laden
            MusicPlayer.LoadSongs();

            // Manager erzeugen
            MenuManager = new MenuManager();
            GameManager = new GameManager();

            // CurrentManager ist standardmäßig das Menu
            CurrentManager = MenuManager;

            // Musik starten
            MusicPlayer.Play(MusicPlayer.LookBehind);
        }


        protected override void UnloadContent()
        {
        }


        /// <summary>
        ///   Von hier aus wird der Updatevorgang des aktuellen Managers angestoßen
        /// </summary>
        /// <param name="gameTime"> Aktuelle GameTime </param>
        protected override void Update(GameTime gameTime)
        {
            //GameTime speichern um für alle zugänglich zu machen
            GameTimeUpdate = gameTime;

            // Wenn Spielfenster keinen Focus hat, wird kein updatevorgang ausgeführt
            if (IsActive)
                // Manager updaten
                CurrentManager.Update();

            base.Update(gameTime);
        }


        /// <summary>
        ///   Von hier aus wird der Drawvorgang des aktuellen Managers angestoßen
        /// </summary>
        /// <param name="gameTime"> Aktuelle GameTime </param>
        protected override void Draw(GameTime gameTime)
        {
            //GameTime speichern um für alle zugänglich zu machen
            GameTimeDraw = gameTime;

            // Alles weis
            GraphicsDevice.Clear(Color.White);

            // currentmanagaer draw
            CurrentManager.Draw(_spriteBatch);
            
            base.Draw(gameTime);
        }


        /// <summary>
        ///   Beim schließen des Fensters alle laufenden Threads beenden
        /// </summary>
        protected override void OnExiting(Object sender, EventArgs args)
        {
            base.OnExiting(sender, args);

            Debug.WriteLine("Laufende Threads beenden");

            // Stop the threads
            AIThread.Running = false;
        }
    }
}