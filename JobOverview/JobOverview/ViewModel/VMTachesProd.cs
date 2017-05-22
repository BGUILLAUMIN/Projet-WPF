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

namespace JobOverview.ViewModel
{
    // enumération les différents types d'édition: mode consultation ou mode edition
    public enum ModesEdition { Consultation, Edition }
    public class VMTachesProd : ViewModelBase
    {
        #region Champs privés
        private TacheProd _nouvelleTache;
        private ModesEdition _mode; 
        #endregion

        #region Propriétés
        public List<Logiciel> Logiciels { get; set; }
        public List<Personne> Personnes { get; set; }
        public ObservableCollection<TacheProd> TachesProds { get;}
        public TacheProd NouvelleTache
        {
            get { return _nouvelleTache; }
            private set
            {
                _nouvelleTache = value;
                SetProperty(ref _nouvelleTache, value);
            }
        }
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
                _mode = value;
                SetProperty(ref _mode, value);
            }
        }

        #endregion

        public VMTachesProd()
        {
            Logiciels = DALLogiciels.GetLogicielsVersions();
            Personnes = DALPersonnes.GetPersonnesFromUser(Properties.Settings.Default.PersonneConnecte);
          //TachesProds = new ObservableCollection<Tache>(DALTaches.GetTachesProd());
        }


        #region Définition des commandes
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

        private ICommand _cmdAnnuler;
        public ICommand CmdAnnuler
        {
            get
            {
                // Définition d'une instance de VMPersonnes comme vue-modèle courante
                if (_cmdAnnuler == null)
                    _cmdAnnuler = new RelayCommand(AnnulerTache, ActiverAnnEnr);
                return _cmdAnnuler;
            }
        }
        #endregion

        #region Code des commandes
        // Crée une nouvelle tâche et l'ajoute à la collection
        private void AjouterTache()
        {
            //Instancie une nouvelle tâche
            NouvelleTache = new TacheProd();
          
            // Ajoute la nouvelle tache dans la liste TachesProds
            TachesProds.Add(NouvelleTache);

            // La nouvelle tâche devient la tâche courante, de façon à ce qu'elle soit automatiquement sélectionnée
            ICollectionView view = CollectionViewSource.GetDefaultView(TachesProds);
            view.MoveCurrentToLast();

            ModeEdit = ModesEdition.Edition;
        }


        private void EnregistrerTache()
        {
            
            {
                DALTaches.EnregistrerTachesProd(TachesProds.ToList());

                ModeEdit = ModesEdition.Consultation;
            }
      
        }

        private void AnnulerTache()
        {
            TachesProds.Remove(TacheCourante);
            ModeEdit = ModesEdition.Consultation;
        }



    

        //méthode d'activation du mode Edition
        // dès que l'on clique sur le bouton ajouter ou supprimer ça désactive l'état du bouton
        private bool ActiverAjout()
        {
            return ModeEdit == ModesEdition.Consultation; ;
        }

        private bool ActiverAnnEnr()
        {
            return ModeEdit == ModesEdition.Edition;
        }
        #endregion
    }
}
