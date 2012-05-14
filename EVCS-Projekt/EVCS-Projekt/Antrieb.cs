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
        public Antrieb(Game game, int id, EGroup group, String name, float rateOfFire, float damage, float weight, string description, ILocationBehavior locationBehavior)
            : base( game,  id,  group,  name,  description,  weight,  locationBehavior)
        {
            this.rateOfFire = rateOfFire;
        }


        //Functions

        public float GetDamage()
        {
            return damage;
        }
    }
}
