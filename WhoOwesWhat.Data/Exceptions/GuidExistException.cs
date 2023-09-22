using System;

namespace WhoOwesWhat.Domain.Exceptions
{
    public class GuidExistException : Exception
    {
        public GuidExistException()
        {
            //throw new Exception("Another person with the same PersonGuid exist");
        }
    }
}