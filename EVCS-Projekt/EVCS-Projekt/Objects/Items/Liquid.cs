using System;
using EVCS_Projekt.Location;
using Microsoft.Xna.Framework;

namespace EVCS_Projekt.Objects.Items
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


        public Liquid(Game game, int id, int typeOfLiquid, String name, float amount, EGroup group, string description, float weight, ILocationBehavior locationBehavior)
            : base( game,  id,  group,  name,  description,  weight,  locationBehavior)
        {
            this.amount = amount;
            this.typeOfLiquid = typeOfLiquid;
        }

    }
}
