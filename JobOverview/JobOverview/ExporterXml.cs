using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobOverview.Entity;
using System.Xml.Serialization;

namespace JobOverview.View
{
   public class ExportXml
    {

        /// <summary>
        /// Sérialisation des tâches. Permet d'exporter les données tâches en données xml
        /// </summary>
        /// <param name="taches"></param>
        public static void EnregistrerTaches(List<Tache> taches)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Tache>),
                                                new XmlRootAttribute("Taches"));
            using (TextWriter writer = new StreamWriter("Taches.xml"))
                serializer.Serialize(writer, taches);
        }
    }
}
