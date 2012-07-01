using System;
using System.Collections.Generic;
using LastMan.Location;
using LastMan.Objects.Items;
using Microsoft.Xna.Framework;
using LastMan.Renderer;

namespace LastMan.Objects.Items
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
        public int MagazineSize { get; set; }
        public IRenderBehavior ShotRenderer { get; set; }

        //Constructor
        public Munition(int id, EGroup group, String name, List<int> buffIDs, int count, int shotId, float damage, string description, float weight, ILocationBehavior locationBehavior)
            : base(id, group, name, description, weight, locationBehavior)
        {
            this.buffIDs = buffIDs;
            Count = count;
            MagazineSize = count;
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
            Munition m = new Munition(TypeId, Group, Name, new List<int>(buffIDs), Count, ShotId, Damage, Description, Weight, LocationBehavior.Clone());
            m.Renderer = Renderer;

            if (ShotRenderer == null)
                m.ShotRenderer = Renderer;
            else
                m.ShotRenderer = ShotRenderer;

            return m;
        }

        public void ResetCount()
        {
            Count = MagazineSize;
        }

        public override string ToString()
        {
            return "Munition: " + base.ToString() + ": " + Count;
        }
    }
}
