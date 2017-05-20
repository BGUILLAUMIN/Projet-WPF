using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

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
                if (_vmCourante == null)
                    _vmCourante = new VMTachesProd();
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

        //private ICommand _cmdMenu2;
        //public ICommand CmdMenu2
        //{
        //    get
        //    {
        //        if (_cmdMenu2 == null)
        //            _cmdMenu2 = new RelayCommand(ActionMenu2);
        //        return _cmdMenu2;
        //    }
        //}
        //private void ActionMenu2(object obj)
        //{
        //    VMCourante = new VMMenu2();
        //}
    }
}

