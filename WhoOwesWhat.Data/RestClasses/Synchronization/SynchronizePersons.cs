namespace WhoOwesWhat.Domain.RestClasses.Synchronization
{
    public class SynchronizePersons
    {
        public WSPerson WSPerson { get; set; }
        public Person PersonByGuid { get; set; }
        public Person PersonByName { get; set; }
    }
}