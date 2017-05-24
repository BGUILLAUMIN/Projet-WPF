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


            // Branchement des gestionnaires d'évenements.
            ckbTachesTerm.Unchecked += CkbTachesTerm_Unchecked;
            ckbTachesTerm.Checked += CkbTachesTerm_Checked;

            cbxLogiciels.SelectionChanged += Filtrer_Click;
            cbxVersions.SelectionChanged += Filtrer_Click;
            cbxPersonnes.SelectionChanged += Filtrer_Click;


        }

        private void CkbTachesTerm_Checked(object sender, RoutedEventArgs e)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(_vmTacheProd.TachesProdsListView);
        }

        private void CkbTachesTerm_Unchecked(object sender, RoutedEventArgs e)
        {
            // On instancie une nouvelle Vue-Modèle TachesProd.
            VMTachesProd list = new VMTachesProd();

            // On filtre sur un critère de durée.
            // Si la Durée restante est égale à zéro, on considère que la tâche est terminée.
            list.TachesProdsListView.Where(t => t.DureeRestante == 0);
            ICollectionView view = CollectionViewSource.GetDefaultView(list);
        }

        private void Filtrer_Click(object sender, SelectionChangedEventArgs e)
        {
            // S'il y a une personne selectionnées dans la combobox Personne, on affecte aux texblocks les 
            // valeurs de temps global restants et réalisées pour cette personne. 
            if (cbxPersonnes.SelectedValue != null)
            {

                var a = (Travail)DALTaches.GetTempsTravailGlobaux(cbxPersonnes.SelectedValue.ToString());
                Txt_Restant.Text = "Temps de travail global restants : " + a.NbrHeuresTravailGlobalRestantes.ToString() + " h    /     ";
                Txt_Realise.Text = "Temps de travail global réalisés  : " + a.NbrHeuresTravailGlobalRealisees.ToString() + " h";

            }

            ICollectionView view = CollectionViewSource.GetDefaultView(_vmTacheProd.TachesProdsListView);

            // S'il y a un logiciel, une version et une personne dans les combobox, on applique un filtre sur les tâches de production
            // en appelant la méthode FiltrerTachesProds et en lui passant en paramètre l'objet view.
            if (cbxVersions.SelectedValue != null && cbxPersonnes.SelectedValue != null && cbxLogiciels.SelectedValue != null)
            {
                //on applique le filtre
                view.Filter = FiltrerTachesProds;
            }
        }


        // Méthode permettant de filtrer les tâches de production en fonction du logiciel, de la version 
        // et de la personne dans les combobox.
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
