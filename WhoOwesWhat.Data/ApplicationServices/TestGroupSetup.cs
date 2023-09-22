using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhoOwesWhat.Domain.ApplicationServices
{
    public static class TestGroupSetup
    {
        public static Group Test01_Original_GroupGuidExistOnMobileNotOnServer_NameExistOnMobileNotOnServer()
        {
            Group p = new Group();
            p.Name = "GroupGuidExistOnMobileNotOnServer_NameExistOnMobileNotOnServer";
            p.GroupGuid = Guid.Parse("b6737732-ba0a-4d41-976b-11d66dd833ae");
            return p;
        }

        public static Group Test02_Original_GroupGuidExistOnMobileNotOnServer_NameExistOnMobileAndOnServer()
        {
            Group p = new Group();
            p.Name = "Eon";
            p.GroupGuid = Guid.Parse("5cc525c6-0c55-44f4-8a19-c45311315041");
            return p;
        }

        public static Group Test03_Original_GroupGuidExistOnMobileAndOnServer_NameExistOnMobileNotOnServer()
        {
            Group p = new Group();
            p.Name = "GroupGuidExistOnMobileAndOnServer_NameExistOnMobileNotOnServer";
            p.GroupGuid = Guid.Parse("7e9e7010-e1b4-42f8-9741-76ecdbfd1f67");
            return p;
        }

        public static Group Test04_Original_GroupGuidExistOnMobileAndOnServer_NameExistOnMobileAndOnServer()
        {
            Group p = new Group();
            p.Name = "GroupGuidExistOnMobileAndOnServer_NameExistOnMobileAndOnServer";
            p.GroupGuid = Guid.Parse("2204cacd-332d-45f6-9d87-19418a1f24ee");
            return p;
        }

        public static Group Test02_Server_GroupGuidExistOnMobileNotOnServer_NameExistOnMobileAndOnServer()
        {
            Group p = new Group();
            p.Name = "Eon";
            p.GroupGuid = Guid.Parse("55586202-f8e0-4442-9251-4df72418f232");
            return p;
        }

        public static Group Test03_Server_GroupGuidExistOnMobileAndOnServer_NameExistOnMobileNotOnServer()
        {
            Group p = new Group();
            p.Name = "Gonzales_NotOnServer";
            p.GroupGuid = Guid.Parse("7e9e7010-e1b4-42f8-9741-76ecdbfd1f67");
            return p;
        }

        public static Group Post1Group_BlueHawaii()
        {
            Group p = new Group();
            p.Name = "Blue Hawaii";
            p.GroupGuid = Guid.Parse("4c65338d-ac7d-48ec-b46e-5490163d2dbb");
            return p;
        }

        public static Group Post2Group_FellesTing()
        {
            Group p = new Group();
            p.Name = "FellesTing";
            p.GroupGuid = Guid.Parse("7947AA25-BDC4-4B22-AAF0-D413CF015939");
            return p;
        }



    }
}
