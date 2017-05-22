using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace JobOverview.Entity
{
    public class Tache
    {
        [XmlIgnore]
        public Guid Id { get; set; }
        [XmlAttribute]
        public string Libelle { get; set; }
        [XmlAttribute]
        public string Description { get; set; }
        [XmlAttribute]
        public string CodeActivite { get; set; }
        [XmlAttribute]
        public string LoginPersonne { get; set; }

        public List<Travail> Travaux { get; set; }
    }

    public class TacheProd : Tache
    {
        [XmlAttribute]
        public int Numero { get; set; }
        [XmlAttribute("DureePrev")]
        public float DureePrevue { get; set; }
        [XmlAttribute("DureeRest")]
        public float DureeRestante { get; set; }
        [XmlAttribute]
        public string CodeLogiciel { get; set; }
        [XmlAttribute]
        public string CodeModule { get; set; }
        [XmlAttribute("NuméroVersion")]
        public float Version { get; set; }
    }

    public class Travail
    {
        [XmlAttribute]
        public DateTime Date { get; set; }
        [XmlAttribute]
        public float Heures { get; set; }
        [XmlAttribute("TauxProduct")]
        public float TauxProductivite { get; set; }
    }
}
