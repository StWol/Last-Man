using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using System.Diagnostics;

namespace EVCS_Projekt.Helper.XMLManager
{
    class XMLManager
    {
       public string urlToConfigFile { get; set; }

       public XMLManager(string FileURL)
       {
            this.urlToConfigFile = FileURL;
       }

        //Zeigt das XML-File in der Debugkonsole
        public void displayXML()
        {
            XmlReader reader = XmlReader.Create(urlToConfigFile);
            Debug.WriteLine("File is founded...");
            Debug.WriteLine("Try to display File...");
            while (reader.Read())
            {
                reader.MoveToElement();
                Debug.WriteLine("<" +reader.LocalName + "> " + reader.Value);
            }
        }

        //Zeigt die Werte des Knotens, den wir haben möchten (falls mehrere Knoten des Namens existieren,
        // zeigt er mehrere an)
        public void DisplayValuesOfNode(string NodeName)
        {
            XmlReader reader = XmlReader.Create(urlToConfigFile);
                Debug.WriteLine("File is founded...");
                Debug.WriteLine("Searching for " + NodeName + " in File");
                while (reader.Read())
                {
                    reader.MoveToElement();
                    if ( NodeName.Equals(reader.Name ))
                    Debug.WriteLine(NodeName +": " + reader.ReadString());
                }
        }


        //Gibt eine Liste der gesammelten Werte von dem Knoten zurück, den wir aufgerufen haben
        public List<string> GetValuesOfNode(string NodeName)
        {
            List<string> valueArray = new List<string>();
            XmlReader reader = XmlReader.Create(urlToConfigFile);
            Debug.WriteLine("File is founded...");
            Debug.WriteLine("Try to collecting Values of Node " + NodeName );
            while (reader.Read())
            {
                reader.MoveToElement();
                if (NodeName.Equals(reader.Name))
                {
                    Debug.WriteLine(NodeName + ": " + reader.ReadString());
                    valueArray.Add(reader.ReadString());
                }

            }
            return valueArray;
        }
    }
}
