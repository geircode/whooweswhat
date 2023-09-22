using System;
using System.Collections.Generic;
using System.Linq;

namespace WhoOwesWhat.Domain.RestClasses.Synchronization
{
    public class SynchronizeGroupService
    {
        public SynchronizeGroupResult SynchronizeGroupResult(List<Group> allGroups, WSGroup wsGroup)
        {
            SynchronizeGroupResult result = new SynchronizeGroupResult();

            Group groupByGuid = allGroups.SingleOrDefault(a => a.GroupGuid == wsGroup.GroupGuid);

            Group groupByName = allGroups.SingleOrDefault(a => a.Name.Equals(wsGroup.Name, StringComparison.InvariantCultureIgnoreCase));

            SynchronizeGroup synchronizeGroup = new SynchronizeGroup();
            synchronizeGroup.WSGroup = wsGroup;
            synchronizeGroup.GroupByGuid = WSGroup.MapFromDomain(groupByGuid);
            synchronizeGroup.GroupByName = WSGroup.MapFromDomain(groupByName);

            if (groupByGuid != null)
            {
                // GroupGuid Finnes på mobil, Finnes på server. Name Finnes på server, Finnes på mobil
                // GroupGuid Finnes på mobil, Finnes på server. Name Finnes på mobil, Finnes ikke på server
                if (groupByName != null)
                {
                    result.GroupGuidExistOnServer_NameExistOnServer = synchronizeGroup;
                }
                else
                {
                    result.GroupGuidExistOnServer_NameDoesNotExistOnServer = synchronizeGroup;
                }
            }
            else
            {
                //GroupGuid Finnes på mobil, Finnes ikke på server. Name Finnes på server, Finnes på mobil
                //GroupGuid Finnes på mobil, Finnes ikke på server. Name Finnes på mobil, Finnes ikke på server
                if (groupByName != null)
                {
                    result.GroupGuidDoesNotExistOnServer_NameExistOnServer = synchronizeGroup;
                }
                else
                {
                    result.GroupGuidDoesNotExistOnServer_NameDoesNotExistOnServer = synchronizeGroup;
                }
            }
            return result;
        }

        public void HandleSynchronizationResult(SynchronizeGroupResult result)
        {
            DomainRepository domain = new DomainRepository();

            bool isSaveChanges = false;

            if(result.GroupGuidDoesNotExistOnServer_NameDoesNotExistOnServer != null)
            {
                Group newGroup = result.GroupGuidDoesNotExistOnServer_NameDoesNotExistOnServer.WSGroup.MapAllToDomain();
                domain.AddGroup(newGroup);
                isSaveChanges = true;
            }
            
            else if(result.GroupGuidExistOnServer_NameDoesNotExistOnServer != null)
            {
                Group toUpdate = domain.GetGroupByGuid(result.GroupGuidExistOnServer_NameDoesNotExistOnServer.WSGroup.GroupGuid);
                result.GroupGuidExistOnServer_NameDoesNotExistOnServer.WSGroup.MapInfoToDomain(toUpdate);
                isSaveChanges = true;
               
            }

            else if(result.GroupGuidExistOnServer_NameExistOnServer != null)
            {
                Group toUpdate = domain.GetGroupByGuid(result.GroupGuidExistOnServer_NameExistOnServer.WSGroup.GroupGuid);
                result.GroupGuidExistOnServer_NameExistOnServer.WSGroup.MapInfoToDomain(toUpdate);
                isSaveChanges = true;
            }

            if(isSaveChanges )
            {
                domain.SaveChanges();    
            }
            
        }
    }
}
