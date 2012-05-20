using System;
using System.Collections.Generic;
using EVCS_Projekt.Location;
using Microsoft.Xna.Framework;

namespace EVCS_Projekt.Objects.Items
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
        public Munition( int id, EGroup group, String name, List<int> buffIDs, string description, float weight, ILocationBehavior locationBehavior)
            : base( id,  group,  name,  description,  weight,  locationBehavior)
        {
            
        }



        //Functions
        public List<Buff> GetBuffList()
        {
            return buffList;
        }
    }
}
