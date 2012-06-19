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
using EVCS_Projekt.Managers;
using EVCS_Projekt.AI;

namespace EVCS_Projekt.Map
{
    public class Karte
    {
        // Quadtree mit den Begehbaren Rechtecken/Objecten
        public QuadTree<StaticObject> QuadTreeWalkable { get; private set; }

        // Startpunkt des Players
        public Vector2 PlayerStart { get; private set; }

        // Wegpunkte für Gegner
        public Dictionary<int, WayPoint> WayPoints { get; set; }
        public QuadTree<WayPoint> QuadTreeWayPoints { get; set; }


        public void LoadMap(GameState gameState, string mapFile)
        {
            // Wegpunkt liste
            WayPoints = new Dictionary<int, WayPoint>();
            QuadTreeWayPoints = new QuadTree<WayPoint>(0, 0, (int)gameState.MapSize.X, (int)gameState.MapSize.Y);

            // Quadtree initialisieren
            QuadTreeWalkable = new QuadTree<StaticObject>(0, 0, (int)gameState.MapSize.X, (int)gameState.MapSize.Y);

            // MapFile einlesen
            ReadMapFile(mapFile);

            // Wege berechnen
            PathFinder.CalculatePath();
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

                // SpawnPoints
                if (split[0].Equals("S") && split.Length == 3)
                {
                    // Variablen parsen
                    int x = int.Parse(split[1]);
                    int y = int.Parse(split[2]);

                    // SP erstellen
                    SpawnPoint sp = new SpawnPoint(new MapLocation(new Vector2(x, y)), EEnemyType.E1, 1);

                    // SP adden
                    Main.MainObject.GameManager.GameState.QuadTreeSpawnPoints.Add(sp);
                }

                // Objekte etc
                if (split[0].Equals("M") && split.Length == 6)
                {
                    // Variablen parsen
                    string img = split[1];
                    int x = int.Parse(split[2]);
                    int y = int.Parse(split[3]);
                    int faktor = int.Parse(split[4]);
                    int winkel = int.Parse(split[5]);

                    // Vars für SO
                    string renderer = "S_Map_" + img;

                    float rotation = (float)((Math.PI * 2) / 360 * winkel);

                    // SP erstellen
                    MapLocation m = new MapLocation(new Vector2(x, y));
                    m.Rotation = rotation;
                    IRenderBehavior r = LoadedRenderer.Get(renderer);
                    StaticObject so = new StaticObject(m, r);
                    so.LocationSizing();

                    // SP adden
                    Main.MainObject.GameManager.GameState.QuadTreeStaticObjects.Add(so);
                }

                // Wegpunkte W,48,1700,4538
                if (split[0].Equals("W") && split.Length == 4)
                {
                    // Variablen parsen
                    int id = int.Parse(split[1]);
                    int x = int.Parse(split[2]);
                    int y = int.Parse(split[3]);

                    // WP
                    WayPoint w = new WayPoint(x, y);
                    w.ID = id;

                    // WP adden
                    Main.MainObject.GameManager.GameState.Karte.WayPoints.Add(id, w);
                    Main.MainObject.GameManager.GameState.Karte.QuadTreeWayPoints.Add(w);
                }

                // Wegpunkte C,11,10
                if (split[0].Equals("C") && split.Length == 3)
                {
                    // Variablen parsen
                    int srcId = int.Parse(split[1]);
                    int desId = int.Parse(split[2]);

                    // Connection in BEIDE richtung
                    WayPoints[srcId].connectedPoints.Add(WayPoints[desId]);
                    WayPoints[desId].connectedPoints.Add(WayPoints[srcId]);
                }
            }

            // File schließen
            tr.Close();

            //Debug
            Debug.WriteLine(rectCount + " Rects geladen");

            return true;
        }

        public static WayPoint SearchNearest(Vector2 position)
        {
            int sizeX = 1000, sizeY = 1000;
            List<WayPoint> wpList = Main.MainObject.GameManager.GameState.Karte.QuadTreeWayPoints.GetObjects(new Rectangle((int)(position.X - sizeX / 2), (int)(position.Y - sizeY / 2), sizeX, sizeY));

            // Nearest
            WayPoint nearest = null;
            float dist = 0;

            foreach (WayPoint w in wpList)
            {
                if (nearest == null)
                {
                    if (GameManager.PointSeePoint(position, w.Location.Position))
                    {
                        // Bei null setzten
                        nearest = w;
                        dist = Vector2.Distance(w.Location.Position, position);
                    }
                }
                else
                {
                    // prüfen ob wp näher an position liegt
                    float tDist = Vector2.Distance(w.Location.Position, position);
                    if (tDist < dist && GameManager.PointSeePoint(position, w.Location.Position))
                    {
                        dist = tDist;
                        nearest = w;
                    }
                }
            }

            return nearest;
        }
    }
}
