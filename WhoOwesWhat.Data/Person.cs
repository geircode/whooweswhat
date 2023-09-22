using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using WhoOwesWhat.Domain;

namespace WhoOwesWhat.Domain
{
    public partial class Person
    {

        private const string DEFAULT_PASSWORD = "wow";

        public Person()
        {
            this.PersonGuid = Guid.NewGuid();
            this.SetPassword(DEFAULT_PASSWORD);
        }

        public Person(string userName) :this()
        {
            this.UserName = userName;            
        }

        public void SetUserName(String userName)
        {
            UserName = userName;
        }

        public String GetUserName()
        {
            return UserName;
        }

        public void SetPersonGuid(Guid personGuid)
        {
            PersonGuid = personGuid;
        }

        public Guid GetPersonGuid()
        {
            return PersonGuid;
        }

        public void SetPassword(string password)
        {
            if(password == null)
            {
                throw new Exception("The password cannot be null");
            }
            byte[] passwordSalt = CreateSalt(10);
            byte[] passwordHash = Hash(password, passwordSalt);

            this.PasswordHash = Convert.ToBase64String(passwordHash);
            this.PasswordSalt = Convert.ToBase64String(passwordSalt);
        }

        public bool IsPasswordSet { get { return String.IsNullOrEmpty(PasswordHash); } }
        public bool IsALogonUser { get { return String.IsNullOrEmpty(PasswordHash); } }

        public static byte[] Hash(string value, byte[] salt)
        {
            return Hash(Encoding.UTF8.GetBytes(value), salt);
        }

        public static byte[] Hash(byte[] value, byte[] salt)
        {
            byte[] saltedValue = value.Concat(salt).ToArray();

            return new SHA256Managed().ComputeHash(saltedValue);
        }

        public bool ConfirmPassword(string password)
        {
            byte[] dbPasswordHash = Convert.FromBase64String(this.PasswordHash);
            byte[] dbPasswordSalt = Convert.FromBase64String(this.PasswordSalt);
            byte[] passwordHashToCheck = Hash(password, dbPasswordSalt);
            return dbPasswordHash.SequenceEqual(passwordHashToCheck);
        }

        private static byte[] CreateSalt(int size)
        {
            //Generate a cryptographic random number.
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buff = new byte[size];
            rng.GetBytes(buff);
            return buff;
            
        }


    }

}
