using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhoOwesWhat.Domain.ApplicationServices
{
    public static class TestPersonSetup
    {
        //PersonGuid Finnes på mobil, Finnes ikke på server. UserName Finnes på mobil, Finnes ikke på server
        //FullName: PersonGuidExistOnMobileNotOnServer_UserNameExistOnMobileNotOnServer
        //UserName: Tetra
        //PersonGuid: 1b5705ec-a81c-43fa-8a68-f801d405dff1

        //PersonGuid Finnes på mobil, Finnes ikke på server. UserName Finnes på server, Finnes på mobil
        //FullName: PersonGuidExistOnMobileNotOnServer_UserNameExistOnMobileAndOnServer
        //UserName: Eon
        //PersonGuid: 43a281b4-f077-4a3c-b680-0a916e5267de

        //PersonGuid Finnes på mobil, Finnes på server. UserName Finnes på mobil, Finnes ikke på server
        //FullName: PersonGuidExistOnMobileAndOnServer_UserNameExistOnMobileNotOnServer
        //UserName: Gonzales
        //PersonGuid: 4e4187a7-6cfe-44fe-8f9e-28983aa2199e

        //PersonGuid Finnes på mobil, Finnes på server. UserName Finnes på server, Finnes på mobil
        //FullName: PersonGuidExistOnMobileAndOnServer_UserNameExistOnMobileAndOnServer
        //UserName: Tidsklemma
        //PersonGuid: 60846c00-5f4a-4e7e-b57c-bdb128630121

        public static Person Test01_Original_PersonGuidExistOnMobileNotOnServer_UserNameExistOnMobileNotOnServer()
        {
            Person p = new Person();
            p.FullName = "PersonGuidExistOnMobileNotOnServer_UserNameExistOnMobileNotOnServer";
            p.SetUserName("Tetra");
            p.SetPersonGuid(Guid.Parse("1b5705ec-a81c-43fa-8a68-f801d405dff1"));
            return p;
        }

        public static Person Test02_Original_PersonGuidExistOnMobileNotOnServer_UserNameExistOnMobileAndOnServer()
        {
            Person p = new Person();
            p.FullName = "PersonGuidExistOnMobileNotOnServer_UserNameExistOnMobileAndOnServer";
            p.SetUserName("Eon");
            p.SetPersonGuid(Guid.Parse("43a281b4-f077-4a3c-b680-0a916e5267de"));
            return p;
        }

        public static Person Test03_Original_PersonGuidExistOnMobileAndOnServer_UserNameExistOnMobileNotOnServer()
        {
            Person p = new Person();
            p.FullName = "PersonGuidExistOnMobileAndOnServer_UserNameExistOnMobileNotOnServer";
            p.SetUserName("Gonzales");
            p.SetPersonGuid(Guid.Parse("4e4187a7-6cfe-44fe-8f9e-28983aa2199e"));
            return p;
        }

        public static Person Test04_Original_PersonGuidExistOnMobileAndOnServer_UserNameExistOnMobileAndOnServer()
        {
            Person p = new Person();
            p.FullName = "PersonGuidExistOnMobileAndOnServer_UserNameExistOnMobileAndOnServer";
            p.SetUserName("Tidsklemma");
            p.SetPersonGuid(Guid.Parse("60846c00-5f4a-4e7e-b57c-bdb128630121"));
            return p;
        }

        public static Person Test02_Server_PersonGuidExistOnMobileNotOnServer_UserNameExistOnMobileAndOnServer()
        {
            Person p = new Person();
            p.FullName = "PersonGuidExistOnMobileNotOnServer_UserNameExistOnMobileAndOnServer";
            p.SetUserName("Eon");
            p.SetPersonGuid(Guid.Parse("53068864-263f-44ad-b491-d77f98059fb3"));
            return p;
        }

        public static Person Test03_Server_PersonGuidExistOnMobileAndOnServer_UserNameExistOnMobileNotOnServer()
        {
            Person p = new Person();
            p.FullName = "PersonGuidExistOnMobileAndOnServer_UserNameExistOnMobileNotOnServer";
            p.SetUserName("Gonzales_NotOnServer");
            p.SetPersonGuid(Guid.Parse("4e4187a7-6cfe-44fe-8f9e-28983aa2199e"));
            return p;
        }

        // ***************
        public static Person Post1User_Geir()
        {
            Person p = new Person();
            p.FullName = "Geir";
            p.SetUserName("Geir");
            p.SetPersonGuid(Guid.Parse("7ab65336-ab4a-4926-9492-1e31bf68218e"));
            return p;
        }
        public static Person Post2User_Marianne()
        {
            Person p = new Person();
            p.FullName = "Marianne";
            p.SetUserName("Marianne");
            p.SetPersonGuid(Guid.Parse("64f2148a-1d46-4978-b17b-5e0584614710"));
            return p;
        }
        public static Person Post3User_Victor()
        {
            Person p = new Person();
            p.FullName = "Victor";
            p.SetUserName("Victor");
            p.SetPersonGuid(Guid.Parse("d42d13c0-5fbe-407c-b970-03c84b1198e1"));
            return p;
        }
        public static Person Post4User_Olav()
        {
            Person p = new Person();
            p.FullName = "Olav";
            p.SetUserName("Olav");
            p.SetPersonGuid(Guid.Parse("205d8e07-b18a-41f9-ad56-572d36553913"));
            return p;
        }
        public static Person Post5User_Beate()
        {
            Person p = new Person();
            p.FullName = "Beate";
            p.SetUserName("Beate");
            p.SetPersonGuid(Guid.Parse("3ce39e98-c6e5-482d-b776-455239d0b1c7"));
            return p;
        }


    }
}
