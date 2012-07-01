using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace LastMan.Renderer
{
    class LoadedRenderer
    {
        public static Dictionary<string, IRenderBehavior> DefaultRenderer { get; set; }

        // ***************************************************************************
        // Clonet ein DefaultRenderer
        public static AnimationRenderer GetAnimation(string e)
        {
            AnimationRenderer a = (AnimationRenderer)DefaultRenderer[e].Clone();
            return a;
        }

        // ***************************************************************************
        // Clonet ein DefaultRenderer
        public static StaticRenderer GetStatic(string e)
        {
            StaticRenderer s = (StaticRenderer)DefaultRenderer[e].Clone();
            return s;
        }

        // ***************************************************************************
        // Clonet ein DefaultRenderer
        public static IRenderBehavior Get(string e)
        {
            return DefaultRenderer[e].Clone();
        }

    }
}
