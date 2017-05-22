using JobOverview.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobOverview.Model;
using System.Collections.ObjectModel;

namespace JobOverview.ViewModel
{
    public class VMTachesProd : ViewModelBase
    {
        public List<Logiciel> Logiciels { get; set; }
        public List<Personne> Personnes { get; set; }
        public ObservableCollection<TacheProd> TachesProds { get; set; }

        public VMTachesProd()
        {
            Logiciels = DALLogiciels.GetLogicielsVersions();
            Personnes = DALPersonnes.GetPersonnesFromUser(Properties.Settings.Default.PersonneConnecte);
            //TachesProds = DALTaches.GetTachesProd();
        }

    }
}
