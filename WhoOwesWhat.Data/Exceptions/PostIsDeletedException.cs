using System;

namespace WhoOwesWhat.Domain.Exceptions
{
    public class PostIsDeletedException : BaseException
    {
        public PostIsDeletedException()
        {

        }

        public PostIsDeletedException(string message)
        {
            CustomErrorMessage = message;
        }
    }
}