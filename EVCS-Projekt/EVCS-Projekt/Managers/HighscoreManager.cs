using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using EVCS_Projekt.Helper;
using Microsoft.Xna.Framework.Input;

namespace EVCS_Projekt.Managers
{
    public class HighscoreManager : Manager
    {
        private GameState _gameState;
        private Texture2D _pixelWhite, _highscore, _score;

        private delegate void UpdateDelegate();
        private delegate void DrawDelegate(SpriteBatch spriteBatch);

        private UpdateDelegate _updateDelegate;
        private DrawDelegate _drawDelegate;

        private SpriteFont _font;

        private Keys lastKey = Keys.None;
        public string _inputText = "";
        private int mousePressed = -1;

        private bool onlyHighscore = false;

        public HighscoreManager()
            : this(null, true)
        {
            onlyHighscore = true;
        }

        public HighscoreManager(GameState gameState)
            : this(gameState, false)
        {
        }

        private HighscoreManager(GameState gameState, bool onlyHighscore)
        {
            _gameState = gameState;

            _pixelWhite = Main.ContentManager.Load<Texture2D>("images/pixelWhite");
            _highscore = Main.ContentManager.Load<Texture2D>("images/highscore/highscore");
            _score = Main.ContentManager.Load<Texture2D>("images/highscore/score");

            if (!onlyHighscore)
            {
                _updateDelegate = UpdateScore;
                _drawDelegate = DrawScore;
            }
            else
            {
                _updateDelegate = UpdateHighscore;
                _drawDelegate = DrawHighscore;
            }

            _font = Main.ContentManager.Load<SpriteFont>(Configuration.Get("defaultFont"));

            // Koordinaten
            DrawHelper.AddDimension("Score_Name", 300, 411);
            DrawHelper.AddDimension("Score_Gesamt", 1, 175);
            DrawHelper.AddDimension("Score_LineHeight", 1, 60);
            DrawHelper.AddDimension("Score_XKoods", 160, 560);

            DrawHelper.AddDimension("Highscore_XKoods", 200, 500);
        }

        // ***********************************************************************

        private void ToMenu()
        {
            if (!onlyHighscore)
            {
                Main.MainObject.MenuManager = new MenuManager();
                Main.MainObject.GameManager = new GameManager();
            }
            Main.MainObject.CurrentManager = Main.MainObject.MenuManager;
        }

        private void UpdateHighscore()
        {
            // Text eingabe verwalten
            if (Keyboard.GetState().IsKeyUp(lastKey))
            {
                lastKey = Keys.None;
            }
            if (Keyboard.GetState().GetPressedKeys().Length > 0 && lastKey == Keys.None)
            {
                lastKey = Keyboard.GetState().GetPressedKeys()[0];
                if (lastKey == Keys.Enter || Mouse.GetState().LeftButton == ButtonState.Pressed)
                {
                    // Zu beenden
                    ToMenu();
                }
            }

            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                if (mousePressed == 0)
                    mousePressed = 1;
            }
            else
            {
                if (mousePressed == 1)
                {
                    // Zu beenden
                    ToMenu();
                }

                mousePressed = 0;
            }
        }

        private void DrawHighscore(SpriteBatch spriteBatch)
        {
            // background
            spriteBatch.Draw(_highscore, new Rectangle(0, 0, Configuration.GetInt("resolutionWidth"), Configuration.GetInt("resolutionHeight")), Color.White);

            // highscore anzeigen
            float lineHeight = DrawHelper.Get("Score_LineHeight").Y;
            float yPos = DrawHelper.Get("Score_Gesamt").Y;
            float xLeft = DrawHelper.Get("Highscore_XKoods").X;
            float xRight = DrawHelper.Get("Highscore_XKoods").Y;

            List<Score> highscoreList = HighscoreHelper.Top5;

            for (int i = 0; i < highscoreList.Count; i++)
            {
                spriteBatch.DrawString(_font, highscoreList[i].Name, new Vector2(xLeft, yPos + i * lineHeight), new Color(0, 0, 0, 200));
                spriteBatch.DrawString(_font, "" + highscoreList[i].Points, new Vector2(xRight, yPos + i * lineHeight), new Color(0, 0, 0, 200));
                spriteBatch.DrawString(_font, "Runde " + highscoreList[i].Round, new Vector2(xRight + xLeft / 3 * 2, yPos + i * lineHeight), new Color(0, 0, 0, 200));
            }

            for (int i = highscoreList.Count; i < 5; i++)
            {
                spriteBatch.DrawString(_font, "--", new Vector2(xLeft, yPos + i * lineHeight), new Color(0, 0, 0, 200));
                spriteBatch.DrawString(_font, "--", new Vector2(xRight, yPos + i * lineHeight), new Color(0, 0, 0, 200));
                spriteBatch.DrawString(_font, "--", new Vector2(xRight + xLeft / 2, yPos + i * lineHeight), new Color(0, 0, 0, 200));
            }
        }

        // ***********************************************************************

        private void UpdateScore()
        {
            // Text eingabe verwalten
            if (Keyboard.GetState().IsKeyUp(lastKey))
            {
                lastKey = Keys.None;
            }
            if (Keyboard.GetState().GetPressedKeys().Length > 0 && lastKey == Keys.None)
            {
                lastKey = Keyboard.GetState().GetPressedKeys()[0];
                if (lastKey == Keys.Back)
                {
                    if (_inputText.Length != 0)
                        _inputText = _inputText.Substring(0, _inputText.Length - 1);
                }
                else if (_inputText.Length < 16)
                {
                    char input = (char)lastKey.GetHashCode();
                    if (char.IsLetterOrDigit(input) || input == ' ')
                        _inputText += (char)lastKey.GetHashCode();
                }

                if (lastKey == Keys.Enter)
                {
                    // Highscore eintragen
                    Score newScore = new Score();
                    newScore.Name = _inputText;
                    newScore.Points = HighscoreHelper.Highscore;
                    newScore.Round = _gameState.Round;

                    HighscoreHelper.Add(newScore);

                    // Zu Highscore wechseln
                    _updateDelegate = UpdateHighscore;
                    _drawDelegate = DrawHighscore;
                }
            }
        }

        private void DrawScore(SpriteBatch spriteBatch)
        {
            // background
            spriteBatch.Draw(_score, new Rectangle(0, 0, Configuration.GetInt("resolutionWidth"), Configuration.GetInt("resolutionHeight")), Color.White);

            // string cursor anhängen
            string tempName = _inputText;
            if (Main.GameTimeDraw.TotalGameTime.TotalSeconds % 1 < 0.5)
                tempName += "|";

            // eingegebenen namen
            spriteBatch.DrawString(_font, tempName, DrawHelper.Get("Score_Name"), new Color(0, 0, 0, 200), 0, new Vector2(), 1.2F, SpriteEffects.None, 0);

            // highscore
            float lineHeight = DrawHelper.Get("Score_LineHeight").Y;
            float yPos = DrawHelper.Get("Score_Gesamt").Y;
            float xLeft = DrawHelper.Get("Score_XKoods").X;
            float xRight = DrawHelper.Get("Score_XKoods").Y;

            string highscoreText = "Score: " + HighscoreHelper.Highscore + "  Platz: " + HighscoreHelper.GetPosition(HighscoreHelper.Highscore) + "/" + (HighscoreHelper.HighscoreCount + 1);

            spriteBatch.DrawString(_font, highscoreText, new Vector2(Configuration.GetInt("resolutionWidth") / 2 - _font.MeasureString(highscoreText).X / 2, yPos), new Color(128, 0, 0, 200));

            spriteBatch.DrawString(_font, "Runde: " + _gameState.Round, new Vector2(xLeft, yPos + lineHeight), new Color(0, 0, 0, 200));

            spriteBatch.DrawString(_font, "Schaden bekommen: " + (long)_gameState.DamageTaken, new Vector2(xLeft, yPos + lineHeight * 2), new Color(0, 0, 0, 200));
            spriteBatch.DrawString(_font, "Schaden gegeben: " + (long)_gameState.DamageGiven, new Vector2(xRight, yPos + lineHeight * 2), new Color(0, 0, 0, 200));

            spriteBatch.DrawString(_font, "Schüsse: " + _gameState.Shots, new Vector2(xLeft, yPos + lineHeight * 3), new Color(0, 0, 0, 200));
            spriteBatch.DrawString(_font, "Gegner getötet: " + _gameState.TotalKilledMonsters, new Vector2(xRight, yPos + lineHeight * 3), new Color(0, 0, 0, 200));
        }

        /// ////////////////////////////////////////////////////////////

        public override void Update()
        {
            _updateDelegate();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

            spriteBatch.Draw(_pixelWhite, new Rectangle(0, 0, Configuration.GetInt("resolutionWidth"), Configuration.GetInt("resolutionHeight")), Color.Black);

            _drawDelegate(spriteBatch);

            spriteBatch.End();
        }
    }
}
