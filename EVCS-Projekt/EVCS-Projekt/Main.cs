using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using System.Diagnostics;

using EVCS_Projekt.Managers;
using EVCS_Projekt.Helper;
using EVCS_Projekt.Objects;
using EVCS_Projekt.Audio;
using EVCS_Projekt.Helper.XMLManager;

namespace EVCS_Projekt
{
    public class Main : Microsoft.Xna.Framework.Game
    {
        // Attribute
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public GameManager GameManager { get; private set; }
        public MenuManager MenuManager { get; private set; }

        public Manager CurrentManager { private get; set; }

        // Statische Attribute
        public static GameTime GameTimeUpdate { get; private set; }
        public static GameTime GameTimeDraw { get; private set; }
        public static Main MainObject { get; private set; }
        public static ContentManager ContentManager { get; private set; }

        public Main()
        {

            // Das ist ein Test :-)
            Debug.WriteLine("Start Game..");

            Debug.WriteLine(Math.Tan(45F * Math.PI / 180));

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }


        protected override void Initialize()
        {
            // Load Config
            Configuration.LoadConfig();

            // Auflösung einstellen
            int resWidht = Configuration.GetInt("resolutionWidth");
            int resHeight = Configuration.GetInt("resolutionHeight");

            graphics.PreferredBackBufferWidth = resWidht;
            graphics.PreferredBackBufferHeight = resHeight;

            graphics.IsFullScreen = Configuration.GetBool("isFullscreen");

            graphics.PreferMultiSampling = Configuration.GetBool("antiAliasing");
           
            graphics.ApplyChanges();

            // Helper updaten
            DrawHelper.Callculate();

            base.Initialize();
        }


        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Main + ContentManager für static Zugriff
            MainObject = this;
            ContentManager = Content;

            // Mauszeiger laden
            UI.MouseCursor.Load();

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


        protected override void Update(GameTime gameTime)
        {
            //GameTime speichern um für alle zugänglich zu machen
            GameTimeUpdate = gameTime;

            // Wenn Spielfenster keinen Focus hat, wird kein updatevorgang ausgeführt
            if (this.IsActive)
                // Manager updaten
                CurrentManager.Update();

            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            //GameTime speichern um für alle zugänglich zu machen
            GameTimeDraw = gameTime;

            // Alles weis
            GraphicsDevice.Clear(Color.White);

            // currentmanagaer draw
            CurrentManager.Draw(spriteBatch);

            base.Draw(gameTime);
        }
    }
}
