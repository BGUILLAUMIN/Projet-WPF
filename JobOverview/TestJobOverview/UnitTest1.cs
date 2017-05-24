using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JobOverview.Model;
using JobOverview.Entity;

namespace TestJobOverview
{
    [TestClass]
    public class TestDAL
    {
        private static Tache ta { get; set; }

        [TestMethod]


        // Test d'enregistrement de tâche annexe.
        public void TestEnregistrementTacheAnnexe()
        {
         
        var listTachesann = DALTaches.GetTachesAnnexe();

           int nbTachesintial  = listTachesann.Count;
             
        var t = new Tache
            {
                Id = Guid.NewGuid(),
                Libelle = "Courrier",
                CodeActivite = "DP",
                LoginPersonne = "AFERRAND",
                Description = "aller le chercher"
            };

            ta = t;
        DALTaches.EnregistrerTachesAnnexes(t);

            listTachesann = DALTaches.GetTachesAnnexe();
            int nbTachesFinal = listTachesann.Count;

            Assert.AreEqual(nbTachesFinal-nbTachesintial, 1);
        }

        [TestMethod]

        // Test de supression de tâche annexe utilisant la variable crée dans le test d'ajout
        // de tache annexe.
        public void TestSupprimerTachesAnnexes()
        {
            var listTachesann = DALTaches.GetTachesAnnexe();

            int nbTachesintial = listTachesann.Count;

            DALTaches.SupprimerTachesAnnexes(ta.Id);

            listTachesann = DALTaches.GetTachesAnnexe();
            int nbTachesFinal = listTachesann.Count;

            Assert.AreEqual(nbTachesFinal - nbTachesintial, -1);
        }




        // Test d'enregistrement de tâche de production.
        [TestMethod]
        public void TestEnregistrementTacheProd()
        {
            var listTachesProd = DALTaches.GetTachesProd();

            int nbTachesintial = listTachesProd.Count;

            var t = new TacheProd
            {
                Id = Guid.NewGuid(),
                Libelle = "CE",
                CodeActivite = "DP",
                LoginPersonne = "AFERRAND",
                Description = "aller le chercher",
                Version = 1,
                DureePrevue =8,
                DureeRestante =1,
                CodeLogiciel= "GENOMICA",
                CodeModule = "POLYMORPHISME"

            };

            DALTaches.EnregistrerTachesProd(t);

            listTachesProd = DALTaches.GetTachesProd();
            int nbTachesFinal = listTachesProd.Count;

            Assert.AreEqual(nbTachesFinal - nbTachesintial, 1);
        }
    }
}

