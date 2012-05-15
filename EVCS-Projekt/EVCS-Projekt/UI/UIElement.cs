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
        protected int width;
        protected int height;
        protected Vector2 position;
        protected List<UIActionListener> listener;
        protected UIElement parent;

        public UIElement(int width, int height, Vector2 position)
        {
            this.height = height;
            this.width = width;
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
