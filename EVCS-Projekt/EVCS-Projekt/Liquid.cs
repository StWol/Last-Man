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
    public class Liquid : Item
    {

        //Attributes

        public const int Liquid_GREEN = 0;
        public const int Liquid_BLUE = 1;
        public const int Liquid_RED = 2; 

        private float amount;
        private int typeOfLiquid = Liquid_GREEN;

        public float Amount 
        {
            get { return amount; }
        }

        
        public int TypeOfLiquid
        {
            get { return typeOfLiquid; }
        }


        public Liquid(Game game, int id, int typeOfLiquid, String name, float amount, EGroup group, string description, float weight, ILocationBehavoir locationBehavior)
            : base( game,  id,  group,  name,  description,  weight,  locationBehavior)
        {
            this.amount = amount;
            this.typeOfLiquid = typeOfLiquid;
        }

    }
}
