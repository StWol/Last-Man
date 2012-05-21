using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EVCS_Projekt.Tree;

namespace EVCS_Projekt
{
    class GameState
    {
        public QuadTree<Enemy> QuadTreeEnemies { get; set; }
    }
}
