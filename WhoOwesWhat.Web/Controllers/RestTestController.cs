using System;
using System.Collections.Generic;
using System.Data;
using System.Data.EntityClient;
using System.Data.Objects;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WhoOwesWhat.Domain;
using WhoOwesWhat.Domain.ApplicationServices;
using WhoOwesWhat.Domain.RestClasses;

namespace WhoOwesWhat.Web.Controllers
{
    public class RestTestController : Controller
    {


        public JsonResult CreateNewPerson(UserCredentials user, WSPerson wsPerson)
        {
            TestService service = new TestService();
            service.ResetDatabaseAndDomain();
            service.AddTestPersonWithPassword();

            PersonControllerService controllerService = new PersonControllerService();
            return controllerService.CreateNewPerson(user, wsPerson);
        }

        public JsonResult SynchronizePerson(UserCredentials user, WSPerson wsPerson)
        {
            // Setup the database
            TestService service = new TestService();

            PersonControllerService controllerService = new PersonControllerService();
            JsonResult json = controllerService.SynchronizePerson(user, wsPerson);

            JsonResult storedJson = controllerService.GetPersonByGuid(user, wsPerson.PersonGuid);

            return json;
        }       

        public JsonResult ChangePasswordOnLoginUser(UserCredentials user, String newPassword)
        {
            PersonControllerService controllerService = new PersonControllerService();
            return controllerService.ChangePasswordOnLoginUser(user, newPassword);
        }

        public JsonResult CheckUserAuthenticated(UserCredentials user)
        {
            PersonControllerService controllerService = new PersonControllerService();
            return controllerService.CheckUserAuthenticated(user);
        }


        public JsonResult SynchronizeGroups(UserCredentials user, List<WSGroup> groups)
        {
            GroupControllerService controllerService = new GroupControllerService();
            return controllerService.SynchronizeGroups(user, groups);
        }


        public JsonResult SynchronizePosts(UserCredentials user, List<WSPost> posts)
        {
            if(posts == null)
            {
                posts = new List<WSPost>();
            }

            PostControllerService controllerService = new PostControllerService();
            return controllerService.SynchronizePosts(user, posts);
        }

      
        

        //public JsonResult SynchronizePersons(UserCredentials user, WSPerson wsPerson)
        //{
        //    // Setup the database
        //    TestService service = new TestService();

        //    JsonResult json;
        //    using (TransactionScope s = new TransactionScope())
        //    {
        //        PersonControllerService controllerService = new PersonControllerService();
        //        json = controllerService.SynchronizePerson(user, wsPerson);
        //    }

        //    return json;
        //}

        public JsonResult SetupTestEnvironment()
        {
            // Setup the database
            TestService service = new TestService();
            service.ResetDatabaseAndDomain();
            service.AddTestPersonWithPassword();
            service.AddTestSetupToDatabase();
            JsonResult json = new JsonResult();
            json.Data = new { wooot = true };
            return json;
        }



    }



}
