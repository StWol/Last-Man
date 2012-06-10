using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace EVCS_Projekt.Renderer
{
    class LoadedRenderer
    {
        public static Dictionary<ERenderer, IRenderBehavior> DefaultRenderer { get; set; }

        // ***************************************************************************
        // Clonet ein DefaultRenderer
        public static AnimationRenderer GetAnimation(ERenderer e)
        {
            AnimationRenderer a = (AnimationRenderer)DefaultRenderer[e].Clone();
            return a;
        }

        // ***************************************************************************
        // Clonet ein DefaultRenderer
        public static StaticRenderer GetStatic(ERenderer e)
        {
            StaticRenderer s = (StaticRenderer)DefaultRenderer[e].Clone();
            return s;
        }

        // ***************************************************************************
        // Clonet ein DefaultRenderer
        public static IRenderBehavior Get(ERenderer e)
        {
            return DefaultRenderer[e].Clone();
        }

    }
}
