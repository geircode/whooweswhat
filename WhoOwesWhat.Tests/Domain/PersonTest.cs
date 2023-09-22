using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WhoOwesWhat.Domain;

namespace WhoOwesWhat.Tests.Domain
{
    [TestClass]
    public class PersonTest
    {
        [TestMethod]
        public void MANUAL_AddAndDeletePersonTest()
        {

            DomainRepository repo = new DomainRepository();
            //c.GetPersons();
            Person p = new Person();
            p.SetUserName("Twinings");

            p = repo.AddPerson(p);
            Assert.IsTrue(p.PersonId != 0);

            List<Person> persons = repo.GetAllPersons();
            Person p1 = persons.Single(a => a.UserName == "Twinings");

            repo.DeletePerson(p1);
            Person p2 = repo.GetAllPersons().SingleOrDefault(a => a.UserName == "Twinings");
            Assert.IsNull(p2);
        }

        [TestMethod]
        public void TestUsePassword()
        {
            Person p = new Person("GeirCrypto");
            p.SetPassword("Møøø");

            bool isAuthenticated = p.ConfirmPassword("Møøø");
            Assert.IsTrue(isAuthenticated);
        }
    }
}
