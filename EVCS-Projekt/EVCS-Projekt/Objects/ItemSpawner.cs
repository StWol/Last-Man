using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LastMan.Map;
using LastMan.Location;
using Microsoft.Xna.Framework;
using LastMan.Objects.Items;
using LastMan.Helper;

namespace LastMan.Objects
{
    public class ItemSpawner
    {
        public static void SpawnItems()
        {
            // Randomwert für "Wahrscheinlichkei"
            Random rand = new Random();

            //Debug.WriteLine("Spawning..");
            int debugCount = 0;

            List<WayPoint> allWayPoints = Main.MainObject.GameManager.GameState.Karte.QuadTreeWayPoints.GetAllObjects();
            List<WayPoint> wayPointsInScreen = Main.MainObject.GameManager.GameState.Karte.QuadTreeWayPoints.GetObjects(new Rectangle((int)Main.MainObject.GameManager.GameState.MapOffset.X, (int)Main.MainObject.GameManager.GameState.MapOffset.Y, Configuration.GetInt("resolutionWidth"), Configuration.GetInt("resolutionHeight")));

            foreach ( WayPoint w in wayPointsInScreen) {
                allWayPoints.Remove(w);
            }

            foreach (WayPoint w in allWayPoints)
            {

                float random = (float)rand.NextDouble();

                // Was würde spawnen
                Item spawn = Item.AllItemsList[rand.Next(0, Item.AllItemsList.Count)];

                // Keine waffen spawnen
                while (typeof(Weapon) == spawn.GetType())
                {
                    spawn = Item.AllItemsList[rand.Next(0, Item.AllItems.Count)];
                }

                // Darf er Spawnen ? - Munition hat größere chance
                if (0.9F < random || spawn.GetType() == typeof(Munition) && 0.8F < random)
                {
                    // Liegt da schon eins ?
                    List<Item> itemsOnGround = Main.MainObject.GameManager.GameState.QuadTreeItems.GetObjects(w.Rect);
                    if (itemsOnGround.Count > 0)
                        continue;


                    // Clonen
                    spawn = Item.Get(spawn.TypeId);
                    spawn.LocationBehavior = new MapLocation(w.Location.Position, DrawHelper.Get("IconOnTheFloor_Size"));

                    Main.MainObject.GameManager.GameState.QuadTreeItems.Add(spawn);

                    debugCount++;
                }
                
            }
        }
    }
}
