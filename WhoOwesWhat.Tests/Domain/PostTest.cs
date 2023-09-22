using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WhoOwesWhat.Domain;

namespace WhoOwesWhat.Tests.Domain
{
    [TestClass]
    public class PostTest : DomainTestBase
    {
        [TestMethod]
        public void MANUAL_TestCreateAPostWithCustomers()
        {
            //"Middag" Cost=500kr, Consumers={Geir, Victor, Marianne} , Payer={Geir} , Valuta="NOK"(ValutaGuid)

            WoWModelContainer domainModel = new WoWModelContainer();

            if(domainModel.DatabaseExists())
            {
                domainModel.DeleteDatabase();    
            }
            
            domainModel.CreateDatabase();

            // Create Person
            Person geirThePerson = new Person("Geir");
            Person victorThePerson = new Person("Victor");
            Person marianneThePerson = new Person("Marianne");

            Consumer geirTheConsumer = new Consumer(geirThePerson);
            Consumer victorTheConsumer = new Consumer(victorThePerson);
            Consumer marianneTheConsumer = new Consumer(marianneThePerson);

            Payer geirThePayer = new Payer(geirThePerson);                     
            
            //Create
            Post post = new Post();
            post.Description = "Middag";
            post.SetTotalCost(500.1f);

            post.AddConsumer(geirTheConsumer);
            post.AddConsumer(victorTheConsumer);
            post.AddConsumer(marianneTheConsumer);
            
            post.AddPayer(geirThePayer);

            post.ISO4217CurrencyCode = "NOK";


            domainModel.AddToPostSet(post);
            domainModel.SaveChanges();

        }

        [TestMethod]
        public void MANUAL_TestCreateAPostWithCustomersWithManuelSetting()
        {
            // "Middag" Cost=1000kr, Consumers={Geir, Victor, Marianne, Ronny} , Payer={Geir} , Valuta="NOK"(ValutaGuid)
            

            WoWModelContainer domainModel = new WoWModelContainer();

            if (domainModel.DatabaseExists())
            {
                domainModel.DeleteDatabase();
            }

            domainModel.CreateDatabase();

            // Create Person
            Person geirThePerson = new Person("Geir");
            Person victorThePerson = new Person("Victor");
            Person marianneThePerson = new Person("Marianne");
            Person ronnyThePerson = new Person("Ronny");

            Consumer geirTheConsumer = new Consumer(geirThePerson);
            Consumer victorTheConsumer = new Consumer(victorThePerson);
            Consumer marianneTheConsumer = new Consumer(marianneThePerson);
            Consumer ronnyTheConsumer = new Consumer(ronnyThePerson);

            Payer geirThePayer = new Payer(geirThePerson);

            //Create
            Post post = new Post();
            post.Description = "Middag";
            post.SetTotalCost(1000f);

            post.AddConsumer(geirTheConsumer);
            post.AddConsumer(victorTheConsumer);
            post.AddConsumer(marianneTheConsumer);

            post.AddPayer(geirThePayer);

            post.ISO4217CurrencyCode = "NOK";


            domainModel.AddToPostSet(post);
            domainModel.SaveChanges();

        }

        [TestMethod]
        public void MANUAL_TestSetup()
        {
            TestSetup ts = new TestSetup();
            ts.AddDinnerExamplePostToDatabaseAddingPayersAndConsumers();

            WoWModelContainer domainModel = new WoWModelContainer();
            var post = domainModel.PostSet.First();

        }

        [TestMethod]
        public void MANUAL_TestSetup2()
        {
            MANUAL_TestSetup();
            WoWModelContainer domainModel = new WoWModelContainer();
            var post = domainModel.PostSet.First();
        }

    }
}
