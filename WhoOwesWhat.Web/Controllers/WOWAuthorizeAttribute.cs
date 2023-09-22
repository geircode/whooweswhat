using System.Web;
using System.Web.Mvc;

namespace WhoOwesWhat.Web.Controllers
{
    public class WOWAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            if (filterContext.IsChildAction)
            {
                return;
            }

            HttpContextBase context = filterContext.HttpContext;

        }
    }
}