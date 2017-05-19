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
    public class DALPersonnes
    {

        public static List<Personne> GetPersonnes()
        {
            var listPersonnes = new List<Personne>();

            string req = @"select Login, Nom, Prenom, Manager CodeEquipe 
                            from jo.Personne";


            using (var connect = new SqlConnection(Settings.Default.ConnectionJobOverview))
            {
                var command = new SqlCommand(req, connect);
                connect.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string login = (string)reader["Login"];

                        Personne pers = null;
                        {
                            pers = new Personne();
                            pers.Login = login;
                            pers.Nom = (string)reader["Nom"];
                            pers.Prenom = (string)reader["Prenom"];
                            pers.CodeMetier = (string)reader["CodeEquipe"];
                           //TODO ajouter Manager
                           //TODO enlever méthode dans le diagramme de classe
                           //TODO mettre commentaire GEtPersone

                            listPersonnes.Add(pers);
                        }
                    }
                }
            }

            return listPersonnes;

            
        }
    }
}
