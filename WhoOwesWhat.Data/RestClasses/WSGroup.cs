using System;

namespace WhoOwesWhat.Domain.RestClasses
{
    public class WSGroup
    {        
        public Guid GroupGuid { get; set; }
        public string Name { get; set; }
       

        public Group MapAllToDomain()
        {
            Group dbGroup = new Group();
            MapAllToDomain(dbGroup);
            return dbGroup;
        }

        public void MapAllToDomain(Group group)
        {
            group.GroupGuid = this.GroupGuid;
            group.Name = this.Name;
        }

        public void MapInfoToDomain(Group group)
        {
            group.Name = this.Name;
        }

        public static WSGroup MapFromDomain(Group grp)
        {
            if (grp == null)
            {
                return null;
            }
            WSGroup group = new WSGroup();
            group.GroupGuid = grp.GroupGuid;
            group.Name = grp.Name;
            return group;
        }


        public static void MapToDomain(Group exitingGroup, WSGroup wsGroup)
        {
            exitingGroup.Name = wsGroup.Name;
        }
    }
}