using System;
using System.Collections.Generic;
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
using JobOverview.ViewModel;
using System.Collections.ObjectModel;
using JobOverview.Entity;
using JobOverview.Model;
using System.ComponentModel;
using JobOverview.View;

namespace JobOverview.View
{

    /// <summary>
    /// Interaction logic for UCTachesProd.xaml
    /// </summary>
    public partial class UCTachesProd : UserControl
    {
        private VMTachesProd _vmTacheProd;
        public UCTachesProd()
        {
            InitializeComponent();
            _vmTacheProd = new VMTachesProd();
            DataContext = _vmTacheProd;

            // Branchement des gestionnaires évènements.
            // Filtrage des tâches de production lors d'un click sur le bouton Appliquer filtre.
            BtnFiltre.Click += BtnFiltre_Click;
            // Mise à jour des temps globaux en fonction de la personne selectionnée dans la comboBox cbxPersonnes.
            cbxPersonnes.SelectionChanged += cbxPersonnes_SelectionChanged;
        }

        private void BtnFiltre_Click(object sender, RoutedEventArgs e)
        {

            // S'il y a une personne selectionnée dans la combobox Personne, on affecte aux texblocks les 
            // valeurs de temps global restants et réalisées pour cette personne. 

            ICollectionView View = CollectionViewSource.GetDefaultView(_vmTacheProd.TachesProds);

            // S'il y a un logiciel, une version et une personne dans les combobox, on applique un filtre sur les tâches de production
            // en appelant la méthode FiltrerTachesProds et en lui passant en paramètre l'objet view.
            if (cbxVersions.SelectedValue != null && cbxPersonnes.SelectedValue != null && cbxLogiciels.SelectedValue != null)
            {
                // On applique le filtre.
                View.Filter = FiltrerTachesProds;

                if (_vmTacheProd.TacheCourante != null)
                    cbxLogiciels.SelectedValue = _vmTacheProd.TacheCourante.CodeLogiciel;
                else
                    cbxLogiciels.SelectedIndex = 0;

                if (_vmTacheProd.TacheCourante != null)
                    cbxVersions.SelectedValue = _vmTacheProd.TacheCourante.Version;
                else
                    cbxVersions.SelectedIndex = 0;
            }

        }

        // Méthode permettant de filtrer les tâches de production en fonction du logiciel, de la version 
        // et de la personne dans les combobox.
        private bool FiltrerTachesProds(object o)
        {
            TacheProd tp = o as TacheProd;
            bool a = true;
            if (ckbTachesTerm.IsChecked == false && tp.DureeRestante == 0)
            {
                a = false;
            }

            return (a && (cbxLogiciels.SelectedValue.ToString() == tp.CodeLogiciel) &&
                (cbxVersions.SelectedValue.ToString() == tp.Version.ToString()) &&
                (cbxPersonnes.SelectedValue.ToString() == tp.LoginPersonne));
        }

        private bool FiltrerTacheTerminées(object o)
        {
            TacheProd tp = o as TacheProd;
            return (tp.DureeRestante == 0);
        }

        private void cbxPersonnes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbxPersonnes.SelectedValue != null)
                Properties.Settings.Default.PersonneCourante = cbxPersonnes.SelectedValue.ToString();

            if (cbxPersonnes.SelectedValue != null)
            {
                var a = (Travail)DALTaches.GetTempsTravailGlobaux(cbxPersonnes.SelectedValue.ToString());
                Txt_Restant.Text = "Temps de travail global restants : " + a.NbrHeuresTravailGlobalRestantes.ToString() + " h    /     ";
                Txt_Realise.Text = "Temps de travail global réalisés  : " + a.NbrHeuresTravailGlobalRealisees.ToString() + " h";
            }
        }
    }
}
