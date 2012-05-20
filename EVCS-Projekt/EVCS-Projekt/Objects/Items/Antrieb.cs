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
    public class Antrieb : Item
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
        private Buff damageBuff;

        //Constructor
        public Antrieb( int id, EGroup group, String name, float rateOfFire, float damage, float weight, string description, ILocationBehavior locationBehavior)
            : base( id,  group,  name,  description,  weight,  locationBehavior)
        {
            this.rateOfFire = rateOfFire;
        }


        //Functions

        public float GetDamage()
        {
            return damageBuff.Modifier;
        }
    }
}
