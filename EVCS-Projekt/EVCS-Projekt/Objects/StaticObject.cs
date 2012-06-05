using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EVCS_Projekt.Location;
using EVCS_Projekt.Renderer;

namespace EVCS_Projekt.Objects
{
    public class StaticObject : GameObject
    {
        public StaticObject(ILocationBehavior location, IRenderBehavior renderer)
            : base(location, renderer)
        {
            LocationSizing();
        }
    }
}
