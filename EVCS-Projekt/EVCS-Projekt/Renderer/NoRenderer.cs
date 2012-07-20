using LastMan.Location;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace LastMan.Renderer
{
    public class NoRenderer : IRenderBehavior
    {
        public string Name { get { return "NoRenderer"; } set { } }

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

        // ***************************************************************************
        // Clone
        public IRenderBehavior Clone()
        {
            return new NoRenderer();
        }
    }
}
