using System.Collections.Generic;
using EVCS_Projekt.Objects;
using Microsoft.Xna.Framework;

namespace EVCS_Projekt.Tree
{
    class Quadtree
    {
        private QuadtreeNode firstNode;

        //Wird size nirgend gespeichert?
        public Quadtree(Vector2 size)
        {
            firstNode = new QuadtreeNode();
            firstNode.Position = new Vector2(0,0);
            firstNode.Size = size;
        }

        public void Add(GameObject o)
        {
            firstNode.AddObject(o);
        }

        public List<GameObject> Get(Rectangle rect)
        {
            return firstNode.GetObjects(rect);
        } 

        public void Remove(GameObject o)
        {
            // TODO
        }

        // Die Methoden GetEnemyList, -ItemList, -ShotList sind doch schon in dem QuadtreeNode drin..
    }
}
