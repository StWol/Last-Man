using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EVCS_Projekt.Tree;
using Microsoft.Xna.Framework;
using EVCS_Projekt.Objects.Items;

namespace EVCS_Projekt
{
    public class GameState
    {
        public QuadTree<Enemy> QuadTreeEnemies { get; set; }
        public List<Shot> ShotList { get; set; }
        public Player Player { get; set; }

        public Vector2 MapOffset { get; set; }

        public Vector2 MapSize { get; set; }
    }
}
