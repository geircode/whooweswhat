using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WhoOwesWhat.Domain.Exceptions;
using WhoOwesWhat.Domain.RestClasses;
using WhoOwesWhat.Domain.RestClasses.Synchronization;

namespace WhoOwesWhat.Domain.ApplicationServices
{
    public class GroupControllerService : ControllerServiceBase
    {
        public GroupControllerService()
        {

        }

        public JsonResult SynchronizeGroups(UserCredentials user, List<WSGroup> groups)
        {
            if (!IsAuthenticated(user))
            {
                return GetJsonResult(new { isAuthenticationFailure = true });
            }

            DomainRepository domain = new DomainRepository();

            List<Group> allgroups = domain.GetAllGroups();

            SynchronizeGroupService service = new SynchronizeGroupService();

            List<SynchronizeGroupResult> results = new List<SynchronizeGroupResult>();
            foreach (WSGroup wsGroup in groups)
            {
                SynchronizeGroupResult result = service.SynchronizeGroupResult(allgroups, wsGroup);
                service.HandleSynchronizationResult(result);
                results.Add(result);
            }

            List<WSGroup> groupsToAdd = GetAllNewGroupsOnServer(groups);

            var json = GetJsonResult(new { synchronizeGroupResultList = results, groupsToAdd});
            return json;
        }

        private List<WSGroup> GetAllNewGroupsOnServer(List<WSGroup> groups)
        {
            DomainRepository domain = new DomainRepository();
            List<Group> newGroups = domain.GetAllGroups().Where(a => groups.All(b => b.GroupGuid != a.GroupGuid)).ToList();
            List<WSGroup> newWSGroups = new List<WSGroup>();
            foreach (Group newGroup in newGroups)
            {
                WSGroup wsGroup = WSGroup.MapFromDomain(newGroup);
               
                newWSGroups.Add(wsGroup);
            }
            return newWSGroups;
        }
    }
}
