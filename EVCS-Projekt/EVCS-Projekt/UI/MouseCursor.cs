using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace EVCS_Projekt.UI
{
    class MouseCursor
    {
        public static Texture2D NoCursor { get; private set; }
        public static Texture2D DefaultCursor { get; private set; }

        public static Texture2D CurrentCursor { private get; set; }

        public static void DrawMouse(SpriteBatch sb)
        {
            sb.Draw(CurrentCursor, new Vector2(Mouse.GetState().X, Mouse.GetState().Y), Color.White);
        }

        public static void Load()
        {
            // ContentManager holen
            ContentManager content = Main.ContentManager;

            // Cursor laden
            NoCursor = content.Load<Texture2D>("images/blankPixel");
            DefaultCursor = content.Load<Texture2D>("images/mouse/default");

            CurrentCursor = NoCursor;
        }
    }
}
