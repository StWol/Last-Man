using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace EVCS_Projekt
{
    interface ILocationBehavior
    {
        public Vector2 GetPostition();
        public Vector2 GetSize();
        public Rectangle GetRectangle();

    }
}
