using System;

namespace WhoOwesWhat.Domain.RestClasses
{
    public class UserCredentials
    {
        public Guid PersonGuid { get; set; }
        public string Password { get; set; }
    }
}