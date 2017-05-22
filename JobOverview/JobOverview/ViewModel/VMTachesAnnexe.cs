using JobOverview.Entity;
using JobOverview.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobOverview.ViewModel
{

  
    public class VMTachesAnnexe:ViewModelBase
    {

        #region champs privés
        private Tache _nouvelleTache;
        private ModesEdition _mode;

        #endregion

        public List<Activite> Activites{ get; set; }
        public List<Personne> Personnes { get; set; }
        public ObservableCollection<Tache> TachesAnnexes { get; set; }
        

        public VMTachesAnnexe()
        {
            Activites = DALPersonnes.GetActivite();
            Personnes = DALPersonnes.GetPersonnesFromUser(Properties.Settings.Default.PersonneConnecte);
           
           TachesAnnexes =new ObservableCollection < Tache >(DALTaches.GetTachesAnnexe());
        }
    }
}
