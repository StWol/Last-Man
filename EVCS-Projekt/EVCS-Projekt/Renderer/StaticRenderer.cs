using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using EVCS_Projekt.Location;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace EVCS_Projekt.Renderer
{
    public class StaticRenderer : IRenderBehavior
    {
        public Texture2D Texture { get; private set; }
        public ERenderer ERenderer { get; set; }

        //public static Dictionary<ERenderer, StaticRenderer> DefaultRenderer { get; private set; }

        // ***************************************************************************
        // Die Größe, des zu rendernen Objektes => Größe der Textur
        public Vector2 Size
        {
            get
            {
                return new Vector2(Texture.Width, Texture.Height);
            }
        }

        public StaticRenderer(Texture2D texture)
        {
            this.Texture = texture;
        }

        public void Draw(SpriteBatch spriteBatch, ILocationBehavior locationBehavoir)
        {
            // Texture zeichnen
            Draw(spriteBatch, locationBehavoir, Color.White);
        }

        public void Draw(SpriteBatch spriteBatch, ILocationBehavior locationBehavoir, Color color)
        {
            // Texture zeichnen
            spriteBatch.Draw(Texture, locationBehavoir.RelativeBoundingBox, null, color, locationBehavoir.Rotation, locationBehavoir.Size / 2, SpriteEffects.None, 0);
        }

        // ***************************************************************************
        // Wird nichts geupdated.. imer die gleiche Textur
        public void Update()
        {
            // Nothing todo
        }

        // ***************************************************************************
        // Clone
        public IRenderBehavior Clone()
        {
            return new StaticRenderer(Texture);
        }

        // ***************************************************************************
        // Load Animation
        public static void Load(ERenderer e, String name)
        {
            // Bilder einladen
            Texture2D load = Main.ContentManager.Load<Texture2D>("images/" + name);

            StaticRenderer s = new StaticRenderer(load);
            s.ERenderer = e;

            // Renderer erstellen
            LoadedRenderer.DefaultRenderer.Add(e, s);
        }
    }
}
