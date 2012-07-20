using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Xna.Framework;

namespace LastMan.AI
{
    class AIThread
    {
        public static bool IsUpdating { get; set; }
        public static bool Running { private get; set; }

        public static void UpdateAI()
        {
            Running = true;

            while (Running)
            {
                // Enemies im updateRect holen
                Rectangle updateRect = Main.MainObject.GameManager.UpdateRectangle;

                // Gegner holen
                List<Enemy> enemies = Main.MainObject.GameManager.GameState.QuadTreeEnemies.GetAllObjects();

                foreach (Enemy e in enemies)
                {
                    double s = DateTime.Now.Ticks;
                    e.CalculateActivity();
                    //Debug.WriteLine((DateTime.Now.Ticks-s)/10+"us");
                }

                // Nur max. alle 50 sekunden ausführen 
                Thread.Sleep(50);
            }
        }
    }
}
