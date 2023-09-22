using System;
using System.Collections.Generic;
using System.Linq;

namespace WhoOwesWhat.Domain.RestClasses.Synchronization
{
    public class SynchronizePersonService
    {
        public SynchronizePersonResult SynchronizePersonResult(List<Person> allPersons, WSPerson wsPerson)
        {
            SynchronizePersonResult result = new SynchronizePersonResult();

            Person personByGuid = allPersons.SingleOrDefault(a => a.PersonGuid == wsPerson.PersonGuid);

            Person personByName = allPersons.SingleOrDefault(a => a.UserName.Equals(wsPerson.UserName, StringComparison.InvariantCultureIgnoreCase));

            SynchronizePersons synchronizePersons = new SynchronizePersons();
            synchronizePersons.WSPerson = wsPerson;
            synchronizePersons.PersonByGuid = personByGuid;
            synchronizePersons.PersonByName = personByName;

            if (personByGuid != null)
            {
                // PersonGuid Finnes på mobil, Finnes på server. UserName Finnes på server, Finnes på mobil
                // PersonGuid Finnes på mobil, Finnes på server. UserName Finnes på mobil, Finnes ikke på server
                if (personByName != null)
                {
                    result.PersonGuidExistOnServer_UserNameExistOnServer = synchronizePersons;
                }
                else
                {
                    result.PersonGuidExistOnServer_UserNameDoesNotExistOnServer = synchronizePersons;
                }
            }
            else
            {
                //PersonGuid Finnes på mobil, Finnes ikke på server. UserName Finnes på server, Finnes på mobil
                //PersonGuid Finnes på mobil, Finnes ikke på server. UserName Finnes på mobil, Finnes ikke på server
                if (personByName != null)
                {
                    result.PersonGuidDoesNotExistOnServer_UserNameExistOnServer = synchronizePersons;
                }
                else
                {
                    result.PersonGuidDoesNotExistOnServer_UserNameDoesNotExistOnServer = synchronizePersons;
                }
            }
            return result;
        }

        public void HandleSynchronizationResult(SynchronizePersonResult result)
        {
            DomainRepository domain = new DomainRepository();

            bool isSaveChanges = false;

            if(result.PersonGuidDoesNotExistOnServer_UserNameDoesNotExistOnServer != null)
            {
                Person newPerson = result.PersonGuidDoesNotExistOnServer_UserNameDoesNotExistOnServer.WSPerson.MapAllToDomain();
                domain.AddPerson(newPerson);
                isSaveChanges = true;
            }
            
            else if(result.PersonGuidExistOnServer_UserNameDoesNotExistOnServer != null)
            {
                // Update other person info on server.  i.e FullName, email etc.

                Person personToUpdate = domain.GetPersonByGuid(result.PersonGuidExistOnServer_UserNameDoesNotExistOnServer.WSPerson.PersonGuid);
                result.PersonGuidExistOnServer_UserNameDoesNotExistOnServer.WSPerson.MapToDomain(personToUpdate);
                isSaveChanges = true;
               
            }

            else if(result.PersonGuidExistOnServer_UserNameExistOnServer != null)
            {
                // Update other person info on server.  i.e FullName, email etc.
                Person personToUpdate = domain.GetPersonByGuid(result.PersonGuidExistOnServer_UserNameExistOnServer.WSPerson.PersonGuid);
                result.PersonGuidExistOnServer_UserNameExistOnServer.WSPerson.MapToDomain(personToUpdate);
                isSaveChanges = true;
            }

            if(isSaveChanges )
            {
                domain.SaveChanges();    
            }
            
        }
    }
}
