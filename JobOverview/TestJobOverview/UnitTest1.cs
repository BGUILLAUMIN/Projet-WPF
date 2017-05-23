using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JobOverview.Model;
using JobOverview.Entity;

namespace TestJobOverview
{
    [TestClass]
    public class TestDAL
    {
        [TestMethod]

        public void TestEnregistrementTacheAnnexe()
        {
            var listTachesann = DALTaches.GetTachesAnnexe();

           int nbTachesintial  = listTachesann.Count;

            var t = new Tache
            {
                Id = Guid.NewGuid(),
                Libelle = "Courrier",
                CodeActivite = "DP",
                LoginPersonne = "AFERRAND"
            };

            DALTaches.EnregistrerTachesAnnexes(t);

            listTachesann = DALTaches.GetTachesAnnexe();
            int nbTachesFinal = listTachesann.Count;

            Assert.AreEqual(nbTachesFinal-nbTachesintial, 1);
        }

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
                Numero = 99,
                DureePrevue =8,
                DureeRestante =1,
                CodeLogiciel= "GENOMICA",
                CodeModule = "POLYMORPHISME"

            };

            DALTaches.EnregistrerTachesAnnexes(t);

            listTachesProd = DALTaches.GetTachesProd();
            int nbTachesFinal = listTachesProd.Count;

            Assert.AreEqual(nbTachesFinal - nbTachesintial, 1);
        }
    }
}

