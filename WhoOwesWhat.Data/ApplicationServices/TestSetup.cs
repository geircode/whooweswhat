using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhoOwesWhat.Domain.ApplicationServices
{
    public static class TestSetup
    {
        private static HashSet<enumTestSetup> TestSetupList = new HashSet<enumTestSetup>();

        public static bool CheckForTestSetup(enumTestSetup testSetup)
        {
            return TestSetupList.Any(a => a == testSetup);
        }

        public static void AddTestSetup(enumTestSetup testSetup)
        {
            TestSetupList.Add(testSetup);
        }




    }

    public enum enumTestSetup
    {
        addTestPostUsers,
        addTestPostGroup,
        addNewPost1ToServer,
        AddPost2WithVersion2ToServer,
        AddPost2WithVersion3ToServer,
        AddPost5_MobileVersion1NotDirty_SynchronizeAgainstServerVersion2ThatIsDeleted
    }
}
