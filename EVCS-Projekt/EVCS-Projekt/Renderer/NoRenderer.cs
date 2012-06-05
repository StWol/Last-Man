using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using EVCS_Projekt.Location;
using Microsoft.Xna.Framework;

namespace EVCS_Projekt.Renderer
{
    public class NoRenderer : IRenderBehavior
    {
        // ***************************************************************************
        // Die Größe, des zu rendernen Objektes => Größe der Textur, da keine Textur 0,0
        public Vector2 Size
        {
            get
            {
                return new Vector2(0,0);
            }
        }

        public void Draw(SpriteBatch spriteBatch, ILocationBehavior locationBehavoir)
        {
            // Nothing todo
        }

        public void Draw(SpriteBatch spriteBatch, ILocationBehavior locationBehavoir, Color color)
        {
            // Nothing todo
        }

        public void Update()
        {
            // Nothing todo
        }
    }
}
