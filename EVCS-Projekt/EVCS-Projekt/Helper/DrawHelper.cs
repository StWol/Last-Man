using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace EVCS_Projekt.Helper
{
    class DrawHelper
    {
        // Die Referenzauflösung die zu berechnung verwendet wird
        private static Vector2 referenceSize = new Vector2(800, 600);

        // Zwischenspeicher
        private static Dictionary<string, Vector2> dimensions = new Dictionary<string,Vector2>();
        private static Dictionary<string, Vector2> unmodifiedDimensions = new Dictionary<string,Vector2>();

        public static void ConvertDimensions()
        {
            // Rechnet die Position in die Aktuelle Bildschirmgröße um
            int resWidht = Configuration.GetInt("resolutionWidth");
            int resHeight = Configuration.GetInt("resolutionHeight");

            // Umrechnen und neu speichern
            foreach (var dim in unmodifiedDimensions )
            {
                Vector2 calc = new Vector2();

                calc.X = (int)(dim.Value.X / referenceSize.X * resWidht);
                calc.Y = (int)(dim.Value.Y / referenceSize.Y * resHeight);

                dimensions[dim.Key] = calc;
            }
        }

        public static void AddDimension(string key, int x, int y)
        {
            // Speicher die werte zusätzlich unverändert
            unmodifiedDimensions.Add(key, new Vector2(x,y));

            // Rechnet die Position in die Aktuelle Bildschirmgröße um
            int resWidht = Configuration.GetInt("resolutionWidth");
            int resHeight = Configuration.GetInt("resolutionHeight");

            Vector2 calc = new Vector2((int)(x / referenceSize.X * resWidht), (int)(y / referenceSize.Y * resHeight));
            dimensions.Add(key, calc);
        }

        public static Vector2 Get(string key)
        {
            // Getter
            return dimensions[key];
        }
    }
}
