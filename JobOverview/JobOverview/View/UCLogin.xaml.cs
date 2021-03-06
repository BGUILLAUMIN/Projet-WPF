﻿using JobOverview.Entity;
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

namespace JobOverview.View
{
	/// <summary>
	/// Logique d'interaction pour UCLogin.xaml
	/// </summary>
	public partial class UCLogin : UserControl
	{
		public UCLogin()
		{
			InitializeComponent();

            //Branchement du gestinnaire d'évenement pour la combobox Utilisateur
            cbxUtilisateur.SelectionChanged += CbxUtilisateur_SelectionChanged;
		}

        /// <summary>
        /// Récupère et enregistre dans la propriété PersonneConnecte
        /// le nom du dernier utilisateur sélectionné
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxUtilisateur_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Personne p = (Personne)cbxUtilisateur.SelectedItem;
            Properties.Settings.Default.PersonneConnecte = p.Login;
            Properties.Settings.Default.ManagerConnecte = p.Manager;
            Properties.Settings.Default.Save();
        }
    }
}
