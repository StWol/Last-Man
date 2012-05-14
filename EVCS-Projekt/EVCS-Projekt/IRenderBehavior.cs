using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace EVCS_Projekt
{
    interface IRenderBehavior
    {
        public void draw(SpriteBatch sriteBatch, ILocationBehavoir locationBehavior);
        public void Update(GameTime gameTime);
    }
}
