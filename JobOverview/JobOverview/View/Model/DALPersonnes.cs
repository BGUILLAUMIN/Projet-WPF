using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobOverview.Entity;
using System.Data.SqlClient;
using JobOverview.Properties;
using System.Xml.Serialization;
using System.IO;

namespace JobOverview.Model
{
    public class DALPersonnes
    {
        /// <summary>
        /// Récupère la liste des personnes depuis la base de
        /// données pour compléter la liste dans la fenêtre d'identification.
        /// </summary>
        /// <returns></returns>
        public static List<Personne> GetPersonnes()
        {
            var listPersonnes = new List<Personne>();

            string req = @"SELECT Login, Nom, Prenom, CodeEquipe, Manager FROM jo.Personne";
            string connectString = Properties.Settings.Default.ConnectionJobOverview;

            using (var connect = new SqlConnection(connectString))
            {
                var command = new SqlCommand(req, connect);
                connect.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {

                        Personne pers = null;
                        {
                            pers = new Personne();
                            pers.Login = (string)reader["Login"];
                            pers.Nom = (string)reader["Nom"];
                            pers.Prenom = (string)reader["Prenom"];
                            pers.CodeMetier = (string)reader["CodeEquipe"];
                            if(reader["Manager"] != DBNull.Value)
                                pers.Manager = (string)reader["Manager"];

                            //TODO enlever méthode dans le diagramme de classe
                            listPersonnes.Add(pers);
                        }
                    }
                }
            }
            return listPersonnes;
        }
    }
}
