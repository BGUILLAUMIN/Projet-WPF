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
        #region Propriétés
        public List<Logiciel> Logiciels { get; }
        public List<Module> Modules { get; }

        #endregion

        #region Constructeurs
        public VMVersion()
        {
            // Permet à la comboBox d'afficher la liste des logiciels disponibles au chargement de la fenêtre.
            Logiciels = DALLogiciels.GetLogicielsVersions();
        
        }

        #endregion
    }
}
