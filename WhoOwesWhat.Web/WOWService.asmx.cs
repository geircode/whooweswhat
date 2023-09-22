using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using WhoOwesWhat.Domain;
using WhoOwesWhat.Domain.RestClasses;
using WhoOwesWhat.Domain.RestClasses.Synchronization;

namespace WhoOwesWhat.Web
{
    /// <summary>
    /// Summary description for WOWService
    /// </summary>
    [WebService(Namespace = "http://WhoOwesWhat.no/WOWServices")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class WOWService : WebService
    {
        public UserCredentials user;

        private bool IsAuthenticated()
        {
            // In this method you can check the username and password 
            // with your database or something
            // You could also encrypt the password for more security
            if (user != null)
            {
                DomainRepository domain = new DomainRepository();
                bool isAuthenticated = domain.Authenticate(user.PersonGuid, user.Password);
                return isAuthenticated;
            }
            return false;
        }

        private void MustBeAuthenticated()
        {
            if (!IsAuthenticated())
            {
                throw new AuthenticationException("User did not Authenticate");
            }
        }



        private Person GetLoggedOnUser()
        {
            DomainRepository domain = new DomainRepository();
            Person person = domain.GetPersonByGuid(user.PersonGuid);
            return person;
        }


        [WebMethod]
        [SoapHeader("user")]
        public Person CreateNewUser(WSPerson wsPerson)
        {
            MustBeAuthenticated();

            Person dbPerson = wsPerson.MapToDomain();
            // Try to create new Person.
            // Guid must be new
            // Name must be unique
            DomainRepository domain = new DomainRepository();
            domain.AddPerson(dbPerson);
            return dbPerson;
        }

        //[WebMethod]
        //[SoapHeader("user")]
        //public bool ChangePassword(string newPassword)
        //{
        //    if (IsAuthenticated())
        //    {
        //        Person person = GetLoggedOnUser();
        //        person.SetPassword(newPassword);
        //        return true;
        //    }
        //    return false;
        //}

        [WebMethod]
        [SoapHeader("user")]
        public SynchronizePersonResult SynchronizePersons(List<WSPerson> wsPersons)
        {
            SynchronizePersonResult result = new SynchronizePersonResult();
            //if (IsAuthenticated())
            //{
            //    DomainRepository domain = new DomainRepository();
            //    List<Person> allPersons = domain.GetAllPersons();
            //    foreach (WSPerson wsPerson in wsPersons)
            //    {
            //        result = SynchronizePersonService.SynchronizePersonResult(allPersons, wsPerson);
            //    }
            //}
            return result;
        }

    }

}
