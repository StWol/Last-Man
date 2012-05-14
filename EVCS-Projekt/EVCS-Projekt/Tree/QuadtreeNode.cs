using System.Collections.Generic;
using EVCS_Projekt.Objects;
using EVCS_Projekt.Objects.Items;
using Microsoft.Xna.Framework;

namespace EVCS_Projekt.Tree
{
    class QuadtreeNode
    {
        private QuadtreeNode[] children;

        public List<Enemy> Enemies { get; private set; }
        public List<Item> Items { get; private set; }
        public List<Shot> Shots { get; private set; }

        public Vector2 Position;
        public Vector2 Size;

        private static int objectLimit;
        private static int minQuadSize;

        public QuadtreeNode()
        {

        }

        public List<GameObject> GetObjects(Rectangle rect)
        {

            return new List<GameObject>();
        }

        public void AddObject(GameObject obj)
        {

        }

        public void AddEnemy(Enemy enemy)
        {
            Enemies.Add(enemy);
        }

        public void AddItem(Item item)
        {
            Items.Add(item);
        }

        public void AddShot(Shot shot)
        {
            Shots.Add(shot);
        }
    }
}
