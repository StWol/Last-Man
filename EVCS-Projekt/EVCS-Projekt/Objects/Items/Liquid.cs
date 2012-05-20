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

        public const int GREEN = 0;
        public const int BLUE = 1;
        public const int RED = 2; 


        public float Amount
        {
            get;
            private set;
        }

        public int TypeOfLiquid
        {
            get;
            private set;
        }


        public Liquid(Game game, int id, int typeOfLiquid, String name, float amount, EGroup group, string description, float weight, ILocationBehavior locationBehavior)
            : base( game,  id,  group,  name,  description,  weight,  locationBehavior)
        {
            this.Amount = amount;
            this.TypeOfLiquid = typeOfLiquid;
        }

    }
}
