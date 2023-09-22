namespace WhoOwesWhat.Domain.RestClasses.Synchronization
{
    public class SynchronizeGroupResult
    {
        public SynchronizeGroup GroupGuidExistOnServer_NameExistOnServer { get; set; }
        public SynchronizeGroup GroupGuidDoesNotExistOnServer_NameExistOnServer{ get; set; }
        public SynchronizeGroup GroupGuidDoesNotExistOnServer_NameDoesNotExistOnServer{ get; set; }
        public SynchronizeGroup GroupGuidExistOnServer_NameDoesNotExistOnServer{ get; set; }
    }
}