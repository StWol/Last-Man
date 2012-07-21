using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LastMan.UI
{
    class MouseCursor
    {
        public static Texture2D NoCursor { get; private set; }
        public static Texture2D DefaultCursor { get; private set; }
        public static Texture2D Cross_01 { get; private set; }

        public static Texture2D CurrentCursor { private get; set; }

        public static void DrawMouse(SpriteBatch sb)
        {
            int x = Mouse.GetState().X-CurrentCursor.Width / 2;
            int y = Mouse.GetState().Y-CurrentCursor.Height / 2;
            sb.Draw(CurrentCursor, new Vector2(x, y), Color.White);
        }

        public static void Load()
        {
            // ContentManager holen
            ContentManager content = Main.ContentManager;

            // Cursor laden
            NoCursor = content.Load<Texture2D>("images/pixelTransparent");
            DefaultCursor = content.Load<Texture2D>("images/mouse/default");
            Cross_01 = content.Load<Texture2D>("images/mouse/cross_01");

            CurrentCursor = NoCursor;
        }
    }
}
