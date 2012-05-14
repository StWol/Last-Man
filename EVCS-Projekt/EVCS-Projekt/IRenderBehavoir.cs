using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace EVCS_Projekt
{
    interface IRenderBehavoir
    {
        public void Draw(SpriteBatch spriteBatch, ILocationBehavoir locationBehavoir);
        public void Update(GameTime gameTime);

    }
}
