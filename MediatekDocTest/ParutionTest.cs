using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;


namespace MediatekDocTest
{
    [TestClass]
    public class ParutionTest
    {
        [TestMethod]
        public void TestParutionDansAbonnement()
        {
            DateTime dateCommande = new DateTime(2024, 03, 18);
            DateTime dateFinAbonnement = new DateTime(2024, 03, 20);
            DateTime dateParution = new DateTime(2024, 03, 19);
            bool result = true;
            bool resultNow = ParutionDansAbonnement(dateCommande, dateFinAbonnement, dateParution);
            Assert.AreEqual(result, resultNow);
        }
        public bool ParutionDansAbonnement(DateTime dateCommande, DateTime dateFinAbonnement, DateTime dateParution)
        {
            return dateCommande < dateParution && dateParution < dateFinAbonnement;
        }
    }
}
