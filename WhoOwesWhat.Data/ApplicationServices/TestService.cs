using System;
using System.Collections.Generic;
using System.Data.EntityClient;
using System.Data.SqlClient;
using WhoOwesWhat.Domain.RestClasses;

namespace WhoOwesWhat.Domain.ApplicationServices
{
    public class TestService
    {
        public static String LoginUserPassword { get { return "smurf"; } }

        private readonly DomainRepository _testDomain;

        public TestService()
        {
            DomainRepository.SetEntityConnectionString(GetTestEntityConnectionString());
            _testDomain = new DomainRepository();
        }

        public void AddTestSetupToDatabase()
        {
            _testDomain.AddPerson(TestPersonSetup.Test02_Server_PersonGuidExistOnMobileNotOnServer_UserNameExistOnMobileAndOnServer());
            _testDomain.AddPerson(TestPersonSetup.Test03_Server_PersonGuidExistOnMobileAndOnServer_UserNameExistOnMobileNotOnServer());

            _testDomain.AddGroup(TestGroupSetup.Test02_Server_GroupGuidExistOnMobileNotOnServer_NameExistOnMobileAndOnServer());
            _testDomain.AddGroup(TestGroupSetup.Test03_Server_GroupGuidExistOnMobileAndOnServer_NameExistOnMobileNotOnServer());

            AddTestPostUsers();
            AddTestPostGroup();

            // used in tests where the Post should be stored on server, but not on mobile
            AddNewPost1ToServer();

            AddPost2WithVersion2ToServer();
            AddPost2WithVersion3ToServer();
            AddPost5_MobileVersion1NotDirty_SynchronizeAgainstServerVersion2ThatIsDeleted();

        }

        private void AddPost2WithVersion2ToServer()
        {
            if (!TestSetup.CheckForTestSetup(enumTestSetup.addTestPostUsers))
            {
                AddTestPostUsers();
            }

            TestSetup.AddTestSetup(enumTestSetup.AddPost2WithVersion2ToServer);

            DomainRepository domain = new DomainRepository();

            Person geirThePerson = domain.GetPersonByGuid(TestPersonSetup.Post1User_Geir().GetPersonGuid());
            Person marianneThePerson = domain.GetPersonByGuid(TestPersonSetup.Post2User_Marianne().GetPersonGuid());

            Consumer geirTheConsumer = new Consumer(geirThePerson);
            Consumer marianneTheConsumer = new Consumer(marianneThePerson);

            Payer geirThePayer = new Payer(geirThePerson);

            //Create
            Post post = new Post();
            post.PostGuid = Guid.Parse("95368591-6E79-499E-ABF6-602CFE3DEF45");
            post.Description = "Klosteret";
            post.Comment = "Romantisk";
            post.SetVersion(0);
            post.Date = new DateTime(2011, 3, 14);
            post.SetTotalCost(1500f);
            post.ISO4217CurrencyCode = "NOK";

            post.SetInternalGroup(TestGroupSetup.Post2Group_FellesTing());

            //Group group = _testDomain.AddOrGetGroup(TestGroupSetup.Post2Group_FellesTing());
            //group.Posts.Add(post);

            post.AddConsumer(geirTheConsumer);
            post.AddConsumer(marianneTheConsumer);

            post.AddPayer(geirThePayer);

            domain.AddPost(post);

            Post postToUpgradeToVersion2 = domain.GetPostByGuidAndVersion(post.PostGuid, 1);
            Post clone = Post.Clone(postToUpgradeToVersion2);
            clone.Comment = "God vin";

            domain.AddPost(clone);
        }

        private void AddPost2WithVersion3ToServer()
        {
            if (!TestSetup.CheckForTestSetup(enumTestSetup.addTestPostUsers))
            {
                AddTestPostUsers();
            }

            TestSetup.AddTestSetup(enumTestSetup.AddPost2WithVersion3ToServer);

            DomainRepository domain = new DomainRepository();

            Person geirThePerson = domain.GetPersonByGuid(TestPersonSetup.Post1User_Geir().GetPersonGuid());
            Person marianneThePerson = domain.GetPersonByGuid(TestPersonSetup.Post2User_Marianne().GetPersonGuid());
            Person victorThePerson = domain.GetPersonByGuid(TestPersonSetup.Post3User_Victor().GetPersonGuid());

            Consumer geirTheConsumer = new Consumer(geirThePerson);
            Consumer marianneTheConsumer = new Consumer(marianneThePerson);
            Consumer victorTheConsumer = new Consumer(victorThePerson);

            Payer geirThePayer = new Payer(geirThePerson);

            //Create
            Post post = new Post();
            post.PostGuid = Guid.Parse("1993032A-B704-4325-A082-DD4A7579411C");
            post.Description = "Polynesian Shores, Maui";
            post.Comment = "Digg!!";
            post.SetVersion(0);
            post.Date = new DateTime(2012, 3, 30);
            post.SetTotalCost(250f);
            post.ISO4217CurrencyCode = "USD";

            post.SetInternalGroup(TestGroupSetup.Post1Group_BlueHawaii());

            post.AddConsumer(geirTheConsumer);
            post.AddConsumer(marianneTheConsumer);
            post.AddConsumer(victorTheConsumer);

            post.AddPayer(geirThePayer);

            domain.AddPost(post);

            Post cloneV2 = Post.Clone(domain.GetPostByGuidAndVersion(post.PostGuid, 1));
            domain.AddPost(cloneV2);
            Post cloneV3 = Post.Clone(domain.GetPostByGuidAndVersion(post.PostGuid, 2));
            domain.AddPost(cloneV3);

        }

        private Post AddPost5_MobileVersion1NotDirty_SynchronizeAgainstServerVersion2ThatIsDeleted()
        {

            if (!TestSetup.CheckForTestSetup(enumTestSetup.addTestPostUsers))
            {
                AddTestPostUsers();
            }

            TestSetup.AddTestSetup(enumTestSetup.AddPost5_MobileVersion1NotDirty_SynchronizeAgainstServerVersion2ThatIsDeleted);

            DomainRepository domain = new DomainRepository();
            Person geirThePerson = domain.GetPersonByGuid(TestPersonSetup.Post1User_Geir().GetPersonGuid());
            Person marianneThePerson = domain.GetPersonByGuid(TestPersonSetup.Post2User_Marianne().GetPersonGuid());
            Person victorThePerson = domain.GetPersonByGuid(TestPersonSetup.Post3User_Victor().GetPersonGuid());

            Consumer geirTheConsumer = new Consumer(geirThePerson);
            Consumer marianneTheConsumer = new Consumer(marianneThePerson);
            Consumer victorTheConsumer = new Consumer(victorThePerson);

            Payer geirThePayer = new Payer(geirThePerson);

            Post post = new Post();
            post.PostGuid = Guid.Parse("8e9e017f-f850-41ae-91ed-578548c80fb2");
            post.Description = "Kauai highlands";
            post.Comment = "Grønt her gitt";
            post.SetVersion(0);
            post.Date = new DateTime(2012, 4, 10);
            post.SetTotalCost(333f);
            post.ISO4217CurrencyCode = "USD";


            post.SetInternalGroup(TestGroupSetup.Post1Group_BlueHawaii());

            post.AddConsumer(geirTheConsumer);
            post.AddConsumer(marianneTheConsumer);
            post.AddConsumer(victorTheConsumer);

            post.AddPayer(geirThePayer);

            domain.AddPost(post);

            Post cloneV2 = Post.Clone(domain.GetPostByGuidAndVersion(post.PostGuid, 1));

            // DELETE IT!            
            domain.AddPost(cloneV2);
            domain.DeletePost(cloneV2);

            return post;
        }

        private void AddNewPost1ToServer()
        {
            if (!TestSetup.CheckForTestSetup(enumTestSetup.addTestPostUsers))
            {
                AddTestPostUsers();
            }

            TestSetup.AddTestSetup(enumTestSetup.addNewPost1ToServer);

            DomainRepository domain = new DomainRepository();

            Person geirThePerson = domain.GetPersonByGuid(TestPersonSetup.Post1User_Geir().GetPersonGuid());
            Person victorThePerson = domain.GetPersonByGuid(TestPersonSetup.Post3User_Victor().GetPersonGuid());
            Person marianneThePerson = domain.GetPersonByGuid(TestPersonSetup.Post2User_Marianne().GetPersonGuid());

            Consumer geirTheConsumer = new Consumer(geirThePerson);
            Consumer victorTheConsumer = new Consumer(victorThePerson);
            Consumer marianneTheConsumer = new Consumer(marianneThePerson);

            Payer geirThePayer = new Payer(geirThePerson);

            //Create
            Post post = new Post();
            post.PostGuid = Guid.Parse("5269aab0-d1ca-4769-b30e-a790c34f15e9");
            post.Description = "Middag";
            post.Comment = "Velsmakende!";
            post.SetVersion(0);
            post.Date = new DateTime(2012, 4, 6);
            post.SetTotalCost(500.1f);
            post.ISO4217CurrencyCode = "NOK";

            post.SetInternalGroup(TestGroupSetup.Post1Group_BlueHawaii());

            post.AddConsumer(geirTheConsumer);
            post.AddConsumer(victorTheConsumer);
            post.AddConsumer(marianneTheConsumer);

            post.AddPayer(geirThePayer);

            domain.AddPost(post);
        }

        public Post GetPost1()
        {
            if (!TestSetup.CheckForTestSetup(enumTestSetup.addNewPost1ToServer))
            {
                AddNewPost1ToServer();
            }

            DomainRepository domain = new DomainRepository();
            Post post = domain.GetLatestPost(Guid.Parse("5269aab0-d1ca-4769-b30e-a790c34f15e9"));

            return post;
        }

        private Group AddTestPostGroup()
        {
            TestSetup.AddTestSetup(enumTestSetup.addTestPostGroup);

            Group group = new Group();
            group.GroupGuid = Guid.Parse("a4120f15-f146-4e08-9e3e-27926e085f8e");
            group.Name = "KauaiPostTestGroup";
            _testDomain.AddGroup(group);
            return group;
        }

        public void AddTestPostUsers()
        {
            TestSetup.AddTestSetup(enumTestSetup.addTestPostUsers);

            _testDomain.AddPerson(TestPersonSetup.Post1User_Geir());
            _testDomain.AddPerson(TestPersonSetup.Post2User_Marianne());
            _testDomain.AddPerson(TestPersonSetup.Post3User_Victor());
            _testDomain.AddPerson(TestPersonSetup.Post4User_Olav());
            _testDomain.AddPerson(TestPersonSetup.Post5User_Beate());
        }

        public Person GetTestLoginPerson()
        {
            Person p = new Person();
            p.SetUserName("Garg1337");
            p.FullName = "Tittei";
            p.SetPassword(LoginUserPassword);
            p.SetPersonGuid(Guid.Parse("B1F33864-780C-4C07-A440-63947F470BEE"));
            return p;
        }

        public UserCredentials CreateOrGetLoginUserCredentials()
        {
            UserCredentials user = new UserCredentials();
            Person loginPerson = GetTestLoginPerson();
            loginPerson = _testDomain.AddOrGetPerson(loginPerson);

            user.Password = LoginUserPassword;
            user.PersonGuid = loginPerson.PersonGuid;

            return user;
        }


        public void ResetDatabaseAndDomain()
        {
            _testDomain.ResetDatabaseAndDomain();
        }

        public Person AddTestPersonWithPassword()
        {
            Person p = GetTestPerson();
            _testDomain.AddPerson(p);
            return p;
        }

        public Person GetTestPerson()
        {
            Person p = new Person();
            p.SetUserName("Garg1337");
            p.SetPassword("monsterinc");
            p.SetPersonGuid(Guid.Parse("b1f33864-780c-4c07-a440-63947f470bee"));
            p.FullName = "Garg Marg";
            return p;
        }

        private static String GetTestEntityConnectionString()
        {

            // Specify the provider name, server and database.
            string providerName = "System.Data.SqlClient";
            string serverName = ".";
            string databaseName = "WhoOwesWhatTest";

            // Initialize the connection string builder for the
            // underlying provider.
            SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder();

            // Set the properties for the data source.
            sqlBuilder.DataSource = serverName;
            sqlBuilder.InitialCatalog = databaseName;
            sqlBuilder.IntegratedSecurity = true;
            sqlBuilder.MultipleActiveResultSets = true;

            // Build the SqlConnection connection string.
            string providerString = sqlBuilder.ToString();

            // Initialize the EntityConnectionStringBuilder.
            EntityConnectionStringBuilder entityBuilder = new EntityConnectionStringBuilder();

            //Set the provider name.
            entityBuilder.Provider = providerName;

            // Set the provider-specific connection string.
            entityBuilder.ProviderConnectionString = providerString;

            // Set the Metadata location.
            entityBuilder.Metadata = @"res://*/WoWModel.csdl|
                            res://*/WoWModel.ssdl|
                            res://*/WoWModel.msl";



            //using (EntityConnection conn = new EntityConnection(entityBuilder.ToString()))
            //{
            //    conn.Open();
            //    Console.WriteLine("Just testing the connection.");
            //    conn.Close();
            //}

            return entityBuilder.ToString();
        }
    }

}
