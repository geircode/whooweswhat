using System;

namespace WhoOwesWhat.Domain.Exceptions
{
    public class WSException
    {
        public String InnerExceptionStackTrace { get; set; }
        public String Message { get; set; }
        public String StackTrace { get; set; }
        public String InnerExceptionMessage { get; set; }
        public String CustomErrorMessage { get; set; }
         
    }
}