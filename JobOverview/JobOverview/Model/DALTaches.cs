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
                        tp.LoginPersonne = (string)reader["Login"];
                        tp.DureePrevue = (float)reader["DureePrevue"];
                        tp.DureeRestante = (float)reader["DureeRestanteEstimee"];

                        listTaches.Add(tp);
                    }
                }
            }
            return listTaches;
        }
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
                      ta.Id= (Guid)reader["IdTache"];
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
        /// Enregistre une liste de tâches de production dans la base
        /// </summary>
        /// <param name="listTaches"></param>
        public static void EnregistrerTachesProd(List<TacheProd> listTaches)
{
    string req = @"Insert jo.Tache(IdTache, Libelle, Annexe, CodeActivite, Login, Description)                                                                                                 
                        select IdTache, Libelle, Annexe, CodeActivite, Login, Description
								from  @table;

                        Insert jo.TacheProd (IdTache, DureePrevue, DureeRestanteEstimee,
									CodeModule, CodeLogicielModule, NumeroVersion, CodeLogicielVersion)
                        select IdTache, DureePrevue, DureeRestanteEstimee,
								CodeModule, CodeLogicielModule, NumeroVersion, CodeLogicielVersion
								from @table";

    // Création du paramètre de type table mémoire
    // /!\ Le type TypeTablePersonne doit être créé au préalable dans la base
    var param = new SqlParameter("@table", SqlDbType.Structured);
    DataTable tableTaches = GetDataTableForTachesProd(listTaches);
    param.TypeName = "TypeTableTachesProd";
    param.Value = tableTaches;

    using (var cnx = new SqlConnection(Settings.Default.ConnectionJobOverview))
    {
        // Ouverture de la connexion et début de la transaction
        cnx.Open();
        SqlTransaction tran = cnx.BeginTransaction();

        try
        {
            // Création et exécution de la commande
            var command = new SqlCommand(req, cnx, tran);
            command.Parameters.Add(param);
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
            XmlSerializer serializer = new XmlSerializer(typeof(List<Tache>),
                                                new XmlRootAttribute("Taches"));
            using (TextWriter writer = new StreamWriter("Taches.xml"))
                serializer.Serialize(writer, taches);
        }

        /// <summary>
        /// Permet de récupérer les activités annexes
        /// </summary>
        public static List<Activite> GetActivitésAnnexes()
        {
            //Requêtage à la BDD pour récupérer les informations sur les activités
            List<Activite> listActivitésAnx = new List<Activite>();

            var conx = new SqlConnection(Settings.Default.ConnectionJobOverview);

            string query = @"select CodeActivite, Libelle from jo.Activite where Annexe = 1";

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
        /// Création et remplissage d'une table mémoire à partir d'une liste de tâches de prod
        /// </summary>
        /// <param name="listTachesProd"></param>
        /// <returns></returns>
        private static DataTable GetDataTableForTachesProd(List<TacheProd> listTachesProd)
        {
            // Création de la table et de ses colonnes
            DataTable table = new DataTable();

    var colIdTache = new DataColumn("IdTache", typeof(Guid));
    colIdTache.AllowDBNull = false;
    table.Columns.Add(colIdTache);
    var colDureePrevue = new DataColumn("DureePrevue", typeof(float));
    colDureePrevue.AllowDBNull = false;
    table.Columns.Add(colDureePrevue);
    var colDureeRestanteEstimee = new DataColumn("DureeRestanteEstimee", typeof(float));
    colDureeRestanteEstimee.AllowDBNull = false;
    table.Columns.Add(colDureeRestanteEstimee);
    var colCodeModule = new DataColumn("CodeModule", typeof(string));
    colCodeModule.AllowDBNull = false;
    table.Columns.Add(colCodeModule);
    var colCodeLogicieModule = new DataColumn("CodeLogicielModule", typeof(string));
    colCodeLogicieModule.AllowDBNull = false;
    table.Columns.Add(colCodeLogicieModule);
    var colNumeroVersion = new DataColumn("NumeroVersion", typeof(float));
    colNumeroVersion.AllowDBNull = false;
    table.Columns.Add(colNumeroVersion);
    var colCodeLogicielVersion = new DataColumn("CodeLogicielVersion", typeof(string));
    colCodeLogicielVersion.AllowDBNull = false;
    table.Columns.Add(colCodeLogicielVersion);
    var colLibelle = new DataColumn("Libelle", typeof(string));
    colLibelle.AllowDBNull = false;
    table.Columns.Add(colLibelle);
    var colAnnexe = new DataColumn("Annexe", typeof(bool));
    colAnnexe.AllowDBNull = false;
    table.Columns.Add(colAnnexe);
    var colCodeActivite = new DataColumn("CodeActivite", typeof(string));
    colCodeActivite.AllowDBNull = false;
    table.Columns.Add(colCodeActivite);
    var colLogin = new DataColumn("Login", typeof(string));
    colLogin.AllowDBNull = false;
    table.Columns.Add(colLogin);
    var colDescription = new DataColumn("Description", typeof(string));
    table.Columns.Add(colDescription);

    // Remplissage de la table
    foreach (var p in listTachesProd)
    {
        DataRow ligne = table.NewRow();
        ligne["IdTache"] = Guid.NewGuid();

        ligne["DureePrevue"] = p.DureePrevue;
        ligne["DureeRestanteEstimee"] = p.DureeRestante;
        ligne["CodeModule"] = p.CodeModule;
        ligne["CodeLogicielModule"] = p.CodeLogiciel;
        ligne["NumeroVersion"] = p.Version;
        ligne["CodeLogicielVersion"] = p.CodeLogiciel;
        ligne["Libelle"] = p.Libelle;
        ligne["Annexe"] = false;
        ligne["CodeActivite"] = p.CodeActivite;
        ligne["Login"] = p.LoginPersonne;
        ligne["Description"] = p.Description;

        table.Rows.Add(ligne);

            }
            return table;
        }
        
        private static void GetActivitésFromDataReader(SqlDataReader reader, List<Activite> listActivitésAnx)
        {
            while (reader.Read())
            {
                Activite act = new Activite();

                act.Code = reader["CodeActivite"].ToString();
                act.Libelle = reader["Libelle"].ToString();

                listActivitésAnx.Add(act);
            }
        }
        #endregion
    }



}
