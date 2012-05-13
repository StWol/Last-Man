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
    public class Liquid : Microsoft.Xna.Framework.GameComponent
    {

        //Attributes
        public int Liquid_RED; 
        public int Liquid_GREEN; 
        public int Liquid_BLUE;

        private float amount;


        public Liquid(Game game, int id, EGroup group, String name, float amount)
            : base(game)
        {
            this.amount = amount;
        }

        public float GetAmount()
        {
            return this.amount;
        }

    }
}
