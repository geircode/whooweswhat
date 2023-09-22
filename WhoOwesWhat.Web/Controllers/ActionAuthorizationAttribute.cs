using System.IO;
using System.Web;
using System.Web.Mvc;
using WhoOwesWhat.Domain;

namespace WhoOwesWhat.Web.Controllers
{
    /// <summary>
    /// This enables the possibility to check Action authorization in ex. Views.
    /// </summary>
    public class ActionAuthorizationAttribute : ActionFilterAttribute
    {

        public ActionDescriptor ExecutingAction { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            DomainRepository domain = new DomainRepository();

            var current = HttpContext.Current;
            Stream inputStream = HttpContext.Current.Request.InputStream;
            StreamReader sr = new StreamReader(inputStream);
            var inputString = sr.ToString();
        }
    }
}