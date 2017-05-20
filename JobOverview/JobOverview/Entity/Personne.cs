using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace JobOverview.Entity
{
    public class Personne
    {
        public string Login { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Manager { get; set; }
        public string CodeMetier { get; set; }
        public string LibelleMetier { get; set; }
        public string CodeEquipe { get; set; }
        public string NomEquipe { get; set; }

        // Activités liées au métier de la personne
        public List<Activite> Activites { get; set; }

        // Propriété utilisée pour affichage uniquement
        public string NomComplet { get { return Nom + " " + Prenom; } }
    }

    public class Activite
    {
        public string Code { get; set; }
        public string Libelle { get; set; }
    }
}
