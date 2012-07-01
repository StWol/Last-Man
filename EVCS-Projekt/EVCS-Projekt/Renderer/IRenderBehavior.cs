using LastMan.Location;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace LastMan.Renderer
{
    public interface IRenderBehavior
    {
        void Draw(SpriteBatch spriteBatch, ILocationBehavior locationBehavoir);
        void Draw(SpriteBatch spriteBatch, ILocationBehavior locationBehavoir, Color color);
        void Update();

        // ***************************************************************************
        // Die Größe, des zu rendernen Objektes => Größe der Textur
        Vector2 Size { get; }
        string Name { get; set; }

        // ***************************************************************************
        // Clone
        IRenderBehavior Clone();
    }
}
