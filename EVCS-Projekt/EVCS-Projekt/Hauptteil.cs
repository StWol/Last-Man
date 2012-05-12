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
    public class Hauptteil : Microsoft.Xna.Framework.GameComponent
    {
        //Attributes
        private float rateOfFire;

        //Constructor
        public Hauptteil(Game game, int id, EGroup group, String name, float rateOfFire, float weight)
            : base(game)
        {
            this.rateOfFire = rateOfFire;
        }

        //Functions

        public float GetRateOfFire()
        {
            return rateOfFire;
        }

    }
}
