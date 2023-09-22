using System.Collections.Generic;
using System.Web.Mvc;
using WhoOwesWhat.Domain.Exceptions;
using WhoOwesWhat.Domain.RestClasses;
using WhoOwesWhat.Domain.RestClasses.Synchronization.SynchronizePost;

namespace WhoOwesWhat.Domain.ApplicationServices
{
    public class PostControllerService : ControllerServiceBase
    {
        public PostControllerService()
        {

        }

        public JsonResult SynchronizePosts(UserCredentials user, List<WSPost> wsPosts)
        {
            if (!IsAuthenticated(user))
            {
                return GetJsonResult(new { isAuthenticationFailure = true });
            }

            SynchronizePostService service = new SynchronizePostService();

            try
            {
                service.SynchronizeWithServer(wsPosts);
            }
            catch (PostNotFoundException postNotFoundException)
            {
                return GetJsonResult(new { postNotFoundWSException = postNotFoundException.GetWSException() });
            }
            catch (NoSuchPostVersionException e1)
            {
                return GetJsonResult(new { noSuchPostVersionWSException = e1.GetWSException() });
            }
            catch (PostGuidWithSameVersionExistsException e2)
            {
                return GetJsonResult(new { postGuidWithSameVersionExistsWSException = e2.GetWSException() });
            }
            catch (SynchronizePostsResultDiscrepancyException e3)
            {
                return GetJsonResult(new { synchronizePostsResultDiscrepancyException = e3.GetWSException() });
            }
            SynchronizePostsResult result = service.SynchronizePostsResult(wsPosts);
            var json = GetJsonResult(new { synchronizePostsResult = result });
            return json;
        }

    }
}
