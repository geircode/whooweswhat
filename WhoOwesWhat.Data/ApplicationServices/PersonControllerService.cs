using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WhoOwesWhat.Domain.Exceptions;
using WhoOwesWhat.Domain.RestClasses;
using WhoOwesWhat.Domain.RestClasses.Synchronization;

namespace WhoOwesWhat.Domain.ApplicationServices
{
    public class PersonControllerService : ControllerServiceBase
    {
        public PersonControllerService()
        {

        }

        public JsonResult SynchronizePerson(UserCredentials user, WSPerson wsPerson)
        {
            if (!IsAuthenticated(user))
            {
                return GetJsonResult(new { isAuthenticationFailure = true });
            }

            //PersonGuid Finnes på mobil, Finnes ikke på server. UserName Finnes på mobil, Finnes ikke på server
            //PersonGuid Finnes på mobil, Finnes ikke på server. UserName Finnes på server, Finnes på mobil
            //PersonGuid Finnes på mobil, Finnes på server. UserName Finnes på mobil, Finnes ikke på server
            //PersonGuid Finnes på mobil, Finnes på server. UserName Finnes på server, Finnes på mobil
            DomainRepository domain = new DomainRepository();

            List<Person> allPersons = domain.GetAllPersons();
            SynchronizePersonService service = new SynchronizePersonService();

            SynchronizePersonResult result = service.SynchronizePersonResult(allPersons, wsPerson);

            service.HandleSynchronizationResult(result);

            // Send back to mobile for update and handling:
            // PersonGuidDoesNotExistOnServer_UserNameExistOnServer
            // PersonGuidExistOnServer_UserNameDoesNotExistOnServer

            bool IsSynchronizationSuccess = true;

            WSPerson PersonGuidDoesNotExistOnServer_UserNameExistOnServer = null;
            if (result.PersonGuidDoesNotExistOnServer_UserNameExistOnServer != null)
            {
                PersonGuidDoesNotExistOnServer_UserNameExistOnServer = result.PersonGuidDoesNotExistOnServer_UserNameExistOnServer.WSPerson;
                IsSynchronizationSuccess = false;
            }

            WSPerson PersonGuidExistOnServer_UserNameDoesNotExistOnServer = null;
            if (result.PersonGuidExistOnServer_UserNameDoesNotExistOnServer != null)
            {
                PersonGuidExistOnServer_UserNameDoesNotExistOnServer = new WSPerson();
                PersonGuidExistOnServer_UserNameDoesNotExistOnServer.MapFromDomain(result.PersonGuidExistOnServer_UserNameDoesNotExistOnServer.PersonByGuid);
                IsSynchronizationSuccess = false;
            }

            var json = GetJsonResult(new { PersonGuidDoesNotExistOnServer_UserNameExistOnServer, PersonGuidExistOnServer_UserNameDoesNotExistOnServer, IsSynchronizationSuccess });
            return json;

        }

        public JsonResult CreateNewPerson(UserCredentials user, WSPerson wsPerson)
        {
            DomainRepository domain = new DomainRepository();
            Person person = wsPerson.MapAllToDomain();

            if (!IsAuthenticated(user))
            {
                return GetJsonResult(new { isAuthenticationFailure = true });
            }

            try
            {
                domain.AddPerson(person);
            }
            catch (GuidExistException guidExistException)
            {
                Person existingPersonGuid = domain.GetPersonByGuid(wsPerson.PersonGuid);
                WSPerson existingWSPerson = new WSPerson();
                existingWSPerson.MapFromDomain(existingPersonGuid);

                return GetJsonResult(new { isGuidExistException = true, existingWSPerson });
            }
            // Guid was unique, meaning new person. Checking if the UserName is taken by another user.
            catch (UserNameExistException personExistException)
            {
                Person existingPersonUserName = domain.GetPersonByUserName(wsPerson.UserName);
                WSPerson existingWSPerson = new WSPerson();
                existingWSPerson.MapFromDomain(existingPersonUserName);

                return GetJsonResult(new { isUserNameExistException = true, existingWSPerson });
            }

            return GetJsonResult(new { wsPerson });
        }

        public JsonResult GetPersonByGuid(UserCredentials user, Guid personGuid)
        {
            if (!IsAuthenticated(user))
            {
                return GetJsonResult(new { isAuthenticationFailure = true });
            }

            DomainRepository domain = new DomainRepository();
            Person person = domain.GetPersonByGuid(personGuid);
            if (person == null)
            {
                return GetJsonResult(new { personNotFound = true });
            }

            WSPerson wsPerson = new WSPerson();
            wsPerson.MapFromDomain(person);

            return GetJsonResult(new { wsPerson });
        }

        /// <summary>
        /// Function will be called the first time the user installs the application
        /// </summary>
        /// <param name="user"></param>
        /// <param name="wsPerson"></param>
        /// <returns></returns>
        public JsonResult CreateNewLoginUser(UserCredentials user, WSPerson wsPerson)
        {
            if (!IsAuthenticated(user))
            {
                return GetJsonResult(new { isAuthenticationFailure = true });
            }


            DomainRepository domain = new DomainRepository();
            Person person = wsPerson.MapToDomain();
            person.SetPassword(user.Password);

            try
            {
                domain.AddPerson(person);
            }
            catch (GuidExistException guidExistException)
            {
                Person existingPersonGuid = domain.GetPersonByGuid(wsPerson.PersonGuid);
                WSPerson existingWSPerson = new WSPerson();
                existingWSPerson.MapFromDomain(existingPersonGuid);
                return GetJsonResult(new { isGuidExistException = true, existingWSPerson });
            }
            // Guid was unique, meaning new person. Checking if the UserName is taken by another user.
            catch (UserNameExistException personExistException)
            {
                Person existingPersonUserName = domain.GetPersonByUserName(wsPerson.UserName);
                WSPerson existingWSPerson = new WSPerson();
                existingWSPerson.MapFromDomain(existingPersonUserName);

                return GetJsonResult(new { isUserNameExistException = true, existingWSPerson });
            }

            return GetJsonResult(new { wsPerson });
        }

        
        public JsonResult ChangePasswordOnLoginUser(UserCredentials user, String newPassword)
        {
            if (!IsAuthenticated(user))
            {
                return GetJsonResult(new { isAuthenticationFailure = true });
            }

            DomainRepository domain = new DomainRepository();
            Person person = domain.GetPersonByGuid(user.PersonGuid);
            person.SetPassword(newPassword);
            domain.SaveChanges();
            
            return GetJsonResult(new { isSaved = true });
        }


    }
}
