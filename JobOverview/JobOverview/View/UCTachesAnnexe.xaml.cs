using JobOverview.Entity;
using JobOverview.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace JobOverview.View
{
    /// <summary>
    /// Interaction logic for UCTachesAnnexe.xaml
    /// </summary>
    public partial class UCTachesAnnexe : UserControl
    {
        #region Propriété privée
        private VMTachesAnnexe _vmTachesAnnexe;
        #endregion
        public UCTachesAnnexe()
        {
            InitializeComponent();

            //Instancie une nouvelle vue Modèle TachesAnnexe et on le définit en DataContext
            _vmTachesAnnexe = new VMTachesAnnexe();
            DataContext = _vmTachesAnnexe;

            //Branchement des gestionnaires d'évennements
            cbPersonne.SelectionChanged += Filtrer_Click;
        }

        private void Filtrer_Click(object sender, SelectionChangedEventArgs e)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(_vmTachesAnnexe.TachesAnnexes);

            //Si La combobox personnes est remplie on va appliquer le filtre
            if (cbPersonne.SelectedValue != null)
            {
                //on applique le filtre
                view.Filter = FiltrerTachesAnnexes;
            }
        }


        //Méthode de filtrage
        private bool FiltrerTachesAnnexes(object o)
        {
            Tache tp = o as Tache;
            //Filtre les tâches en fonction de la personne délectionné dans la combobox
            return (cbPersonne.SelectedValue.ToString() == tp.LoginPersonne);

        }
    }

}
