using System.Collections.Generic;

namespace WhoOwesWhat.Domain.RestClasses.Synchronization
{
    public class SynchronizeGroupsResult
    {
        public List<SynchronizeGroupResult> SynchronizeGroupResultList { get; set; }
    }
}