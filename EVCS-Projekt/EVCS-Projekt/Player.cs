using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EVCS_Projekt.Location;
using EVCS_Projekt.Objects.Items;
using EVCS_Projekt.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Diagnostics;
using EVCS_Projekt.Renderer;


namespace EVCS_Projekt
{
    public class Player : GameObject
    {
        //Attributes
        private float maxHealth;
        public float Health { get; private set; }
        public float Speed { get; set; }
        public float[] Liquid { get; set; }
        public Item Weapon { set; get; }
        public List<Item> Inventar { get; private set; }
        private Dictionary<int, Item> shortcuts;
        private List<Buff> bufflist;
        public Vector2 Direction { get; set; }

        private bool isMoving = false;
        public bool IsMoving
        {
            get { return isMoving; }
            set
            {
                if (isMoving == value)
                    return;

                isMoving = value;
                if (isMoving)
                    Renderer = RendererMoving;
                else
                    Renderer = RendererStanding;
            }
        }

        // Verschiedene Renderer
        private AnimationRenderer RendererMoving { get; set; }
        private StaticRenderer RendererStanding { get; set; }

        /*
        *  Die(), das DropItem() und das Prüfen, ob ein Enemy gestorben ist - muss der GameManager checken und
        *  behandeln!
        */

        // ***************************************************************************
        // Konstruktor
        public Player(ILocationBehavior locationBehavior, float maxHealth, float health, float speed)
            : base(locationBehavior)
        {
            bufflist = new List<Buff>();
            Speed = speed;

            // Texturen für Renderer laden
            Texture2D textureStanding = Main.ContentManager.Load<Texture2D>("images/character/standing");
            RendererStanding = new StaticRenderer(textureStanding);

            Texture2D[] textureMoving = new Texture2D[]{ 
                Main.ContentManager.Load<Texture2D>("images/character/moving_1"),
                Main.ContentManager.Load<Texture2D>("images/character/moving_2")
            };
            RendererMoving = new AnimationRenderer(textureMoving, 4F);

            // Standardmäßig den StandingRenderer zuweisen
            Renderer = RendererStanding;

            // LocationSize anpassen
            LocationSizing();
        }

        // ***************************************************************************
        // Player wird von Schuss getroffen => Schaden nehmen und Buffs übernehmen - manager muss auf Tod reagieren
        public void TakeDamage(Shot shot)
        {
            // Schaden abziehen
            Health -= shot.Damage;

            // Buffs übernehmen
            AddBuffs(shot.BuffList);
        }

        // ***************************************************************************
        // Entfernt das Item aus dem Inventar
        public void EatItem(Item item)
        {
            this.Inventar.Remove(item);
        }

        // ***************************************************************************
        // Fügt von Liquid mit der ID id, die Menge amount hinzu
        public void AddLiquid(int id, float amount)
        {
            this.Liquid[id] += amount;
        }

        // ***************************************************************************
        // Zieht von Liquid mit der ID id, die Menge amount ab
        public void ReduceLiquid(int id, float amount)
        {
            this.Liquid[id] -= amount;
            if (Liquid[id] - amount <= 0)
                amount = 0;
        }

        // ***************************************************************************
        // Fügt die übergebene Liste mit Buffs dem Player hinzu
        public void AddBuffs(List<Buff> buffList)
        {
            this.bufflist.AddRange(bufflist);
        }

    }
}
