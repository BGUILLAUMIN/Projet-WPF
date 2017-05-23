using JobOverview.Entity;
using JobOverview.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace JobOverview.ViewModel
{


    public class VMTachesAnnexe : ViewModelBase
    {

        #region champs privés
       
        private ModesEdition _mode;

        #endregion

        public List<Activite> Activites { get; set; }
        public List<Personne> Personnes { get; set; }
        public ObservableCollection<Tache> TachesAnnexes { get; set; }

    
        public Tache TacheCourante
        {
            get
            {
                return (Tache)CollectionViewSource.GetDefaultView(TachesAnnexes).CurrentItem;
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
        //*********************************************************************************************************************************
        #region Constructeur
        public VMTachesAnnexe()
        {
            // Permet à la comboBox d'afficher la liste des activités annexes disponibles.
            Activites = DALPersonnes.GetActivite();
            // Permet à la comboBox d'afficher la liste des personnes d'une même équipe (manager compris) en fonction du Login mémorisé.
            Personnes = DALPersonnes.GetPersonnesFromUser(Properties.Settings.Default.PersonneConnecte);
            //Permet à la ListeView d'afficher la liste de taches annexes
            TachesAnnexes = new ObservableCollection<Tache>(DALTaches.GetTachesAnnexe());
            ModeEdit = ModesEdition.Consultation;
        }
        
        #endregion

        //**************************** Ajout des commandes*********************************************************************************
        #region COMMANDES
        //lors du clic sur le bouton Ajouter
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
        //lors du clic sur le bouton Supprimer
        private ICommand _cmdSupprimer;
        public ICommand CmdSupprimer
        {
            get
            {
                if (_cmdSupprimer == null)
                    _cmdSupprimer = new RelayCommand(SupprimerTache, ActiverSupprimer);

                return _cmdSupprimer;
            }
        }
        //lors du clic sur le bouton Enregistrer
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

        //lors du clic sur le bouton Annuler
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
        #endregion


        #region méthodes des commandes

        // Crée une nouvelle tâche et l'ajoute à la collection
        // mode d'édition
        private void AjouterTache()
        {
            //Instancie une nouvelle tâche

           
            var NouvelleTache = new Tache();

            // Ajoute la nouvelle tache dans la liste TachesAnnexes
            TachesAnnexes.Add(NouvelleTache);

            // La nouvelle tâche devient la tâche courante, de façon à ce qu'elle soit automatiquement sélectionnée
            ICollectionView view = CollectionViewSource.GetDefaultView(TachesAnnexes);
            view.MoveCurrentToLast();

            ModeEdit = ModesEdition.Edition;
        }

        // Supprime la tâche sélectionnée et la supprime de la collection
        private void SupprimerTache()
        {
            try
            {
                TachesAnnexes.Remove(TacheCourante);
                DALTaches.EnregistrerTachesAnnexes(TacheCourante);//ToDO changer méthode suppression
                MessageBox.Show("Confirmez-vous la suppression de cette tâche ?", "Attention", MessageBoxButton.OKCancel);

            }
            catch (Exception)
            {

                MessageBox.Show("Tâche non supprimée", "Attention", MessageBoxButton.OK);
            }

        }



        //Appel de la méthode d'enregistrement des tâches de production dans la base
        //et définit le mode d'édition
        private void EnregistrerTache()
        {
            {
                try
                {
                    //Enregistre dans la base la liste mis à jour de la listview 
                    MessageBox.Show("Confirmez-vous l'enregistrement de cette tâche ?", "Attention", MessageBoxButton.OKCancel);
                    DALTaches.EnregistrerTachesAnnexes(TacheCourante);
                }
                catch (Exception)
                {

                    MessageBox.Show("Tâche non enregistrée", "Attention", MessageBoxButton.OK);
                }

                //Lorsque l'on clique sur le bouton Enregistrer, on passe la fenêtre en mode Consultation
                ModeEdit = ModesEdition.Consultation;
            }
        }

        //Appel de la méthode d'enregistrement des tâches de production dans la base
        //et définit le mode d'édition
        private void AnnulerTache()
        {
            //Enlève de l'affichage de la Listviw la tache qui est sélectionnée
            TachesAnnexes.Remove(TacheCourante);

            //Lorsque l'on clique sur le bouton annuler, on passe la fenêtre en mode Consultation
            ModeEdit = ModesEdition.Consultation;
        }

        //méthodes d'activation du Mode Edition
        // dès que l'on clique sur le bouton ajouter, cela désactive l'état du bouton
        private bool ActiverAjout()
        {
            return ModeEdit == ModesEdition.Consultation; ;
        }
        private bool ActiverSupprimer()
        {
            return ModeEdit == ModesEdition.Consultation; 
        }
        // dès que l'on clique sur le bouton Enregistrer ou Annuler, cela désactive l'état des boutons
        private bool ActiverAnnEnr()
        {
            return ModeEdit == ModesEdition.Edition;
        }
        #endregion
    }



}

