namespace WhoOwesWhat.Domain.RestClasses.Synchronization
{
    public class SynchronizeGroup
    {
        public WSGroup WSGroup { get; set; }
        public WSGroup GroupByGuid { get; set; }
        public WSGroup GroupByName { get; set; }
    }
}