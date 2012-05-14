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
    public class Powerup : Item
    {
        //Attributes
        private float regeneration;

        //Constructor
        public Powerup(Game game, int id, EGroup group, String name, float regeneration, string description, float weight, ILocationBehavior locationBehavior)
            : base( game,  id,  group,  name,  description,  weight,  locationBehavior)
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
