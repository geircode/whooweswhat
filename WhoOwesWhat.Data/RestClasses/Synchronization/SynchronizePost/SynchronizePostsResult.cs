using System;
using System.Collections.Generic;

namespace WhoOwesWhat.Domain.RestClasses.Synchronization.SynchronizePost
{
    public class SynchronizePostsResult
    {
        public List<DirtyPost> DirtyPosts;
        public List<WSPost> NewPosts;
        public List<WSPost> UpdatePosts;
        public List<Guid> DeletePosts;
    }
}