using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using EVCS_Projekt.Location;
using Microsoft.Xna.Framework;

namespace EVCS_Projekt.Renderer
{
    class StaticRenderer : IRenderBehavior
    {
        private Texture2D texture;

        // ***************************************************************************
        // Die Größe, des zu rendernen Objektes => Größe der Textur
        public Vector2 Size
        {
            get
            {
                return new Vector2(texture.Width, texture.Height);
            }
        }

        public StaticRenderer(Texture2D texture)
        {
            this.texture = texture;
        }

        public void Draw(SpriteBatch spriteBatch, ILocationBehavior locationBehavoir)
        {
            // Texture zeichnen
            spriteBatch.Draw(texture, locationBehavoir.BoundingBox, Color.White);
        }

        // ***************************************************************************
        // Wird nichts geupdated.. imer die gleiche Textur
        public void Update()
        {
            // Nothing todo
        }
    }
}
