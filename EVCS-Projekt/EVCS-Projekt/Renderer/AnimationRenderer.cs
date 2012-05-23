using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using EVCS_Projekt.Location;
using System.Diagnostics;

namespace EVCS_Projekt.Renderer
{
    class AnimationRenderer : IRenderBehavior
    {
        private Texture2D[] textures;
        private float framesPerSecond;
        private float animationTimer;
        private float frameDuration;

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
                return new Vector2(textures[0].Width, textures[0].Height);
            }
        }

        public AnimationRenderer(Texture2D[] textures, float fps)
        {
            this.textures = textures;
            this.framesPerSecond = fps;
            this.animationTimer = 0;
            this.frameDuration = 1 / framesPerSecond;
        }

        public void Draw(SpriteBatch spriteBatch, ILocationBehavior locationBehavoir)
        {
            // Texture zeichnen
            spriteBatch.Draw(textures[Frame], locationBehavoir.BoundingBox, Color.White);
        }

        // ***************************************************************************
        // Frame weiterzählen
        public void Update()
        {
            // Gametime auf timer adden
            animationTimer += (float)Main.GameTimeUpdate.ElapsedGameTime.TotalSeconds;

            // animationTimer resetten
            if (textures.Length / framesPerSecond <= animationTimer)
            {
                animationTimer = 0;
            }
        }
    }
}
