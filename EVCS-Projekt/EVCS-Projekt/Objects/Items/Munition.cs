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
        public int ShotId { get; private set; }
        public float Damage { get; private set; }

        //Constructor
        public Munition( int id, EGroup group, String name, List<int> buffIDs, int count, int shotId, float damage, string description, float weight, ILocationBehavior locationBehavior)
            : base( id,  group,  name,  description,  weight,  locationBehavior)
        {
            this.buffIDs = buffIDs;
            Count = count;
            ShotId = shotId;
            Damage = damage;
        }

        // ***************************************************************************
        // Konstruktor Inner
        public Munition(MunitionInner mi)
            : base(mi.item)
        {
            buffIDs = mi.buffIDs;
            Count = mi.count;
            ShotId = mi.shotId;
            Damage = mi.damage;
        }

        // ***************************************************************************
        // Objekt zum Serialisieren
        public class MunitionInner
        {
            public List<int> buffIDs;
            public int count, shotId;
            public ItemInner item;
            public float damage;
        }

        // ***************************************************************************
        // Erzeugt Objekt zum Serialisieren
        public MunitionInner GetInner()
        {
            MunitionInner mi = new MunitionInner();

            mi.buffIDs = buffIDs;
            mi.count = Count;
            mi.shotId = ShotId;
            mi.damage = Damage;

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
            Munition m = new Munition(Id, Group, Name, new List<int>(buffIDs), Count, ShotId, Damage, Description, Weight, LocationBehavior.Clone());
            m.Renderer = Renderer;
            return m;
        }
    }
}
