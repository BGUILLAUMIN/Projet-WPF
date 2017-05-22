﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobOverview.Entity;
using JobOverview.Model;

namespace JobOverview.ViewModel
{
	public class VMLogin : ViewModelBase
	{
		public List<Personne> Personnes { get; private set; }

		public VMLogin()
		{
            Personnes = new List<Personne>();
            Personnes = DALPersonnes.GetPersonnes();
		}
	}
}
