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
using System.Reflection;


namespace EVCS_Projekt.Objects
{

    public abstract class Item : GameObject
    {
        // ***************************************************************************
        // Dictionaries
        public static Dictionary<int, Visier> DefaultVisiere;
        public static Dictionary<int, Stabilisator> DefaultStabilisatoren;
        public static Dictionary<int, Shot> DefaultShots;
        public static Dictionary<int, Powerup> DefaultPowerups;
        public static Dictionary<int, Munition> DefaultMunition;
        public static Dictionary<int, Hauptteil> DefaultHauptteil;
        public static Dictionary<int, Antrieb> DefaultAntrieb;
        public static Dictionary<int, Weapon> DefaultWeapon;

        public static Dictionary<int, Item> AllItems;

        // icons der Items
        private static Dictionary<int, Texture2D> ItemIcons;

        private static Texture2D defaultWeaponIcon;
        public static int StaticID = 800;


        // ***************************************************************************
        // Läd Items
        public static void LoadItems()
        {
            // Dics init
            DefaultVisiere = new Dictionary<int, Visier>();
            DefaultStabilisatoren = new Dictionary<int, Stabilisator>();
            DefaultShots = new Dictionary<int, Shot>();
            DefaultPowerups = new Dictionary<int, Powerup>();
            DefaultMunition = new Dictionary<int, Munition>();
            DefaultHauptteil = new Dictionary<int, Hauptteil>();
            DefaultAntrieb = new Dictionary<int, Antrieb>();
            DefaultWeapon = new Dictionary<int, Weapon>();

            AllItems = new Dictionary<int, Item>();

            ItemIcons = new Dictionary<int, Texture2D>();

            defaultWeaponIcon = Main.ContentManager.Load<Texture2D>("images/itemIcons/1");
            ItemIcons.Add(800, defaultWeaponIcon);
            // Laden
            //Debug.WriteLine("Lade antrieb.xml");
            LoadXML<Antrieb, Antrieb.AntriebInner>("antrieb.xml");
            //Debug.WriteLine("Lade hauptteil.xml");
            LoadXML<Hauptteil, Hauptteil.HauptteilInner>("hauptteil.xml");
            //Debug.WriteLine("Lade munition.xml");
            LoadXML<Munition, Munition.MunitionInner>("munition.xml");
            //Debug.WriteLine("Lade powerup.xml");
            LoadXML<Powerup, Powerup.PowerupInner>("powerup.xml");
            //Debug.WriteLine("Lade shot.xml");
            LoadXML<Shot, Shot.ShotInner>("shot.xml");
            //Debug.WriteLine("Lade stabilisator.xml");
            LoadXML<Stabilisator, Stabilisator.StabilisatorInner>("stabilisator.xml");
            //Debug.WriteLine("Lade visier.xml");
            LoadXML<Visier, Visier.VisierInner>("visier.xml");
            //Debug.WriteLine("Lade weapon.xml");
            LoadXML<Weapon, Weapon.WeaponInner>("weapon.xml");

            // Icons
            foreach (Item i in AllItems.Values)
            {
                // Schüsse skippen
                if (i.GetType() == typeof(Shot))
                    continue;

                Texture2D ico;
                try
                {
                    ico = Main.ContentManager.Load<Texture2D>(Configuration.Get("iconDir") + i.TypeId);
                }
                catch
                {
                    Debug.WriteLine("Dummy für " + i.TypeId);
                    ico = Main.ContentManager.Load<Texture2D>(Configuration.Get("iconDir") + "dummy");
                }

                // StaticRenderer erstellen, da icons immer staticrenderer haben! is so
                StaticRenderer s = new StaticRenderer(ico);
                s.Name = "S_IconRenderer_" + i.TypeId;

                LoadedRenderer.DefaultRenderer.Add("S_IconRenderer_" + i.TypeId, s);

                ItemIcons.Add(i.TypeId, ico);
            }
        }

        private static List<Inner> GetList<Inner>(FileStream fs)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Inner>));
            List<Inner> loaded = (List<Inner>)serializer.Deserialize(fs);
            return loaded;
        }

        private static void LoadXML<Outter, Inner>(string file)
        {

            //BaseFruit fruit = constructor.Invoke(new object[] { (int)150 }) as BaseFruit;


            // File öffnen
            FileStream fs = new FileStream(Configuration.Get("itemDir") + file, FileMode.Open);


            if (typeof(Outter) == typeof(Antrieb))
            {
                foreach (Antrieb.AntriebInner i in GetList<Antrieb.AntriebInner>(fs))
                {
                    Antrieb v = new Antrieb(i);
                    DefaultAntrieb.Add(v.TypeId, v);
                    AllItems.Add(v.TypeId, v);
                }
            }
            else if (typeof(Outter) == typeof(Hauptteil))
            {
                foreach (Hauptteil.HauptteilInner i in GetList<Hauptteil.HauptteilInner>(fs))
                {
                    Hauptteil v = new Hauptteil(i);
                    DefaultHauptteil.Add(v.TypeId, v);
                    AllItems.Add(v.TypeId, v);
                }
            }
            else if (typeof(Outter) == typeof(Munition))
            {
                foreach (Munition.MunitionInner i in GetList<Munition.MunitionInner>(fs))
                {
                    Munition v = new Munition(i);
                    DefaultMunition.Add(v.TypeId, v);
                    AllItems.Add(v.TypeId, v);
                }
            }
            else if (typeof(Outter) == typeof(Powerup))
            {
                foreach (Powerup.PowerupInner i in GetList<Powerup.PowerupInner>(fs))
                {
                    Powerup v = new Powerup(i);
                    DefaultPowerups.Add(v.TypeId, v);
                    AllItems.Add(v.TypeId, v);
                }
            }
            else if (typeof(Outter) == typeof(Stabilisator))
            {
                foreach (Stabilisator.StabilisatorInner i in GetList<Stabilisator.StabilisatorInner>(fs))
                {
                    Stabilisator v = new Stabilisator(i);
                    DefaultStabilisatoren.Add(v.TypeId, v);
                    AllItems.Add(v.TypeId, v);
                }
            }
            else if (typeof(Outter) == typeof(Visier))
            {
                foreach (Visier.VisierInner i in GetList<Visier.VisierInner>(fs))
                {
                    Visier v = new Visier(i);
                    DefaultVisiere.Add(v.TypeId, v);
                    AllItems.Add(v.TypeId, v);
                }
            }
            else if (typeof(Outter) == typeof(Weapon))
            {
                foreach (Weapon.WeaponInner i in GetList<Weapon.WeaponInner>(fs))
                {
                    Weapon v = new Weapon(i);
                    DefaultWeapon.Add(v.TypeId, v);
                    AllItems.Add(v.TypeId, v);
                }
            }
        }

        //Attributes
        public int TypeId { get; private set; }
        public EGroup Group { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public float Weight { get; private set; }

        public Texture2D Icon { get
        {
            if (GetType() == typeof(Weapon))
                return ItemIcons[800];
            return ItemIcons[TypeId];
        } }
        public Vector3 RequiredLiquid { get; private set; }

        public Texture2D Icon { get { return ItemIcons[TypeId]; } }

        // ***************************************************************************
        // Konstruktor 1
        public Item(int id, EGroup group, string name, string description, float weight, ILocationBehavior locationBehavior, IRenderBehavior renderBehavior)
            : base(locationBehavior, renderBehavior)
        {
            this.TypeId = id;
            Name = name;
            Description = description;
            Weight = weight;
            this.Group = group;

            // RendererFix
            //FixRenderer();
        }

        // ***************************************************************************
        // Konstruktor 2
        public Item(int id, EGroup group, string name, string description, float weight, ILocationBehavior locationBehavior)
            : base(locationBehavior)
        {
            this.TypeId = id;
            Name = name;
            Description = description;
            Weight = weight;
            this.Group = group;

            // RendererFix
            //FixRenderer();
        }

        // ***************************************************************************
        // Konstruktor 3
        public Item(ItemInner ii)
            : base(ii.gameObject)
        {
            TypeId = ii.id;
            Name = ii.name;
            Description = ii.description;
            Weight = ii.weight;
            Group = ii.group;
            RequiredLiquid = ii.requiredLiquid;
        }

        // ItemIcon in den Renderer Quetschen
        private void FixRenderer()
        {
            if (typeof(Munition) == GetType())
            {
                ((Munition)this).ShotRenderer = Renderer;
            if (typeof(Weapon) == GetType())
            {
                Renderer = LoadedRenderer.GetStatic("S_Weapon_Deafault");
            }
            else
            {
                Renderer = LoadedRenderer.GetStatic("S_IconRenderer_" + TypeId);
            }
            }

            Renderer = LoadedRenderer.GetStatic("S_IconRenderer_" + TypeId);
            
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
            public Vector3 requiredLiquid;
        }

        // ***************************************************************************
        // Erzeugt Objekt zum Serialisieren
        public ItemInner GetInner()
        {
            ItemInner ii = new ItemInner();
            ii.id = TypeId;
            ii.weight = Weight;
            ii.name = Name;
            ii.description = Description;
            ii.group = Group;
            ii.gameObject = base.GetInner();
            ii.requiredLiquid = RequiredLiquid;
            return ii;
        }

        // ***************************************************************************
        // HashCode
        protected int HashCode()
        {
            return 0;
        }

        //
        public static Item Get(int x)
        {
            Item i = AllItems[x];

            Item ret = null;

            if (i.GetType() == typeof(Antrieb))
                ret = ((Antrieb)i).Clone();
            if (i.GetType() == typeof(Hauptteil))
                ret = ((Hauptteil)i).Clone();
            if (i.GetType() == typeof(Liquid))
                ret = ((Liquid)i).Clone();
            if (i.GetType() == typeof(Munition))
                ret = ((Munition)i).Clone();
            if (i.GetType() == typeof(Powerup))
                ret = ((Powerup)i).Clone();
            if (i.GetType() == typeof(Shot))
                ret = ((Shot)i).Clone();
            if (i.GetType() == typeof(Stabilisator))
                ret = ((Stabilisator)i).Clone();
            if (i.GetType() == typeof(Visier))
                ret = ((Visier)i).Clone();
            if (i.GetType() == typeof(Weapon))
                ret = ((Weapon)i).Clone();

            // Renderer fixen (icon)
            ret.FixRenderer();

            return ret;
        }
    }
}
