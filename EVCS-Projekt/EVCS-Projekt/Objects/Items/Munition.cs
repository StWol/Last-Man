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
        private List<int> buffIDs;

        //Constructor
        public Munition( int id, EGroup group, String name, List<int> buffIDs, string description, float weight, ILocationBehavior locationBehavior)
            : base( id,  group,  name,  description,  weight,  locationBehavior)
        {
            this.buffIDs = buffIDs;
        }

        // ***************************************************************************
        // Konstruktor Inner
        public Munition(MunitionInner mi)
            : base(mi.item)
        {
            buffIDs = mi.buffIDs;
        }

        // ***************************************************************************
        // Objekt zum Serialisieren
        public class MunitionInner
        {
            public List<int> buffIDs;
            public ItemInner item;
        }

        // ***************************************************************************
        // Erzeugt Objekt zum Serialisieren
        public MunitionInner GetInner()
        {
            MunitionInner mi = new MunitionInner();

            mi.buffIDs = buffIDs;

            mi.item = base.GetInner();
            return mi;
        }

        //Functions
        public List<Buff> GetBuffList()
        {
            return null;
        }

        // ***************************************************************************
        // Clont Object
        public Munition Clone()
        {
            Munition m = new Munition(Id, Group, Name, new List<int>(buffIDs), Description, Weight, LocationBehavior.Clone());
            return m;
        }
    }
}
