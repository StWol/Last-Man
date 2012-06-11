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
    abstract class UIElement
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
        private bool isHover;
        private bool mouseDown;

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
            MouseState state = Mouse.GetState();
            int mX = state.X;
            int mY = state.Y;
            int x = (int)(position.X + parent.GetPosition().X);
            int y = (int)(position.Y + parent.GetPosition().Y);
            int w = GetWidth();
            int h = GetHeight();




            if (mX > x && mX < x + w && mY > y && mY < y + h)
            {
                isHover = true;

                // Wenn nicht mehr gedrückt, aber im vorherigen Durchgang gedrückt war => Man kann die Maustaste gedrückt halten ohne das jedesmal ein Event ausgelöst wird
                if (state.LeftButton != ButtonState.Pressed && mouseDown == true)
                {
                    List<UIActionListener> listenerList = new List<UIActionListener>(listener);
                    foreach (UIActionListener al in listenerList)
                    {
                        al.ActionEvent(this);
                    }
                }
            }
            else
            {
                isHover = false;
            }

            if (state.LeftButton == ButtonState.Pressed)
            {
                mouseDown = true;
            }
            else
            {
                mouseDown = false;
            }
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
