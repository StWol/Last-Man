using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace EVCS_Projekt
{
    class Player : GameObject
    {
        //Attributes
        private float maxHealth;
        private float health;
        private Item weapon;
        private List<Item> inventar;
        private float speed;
        private Dictionary<int, Item> shortcuts;
        private float[] liquid;
        private List<Buff> bufflist;

        public Player (Game game, ILocationBehavoir locationBehavior)
            : base(game, locationBehavior)
        {

        }

    }
}
