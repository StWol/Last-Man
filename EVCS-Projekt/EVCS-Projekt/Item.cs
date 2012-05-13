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
    public class Item : Microsoft.Xna.Framework.GameComponent
    {

        //Attributes
        private int id;
        private EGroup group;
        private String name;
        private float weight;


        //Constructor
        public Item(Game game, int id, EGroup group,String name,float weight, ILocationBehavior locationBehavior)
            : base(game)
        {
            this.id = id;
            this.name = name;
            this.weight = weight;
        }

        //Functions
        protected int HashCode()
        {
            return 0;
        }
        public String GetDescription()
        {
            return null;
        }
        public String GetName()
        {
            return name;
        }
        public String GetDetails()
        {
            return null;
        }
        public void GetDetails(String name)
        {
            this.name = name;
        }



    }
}
