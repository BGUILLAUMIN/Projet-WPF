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

            cbxLogiciels.SelectionChanged += Filtrer_Click;
            cbxVersions.SelectionChanged += Filtrer_Click;
            cbxPersonnes.SelectionChanged += Filtrer_Click;
            //cbxModule.SelectionChanged += Filtrer_Click;
        }


        private void Filtrer_Click(object sender, SelectionChangedEventArgs e)
        {
            var list = _vmTacheProd.TachesProds;

            if(cbxPersonnes.SelectedValue != null)
            {

            var a = (Travail)DALTaches.GetTempsTravailGlobaux(cbxPersonnes.SelectedValue.ToString());
            Txt_Restant.Text = "Temps de travail global restants : " + a.NbrHeuresTravailGlobalRestantes.ToString();
            Txt_Realise.Text = "Temps de travail global réalisés :   " + a.NbrHeuresTravailGlobalRealisees.ToString();

            }


            if (!(bool)ckbTachesTerm.IsChecked)
            {
                list = new ObservableCollection<TacheProd>(_vmTacheProd.TachesProds.Where(t => t.DureeRestante != 0).ToList());
            }
                ICollectionView view = CollectionViewSource.GetDefaultView(list);

            //Si les combobox de Version Logiciel et Personnes sont vides, on ne fait rien
            if (cbxVersions.SelectedValue != null && cbxLogiciels.SelectedValue != null && cbxPersonnes.SelectedValue != null)
            {
                view.Filter = FiltrerTachesProds;
            }
        }

        private bool FiltrerTachesProds(object o)
        {
            TacheProd tp = o as TacheProd;
            return ((cbxLogiciels.SelectedValue.ToString() == tp.CodeLogiciel) &&
                (cbxVersions.SelectedValue.ToString() == tp.Version.ToString()) &&
                (cbxPersonnes.SelectedValue.ToString() == tp.LoginPersonne)); /*&&*/
             //   (cbxModule.SelectedValue.ToString() == tp.LoginPersonne));
                
        }
    }
}
