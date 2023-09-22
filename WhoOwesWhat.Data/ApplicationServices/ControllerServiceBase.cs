using System.Collections.Generic;
using System.Web.Mvc;
using WhoOwesWhat.Domain.Exceptions;
using WhoOwesWhat.Domain.RestClasses;
using WhoOwesWhat.Domain.RestClasses.Synchronization;

namespace WhoOwesWhat.Domain.ApplicationServices
{
    public class ControllerServiceBase
    {
        public ControllerServiceBase()
        {
           
        }
    
        protected JsonResult GetJsonResult(object data)
        {
            JsonResult result = new JsonResult() { JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            result.Data = data;
            return result;
        }

        protected bool IsAuthenticated(UserCredentials user)
        {
            if (user != null)
            {
                DomainRepository domain = new DomainRepository();
                bool isAuthenticated = domain.Authenticate(user.PersonGuid, user.Password);
                return isAuthenticated;
            }
            return false;
        }

        public JsonResult CheckUserAuthenticated(UserCredentials user)
        {
            if (!IsAuthenticated(user))
            {
                return GetJsonResult(new { isAuthenticationFailure = true });
            }
            return GetJsonResult(new { isAuthenticationFailure = false });
        }
    }
}
