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
        //private ViewModelBase _VMCourante;
        //public ViewModelBase VMCourante
        //{
        //	get { return _VMCourante; }
        //	private set
        //	{
        //		SetProperty(ref _VMCourante, value);
        //	}
        //}

        //#region Commandes
        //private ICommand _cmdLogin;
        //public ICommand CmdLogin
        //{
        //	get
        //	{
        //		if (_cmdLogin == null)
        //			_cmdLogin = new RelayCommand(() => VMCourante = new VMLogin());
        //		return _cmdLogin;
        //	}
        //}

        //#endregion

        private ViewModelBase _vmCourante;
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
        private void ActionMenuTachesProd()
        {
            VMCourante = new VMTachesProd();
        }

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
        private void ActionMenuTachesAnnexe()
        {
            VMCourante = new VMTachesAnnexe();
        }


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
        private void ActionMenuAPropos()
        {
            VMCourante = new VMAPropos();
        }



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

        private void ActionMenuSaisieTemps()
        {
            VMCourante = new VMSaisieTemps();
        }

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

        private void ActionMenuVersion()
        {
            VMCourante = new VMVersion();
        }
    }


}

