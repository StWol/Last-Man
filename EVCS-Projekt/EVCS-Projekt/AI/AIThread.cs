using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;

namespace EVCS_Projekt.AI
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
                List<Enemy> enemies = Main.MainObject.GameManager.GameState.QuadTreeEnemies.GetObjects(updateRect);

                foreach (Enemy e in enemies)
                {
                    e.CalculateActivity();
                }

                // Nur max. alle 50 sekunden ausführen 
                Thread.Sleep(50);
            }
        }
    }
}
