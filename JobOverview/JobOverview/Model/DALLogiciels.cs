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
   public class DALLogiciels
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
				max(r.NumeroRelease) DerniereRelease
				from jo.Logiciel l
				left outer join jo.Version v on l.CodeLogiciel = v.CodeLogiciel
				left outer join jo.Release r on (r.NumeroVersion = v.NumeroVersion and
				r.CodeLogiciel = v.CodeLogiciel)
				group by l.CodeLogiciel, l.Nom,
				v.DateOuverture, v.DateSortiePrevue, v.DateSortieReelle,
				v.Millesime, v.NumeroVersion";

            

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
        /// Obtient et renvoie la liste des logiciels et leurs modules associés,
        /// La liste est triée par nom de logiciel et libellé de module
        /// </summary>
        /// <returns></returns>
        public static List<Logiciel> GetLogicielsModules()
        {
            var listLogiciels = new List<Logiciel>();

            string req = @"select l.CodeLogiciel, l.Nom, m.CodeModule, m.Libelle, m.CodeModuleParent
							from jo.Logiciel l
							inner join jo.Module m on l.CodeLogiciel = m.CodeLogiciel
							order by Nom, Libelle";

            using (var connect = new SqlConnection(Settings.Default.ConnectionJobOverview))
            {
                var command = new SqlCommand(req, connect);
                connect.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string codeLogi = (string)reader["CodeLogiciel"];

                        // Si le code du logiciel courant est != de celui du dernier logiciel de la liste,
                        // on crée un nouvel objet Logiciel,
                        Logiciel logi = null;
                        if (listLogiciels.Count == 0 || listLogiciels[listLogiciels.Count - 1].Code != codeLogi)
                        {
                            logi = new Logiciel();
                            logi.Code = (string)reader["CodeLogiciel"];
                            logi.Nom = (string)reader["Nom"];
                            logi.Modules = new List<Module>();

                            listLogiciels.Add(logi);
                        }
                        else logi = listLogiciels[listLogiciels.Count - 1];

                        Module m = new Module();
                        m.Code = (string)reader["CodeModule"];
                        m.Libelle = (string)reader["Libelle"];
                        if (reader["CodeModuleParent"] != DBNull.Value)
                            m.CodeModuleParent = (string)reader["CodeModuleParent"];
                        logi.Modules.Add(m);
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
                logi.Code = (string)reader["CodeLogiciel"];
                logi.Nom = (string)reader["Nom"];
                logi.Versions = new List<Entity.Version>();

                listLogiciels.Add(logi);
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
                if(reader["DerniereRelease"] != DBNull.Value)
                    v.DerniereRelease = (Int16)reader["DerniereRelease"];

                logi.Versions.Add(v);
            }
        }
    }
}
