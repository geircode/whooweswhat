using System.Collections.Generic;

namespace WhoOwesWhat.Domain.RestClasses.Synchronization.SynchronizePost
{
    public class DirtyPost
    {
        public WSPost ServerPost;
        public WSPost MobilePost;
        public List<enumPostDifference> Differences { get; set; }
    }
}