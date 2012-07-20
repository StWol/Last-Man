using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LastMan.Helper
{
    public class Draw2D
    {
        private static Texture2D blank = null;


        public static void DrawLine(SpriteBatch batch, float width, Color color, Vector2 point1, Vector2 point2)
        {
            if (blank == null)
            {
                blank = new Texture2D(Main.MainObject.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
                blank.SetData(new[] { Color.White }); 
            }

            float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
            float length = Vector2.Distance(point1, point2);

            batch.Draw(blank, point1, null, color,
                       angle, Vector2.Zero, new Vector2(length, width),
                       SpriteEffects.None, 0);
        }
    }
}
