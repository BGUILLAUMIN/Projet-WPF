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
using JobOverview.Model;
using JobOverview.Entity;
using JobOverview.ViewModel;

namespace JobOverview.View
{
    /// <summary>
    /// Logique d'interaction pour UCVersions.xaml
    /// </summary>
    public partial class UCVersions : UserControl
    {
        private VMVersion _vmVersions;

        public UCVersions()
        {
            InitializeComponent();
            _vmVersions = new VMVersion();
            DataContext = _vmVersions;

           // cbxVersion.SelectionChanged += Filtrer_Click;


        }

        // Todo réaliser un filtrage sur la listview de UCVersion par rapport au numéro de version selectionné dans la ComboBox cbxVersion.

        //private void Filtrer_Click(object sender, SelectionChangedEventArgs e)
        //{

        //        ICollectionView view = CollectionViewSource.GetDefaultView(_vmVersions.Logiciels);
        //    if (cbxVersion.SelectedValue != null)
        //    {
        //        view.Filter = FiltrerVersions;
        //    }
        //}

        //private bool FiltrerVersions(object o)
        //{
        //    Logiciel lo = o as Logiciel;
        //    return ((cbxVersion.SelectedValue.ToString() == lo.Versions.ToString()));
        //}
    }
}
