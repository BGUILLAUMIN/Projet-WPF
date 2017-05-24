using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobOverview.Entity;
using JobOverview.Model;

namespace JobOverview.ViewModel
{
	public class VMLogin : ViewModelBase
	{
        #region Propriétés
        public List<Personne> Personnes { get;}
        #endregion

        #region Constructeurs
        public VMLogin()
        {
            // On instancie une nouvelle liste de Personnes.
            Personnes = new List<Personne>();

            // On remplie cette liste avec les Personnes de la base.
            Personnes = DALPersonnes.GetPersonnes();
        } 
        #endregion
    }
}
