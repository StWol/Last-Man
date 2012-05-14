using System;
using EVCS_Projekt.Location;
using Microsoft.Xna.Framework;
using EVCS_Projekt;

namespace EVCS_Projekt.Objects.Items
{
    /**
     * @UMLVersion = 12.Mai2012
     * @Last Changes = 12.Mai2012
     *
     */
    public class Hauptteil : Item
    {
        //Attributes
        private float rateOfFire;
        public float RateOfFire
        {
            get
            {
                return this.rateOfFire;
            }
        }

        //Constructor
        public Hauptteil(Game game, int id, EGroup group, String name, float rateOfFire, float weight, string description, ILocationBehavior locationBehavior)
            : base( game,  id,  group,  name,  description,  weight,  locationBehavior)
        {
            this.rateOfFire = rateOfFire;
        }

        //Functions


    }
}
