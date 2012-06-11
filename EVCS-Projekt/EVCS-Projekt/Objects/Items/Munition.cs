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
        public int Count { get; set; }

        //Constructor
        public Munition( int id, EGroup group, String name, List<int> buffIDs, int count, string description, float weight, ILocationBehavior locationBehavior)
            : base( id,  group,  name,  description,  weight,  locationBehavior)
        {
            this.buffIDs = buffIDs;
            Count = count;
        }

        // ***************************************************************************
        // Konstruktor Inner
        public Munition(MunitionInner mi)
            : base(mi.item)
        {
            buffIDs = mi.buffIDs;
            Count = mi.count;
        }

        // ***************************************************************************
        // Objekt zum Serialisieren
        public class MunitionInner
        {
            public List<int> buffIDs;
            public int count;
            public ItemInner item;
        }

        // ***************************************************************************
        // Erzeugt Objekt zum Serialisieren
        public MunitionInner GetInner()
        {
            MunitionInner mi = new MunitionInner();

            mi.buffIDs = buffIDs;
            mi.count = Count;

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
            Munition m = new Munition(Id, Group, Name, new List<int>(buffIDs), Count, Description, Weight, LocationBehavior.Clone());
            return m;
        }
    }
}
