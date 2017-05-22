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
        public List<Logiciel> Logiciels { get; set; }
        public List<Personne> Personnes { get; set; }
        public ObservableCollection<Tache> TachesAnnexes { get; set; }
        public List<Tache> list { get; set; }

        public VMTachesAnnexe()
        {
            Logiciels = DALLogiciels.GetLogicielsVersions();
            Personnes = DALPersonnes.GetPersonnesFromUser(Properties.Settings.Default.PersonneConnecte);
           
           TachesAnnexes =new ObservableCollection < Tache >(DALTaches.GetTachesAnnexe());
        }
    }
}
