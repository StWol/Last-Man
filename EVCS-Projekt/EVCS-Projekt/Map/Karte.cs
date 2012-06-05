using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using EVCS_Projekt.Tree;
using Microsoft.Xna.Framework;
using EVCS_Projekt.Objects;
using EVCS_Projekt.Location;
using EVCS_Projekt.Renderer;
using System.Diagnostics;

namespace EVCS_Projekt.Map
{
    public class Karte
    {
        // Quadtree mit den Begehbaren Rechtecken/Objecten
        public QuadTree<StaticObject> QuadTreeWalkable { get; private set; }

        // Startpunkt des Players
        public Vector2 PlayerStart { get; private set; }

        public Texture2D[] sprites { get; set; }


        public void LoadMap(GameState gameState, string mapFile)
        {
            // Playerstart
            PlayerStart = new Vector2(300, 300);

            // Quadtree initialisieren
            QuadTreeWalkable = new QuadTree<StaticObject>(0, 0, (int)gameState.MapSize.X, (int)gameState.MapSize.Y);


            //Testobjekte
            StaticObject s1 = new StaticObject(new MapLocation(new Rectangle(300, 300, 400, 400)), new SimpleRenderer(Color.White), false);
            StaticObject s2 = new StaticObject(new MapLocation(new Rectangle(700, 300, 400, 200)), new SimpleRenderer(Color.White), false);
            StaticObject s3 = new StaticObject(new MapLocation(new Rectangle(700, 600, 100, 400)), new SimpleRenderer(Color.White), false);


            QuadTreeWalkable.Add(s1);
            QuadTreeWalkable.Add(s2);
            QuadTreeWalkable.Add(s3);
            QuadTreeWalkable.Add(new StaticObject(new MapLocation(new Rectangle(1000, 900, 2000, 200)), new SimpleRenderer(Color.White), false));

            return;
            string fileContents = string.Empty;
            TextReader reader = new StreamReader(mapFile);

            while (reader.Peek() != -1)
            {
                fileContents += reader.ReadLine().ToString();
            }
            reader.Close();
        }
    }
}
