using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobOverview.Entity;
using System.Data.SqlClient;
using JobOverview.Properties;

namespace JobOverview.Model
{
    class DALLogiciels
    {
        /// <summary>
		/// Obtient et renvoie la liste des logiciels et leurs versions associées,
		/// avec leur dernier N° de release
		/// La liste est triée par nom de logiciel et N° de version 
		/// </summary>
		/// <returns></returns>
		public static List<Logiciel> GetLogicielsVersions()
        {
            var listLogiciels = new List<Logiciel>();


            string req = @"select l.CodeLogiciel, l.Nom, 
				v.DateOuverture, v.DateSortiePrevue, v.DateSortieReelle, 
				v.Millesime, v.NumeroVersion, 
				max(r.NumeroRelease) DerniereRelease,
				SUM( tr.Heures/8) NombreJours ,  COUNT(distinct p.Login) NombrePersonnes			
				from jo.Logiciel l
				left outer join jo.Version v on l.CodeLogiciel = v.CodeLogiciel
				left outer join jo.Release r on (r.NumeroVersion = v.NumeroVersion and r.CodeLogiciel = v.CodeLogiciel)
				left outer join jo.Module m on m.CodeLogiciel = l.CodeLogiciel
				left outer join jo.TacheProd tp on tp.CodeModule = m.CodeModule
				left outer join jo.Tache t on t.IdTache = tp.IdTache
				left outer join jo.Personne p on p.Login = t.Login
				left outer join jo.Travail tr on tr.IdTache = t.IdTache
				group by l.CodeLogiciel, l.Nom,
				v.DateOuverture, v.DateSortiePrevue, v.DateSortieReelle,
				v.Millesime, v.NumeroVersion
				order by l.CodeLogiciel desc";

            using (var connect = new SqlConnection(Settings.Default.ConnectionJobOverview))
            {
                var command = new SqlCommand(req, connect);
                connect.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        GetLogicielsFromDataReader(listLogiciels, reader);
                    }
                }
            }

            return listLogiciels;
        }

        /// <summary>
        /// Charge la liste de logiciels passée en paramètre à partir du datareader
        /// </summary>
        /// <param name="listLogiciels"></param>
        /// <param name="reader"></param>
        private static void GetLogicielsFromDataReader(List<Logiciel> listLogiciels, SqlDataReader reader)
        {
            string codeLogi = (string)reader["CodeLogiciel"];

            // Si le code du logiciel courant est != de celui du dernier logiciel de la liste,
            // on crée un nouvel objet Logiciel,
            Logiciel logi = null;
            if (listLogiciels.Count == 0 || listLogiciels[listLogiciels.Count - 1].Code != codeLogi)
            {
                logi = new Logiciel();
                logi.Modules = new List<Module>();
                logi.Code = (string)reader["CodeLogiciel"];
                logi.Nom = (string)reader["Nom"];
                logi.Versions = new List<Entity.Version>();

                listLogiciels.Add(logi);

                if(listLogiciels.Count == 0 || listLogiciels[listLogiciels.Count - 1].Nom == reader["Nom"].ToString())
                DALLogiciels.GetModules(listLogiciels);

            }
            else logi = listLogiciels[listLogiciels.Count - 1];

            Entity.Version v = new Entity.Version();
            // Si le N° de version est null, c'est que le logiciel n'a pas encore
            // de version. Dans ce cas, on n'ajoute pas de version à la collection
            if (reader["NumeroVersion"] != DBNull.Value)
            {
                v.Numero = (float)reader["NumeroVersion"];
                v.Millesime = (Int16)reader["Millesime"];
                v.DateOuverture = (DateTime)reader["DateOuverture"];
                v.DateSortiePrevue = (DateTime)reader["DateSortiePrevue"];
                if (reader["DateSortieReelle"] != DBNull.Value)
                    v.DateSortieReelle = (DateTime)reader["DateSortieReelle"];
                if (reader["DerniereRelease"] != DBNull.Value)
                    v.DerniereRelease = (Int16)reader["DerniereRelease"];

                if (reader["NombreJours"] != DBNull.Value)
                    v.NombreJours = (double)reader["NombreJours"];

                if (reader["NombrePersonnes"] != DBNull.Value)
                    v.NombrePersonnes = (int)reader["NombrePersonnes"];
                logi.Versions.Add(v);
            }
        }

        public static List<Module> GetModules()
        {
            var listModules = new List<Module>();

            string req = @" CodeModule, Libelle from jo.Module"; 
                            

            using (var connect = new SqlConnection(Settings.Default.ConnectionJobOverview))
            {
                var command = new SqlCommand(req, connect);
                connect.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                        GetModuleFromDataReader(listModules, reader);
                }
            }
            return listModules;
        }

        private static void GetModuleFromDataReader(List<Module> listmod, SqlDataReader reader)
        {
            while (reader.Read())
            {
                Module mod = new Module();

                mod.Code = reader["CodeModule"].ToString();
                mod.Libelle = reader["Libelle"].ToString();

                listmod.Add(mod);
            }

        }
    }
}
