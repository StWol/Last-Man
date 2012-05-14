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
    public class Stabilisator : Item
    {
        //Attributes
        private float accuracy;

        //Constructor
        public Stabilisator(Game game, int id, EGroup group, String name, float accuracy, float weight, string description, ILocationBehavoir locationBehavior)
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
