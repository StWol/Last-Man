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

namespace EVCS_Projekt.Managers
{
    public class MenuManager : Manager
    {
        private UIPanel menu;
        private Texture2D background;

        public MenuManager()
        {
            Load();
        }

        public void Load()
        {
            Debug.WriteLine("Benötigter Content für Menu laden.");

            ContentManager content = Main.ContentManager;

            // Läd Texturen die bnötigt werden
            background = content.Load<Texture2D>("images/menu/background");
        }

        public override void Update()
        {

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(background, new Rectangle(0,0, Configuration.GetInt("resolutionWidth"), Configuration.GetInt("resolutionHeight")), Color.White);

            spriteBatch.End();
        }
    }
}
