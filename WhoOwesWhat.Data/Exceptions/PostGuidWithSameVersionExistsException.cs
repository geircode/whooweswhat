using System;

namespace WhoOwesWhat.Domain.Exceptions
{
    public class PostGuidWithSameVersionExistsException : BaseException
    {
        public PostGuidWithSameVersionExistsException()
        {

        }

        public PostGuidWithSameVersionExistsException(string message)
        {
            CustomErrorMessage = message;
        }
    }
}