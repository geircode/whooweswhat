using System;

namespace WhoOwesWhat.Domain.Exceptions
{
    public class UserNameExistException : Exception
    {
        public UserNameExistException()
        {
            //throw new Exception("Another person with the same username exist");
        }
    }
}