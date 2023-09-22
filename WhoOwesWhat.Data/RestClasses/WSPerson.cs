using System;

namespace WhoOwesWhat.Domain.RestClasses
{
    public class WSPerson
    {        
        public Guid PersonGuid { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }

        public Person MapToDomain()
        {
            Person dbPerson = new Person();
            dbPerson.FullName = this.FullName;
            return dbPerson;
        }

        public void MapToDomain(Person person)
        {
            person.FullName = this.FullName;
        }

        public Person MapAllToDomain()
        {
            Person dbPerson = new Person();
            dbPerson.SetUserName(this.UserName);
            dbPerson.FullName = this.FullName;
            dbPerson.SetPersonGuid(this.PersonGuid);

            return dbPerson;
        }

        public void MapFromDomain(Person person)
        {
            this.UserName = person.UserName;
            this.FullName = person.FullName;
            this.PersonGuid = person.PersonGuid;
        }
    }
}