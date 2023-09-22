using System;

namespace WhoOwesWhat.Domain.Exceptions
{
    public class BaseException : Exception
    {
        public String CustomErrorMessage { get; set; }

        public WSException GetWSException()
        {
            WSException e = new WSException();
            e.Message = this.Message;
            if (InnerException != null)
            {
                e.InnerExceptionMessage = InnerException.Message;
                e.InnerExceptionStackTrace = InnerException.StackTrace;
            }
            
            e.StackTrace = this.StackTrace;
            e.CustomErrorMessage = this.CustomErrorMessage;
            return e;
        }

    }
}