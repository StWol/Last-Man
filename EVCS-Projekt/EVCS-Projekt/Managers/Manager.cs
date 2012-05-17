using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EVCS_Projekt.Managers
{
    public abstract class Manager
    {
        public abstract void Update();
        public abstract void Draw(SpriteBatch batch);
    }
}
