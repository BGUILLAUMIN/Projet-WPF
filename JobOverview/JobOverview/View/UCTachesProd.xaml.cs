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

            //Branchement des gestionnaires évenements
            cbxLogiciels.SelectionChanged += Filtrer_Click;
            cbxVersions.SelectionChanged += Filtrer_Click;
            cbxPersonnes.SelectionChanged += Filtrer_Click;
        }

        private void Filtrer_Click(object sender, SelectionChangedEventArgs e)
        {
            //s'il y a une personne dans la combobox Personne 
            if (cbxPersonnes.SelectedValue != null)
            {
                var a = (Travail)DALTaches.GetTempsTravailGlobaux(cbxPersonnes.SelectedValue.ToString());
                Txt_Restant.Text = "Temps de travail global restants : " + a.NbrHeuresTravailGlobalRestantes.ToString() + " h    /     ";
                Txt_Realise.Text = "Temps de travail global réalisés  : " + a.NbrHeuresTravailGlobalRealisees.ToString() + " h";
            }
            else
            {
                cbxLogiciels.SelectedIndex = 0;
                cbxVersions.SelectedIndex = 0;
            }

            ICollectionView View = CollectionViewSource.GetDefaultView(_vmTacheProd.TachesProds);

            //s'il y a un logiciel, une version et une personne dans les combobox
            if (cbxVersions.SelectedValue != null && cbxPersonnes.SelectedValue != null && cbxLogiciels.SelectedValue != null)
            {
                //on applique le filtre
                View.Filter = FiltrerTachesProds;
            }
        }

        private bool FiltrerTachesProds(object o)
        {
            TacheProd tp = o as TacheProd;
            return ((cbxLogiciels.SelectedValue.ToString() == tp.CodeLogiciel) &&
                (cbxVersions.SelectedValue.ToString() == tp.Version.ToString()) &&
                (cbxPersonnes.SelectedValue.ToString() == tp.LoginPersonne));
        }

        private void cbxPersonnes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
