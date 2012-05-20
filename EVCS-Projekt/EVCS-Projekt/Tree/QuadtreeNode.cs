using System.Collections.Generic;
using EVCS_Projekt.Objects;
using EVCS_Projekt.Objects.Items;
using Microsoft.Xna.Framework;

namespace EVCS_Projekt.Tree
{
    class QuadtreeNode
    {
        private QuadtreeNode[] children;

        public List<GameObject> Objects { get; private set; }
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
            if (children == null)
            {
                List<GameObject> res = new List<GameObject>();

                foreach (GameObject o in Objects)
                {
                    if (rect.Intersects(o.GetBoundingBox()))
                    {
                        res.Add(o);
                    }
                }

                return res;
            }
            else
            {
                List<GameObject> res = new List<GameObject>();

                for (int i = 0; i < 4; i++)
                {
                    if (rect.Intersects(new Rectangle((int)children[i].Position.X, (int)children[i].Position.Y, (int)children[i].Size.X, (int)children[i].Size.Y)))
                    {
                        res.AddRange(children[i].GetObjects(rect));
                    }
                }

                return res;
            }
        }

        public void AddObject(GameObject obj)
        {
            if (children == null)
            {

                Objects.Add(obj);

                if (Objects.Count > objectLimit && (Size.X > minQuadSize || Size.Y > minQuadSize))
                {
                    children = new QuadtreeNode[4];

                    for (int i = 0; i < 4; i++)
                        children[i] = new QuadtreeNode();

                    children[0].Position = new Vector2(Position.X, Position.Y);
                    children[1].Position = new Vector2(Position.X + Size.X / 2, Position.Y);
                    children[2].Position = new Vector2(Position.X, Position.Y + Size.Y / 2);
                    children[3].Position = new Vector2(Position.X + Size.X / 2, Position.Y + Size.Y / 2);

                    for (int i = 0; i < 4; i++)
                        children[i].Size = new Vector2(Size.X / 2, Size.Y / 2);

                    foreach (GameObject o in Objects)
                    {
                        Vector2 position = o.LocationBehavior.Position;

                        if (position.X < children[1].Position.X)
                        {
                            if (position.Y < children[2].Position.Y)
                                children[0].AddObject(o);
                            else
                                children[2].AddObject(o);
                        }
                        else
                        {
                            if (position.Y < children[3].Position.Y)
                                children[1].AddObject(o);
                            else
                                children[3].AddObject(o);
                        }

                    }

                    Objects = null;
                }

            }
            else
            {
                Vector2 position = obj.LocationBehavior.Position;

                if (position.X < children[1].Position.X)
                {
                    if (position.Y < children[2].Position.Y)
                        children[0].AddObject(obj);
                    else
                        children[2].AddObject(obj);
                }
                else
                {
                    if (position.Y < children[3].Position.Y)
                        children[1].AddObject(obj);
                    else
                        children[3].AddObject(obj);
                }
            }
        }

        public void AddEnemy(Enemy enemy)
        {
            // TODO: Alles in eine Methode packen
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
