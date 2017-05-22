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

        public VMVersion()
        {
            Logiciels = DALLogiciels.GetLogicielsVersions();
        }
    }
}
