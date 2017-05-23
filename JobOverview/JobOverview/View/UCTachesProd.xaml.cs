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

            ckbTachesTerm.Unchecked += CkbTachesTerm_Unchecked;
            cbxLogiciels.SelectionChanged += Filtrer_Click;
            cbxVersions.SelectionChanged += Filtrer_Click;
            cbxPersonnes.SelectionChanged += Filtrer_Click; 
        }

        private void CkbTachesTerm_Unchecked(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Filtrer_Click(object sender, SelectionChangedEventArgs e)
        {
            var list = _vmTacheProd.TachesProds;
            if (!(bool)ckbTachesTerm.IsChecked)
            {
                list = new ObservableCollection<TacheProd>(_vmTacheProd.TachesProds.Where(t => t.DureeRestante != 0).ToList());
            }
                ICollectionView view = CollectionViewSource.GetDefaultView(list);

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
                (cbxPersonnes.SelectedValue.ToString() == tp.LoginPersonne));
        }
    }
}
