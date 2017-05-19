using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobOverview.Entity
{
    public class Logiciel
    {
        public string Code { get; set; }
        public string Nom { get; set; }
        public List<Version> Versions { get; set; }
        public List<Module> Modules { get; set; }
    }

    public class Version
    {
        public DateTime DateOuverture { get; set; }
        public DateTime DateSortiePrevue { get; set; }
        public DateTime? DateSortieReelle { get; set; }
        public float Numero { get; set; }
        public int Millesime { get; set; }
        public int DerniereRelease { get; set; }
    }

    public class Module
    {
        public string Code { get; set; }
        public string Libelle { get; set; }
        public string CodeModuleParent { get; set; }
    }
}
