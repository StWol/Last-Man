using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;

using EVCS_Projekt.UI;
using EVCS_Projekt.Helper;

namespace EVCS_Projekt.Managers
{
    public class MenuManager : Manager, UIActionListener
    {
        private UIPanel menuPanel;
        private UIButton btnStart;
        private UIButton btnExit;
        private UIButton btnCredits;
        private UIButton btnOptions;

        private Texture2D background;
        private Texture2D pixelWhite;

        private SpriteFont fontDefault;

        // Für weichen ÜBergang
        private float fadeIn = 1F;
        private float fadeTime = 0.25F;

        public MenuManager()
        {
            LoadMenu();
        }

        public void LoadMenu()
        {
            Debug.WriteLine("Benötigter Content für Menu laden.");

            ContentManager content = Main.ContentManager;

            // Initialisierung
            UI.MouseCursor.CurrentCursor = UI.MouseCursor.DefaultCursor;

            // Läd Texturen die bnötigt werden
            background = content.Load<Texture2D>("images/menu/background");
            pixelWhite = content.Load<Texture2D>("images/pixelWhite");

            Texture2D imgStart = content.Load<Texture2D>("images/menu/start");
            Texture2D imgStartHover = content.Load<Texture2D>("images/menu/startH");

            Texture2D imgExit = content.Load<Texture2D>("images/menu/exit");
            Texture2D imgExitHover = content.Load<Texture2D>("images/menu/exitH");

            Texture2D imgCredits = content.Load<Texture2D>("images/menu/credits");
            Texture2D imgCreditsHover = content.Load<Texture2D>("images/menu/creditsH");

            Texture2D imgOptions = content.Load<Texture2D>("images/menu/options");
            Texture2D imgOptionsHover = content.Load<Texture2D>("images/menu/optionsH");

            // Fonts laden
            fontDefault = content.Load<SpriteFont>("fonts/defaultMedium");

            // Menü erzeugen
            menuPanel = new UIPanel(0, 0, new Vector2(0, 0));

            btnStart = new UIButton(new Vector2(99, 152), imgStart, imgStartHover);
            btnExit = new UIButton(new Vector2(649, 490), imgExit, imgExitHover);
            btnCredits = new UIButton(new Vector2(0, 507), imgCredits, imgCreditsHover);
            btnOptions = new UIButton(new Vector2(337, 209), imgOptions, imgOptionsHover);

            btnStart.AddActionListener(this);
            btnExit.AddActionListener(this);
            btnCredits.AddActionListener(this);
            btnOptions.AddActionListener(this);

            menuPanel.Add(btnStart);
            menuPanel.Add(btnExit);
            menuPanel.Add(btnCredits);
            menuPanel.Add(btnOptions);
        }

        public override void Update()
        {
            // Menubuttons updaten
            menuPanel.Update();

            // Weicher übergang
            if (fadeIn > 0)
            {
                fadeIn -= (1F / fadeTime) * (float)Main.GameTime.ElapsedGameTime.TotalSeconds;

                if (fadeIn < 0)
                    fadeIn = 0;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

            // Hintergrund
            spriteBatch.Draw(background, new Rectangle(0, 0, Configuration.GetInt("resolutionWidth"), Configuration.GetInt("resolutionHeight")), Color.White);

            // Buttons zeichnen
            menuPanel.Draw(spriteBatch);

            // Maus zeichnen
            UI.MouseCursor.DrawMouse(spriteBatch);

            // Komplett schwarz um menü einzufaden
            spriteBatch.Draw(pixelWhite, new Rectangle(0, 0, Configuration.GetInt("resolutionWidth"), Configuration.GetInt("resolutionHeight")), Color.Black * fadeIn);

            spriteBatch.DrawString(fontDefault, "test", new Vector2(0,0), Color.Blue, 0F, new Vector2(0,0), DrawHelper.Scale, SpriteEffects.None, 0);

            spriteBatch.End();
        }



        void UIActionListener.ActionEvent(UIElement element)
        {
            if (element == btnExit)
                Environment.Exit(0);
            else if (element == btnStart)
                Debug.WriteLine("Start Button");
            else if (element == btnOptions)
                Debug.WriteLine("Option Button");
            else if (element == btnCredits)
                Debug.WriteLine("Credits Button");
        }
    }
}
