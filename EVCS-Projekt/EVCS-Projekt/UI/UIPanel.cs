using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EVCS_Projekt.UI
{
    class UIPanel : UIElement
    {
        private List<UIElement> children;

        public UIPanel(int width, int height, Vector2 position)
            : base(width, height, position)
        {
            children = new List<UIElement>();
        }

        public void Add(UIElement element)
        {
            element.SetParent(this);
            children.Add(element);
        }

        public void Remove(UIElement element)
        {
            children.Remove(element);
        }

        public override void Draw(SpriteBatch sb)
        {
            foreach (UIElement e in children)
            {
                e.Draw(sb);
            }
        }

        public override void Update()
        {
            List<UIElement> clone = new List<UIElement>(children);
            foreach (UIElement e in clone)
            {
                e.Update();
            }
        }

    }
}
