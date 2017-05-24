using JobOverview.Entity;
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
    public class VMTachesAnnexe : ViewModelBase
    {

        #region champs privés

        private ModesEdition _mode;
        private List<Activite> _activitesAutorisées;

        #endregion

        #region Propriétés
        public List<Activite> ActivitesAutorisées
        {
            get { return _activitesAutorisées; }
            set
            {
                SetProperty(ref _activitesAutorisées, value);
            }
        }

        public List<Activite> Activites { get; }
        public List<Personne> Personnes { get; }
        public ObservableCollection<Tache> TachesAnnexes { get; }


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
        #endregion

        #region Constructeurs
        public VMTachesAnnexe()
        {
            // Appels des méthodes de DAL pour remplir le visuel au chargement de la fenêtre.
            // Permet à la comboBox d'afficher la liste des activités annexes disponibles.
            Activites = DALTaches.GetActivités().Where(a => a.Annexe == true).ToList();
            // Permet à la comboBox d'afficher la liste des personnes d'une même équipe (manager compris) en fonction du Login mémorisé.
            Personnes = DALPersonnes.GetPersonnesFromUser(Properties.Settings.Default.PersonneConnecte);
            // Permet à la ListeView d'afficher la liste de taches annexes.
            TachesAnnexes = new ObservableCollection<Tache>(DALTaches.GetTachesAnnexe());
            ActivitesAutorisées = new List<Activite>();
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
        // Lors du clic sur le bouton Supprimer.
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
        #endregion

        #region méthodes des commandes

        // Création d'une nouvelle tâche et ajout à la collection,
        // puis passage en mode édition.
        private void AjouterTache()
        {
            // Instancie une nouvelle tâche.

            var NouvelleTache = new Tache() { Id = Guid.NewGuid() };

            NouvelleTache.LoginPersonne = Properties.Settings.Default.PersonneCourante;
            // Ajoute la nouvelle tache dans la liste TachesAnnexes.
            TachesAnnexes.Add(NouvelleTache);

            // La nouvelle tâche devient la tâche courante, de façon à ce qu'elle soit automatiquement sélectionnée.
            ICollectionView view = CollectionViewSource.GetDefaultView(TachesAnnexes);
            view.MoveCurrentToLast();

            ModeEdit = ModesEdition.Edition;
        }

        // Suppression de la tâche sélectionnée et suppression de celle-ci dans la collection.
        private void SupprimerTache()
        {
            try
            {
                DialogResult res = MessageBox.Show("Confirmez-vous la suppression de cette tâche ?", "Attention", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

                if (res == DialogResult.OK)
                {
                    DALTaches.SupprimerTachesAnnexes(TacheCourante.Id);
                    TachesAnnexes.Remove(TacheCourante);

                    MessageBox.Show("Suppression réussie", "Suppression", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception e)
            {

                MessageBox.Show("Tâche non supprimée - il y a déjà du temps de travail saisi sur cette tache!!", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }



        // Appel de la méthode d'enregistrement des tâches de production dans la base
        // et définition du mode d'édition.
        private void EnregistrerTache()
        {
            try
            {
                DialogResult res = MessageBox.Show("Confirmez-vous l'enregistrement de cette tâche ?", "Attention", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

                if (res == DialogResult.OK)
                {
                    // Enregistrement dans la base de la listeview mise à jour. 
                    DALTaches.EnregistrerTachesAnnexes(TacheCourante);
                    MessageBox.Show("Enregistrement réussi", "Enregistrement", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                // Lorsque l'on clique sur le bouton Enregistrer, passage de la fenêtre en mode Consultation.
                ModeEdit = ModesEdition.Consultation;
            }
            catch (Exception)
            {
                // Supprime de la Listview la tache en cours d'enregistrement.
                TachesAnnexes.Remove(TacheCourante);
                MessageBox.Show("Tâche non enregistrée", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Appel de la méthode d'annulation des tâches de production dans la base
        // et définition du mode d'édition.
        private void AnnulerTache()
        {
            // Enlève de l'affichage de la Listviw la tache qui est sélectionnée.
            TachesAnnexes.Remove(TacheCourante);

            // Lorsque l'on clique sur le bouton annuler, on passe la fenêtre en mode Consultation.
            ModeEdit = ModesEdition.Consultation;
        }

        // Méthodes d'activation du Mode Edition.
        // Dès que l'on clique sur le bouton ajouter, cela désactive l'état du bouton.
        private bool ActiverAjout()
        {
            return ModeEdit == ModesEdition.Consultation;
        }
        private bool ActiverSupprimer()
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

