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
            _vmTachesAnnexe = new VMTachesAnnexe();
            DataContext = _vmTachesAnnexe;
            cbPersonne.SelectionChanged += Filtrer_Click;
        }

        private void Filtrer_Click(object sender, SelectionChangedEventArgs e)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(_vmTachesAnnexe.TachesAnnexes);

            if (cbPersonne.SelectedValue != null)
            {
                view.Filter = FiltrerTachesAnnexes;
            }
        }

        private bool FiltrerTachesAnnexes(object o)
        {
            Tache tp = o as Tache;
            return (cbPersonne.SelectedValue.ToString() == tp.LoginPersonne);
               
        }
    }

}
