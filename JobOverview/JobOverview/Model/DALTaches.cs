using JobOverview.Properties;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobOverview.Entity;
using System.Xml.Serialization;
using System.IO;
using System.Collections.ObjectModel;

namespace JobOverview.Model
{
    public class DALTaches
    {
        #region Méthodes publiques
        /// <summary>
        /// Permet de récupérer les tâches de production
        /// </summary>
        public static List<TacheProd> GetTachesProd()
        {
            var listTaches = new List<TacheProd>();

            string req = @"select t.IdTache, t.Libelle, t.Description, t.CodeActivite, t.Login,
						tp.Numero, tp.DureePrevue, tp.DureeRestanteEstimee,
						tp.CodeLogicielVersion, tp.NumeroVersion, tp.CodeModule
					    from jo.Tache t
					    inner join jo.TacheProd tp on t.IdTache = tp.IdTache
					    where Annexe = 0
					    order by Numero";

            using (var connect = new SqlConnection(Settings.Default.ConnectionJobOverview))
            {
                var command = new SqlCommand(req, connect);

                connect.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var tp = new TacheProd();
                        tp.Numero = (int)reader["Numero"];
                        tp.Libelle = (string)reader["Libelle"];
                        if (reader["Description"] != DBNull.Value)
                            tp.Description = (string)reader["Description"];
                        tp.CodeActivite = (string)reader["CodeActivite"];
                        tp.CodeModule = (string)reader["CodeModule"];
                        tp.Version = (float)reader["NumeroVersion"];
                        tp.LoginPersonne = (string)reader["Login"];
                        tp.DureePrevue = (float)reader["DureePrevue"];
                        tp.DureeRestante = (float)reader["DureeRestanteEstimee"];
                        tp.CodeLogiciel = (string)reader["CodeLogicielVersion"];

                        listTaches.Add(tp);
                    }
                }
            }
            return listTaches;
        }

        /// <summary>
        /// Permet de récupérer les tâches annexes
        /// </summary>
        public static List<Tache> GetTachesAnnexe()
        {
            var listTachesAnnexe = new List<Tache>();

            string req = @"select IdTache, Libelle, Description, CodeActivite, Login
                        from jo.Tache
                        where Annexe = 1";



            using (var connect = new SqlConnection(Settings.Default.ConnectionJobOverview))
            {
                var command = new SqlCommand(req, connect);
                connect.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var ta = new Tache();
                        ta.Id = (Guid)reader["IdTache"];
                        ta.Libelle = (string)reader["Libelle"];
                        if (reader["Description"] != DBNull.Value)
                            ta.Description = (string)reader["Description"];
                        ta.CodeActivite = (string)reader["CodeActivite"];
                        ta.LoginPersonne = (string)reader["Login"];

                        listTachesAnnexe.Add(ta);
                    }
                }
            }

            return listTachesAnnexe;
        }

        /// <summary>
        /// Enregistre une tâches de production dans la base
        /// </summary>
        /// <param name="listTaches"></param>
        public static void EnregistrerTachesProd(TacheProd tacheProd)
        {
            string req = @"Insert jo.Tache(IdTache, Libelle, Annexe, CodeActivite, Login, Description)                                                                                                 
                        Values  (@IdTache, @Libelle, 0 , @CodeActivite, @Login, @Description);

                        Insert jo.TacheProd (IdTache, DureePrevue, DureeRestanteEstimee,
									CodeModule, CodeLogicielModule, NumeroVersion, CodeLogicielVersion)
                        values  (@IdTache, @DureePrevue, @DureeRestanteEstimee,
								@CodeModule, @CodeLogicielModule, @NumeroVersion, @CodeLogicielVersion)";


            using (var cnx = new SqlConnection(Settings.Default.ConnectionJobOverview))
            {
                // Ouverture de la connexion et début de la transaction
                cnx.Open();
                SqlTransaction tran = cnx.BeginTransaction();

                #region Paramètres
                SqlParameter paramIdTache = new SqlParameter("@IdTache", SqlDbType.UniqueIdentifier);
                paramIdTache.Value = Guid.NewGuid();
                SqlParameter paramLibellé = new SqlParameter("@Libelle", DbType.String);
                paramLibellé.Value = tacheProd.Libelle;
                SqlParameter paramCodeActivite = new SqlParameter("@CodeActivite", DbType.String);
                paramCodeActivite.Value = tacheProd.CodeActivite;
                SqlParameter paramLogin = new SqlParameter("@Login", DbType.String);
                paramLogin.Value = tacheProd.LoginPersonne;


                SqlParameter paramDescription = new SqlParameter("@Description", DbType.String);
                paramDescription.Value = tacheProd.Description;
                SqlParameter paramDureePrevue = new SqlParameter("@DureePrevue", SqlDbType.Float);
                paramDureePrevue.Value = tacheProd.DureePrevue;
                SqlParameter paramDureeRestanteEstimee = new SqlParameter("@DureeRestanteEstimee", SqlDbType.Float);
                paramDureeRestanteEstimee.Value = tacheProd.DureeRestante;
                SqlParameter paramCodeModule = new SqlParameter("@CodeModule", DbType.String);
                paramCodeModule.Value = tacheProd.CodeModule;
                SqlParameter paramCodeLogicielModule = new SqlParameter("@CodeLogicielModule", DbType.String);
                paramCodeLogicielModule.Value = tacheProd.CodeLogiciel;
                                SqlParameter paramNumeroVersion = new SqlParameter("@NumeroVersion", SqlDbType.Float);
                paramNumeroVersion.Value = tacheProd.Numero;
                                SqlParameter paramCodeLogicielVersion = new SqlParameter("@CodeLogicielVersion", DbType.String);
                paramCodeLogicielVersion.Value = tacheProd.CodeLogiciel;

                // Création  de la commande
                var command = new SqlCommand(req, cnx, tran);
                command.Parameters.Add(paramIdTache);
                command.Parameters.Add(paramLibellé);
                command.Parameters.Add(paramCodeActivite);
                command.Parameters.Add(paramLogin);
                command.Parameters.Add(paramDescription);
                command.Parameters.Add(paramDureePrevue);
                command.Parameters.Add(paramDureeRestanteEstimee);
                command.Parameters.Add(paramCodeModule);
                command.Parameters.Add(paramCodeLogicielModule);
                command.Parameters.Add(paramNumeroVersion);
                command.Parameters.Add(paramCodeLogicielVersion);

                #endregion
                try
                {
                    //exécution de la commande

                command.ExecuteNonQuery();

                    //Validation de la transaction s'il n'y a pas eu d'erreur
                    tran.Commit();
                }
                catch (Exception)
                {
                    tran.Rollback(); // Annulation de la transaction en cas d'erreur
                    throw;   // Remontée de l'erreur à l'appelant
                }
            }
        }

        /// <summary>
        /// Enregistre une tâche annexe dans la base
        /// </summary>
      
        public static void EnregistrerTachesAnnexes(Tache TachesAnn)
        {
            // Ecriture de la requête d'insertion 
            string req = @"Insert jo.Tache(IdTache, Libelle, Annexe, CodeActivite, Login, Description)                                                                                                 
                        Values (@IdTache, @Libelle, 1 , @CodeActivite, @Login, @Description";
								
         
            using (var cnx = new SqlConnection(Settings.Default.ConnectionJobOverview))
            {
                // Ouverture de la connexion et début de la transaction
                cnx.Open();
                SqlTransaction tran = cnx.BeginTransaction();

                #region Paramètres
                SqlParameter paramIdTache = new SqlParameter("@IdTache", SqlDbType.UniqueIdentifier);
                paramIdTache.Value = Guid.NewGuid();
                SqlParameter paramLibellé = new SqlParameter("@Libelle", DbType.String);
                paramLibellé.Value = TachesAnn.Libelle;
                SqlParameter paramCodeActivite = new SqlParameter("@CodeActivite", DbType.String);
                paramCodeActivite.Value = TachesAnn.CodeActivite;
                SqlParameter paramLogin = new SqlParameter("@Login", DbType.String);
                paramLogin.Value = TachesAnn.LoginPersonne;

                SqlParameter paramDescription = new SqlParameter("@Description", DbType.String);
                paramDescription.Value = TachesAnn.Description;

                // Création  de la commande
                var command = new SqlCommand(req, cnx, tran);
                command.Parameters.Add(paramIdTache);
                command.Parameters.Add(paramLibellé);
                command.Parameters.Add(paramCodeActivite);
                command.Parameters.Add(paramLogin);
                command.Parameters.Add(paramDescription);

                #endregion

                try
                {
                    // exécution de la commande
                   
                    command.ExecuteNonQuery();

                    // Validation de la transaction s'il n'y a pas eu d'erreur
                    tran.Commit();
                }
                catch (Exception)
                {
                    tran.Rollback(); // Annulation de la transaction en cas d'erreur
                    throw;   // Remontée de l'erreur à l'appelant
                }
            }
        }

        /// <summary>
        /// Sérialisation des tâches. Permet d'exporter les données tâches en données xml
        /// </summary>
        /// <param name="taches"></param>
        public static void ExportTachesXml(List<TacheProd> taches)
        {
            // Exportation dans le fichier xml
            XmlSerializer serializer = new XmlSerializer(typeof(List<TacheProd>),
                                                   new XmlRootAttribute("Taches"));
            using (TextWriter writer = new StreamWriter("TachesProduction.xml"))
                serializer.Serialize(writer, taches);

        }

        /// <summary>
        /// Permet de récupérer les activités annexes
        /// </summary>
        public static List<Activite> GetActivités()
        {
            //Requêtage à la BDD pour récupérer les informations sur les activités
            List<Activite> listActivitésAnx = new List<Activite>();

            var conx = new SqlConnection(Settings.Default.ConnectionJobOverview);

            string query = @"select CodeActivite, Libelle, Annexe from jo.Activite";

            var com = new SqlCommand(query, conx);
            conx.Open();

            using (SqlDataReader reader = com.ExecuteReader())
            {
                GetActivitésFromDataReader(reader, listActivitésAnx);
            }

            return listActivitésAnx;
        }
        #endregion

        #region Méthodes Privées

      
        /// <summary>
        /// Obtient et renvoie la liste des activités 
        /// </summary>
        /// <returns></returns>
        private static void GetActivitésFromDataReader(SqlDataReader reader, List<Activite> listActivitésAnx)
        {
            while (reader.Read())
            {
                Activite act = new Activite();

                act.Code = (string)reader["CodeActivite"];
                act.Libelle = (string)reader["Libelle"];
                act.Annexe = (bool)reader["Annexe"];

                listActivitésAnx.Add(act);
            }
        }
        #endregion
    }



}
