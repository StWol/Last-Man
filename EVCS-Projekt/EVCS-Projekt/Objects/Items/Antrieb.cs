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
        public float RateOfFire { get; private set; }
        public float Damage { get; private set; }
        private Buff damageBuff;

        //Constructor
        public Antrieb( int id, EGroup group, String name, float rateOfFire, float damage, float weight, string description, ILocationBehavior locationBehavior)
            : base( id,  group,  name,  description,  weight,  locationBehavior)
        {
            RateOfFire = rateOfFire;
            Damage = damage;
        }


        //Functions

        public float GetDamage()
        {
            return damageBuff.Modifier;
        }

        // ***************************************************************************
        // Clont Object
        public Antrieb Clone()
        {
            Antrieb a = new Antrieb(Id, Group, Name, RateOfFire, Damage, Weight, Description, LocationBehavior.Clone());
            return a;
        }
    }
}
