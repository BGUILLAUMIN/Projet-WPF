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
using System.Data;

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
                            if (reader["Manager"] != DBNull.Value)
                                pers.Manager = false;
                            else
                                pers.Manager = true;

                            //TODO enlever méthode dans le diagramme de classe
                            listPersonnes.Add(pers);
                        }
                    }
                }
            }
            return listPersonnes;
        }

        public static List<Activite> GetActivite()
        {
            var listActivites = new List<Activite>();

            string req = @"select CodeActivite,Libelle,Annexe from jo.activite where Annexe=1";
            string connectString = Properties.Settings.Default.ConnectionJobOverview;

            using (var connect = new SqlConnection(connectString))
            {
                var command = new SqlCommand(req, connect);
                connect.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {

                        Activite activite = null;
                        {
                            activite = new Activite();
                            activite.Code = (string)reader["CodeActivite"];
                            activite.Libelle = (string)reader["Libelle"];



                            //TODO enlever méthode dans le diagramme de classe
                            listActivites.Add(activite);
                        }
                    }
                }
            }
            return listActivites;
        }


        public static List<Personne> GetPersonnesFromUser(string login)
        {
            List<Personne> Personnes = new List<Personne>();
            string req = @"select p2.Login, p2.Nom, p2.Prenom, M.CodeMetier ,M.Libelle, E.CodeEquipe, E.Nom as NomEquipe
                            from jo.Personne p1
                            inner join jo.Personne p2 on p1.Login = p2.Manager
                            inner join jo.Metier M on p2.CodeMetier = M.CodeMetier 
                            inner join jo.Equipe E on p2.CodeEquipe = E.CodeEquipe 
                            where p1.Login = @Login and p1.CodeEquipe = p2.CodeEquipe
                            union
                            select P.Login, P.Nom, P.Prenom, M.CodeMetier, M.Libelle, E.CodeEquipe, E.Nom as NomEquipe
                            from jo.Personne P
                            inner join jo.Metier M on P.CodeMetier = M.CodeMetier
                            inner join jo.Equipe E on P.CodeEquipe = E.CodeEquipe  
                            where login = @Login";

            var param = new SqlParameter("@Login", DbType.String);
            param.Value = login;

            using (SqlConnection connect = new SqlConnection(Settings.Default.ConnectionJobOverview))
            {
                var command = new SqlCommand(req, connect);
                command.Parameters.Add(param);
                connect.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        GetPersonnesFromUserFromDataReader(Personnes, reader);
                    }
                }
            }
            return Personnes;

        }
        private static void GetPersonnesFromUserFromDataReader(List<Personne> Personnes ,SqlDataReader reader)
        {
            //Aucun de ses champs n'est nullable
            Personne p = new Personne();
            p.Login = (string)reader["Login"];
            p.Nom = (string)reader["Nom"];
            p.Prenom = (string)reader["Prenom"];
            p.CodeEquipe = (string)reader["CodeEquipe"];
            p.NomEquipe = (string)reader["NomEquipe"];
            p.CodeMetier = (string)reader["CodeMetier"];
            p.LibelleMetier = (string)reader["Libelle"];

            Personnes.Add(p);
        }
    }
}
