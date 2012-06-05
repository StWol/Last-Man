using EVCS_Projekt.Location;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace EVCS_Projekt.Renderer
{
    public interface IRenderBehavior
    {
        void Draw(SpriteBatch spriteBatch, ILocationBehavior locationBehavoir);
        void Draw(SpriteBatch spriteBatch, ILocationBehavior locationBehavoir, Color color);
        void Update();

        // ***************************************************************************
        // Die Größe, des zu rendernen Objektes => Größe der Textur
        Vector2 Size { get; }

        // ***************************************************************************
        // Clone
        IRenderBehavior Clone();
    }
}
