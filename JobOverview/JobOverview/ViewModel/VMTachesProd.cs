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
    // enumération les différents types d'édition: mode consultation ou mode edition
    public enum ModesEdition { Consultation, Edition }

    public class VMTachesProd : ViewModelBase
    {
        #region Champs privés
        private ModesEdition _mode;
        #endregion

        #region Propriétés
        public List<Logiciel> Logiciels { get; set; }
        public List<Personne> Personnes { get; set; }
        public List<Activite> Activités { get; set; }
        public List<Module> Modules { get; set; }
        public ObservableCollection<TacheProd> TachesProds { get; }

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

        #region Constructeur
        public VMTachesProd()
        {
            Logiciels = DALLogiciels.GetLogicielsVersions();
            Personnes = DALPersonnes.GetPersonnesFromUser(Properties.Settings.Default.PersonneConnecte);
            Activités = DALTaches.GetActivités().Where(a => a.Annexe == false).ToList();
            Modules = DALLogiciels.GetModulesLibellé();
            TachesProds = new ObservableCollection<TacheProd>(DALTaches.GetTachesProd());
            ModeEdit = ModesEdition.Consultation;
        }
        #endregion


        #region Définition des commandes
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

        //lors du clic sur le bouton Export XML
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

        #region Code des commandes
        private void AppelExport()
        {
            try
            {
                DALTaches.ExportTachesXml(TachesProds.ToList());
                MessageBox.Show("Exportation réalisée avec succès",
                         "Exportation", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception)
            {

                MessageBox.Show("L'exportation a échoué", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Crée une nouvelle tâche et l'ajoute à la collection
        // mode d'édition
        private void AjouterTache()
        {
            //Instancie une nouvelle tâche
            var NouvelleTache = new TacheProd();

            // Ajoute la nouvelle tache dans la liste TachesProds
            TachesProds.Add(NouvelleTache);

            // La nouvelle tâche devient la tâche courante, de façon à ce qu'elle soit automatiquement sélectionnée
            ICollectionView view = CollectionViewSource.GetDefaultView(TachesProds);
            view.MoveCurrentToLast();

            ModeEdit = ModesEdition.Edition;
        }

        //Appel de la méthode d'enregistrement des tâches de production dans la base
        //et définit le mode d'édition
        private void EnregistrerTache()
        {
            {
            //    try
            //    {
                    //Enregistre dans la base la liste mis à jour de la listview 
                    DALTaches.EnregistrerTachesProd(TacheCourante);
                    MessageBox.Show("Confirmez-vous l'enregistrement de cette tâche ?",
                       "Enregistrement", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
              //  }
                //catch (Exception)
                //{
                //    MessageBox.Show("Veuillez saisir tous les champs", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //}

                //Lorsque l'on clique sur le bouton Enregistrer, on passe la fenêtre en mode Consultation
                ModeEdit = ModesEdition.Consultation;
            }
        }

        //Appel de la méthode d'enregistrement des tâches de production dans la base
        //et définit le mode d'édition
        private void AnnulerTache()
        {
            //Enlève de l'affichage de la Listviw la tache qui est sélectionnée
            TachesProds.Remove(TacheCourante);

            //Lorsque l'on clique sur le bouton annuler, on passe la fenêtre en mode Consultation
            ModeEdit = ModesEdition.Consultation;
        }

        //méthodes d'activation du Mode Edition
        // dès que l'on clique sur le bouton ajouter, cela désactive l'état du bouton
        private bool ActiverAjout()
        {
            return ModeEdit == ModesEdition.Consultation; ;
        }

        // dès que l'on clique sur le bouton Enregistrer ou Annuler, cela désactive l'état des boutons
        private bool ActiverAnnEnr()
        {
            return ModeEdit == ModesEdition.Edition;
        }
        #endregion
    }
}
