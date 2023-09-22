using WhoOwesWhat.Domain;

namespace WhoOwesWhat.Tests.Domain
{
    public class TestSetup
    {
        private readonly WoWModelContainer _domainModel = new WoWModelContainer();


        public TestSetup()
        {
        
            if (_domainModel.DatabaseExists())
            {
                _domainModel.DeleteDatabase();
            }

            _domainModel.CreateDatabase();

            // Create Person
            GeirThePerson = new Person("Geir");
            VictorThePerson = new Person("Victor");
            MarianneThePerson = new Person("Marianne");
            RonnyThePerson = new Person("Ronny");          
        }      

        public void AddPersonsToDatabase()
        {
            _domainModel.PersonSet.AddObject(GeirThePerson);
            _domainModel.PersonSet.AddObject(VictorThePerson);
            _domainModel.PersonSet.AddObject(MarianneThePerson);
            _domainModel.PersonSet.AddObject(RonnyThePerson);

            _domainModel.SaveChanges();
        }

        public void AddDinnerExamplePostToDatabaseAddingPayersAndConsumers()
        {
            //Create
            Post post = new Post();
            post.Description = "Middag";
            post.SetTotalCost(1000f);

            //post.AddConsumer(GeirTheConsumer);
            post.AddConsumer(VictorTheConsumer);
            post.AddConsumer(VictorTheConsumer);
            //post.AddConsumer(MarianneTheConsumer);

            //post.AddPayer(GeirThePayer);

            post.ISO4217CurrencyCode = "NOK";


            _domainModel.AddToPostSet(post);
            _domainModel.SaveChanges();
        }

        public Person GeirThePerson { get; private set; }
        public Person VictorThePerson { get; private set; }
        public Person MarianneThePerson { get; private set; }
        public Person RonnyThePerson { get; private set; }

        public Consumer GeirTheConsumer { get; private set; }
        public Consumer VictorTheConsumer { get; private set; }
        public Consumer MarianneTheConsumer { get; private set; }
        public Consumer RonnyTheConsumer { get; private set; }


        public Payer GeirThePayer { get; private set; }
        public Payer VictorThePayer { get; private set; }
        public Payer MarianneThePayer { get; private set; }
        public Payer RonnyThePayer { get; private set; }

    }
}