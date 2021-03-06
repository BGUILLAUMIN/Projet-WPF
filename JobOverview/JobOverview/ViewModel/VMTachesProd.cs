﻿using JobOverview.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobOverview.Model;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Forms;


namespace JobOverview.ViewModel
{
    // Enumération des différents types d'édition : mode consultation ou mode edition.
    public enum ModesEdition { Consultation, Edition }

    public class VMTachesProd : ViewModelBase
    {
        #region Champs privés
        private ModesEdition _mode;
        #endregion

        #region Propriétés
        public List<Logiciel> Logiciels { get;}
        public List<Personne> Personnes { get;}
        public List<Activite> Activités { get;}
        public ObservableCollection<TacheProd> TachesProds { get;}

        public TacheProd TacheCourante
        {
            get
            {
                return (TacheProd)CollectionViewSource.GetDefaultView(TachesProds).CurrentItem;
            }
        }
        public ModesEdition ModeEdit
        {
            get { return _mode; }
            private set
            {
                SetProperty(ref _mode, value);
            }
        }
        #endregion

        #region Constructeurs
        public VMTachesProd()
        {
            // Appels des méthodes de DAL pour remplir le visuel au chargement de la fenêtre.
            Logiciels = DALLogiciels.GetLogicielsVersions();
            Personnes = DALPersonnes.GetPersonnesFromUser(Properties.Settings.Default.PersonneConnecte);
            Activités = DALTaches.GetActivités().Where(a => a.Annexe == false).ToList();
            TachesProds = new ObservableCollection<TacheProd>(DALTaches.GetTachesProd());
            ModeEdit = ModesEdition.Consultation;
        }
        #endregion
        
        #region Définition des commandes
        // Lors du clic sur le bouton Ajouter.
        private ICommand _cmdAjouter;
        public ICommand CmdAjouter
        {
            get
            {
                if (_cmdAjouter == null)
                    _cmdAjouter = new RelayCommand(AjouterTache, ActiverAjout);
                return _cmdAjouter;
            }
        }

        // Lors du clic sur le bouton Enregistrer.
        private ICommand _cmdEnregistrer;
        public ICommand CmdEnregistrer
        {
            get
            {
                if (_cmdEnregistrer == null)
                    _cmdEnregistrer = new RelayCommand(EnregistrerTache, ActiverAnnEnr);
                return _cmdEnregistrer;
            }
        }

        // Lors du clic sur le bouton Annuler.
        private ICommand _cmdAnnuler;
        public ICommand CmdAnnuler
        {
            get
            {
                if (_cmdAnnuler == null)
                    _cmdAnnuler = new RelayCommand(AnnulerTache, ActiverAnnEnr);
                return _cmdAnnuler;
            }
        }

        // Lors du clic sur le bouton Export XML.
        private ICommand _cmdExport;
        public ICommand CmdExport
        {
            get
            {
                if (_cmdExport == null)
                    _cmdExport = new RelayCommand(AppelExport);
                return _cmdExport;
            }
        }


        #endregion

        #region Méthodes des commandes
        private void AppelExport()
        {
            try
            {
               DialogResult res= MessageBox.Show("Confirmez-vous l'export des données? ",
                         "Exportation", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

                if (res == DialogResult.OK)
                {
                    // Appel de la méthode d'export des données en passsant en paramêtre la liste de tâche de production.
                    DALTaches.ExportTachesXml(TachesProds.ToList());

                    MessageBox.Show("Exportation réalisée avec succès",
                             "Exportation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("L'exportation a échoué", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // On Crée une nouvelle tâche et on l'ajoute à la collection
        // puis, on passe en mode édition.
        private void AjouterTache()
        {
            // Instanciation d'une nouvelle tâche de production.
            var NouvelleTache = new TacheProd() { Id = Guid.NewGuid() };
            // Initiatialisation des propriétés de la nouvelle tâche.
            // On récupère la personne selectionnée.
            NouvelleTache.LoginPersonne = Properties.Settings.Default.PersonneCourante;
            // On incrémente le nouveau numéro de tâche de production en se basant sur le dernier de la liste.
            // Le numero est en auto-incrément, cette information ne sert que pendant l'ajout de Tâche. 
            NouvelleTache.Numero = TachesProds.Max(n => n.Numero) + 1;
            // Ajoute la nouvelle tache dans la liste TachesProds.
            TachesProds.Add(NouvelleTache);
            
            // La nouvelle tâche devient la tâche courante, de façon à ce qu'elle soit automatiquement sélectionnée.
            ICollectionView view = CollectionViewSource.GetDefaultView(TachesProds);
            view.MoveCurrentToLast();

            // Lorsque l'on clique sur le bouton Enregistrer, on passe la fenêtre en mode Edition.
            ModeEdit = ModesEdition.Edition;
        }

        // Appel de la méthode d'enregistrement des tâches de production dans la base
        // puis, passage en mode édition.
        private void EnregistrerTache()
        {
            {
                //try
                //{
                    DialogResult res = MessageBox.Show("Confirmez-vous l'enregistrement de cette tâche ?",
                         "Enregistrement", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

                    if (res == DialogResult.OK)
                    {
                        // Enregistre dans la base la liste mis à jour de la listview. 
                        DALTaches.EnregistrerTachesProd(TacheCourante);

                        MessageBox.Show("Tâche de production enregistrée",
                         "Enregistrement", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        // Lorsque l'on clique sur le bouton Enregistrer, on passe la fenêtre en mode Consultation.
                        ModeEdit = ModesEdition.Consultation;
                    }
                //}
                //catch (Exception)
                //{
                //    // Supprime de la Listview la tache en cours d'enregistrement.
                //    TachesProds.Remove(TacheCourante);
                //    MessageBox.Show("Veuillez saisir tous les champs!", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //}
            }
        }

        // Appel de la méthode d'annulation de la création d'une tâches de production
        // puis pasage en mode édition.
        private void AnnulerTache()
        {
            // Enlève de l'affichage de la Listview la tâche qui est sélectionnée.
            TachesProds.Remove(TacheCourante);

            // Lorsque l'on clique sur le bouton annuler, on passe la fenêtre en mode Consultation.
            ModeEdit = ModesEdition.Consultation;
        }

        // Méthodes d'activation du Mode Edition.
        // Dès que l'on clique sur le bouton ajouter, cela désactive l'état du bouton.
        private bool ActiverAjout()
        {
            return ModeEdit == ModesEdition.Consultation;
        }

        // Dès que l'on clique sur le bouton Enregistrer ou Annuler, cela désactive l'état des boutons.
        private bool ActiverAnnEnr()
        {
            return ModeEdit == ModesEdition.Edition;
        }
        #endregion
    }
}
