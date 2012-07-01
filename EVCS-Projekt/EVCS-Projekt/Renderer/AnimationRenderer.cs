using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LastMan.Location;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace LastMan.Renderer
{
    public class AnimationRenderer : IRenderBehavior
    {
        public Texture2D[] Textures { get; private set; }
        private float framesPerSecond;
        private float animationTimer;
        private float frameDuration;
        public string Name { get; set; }

        private delegate void UpdateDelegate();
        private UpdateDelegate update;

        //public static Dictionary<ERenderer, AnimationRenderer> DefaultRenderer { get; private set; }

        // ***************************************************************************
        // Aktuelle Framenummer
        public int Frame
        {
            get
            {
                return (int)(animationTimer / frameDuration);
            }
        }

        // ***************************************************************************
        // Die Größe, des zu rendernen Objektes => Größe der Textur
        public Vector2 Size
        {
            get
            {
                return new Vector2(Textures[0].Width, Textures[0].Height);
            }
        }

        public AnimationRenderer(Texture2D[] textures, float fps, string name)
            : this(textures, fps)
        {
            Name = name;
        }

        public AnimationRenderer(Texture2D[] textures, float fps)
        {
            this.Textures = textures;
            this.framesPerSecond = fps;
            this.animationTimer = 0;
            this.frameDuration = 1 / framesPerSecond;
            update = UpdateLoop;
        }

        public void Draw(SpriteBatch spriteBatch, ILocationBehavior locationBehavoir)
        {
            // Texture zeichnen
            Draw(spriteBatch, locationBehavoir, Color.White);
        }

        public void Draw(SpriteBatch spriteBatch, ILocationBehavior locationBehavoir, Color color)
        {
            // Texture zeichnen
            spriteBatch.Draw(Textures[Frame], locationBehavoir.RelativeBoundingBox, null, color, locationBehavoir.Rotation, locationBehavoir.Size / 2, SpriteEffects.None, 0);
        }

        // ***************************************************************************
        // Frame weiterzählen
        public void Update()
        {
            update();
        }

        // ***************************************************************************
        // Frame weiterzählen - Schleife
        private void UpdateLoop()
        {
            // Gametime auf timer adden
            animationTimer += (float)Main.GameTimeUpdate.ElapsedGameTime.TotalSeconds;

            // animationTimer resetten
            if (Textures.Length / framesPerSecond <= animationTimer)
            {
                animationTimer = 0;
            }

        }

        // ***************************************************************************
        // Frame weiterzählen - Einmal abspielen dann stop
        private void UpdateOnce()
        {
            // Gametime auf timer adden - aber nur wenn es nicht durchgelaufen ist
            if (Textures.Length / framesPerSecond > animationTimer + (float)Main.GameTimeUpdate.ElapsedGameTime.TotalSeconds)
            {
                animationTimer += (float)Main.GameTimeUpdate.ElapsedGameTime.TotalSeconds;
            }

        }

        // ***************************************************************************
        // Frame weiterzählen
        public void PlayOnce()
        {
            update = UpdateOnce;
        }

        // ***************************************************************************
        // Animation auf 0 setzten
        public void ResetAnimation()
        {
            animationTimer = 0;
        }

        // ***************************************************************************
        // Clone
        public IRenderBehavior Clone()
        {
            return new AnimationRenderer(Textures, framesPerSecond, Name);
        }

        /*
        // ***************************************************************************
        // Clonet ein DefaultRenderer
        public static AnimationRenderer Get(ERenderer e)
        {
            AnimationRenderer a = (AnimationRenderer)DefaultRenderer[e].Clone();
            return a;
        }*/

        // ***************************************************************************
        // Load Animation
        public static void Load(string name, String file, int frames, float framesPerSecond)
        {
            // Array mit texturen erstellen
            Texture2D[] array = new Texture2D[frames];

            // Einzelne Bilder einladen
            for (int i = 0; i < frames; i++)
            {
                String f = "";
                if (i < 100 && frames >= 100)
                    f += "0";
                if (i < 10 && frames >= 10)
                    f += "0";
                f += i;

                array[i] = Main.ContentManager.Load<Texture2D>("animation/" + file + "/" + file + "_" + f);
            }

            AnimationRenderer a = new AnimationRenderer(array, framesPerSecond);
            a.Name = name;

            // Renderer erstellen
            LoadedRenderer.DefaultRenderer.Add(a.Name, a);
        }
    }
}
