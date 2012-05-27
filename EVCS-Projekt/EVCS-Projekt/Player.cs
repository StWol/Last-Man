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
        private float speed;
        public float[] Liquid { get; set; }
        public Item Weapon { set; get; }
        public List<Item> Inventar { get; private set; }
        private Dictionary<int, Item> shortcuts;
        private List<Buff> bufflist;
        public Vector2 Direction { get; set; }

        /*
        *  Die(), das DropItem() und das Prüfen, ob ein Enemy gestorben ist - muss der GameManager checken und
        *  behandeln!
        */

        public Player(ILocationBehavior locationBehavior, float maxHealth, float health, float speed)
            : base(locationBehavior)
        {
            bufflist = new List<Buff>();
        }

        // Player wird getroffen von Shot:
        // holt sich Leben des Players und zieht den Schaden des Schusses davon ab - falls Leben danach <= 0, Player kaputt :)
        public void TakeDamage( Objects.Items.Shot shot )
        {
            this.Health = this.Health - shot.Damage;
            if ( this.Health <= 0 )
            {
                
                //Die(); //Muss vom GameManager aufgerufen werden!
                Debug.WriteLine("Player kaputt :) -> Leben <= 0");
            }
        }

        //Bekommt das Item und entfernt es aus der eigenen Itemliste
        public void EatItem(Item item)
        {
            this.Inventar.Remove(item);
        }

        //Bekommt die ID des Liquids und die Menge, die hinzugefügt werden soll
        public void AddLiquid(int id, float amount)
        {
            this.Liquid[id] += amount;
        }


        public void ReduceLiquid(int id, float amount)
        {
            this.Liquid[id] -= amount;
            if ( Liquid[id] - amount <= 0 )
                amount = 0;
        }

        public void AddBuffs(List<Buff> buffList)
        {
            this.bufflist.AddRange(bufflist);
        }

    }
}
