using JobOverview.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobOverview.Model;


namespace JobOverview.ViewModel
{

    class VMVersion : ViewModelBase
    {
        public List<Logiciel> Logiciels { get; set; }
        public List<Module> Modules { get; set; }

        public VMVersion()
        {
            // Permet à la comboBox d'afficher la liste des logiciels disponibles au chargement de la fenêtre.
            Logiciels = DALLogiciels.GetLogicielsVersions();

        
        }
    }
}
