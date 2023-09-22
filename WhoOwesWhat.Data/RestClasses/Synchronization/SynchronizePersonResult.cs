using System.Collections.Generic;

namespace WhoOwesWhat.Domain.RestClasses.Synchronization
{
    public class SynchronizePersonResult
    {
        public SynchronizePersons PersonGuidExistOnServer_UserNameDoesNotExistOnServer { get; set; }
        public SynchronizePersons PersonGuidExistOnServer_UserNameExistOnServer { get; set; }
        public SynchronizePersons PersonGuidDoesNotExistOnServer_UserNameExistOnServer { get; set; }
        public SynchronizePersons PersonGuidDoesNotExistOnServer_UserNameDoesNotExistOnServer { get; set; }
   
    }
}