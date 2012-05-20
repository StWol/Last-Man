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
        public static GameTime GameTime { get; private set; }
        public static Main MainObject { get; private set; }
        public static ContentManager ContentManager { get; private set; }

        public Main()
        {

            // Das ist ein Test :-)
            Debug.WriteLine("Start Game..");
            
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

            // Manager erzeugen
            MenuManager = new MenuManager();
            GameManager = new GameManager();

            // CurrentManager ist standardmäßig das Menu
            CurrentManager = MenuManager;
        }


        protected override void UnloadContent()
        {

        }


        protected override void Update(GameTime gameTime)
        {
            //GameTime speichern um für alle zugänglich zu machen
            GameTime = gameTime;

            // Manager updaten
            CurrentManager.Update();

            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            //GraphicsDevice.Clear(Color.CornflowerBlue);

            // currentmanagaer draw
            CurrentManager.Draw(spriteBatch);

            base.Draw(gameTime);
        }
    }
}
