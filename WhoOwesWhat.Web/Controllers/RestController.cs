using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using WhoOwesWhat.Domain.ApplicationServices;
using WhoOwesWhat.Domain.Exceptions;
using WhoOwesWhat.Domain.RestClasses;

namespace WhoOwesWhat.Web.Controllers
{
    [ActionAuthorizationAttribute]
    public class RestController : Controller
    {
        public JsonResult GetPersonByGuid(UserCredentials user, Guid personGuid)
        {
            PersonControllerService service = new PersonControllerService();
            return service.GetPersonByGuid(user, personGuid);
        }

        public JsonResult CreateNewLoginUser(UserCredentials user, WSPerson wsPerson)
        {
            PersonControllerService service = new PersonControllerService();
            return service.CreateNewLoginUser(user, wsPerson);
        }

        public JsonResult CreateNewPerson(UserCredentials user, WSPerson wsPerson)
        {
            PersonControllerService service = new PersonControllerService();
            return service.CreateNewPerson(user, wsPerson);
        }

        public JsonResult SynchronizePerson(UserCredentials user, WSPerson wsPerson)
        {
            PersonControllerService controllerService = new PersonControllerService();
            return controllerService.SynchronizePerson(user, wsPerson);

        }
    }
}
