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
            //PlayerStart = new Vector2(300, 300);

            // Quadtree initialisieren
            QuadTreeWalkable = new QuadTree<StaticObject>(0, 0, (int)gameState.MapSize.X, (int)gameState.MapSize.Y);

            // MapFile einlesen
            ReadMapFile(mapFile);

            /*
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
             * */
        }

        private bool ReadMapFile(string mapFile)
        {
            Debug.WriteLine("Lade Map " + mapFile + "..");

            // prüfen ob config.ini existiert
            if (!File.Exists(Configuration.Get("mapDir") + mapFile))
            {
                Debug.WriteLine("MapFile " + mapFile + " nicht vorhanden");
                return false;
            }

            // Map File öffnen
            TextReader tr = new StreamReader(Configuration.Get("mapDir") + mapFile);

            // Debug
            int rectCount = 0;

            // Alle lines einlesen, bei ',' trennen und diese bei [0]=R in Rechtecke einteilen
            string input;
            while ((input = tr.ReadLine()) != null)
            {
                // falls erstes zeichen eine # ist, dann ist die zeile ein kommenatar
                if (input.Length < 1 || input.Substring(0, 1).Equals("#"))
                {
                    continue;
                }

                string[] split = input.Split(new char[] { ',' });

                // Map Rechteck
                if (split[0].Equals("R") && split.Length == 5)
                {
                    rectCount++;

                    // Variablen parsen
                    int x = int.Parse(split[1]);
                    int y = int.Parse(split[2]);
                    int w = int.Parse(split[3]);
                    int h = int.Parse(split[4]);

                    // SO erstellen
                    StaticObject so = new StaticObject(new MapLocation(new Rectangle(x, y, w, h)), new SimpleRenderer(Color.White), false);

                    // SO dem QT zufügen
                    QuadTreeWalkable.Add(so);
                }

                // Playerstart
                if (split[0].Equals("X") && split.Length == 3)
                {
                    // Variablen parsen
                    int x = int.Parse(split[1]);
                    int y = int.Parse(split[2]);

                    // SO erstellen
                    PlayerStart = new Vector2(x, y);

                    Debug.WriteLine("Playerstart: " + x + "/" + +y);
                }
            }

            // File schließen
            tr.Close();

            //Debug
            Debug.WriteLine(rectCount + " Rects geladen");

            return true;
        }
    }
}
