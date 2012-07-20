using System.Collections.Generic;
using System.Linq;
using System.Xml;

using System.Diagnostics;

namespace LastMan.Helper.XMLManager
{
    class XMLManager
    {

        /*
         * Beim Erzeugen eines Objektes wird die URL zu der Fiel benötigt. Damit ist sichergestellt, 
         * dass jede Datei nur von einem Objekt bearbeitet werden kann. Die Microsoftleute raten dazu,
         * damit konsistenter Datentransfer gewährleistet ist.
         */

        public string urlToConfigFile { get; set; }



        public XMLManager(string FileURL)
        {
            this.urlToConfigFile = FileURL;
        }




        // ***************************************************************************
        // Fängt an zu schreiben
        public static XmlWriter StartXML(string outFile)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = ("    ");

            XmlWriter writer = XmlWriter.Create(outFile, settings);
            writer.WriteStartDocument();
            writer.WriteStartElement("root");

            return writer;
        }

        // ***************************************************************************
        // Beendet an zu schreiben
        public static void EndXML(XmlWriter writer)
        {
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            writer.Close();
        }



        //Zeigt das XML-File in der Debugkonsole
        public void displayXML()
        {
            XmlReader reader = XmlReader.Create(urlToConfigFile);
            Debug.WriteLine("File is founded...");
            Debug.WriteLine("Try to display File...");
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element: // Der Knoten ist ein Element.
                        Debug.WriteLine("<" + reader.Name + ">");
                        break;
                    case XmlNodeType.Text: //Anzeigen des Textes in jedem Element.
                        Debug.WriteLine(reader.Value);
                        break;
                    case XmlNodeType.EndElement: //Anzeigen des Endes des Elements.
                        Debug.WriteLine("</" + reader.Name + ">");
                        break;
                }
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
                if (NodeName.Equals(reader.Name))
                    Debug.WriteLine(NodeName + ": " + reader.ReadString());
            }
        }


        //Gibt eine Liste der gesammelten Werte von dem Knoten zurück, den wir aufgerufen haben
        public List<string> GetValuesOfNode(string NodeName)
        {
            List<string> valueArray = new List<string>();
            XmlReader reader = XmlReader.Create(urlToConfigFile);
            Debug.WriteLine("File is founded...");
            Debug.WriteLine("Try to collecting Values of Node " + NodeName);
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

        //Erstellt ein Dokument für einen abänderbares XML. Diese Methode soll nur als Template dienen, damit
        //wir später leichter gezielte Dokumente schreiben können, ohne mit Listen arbeiten zu müssen
        public void WriteCustomXML(string value1, string value2)
        {
            Debug.WriteLine("Try to write in XML-File...");
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = ("    ");
            using (XmlWriter writer = XmlWriter.Create(urlToConfigFile, settings))
            {
                writer.WriteStartElement("player");
                writer.WriteElementString("Health", value1);
                writer.WriteElementString("Score", value2);
                writer.WriteEndElement();
                writer.Flush();
            }

        }

        //Erstellt ein XML-Dokument, bei dem alle Kindknoten des Rootelements auf einem Level sind
        public void CreateXMLFromLists_L1(string rootName, List<string> nodeNameList, List<string> valueList)
        {
            Debug.WriteLine("Try to write in XML-File...");
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = ("    ");
            if (nodeNameList.Count != valueList.Count)
            {
                Debug.WriteLine("Warnung: Listen sind nicht gleich lang - Störung im XML- Dokument erwartet: Aktion abgebrochen!");
            }
            else
            {
                using (XmlWriter writer = XmlWriter.Create(urlToConfigFile, settings))
                {
                    writer.WriteStartElement(rootName);
                    for (int i = 0; i < nodeNameList.Count; i++)
                    {
                        writer.WriteElementString(nodeNameList.ElementAt(i), valueList.ElementAt(i));
                    }
                    writer.WriteEndElement();
                    writer.Flush();
                    Debug.WriteLine("XML created");
                }
            }
        }
    }
}
