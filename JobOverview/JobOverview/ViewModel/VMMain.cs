using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using JobOverview.Model;

namespace JobOverview.ViewModel
{
    public class VMMain : ViewModelBase
    {
        // Vue-modèle courante sur laquelle est liées le ContentControl
        // de la zone principale
         #region Champs privés
        private ViewModelBase _vmCourante;
        #endregion

        #region Propriétés 
        public ViewModelBase VMCourante
        {
            get
            {
                return _vmCourante;
            }
            set
            {
                SetProperty(ref _vmCourante, value);
            }
        } 
        #endregion

        #region Définition des commandes
        //au clic sur le Menu tâches de production
        private ICommand _CmdTachesProd;
        public ICommand CmdTachesProd
        {
            get
            {
                if (_CmdTachesProd == null)
                    _CmdTachesProd = new RelayCommand(ActionMenuTachesProd);
                return _CmdTachesProd;
            }
        }

        //au clic sur le Menu tâches annexes
        private ICommand _cmdTachesAnnexe;
        public ICommand CmdTachesAnnexe
        {
            get
            {
                if (_cmdTachesAnnexe == null)
                    _cmdTachesAnnexe = new RelayCommand(ActionMenuTachesAnnexe);
                return _cmdTachesAnnexe;
            }
        }

        //au clic sur le Menu à propos
        private ICommand _cmdAPropos;
        public ICommand CmdAPropos
        {
            get
            {
                if (_cmdAPropos == null)
                    _cmdAPropos = new RelayCommand(ActionMenuAPropos);
                return _cmdAPropos;
            }
        }

        //au clic sur le Menu SaisieTemps
        private ICommand _CmdSaisieTemps;

        public ICommand CmdSaisieTemps
        {
            get
            {
                if (_CmdSaisieTemps == null)
                    _CmdSaisieTemps = new RelayCommand(ActionMenuSaisieTemps);
                return _CmdSaisieTemps;
            }

        }

        //au clic sur le Menu Version
        private ICommand _CmdVersion;

        public ICommand CmdVersion
        {
            get
            {
                if (_CmdVersion == null)
                    _CmdVersion = new RelayCommand(ActionMenuVersion);
                return _CmdVersion;
            }
        }
        #endregion


        #region Méthodes privées des commandes
        // On définit une instance de VMTachesProd comme vue-modèle courante
        private void ActionMenuTachesProd()
        {
            VMCourante = new VMTachesProd();
        }

        // On définit une instance de VMTachesAnnexe comme vue-modèle courante
        private void ActionMenuTachesAnnexe()
        {
            VMCourante = new VMTachesAnnexe();
        }

        // On définit une instance de VMPropos comme vue-modèle courante
        private void ActionMenuAPropos()
        {
            VMCourante = new VMAPropos();
        }

        // On définit une instance de VMSaisieTemps comme vue-modèle courante
        private void ActionMenuSaisieTemps()
        {
            VMCourante = new VMSaisieTemps();
        }

        // On définit une instance de VMVersion comme vue-modèle courante
        private void ActionMenuVersion()
        {
            VMCourante = new VMVersion();
        } 
        #endregion
    }


}

