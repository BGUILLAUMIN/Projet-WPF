using JobOverview.Entity;
using JobOverview.Model;
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

            // Instanciation d'une nouvelle vue-Modèle TachesAnnexe.
            // Puis, on le définit en DataContext.
            _vmTachesAnnexe = new VMTachesAnnexe();
            DataContext = _vmTachesAnnexe;

            // Branchement des gestionnaires d'évennements.
            cbPersonne.SelectionChanged += Filtrer_Click;
            btnAjouter.Click += BtnAjouter_Click;
            cbPersonne2.SelectionChanged += CbPersonne2_SelectionChanged;
            cbPersonne.SelectionChanged += CbPersonne_SelectionChanged;
        }

        private void CbPersonne_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Properties.Settings.Default.PersonneCourante = cbPersonne.SelectedValue.ToString();
        }

        private void CbPersonne2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbPersonne.SelectedValue != null)
            {
                _vmTachesAnnexe.ActivitesAutorisées = DALTaches.GetActivitésAnnexesFiltrées(cbPersonne.SelectedValue.ToString());
            }
        }

        private void BtnAjouter_Click(object sender, RoutedEventArgs e)
        {
            _vmTachesAnnexe.ActivitesAutorisées = DALTaches.GetActivitésAnnexesFiltrées(cbPersonne.SelectedValue.ToString());
        }

        private void Filtrer_Click(object sender, SelectionChangedEventArgs e)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(_vmTachesAnnexe.TachesAnnexes);

            // Si une personne est selectionnée dans la combobox personnes, on applique le filtre FiltrerTachesAnnexes.
            if (cbPersonne.SelectedValue != null)
            {
                // On applique le filtre en appelant la méthode FiltrerTachesAnnexes.
                view.Filter = FiltrerTachesAnnexes;
            }
        }


        // Méthode de filtrage.
        private bool FiltrerTachesAnnexes(object o)
        {
            Tache tp = o as Tache;
            // Filtrage des tâches en fonction de la personne selectionné dans la combobox.
            return (cbPersonne.SelectedValue.ToString() == tp.LoginPersonne);

        }
    }

}
