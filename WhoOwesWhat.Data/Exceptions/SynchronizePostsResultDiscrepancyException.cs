using System;

namespace WhoOwesWhat.Domain.Exceptions
{
    public class SynchronizePostsResultDiscrepancyException : BaseException
    {
        public SynchronizePostsResultDiscrepancyException()
        {

        }

        public SynchronizePostsResultDiscrepancyException(string message)
        {
            CustomErrorMessage = message;
        }
    }
}