using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace EVCS_Projekt.UI
{
    class UIElement
    {
        protected int width
        {
            get
            {
                return (int)(Helper.DrawHelper.Get(this.GetHashCode() + "Size").X);
            }
        }
        protected int height
        {
            get
            {
                return (int)(Helper.DrawHelper.Get(this.GetHashCode() + "Size").Y);
            }
        }
        protected Vector2 position
        {
            get { return Helper.DrawHelper.Get(this.GetHashCode() + "Position"); }
            private set 
            {
                string key = this.GetHashCode() + "Position";

                Helper.DrawHelper.AddDimension(key, (int)value.X, (int)value.Y);
            }
        }

        protected List<UIActionListener> listener;
        protected UIElement parent;

        public UIElement(int width, int height, Vector2 position)
        {
            string key = this.GetHashCode() + "Size";
            Helper.DrawHelper.AddDimension(key, width, height);
            
            this.position = position;
            listener = new List<UIActionListener>();
        }

        public void SetParent(UIElement parent)
        {
            this.parent = parent;
        }

        public int GetHeight()
        {
            return height;
        }

        public int GetWidth()
        {
            return width;
        }

        public Vector2 GetPosition()
        {
            return position;
        }

        // Wird von den Unterklassen überschrieben
        public virtual void Draw(SpriteBatch sb)
        {
        }

        // Wird von den Unterklassen überschrieben
        public virtual void Update()
        {
        }

        public void AddActionListener(UIActionListener al)
        {
            listener.Add(al);
        }

        public void RemoveActionListener(UIActionListener al)
        {
            listener.Remove(al);
        }
    }
}
