using EVCS_Projekt.Location;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace EVCS_Projekt.Renderer
{
    interface IRenderBehavior
    {
        void Draw(SpriteBatch spriteBatch, ILocationBehavior locationBehavoir);
        void Update(GameTime gameTime);

    }
}
