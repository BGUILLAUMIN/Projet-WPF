﻿using System;
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

namespace JobOverview.View
{
    /// <summary>
    /// Interaction logic for UCTachesProd.xaml
    /// </summary>
    public partial class UCTachesProd : UserControl
    {
        public UCTachesProd()
        {
            InitializeComponent();
            DataContext = new VMTachesProd();
            cbxLogiciels.SelectionChanged += FiltrerTachesProds;
            cbxVersions.SelectionChanged += FiltrerTachesProds;
            cbxPersonnes.SelectionChanged += FiltrerTachesProds;

        }

        private void FiltrerTachesProds(object sender, SelectionChangedEventArgs e)
        {
            //ICollectionView view = CollectionViewSource.GetDefaultView(DataContext);

            //view.SortDescriptions.Clear();
            //view.GroupDescriptions.Clear();
        }

    }
}
