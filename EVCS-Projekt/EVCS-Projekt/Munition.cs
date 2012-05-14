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
     * @Last Changes = 14.Mai2012
     *
     */
    public class Munition : Item
    {

        //Attributes
        private List<Buff> buffList;

        //Constructor
        public Munition(Game game, int id, EGroup group, String name, List<int> buffIDs, string description, float weight, ILocationBehavoir locationBehavior)
            : base( game,  id,  group,  name,  description,  weight,  locationBehavior)
        {
            
        }



        //Functions
        public List<Buff> GetBuffList()
        {
            return buffList;
        }
    }
}
