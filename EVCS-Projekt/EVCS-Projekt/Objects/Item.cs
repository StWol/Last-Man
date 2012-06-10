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
using EVCS_Projekt.Renderer;
using System.Xml;
using System.IO;
using System.Xml.Serialization;
using System.Diagnostics;


namespace EVCS_Projekt.Objects
{

    public abstract class Item : GameObject
    {
        // ***************************************************************************
        // Dictionaries
        public static Dictionary<string, Visier> DefaultVisiere;
        public static Dictionary<string, Stabilisator> DefaultStabilisatoren;
        public static Dictionary<string, Shot> DefaultShots;
        public static Dictionary<string, Powerup> DefaultPowerups;
        public static Dictionary<string, Munition> DefaultMunition;
        public static Dictionary<string, Hauptteil> DefaultHauptteil;
        public static Dictionary<string, Antrieb> DefaultAntrieb;

        // ***************************************************************************
        // Läd Items
        public static void LoadItems()
        {
            // Dics init
            DefaultVisiere = new Dictionary<string, Visier>();
            DefaultStabilisatoren = new Dictionary<string, Stabilisator>();
            DefaultShots = new Dictionary<string, Shot>();
            DefaultPowerups = new Dictionary<string, Powerup>();
            DefaultMunition = new Dictionary<string, Munition>();
            DefaultHauptteil = new Dictionary<string, Hauptteil>();
            DefaultAntrieb = new Dictionary<string, Antrieb>();

            // Laden
            LoadXML<Visier>("visiere.xml");
        }

        private static void LoadXML<Outter>(string file)
        {
            // File öffnen
            FileStream fs = new FileStream(Configuration.Get("itemDir") + file, FileMode.Open);

            if (typeof(Outter) == typeof(Visier))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Visier.VisierInner>));
                // Geladene liste
                List<Visier.VisierInner> loaded = (List<Visier.VisierInner>)serializer.Deserialize(fs);
                // itterien
                foreach (Visier.VisierInner i in loaded)
                {
                    // Objekt erstellen
                    Visier v = new Visier(i);
                    // Adden
                    DefaultVisiere.Add("v_" + v.Id, v);
                }
            }
        }

        //Attributes
        public int Id { get; private set; }
        public EGroup Group { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public float Weight { get; private set; }

        // ***************************************************************************
        // Konstruktor 1
        public Item(int id, EGroup group, string name, string description, float weight, ILocationBehavior locationBehavior, IRenderBehavior renderBehavior)
            : base(locationBehavior, renderBehavior)
        {
            this.Id = id;
            Name = name;
            Description = description;
            Weight = weight;
            this.Group = group;
        }

        // ***************************************************************************
        // Konstruktor 2
        public Item(int id, EGroup group, string name, string description, float weight, ILocationBehavior locationBehavior)
            : base(locationBehavior)
        {
            this.Id = id;
            Name = name;
            Description = description;
            Weight = weight;
            this.Group = group;
        }

        // ***************************************************************************
        // Konstruktor 3
        public Item(ItemInner ii)
            : base(ii.gameObject)
        {
            Id = ii.id;
            Name = ii.name;
            Description = ii.description;
            Weight = ii.weight;
            Group = ii.group;
        }

        // ***************************************************************************
        // Objekt zum Serialisieren
        public class ItemInner
        {
            public float weight;
            public int id;
            public string name, description;
            public EGroup group;
            public GameObjectInner gameObject;
        }

        // ***************************************************************************
        // Erzeugt Objekt zum Serialisieren
        public ItemInner GetInner()
        {
            ItemInner ii = new ItemInner();
            ii.id = Id;
            ii.weight = Weight;
            ii.name = Name;
            ii.description = Description;
            ii.group = Group;
            ii.gameObject = base.GetInner();
            return ii;
        }

        // ***************************************************************************
        // HashCode
        protected int HashCode()
        {
            return 0;
        }
    }
}
