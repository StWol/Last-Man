using System;
using System.Collections.Generic;
using System.Linq;
using EVCS_Projekt.Location;
using EVCS_Projekt.Objects.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace EVCS_Projekt.Objects
{
    /**
     * @UMLVersion = 12.Mai2012
     * @Last Changes = 14.Mai2012
     *
     */
    public abstract class Item : GameObject
    {

        //Attributes
        private int id;
        private EGroup group;
        private string name;
        private string description;
        private float weight;

        public string Name
        {
            get
            {
                return name;
            }
        }
       
        public string Description
        {
            get 
            {
                return description;
            }
        }
        
        public float Weight
        {
            get
            {
                return weight;
            }
        }

        //Constructor
        public Item( int id, EGroup group, string name, string description, float weight, ILocationBehavior locationBehavior)
            : base( locationBehavior)
        {
            this.id = id;
            this.name = name;
            this.description = description;
            this.weight = weight;
            this.group = group;
        }



        //Functions
        protected int HashCode()
        {
            return 0;
        }
    }
}
