using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace LastMan.UI
{
    class UIMouseEvent
    {
        private int x;
        private int y;
        
        public UIMouseEvent(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public UIMouseEvent(MouseState state)
        {
            this.x = state.X;
            this.y = state.Y;
        }

        public bool isMouseIn(Rectangle rect)
        {
            int xRect = (rect.X);
            int yRect = (rect.Y);
            int w = rect.Width;
            int h = rect.Height;

            return x > xRect && x < xRect + w && y > yRect && y < yRect + h;
        }
    }
}
