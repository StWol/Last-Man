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
    public class Stabilisator : Item
    {
        //Attributes
        private float accuracy;

        //Constructor
        public Stabilisator(Game game, int id, EGroup group, String name, float accuracy, float weight, string description, ILocationBehavior locationBehavior)
            : base( game,  id,  group,  name,  description,  weight,  locationBehavior)
        {
            this.accuracy = accuracy;
        }

        //Functions
        public float GetAccuracy()
        {
            return accuracy;
        }
    }
}
