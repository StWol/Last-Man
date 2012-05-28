using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using EVCS_Projekt.Location;
using Microsoft.Xna.Framework;

namespace EVCS_Projekt.Renderer
{
    // Render einfach "nur" die größe des Elements als Rechteck
    class SimpleRenderer : IRenderBehavior
    {
        private static Texture2D pixel;
        private Color color;

        // ***************************************************************************
        // Hat im Prinzip keine Größe => Nicht geeignet für GameObject.Sizing()
        public Vector2 Size
        {
            get
            {
                return new Vector2(1, 1);
            }
        }

        public SimpleRenderer(Color color)
        {
            // Farbe in der gezeichnet werden soll
            this.color = color;

            // Texture laden falls nicht vorhanden - Alle SimpleRenderer haben die gleiche Textur
            if (pixel == null)
                pixel = Main.ContentManager.Load<Texture2D>("images/pixelWhite");
        }


        public void Draw(SpriteBatch spriteBatch, ILocationBehavior locationBehavoir)
        {
            Draw(spriteBatch, locationBehavoir, color);
        }

        public void Draw(SpriteBatch spriteBatch, ILocationBehavior locationBehavoir, Color renderColor)
        {
            Rectangle temp = locationBehavoir.RelativeBoundingBox;
            temp.X -= temp.Width / 2;
            temp.Y -= temp.Height / 2;
            spriteBatch.Draw(pixel, temp, renderColor);
        }

        // ***************************************************************************
        // Wird nichts geupdated.. imer die gleiche Textur
        public void Update()
        {
            // Nothing todo
        }
    }
}
