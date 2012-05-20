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
        private Texture2D hdmLogo;

        private SpriteFont fontDefault;
        private SpriteFont fontArialSmall;

        // Für weichen ÜBergang
        private float fadeIn = 1F;
        private float fadeTime = 0.25F;

        // Intro
        private float introTimer = 0;
        private float fadeHdm = 0;
        private float fadeText = 0;

        private Rectangle hdmPosition;
        private Vector2 textPosition;

        private string text;

        //
        private delegate void DrawSub(SpriteBatch sb);
        private DrawSub drawSub;

        private delegate void UpdateSub();
        private UpdateSub updateSub;

        public MenuManager()
        {
            LoadMenu();
        }

        // ***************************************************************************
        // Initialisierung von Variable, Laden der Texturen etc.
        public void LoadMenu()
        {
            Debug.WriteLine("Benötigter Content für Menu laden.");

            ContentManager content = Main.ContentManager;

            // Initialisierung
            UI.MouseCursor.CurrentCursor = UI.MouseCursor.DefaultCursor;
            drawSub = DrawIntro;
            updateSub = UpdateIntro;

            hdmPosition = new Rectangle();
            textPosition = new Vector2();

            text = "PROJEKT SS 2012 - MEDIENINFORMATIK ";

            // Läd Texturen die bnötigt werden
            background = content.Load<Texture2D>("images/menu/background");
            pixelWhite = content.Load<Texture2D>("images/pixelWhite");
            hdmLogo = content.Load<Texture2D>("images/hdm_transparent");

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
            fontArialSmall = content.Load<SpriteFont>("fonts/arialSmall");

            // Menü erzeugen
            menuPanel = new UIPanel(0, 0, new Vector2(0, 0));

            btnStart = new UIButton(new Vector2(99, 152), imgStart, imgStartHover);
            btnExit = new UIButton(new Vector2(849, 490), imgExit, imgExitHover);
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

        // ***************************************************************************
        // Updatemethode verweist nur auf das Delegate, welches dann konkret angibt was gemacht werden soll
        public override void Update()
        {
            updateSub();
        }

        // ***************************************************************************
        // Update des Startmenüs. Buttonlistener etc
        private void UpdateStartmenu()
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

        // ***************************************************************************
        // Update des Intro
        private void UpdateIntro()
        {
            // Zeit seit dem start des intros
            introTimer += (float)Main.GameTime.ElapsedGameTime.TotalSeconds;

            // Intro überspringen
            if (Mouse.GetState().LeftButton == ButtonState.Pressed || Mouse.GetState().RightButton == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape) || introTimer > 11)
            {
                Debug.WriteLine("Intro beendet");
                drawSub = DrawStartmenu;
                updateSub = UpdateStartmenu;
            }

            // Position von hdm logo berechnen
            float faktor = 5;
            hdmPosition.X = Configuration.GetInt("resolutionWidth") / 2 - (int)(hdmLogo.Width / faktor / 2);
            hdmPosition.Y = Configuration.GetInt("resolutionHeight") / 2 - (int)(hdmLogo.Height / faktor / 2);
            hdmPosition.Width = (int)(hdmLogo.Width / faktor);
            hdmPosition.Height = (int)(hdmLogo.Height / faktor);

            // Position von Text berechnen
            Vector2 textSize = fontArialSmall.MeasureString(text);
            textPosition.X = Configuration.GetInt("resolutionWidth") / 2 - (int)(textSize.X / 2);
            textPosition.Y = Configuration.GetInt("resolutionHeight") / 2 - (int)(textSize.Y / 2);

            // Fade HDM
            if (introTimer < 5)
                fadeHdm = MathHelper.Clamp(introTimer - 1, 0, 1);
            else
                fadeHdm = MathHelper.Clamp(1 - MathHelper.Clamp(introTimer - 5, 0, 1), 0, 1);

            // Fade Text
            if (introTimer < 10)
                fadeText = MathHelper.Clamp(introTimer - 6, 0, 1);
            else
                fadeText = MathHelper.Clamp(1 - MathHelper.Clamp(introTimer - 10, 0, 1), 0, 1);
        }

        // ***************************************************************************
        // Wären dem Laden wird eigentlich nicht viel gemacht..
        private void UpdateLoading()
        {
        }

        // ***************************************************************************
        // Drawmethode verweist nur auf das Delegate, welches dann konkret angibt was gemacht werden soll
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

            drawSub(spriteBatch);

            spriteBatch.End();
        }

        // ***************************************************************************
        // Zeichnet das "standard"menü, dass als erstes angezeigt wird
        public void DrawStartmenu(SpriteBatch spriteBatch)
        {
            // Hintergrund
            spriteBatch.Draw(background, new Rectangle(0, 0, Configuration.GetInt("resolutionWidth"), Configuration.GetInt("resolutionHeight")), Color.White);

            // Buttons zeichnen
            menuPanel.Draw(spriteBatch);

            // Maus zeichnen
            UI.MouseCursor.DrawMouse(spriteBatch);

            // Komplett schwarz um menü einzufaden
            spriteBatch.Draw(pixelWhite, new Rectangle(0, 0, Configuration.GetInt("resolutionWidth"), Configuration.GetInt("resolutionHeight")), Color.Black * fadeIn);
        }

        // ***************************************************************************
        // Zeichnet das intro
        public void DrawIntro(SpriteBatch spriteBatch)
        {
            // Komplett schwarz
            spriteBatch.Draw(pixelWhite, new Rectangle(0, 0, Configuration.GetInt("resolutionWidth"), Configuration.GetInt("resolutionHeight")), Color.Black);

            // HDM Logo
            spriteBatch.Draw(hdmLogo, hdmPosition, Color.White * fadeHdm);

            spriteBatch.DrawString(fontArialSmall, text, textPosition, Color.White * fadeText);
        }

        // ***************************************************************************
        // Zeichnet den loading bildschirm
        public void DrawLoading(SpriteBatch spriteBatch)
        {
            // Komplett schwarz
            spriteBatch.Draw(pixelWhite, new Rectangle(0, 0, Configuration.GetInt("resolutionWidth"), Configuration.GetInt("resolutionHeight")), Color.Black);

            string text = "loading";

            // Breite des texts
            Vector2 textSize = fontDefault.MeasureString(text);

            // Kleine animation. es blinken zwei punkte hinter dem loading schriftzug
            if (Main.GameTime.TotalGameTime.TotalSeconds % 3 < 1)
                text += "";
            else if (Main.GameTime.TotalGameTime.TotalSeconds % 3 < 2)
                text += ".";
            else
                text += "..";

            Vector2 position = new Vector2(Configuration.GetInt("resolutionWidth") / 2 - textSize.X / 2, Configuration.GetInt("resolutionHeight") / 2 - textSize.Y / 2);

            spriteBatch.DrawString(fontDefault, text, position, Color.White, 0F, new Vector2(0, 0), DrawHelper.Scale, SpriteEffects.None, 0);
        }

        // ***************************************************************************
        // Listener für die Buttons
        void UIActionListener.ActionEvent(UIElement element)
        {
            if (element == btnExit)
                Environment.Exit(0);
            else if (element == btnStart)
                startButtonEvent();
            else if (element == btnOptions)
                Debug.WriteLine("Option Button");
            else if (element == btnCredits)
                Debug.WriteLine("Credits Button");
        }

        // ***************************************************************************
        // Läd und erzeugt das eigentliche Spiel
        private void startButtonEvent()
        {
            Debug.WriteLine("Erstelle / Lade spiel (startButtonEvent)");

            drawSub = DrawLoading;
            updateSub = UpdateLoading;

            Main.MainObject.GameManager.Load();
        }
    }
}
