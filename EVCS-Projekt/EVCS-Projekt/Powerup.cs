using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace EVCS_Projekt
{
    /**
     * @UMLVersion = 12.Mai2012
     * @Last Changes = 12.Mai2012
     *
     */
    public class Powerup : Microsoft.Xna.Framework.GameComponent
    {
        //Attributes
        private float regeneration;

        //Constructor
        public Powerup(Game game, int id, EGroup group, String name, float regeneration)
            : base(game)
        {
            this.regeneration = regeneration;
        }


        //Functions

        public float GetRegeneration()
        {
            return this.regeneration;
        }


    }
}
