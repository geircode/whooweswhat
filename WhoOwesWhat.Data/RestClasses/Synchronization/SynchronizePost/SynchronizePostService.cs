using System;
using System.Collections.Generic;
using System.Linq;
using WhoOwesWhat.Domain.ApplicationServices;
using WhoOwesWhat.Domain.Exceptions;

namespace WhoOwesWhat.Domain.RestClasses.Synchronization.SynchronizePost
{
    public class SynchronizePostService
    {
        readonly DomainRepository _domain = new DomainRepository();

        public SynchronizePostsResult SynchronizePostsResult(List<WSPost> wsPosts)
        {
            List<Post> latestPosts = _domain.GetAllLatestPosts();
            SynchronizePostsResult result = new SynchronizePostsResult();

            result.DirtyPosts = GetDirtyPosts(latestPosts, wsPosts);
            result.NewPosts = GetNewPosts(latestPosts, wsPosts);
            result.UpdatePosts = GetUpdatePosts(latestPosts, wsPosts);
            result.DeletePosts = _domain.GetDeletedPostsInGuidList(wsPosts);
            int noUpdatePostsCount = GetNoUpdatePostsCount(latestPosts, wsPosts);

            ValidateResult(result, noUpdatePostsCount);

            return result;
        }

        private void ValidateResult(SynchronizePostsResult result, int noUpdatePostsCount)
        {
            int allpostsCount = _domain.GetAllLatestPostsIncludingDeleted().Count;
            int resultPostCount = result.DirtyPosts.Count + result.NewPosts.Count + result.UpdatePosts.Count + result.DeletePosts.Count + noUpdatePostsCount;
            int deletedPostNotOnMobile = _domain.GetDeletedPosts().Count(a => result.DeletePosts.All(b => a.PostGuid != b));

            if (allpostsCount != (resultPostCount + deletedPostNotOnMobile))
            {
                throw new SynchronizePostsResultDiscrepancyException(String.Format("There is a discrepancy in the number of posts that should go to the mobile for synchronization and the posts that actually are sent: Shoud have been ({0}), but was ({1})",allpostsCount, resultPostCount));
            }

        }

        private int GetNoUpdatePostsCount(List<Post> latestPosts, List<WSPost> wsPosts)
        {
            int count = 0;
            var notDirty = wsPosts.Where(a => a.IsDirty == false);
            foreach (WSPost wsPost in notDirty)
            {
                Post updatePost = latestPosts.SingleOrDefault(a => a.PostGuid == wsPost.PostGuid && a.Version == wsPost.Version);
                if(updatePost != null)
                {
                    count++;
                }
            }
            return count;
        }

        private List<WSPost> GetUpdatePosts(List<Post> allPosts, List<WSPost> wsPosts)
        {
            List<WSPost> updateWSPosts = new List<WSPost>();
            var notDirty = wsPosts.Where(a => a.IsDirty == false);
            foreach (WSPost wsPost in notDirty)
            {
                var updatePost = allPosts.SingleOrDefault(a => a.PostGuid == wsPost.PostGuid && a.Version > wsPost.Version);
                if (IsPostNotDeleted(updatePost))
                {
                    updateWSPosts.Add(WSPost.CopyFromDomain(updatePost));
                }

            }
            return updateWSPosts;
        }

        private List<WSPost> GetNewPosts(List<Post> allPosts, List<WSPost> wsPosts)
        {
            List<WSPost> newWSPosts = new List<WSPost>();

            var newPosts = allPosts.Where(a => wsPosts.All(b => a.PostGuid != b.PostGuid));

            foreach (var post in newPosts)
            {
                newWSPosts.Add(WSPost.CopyFromDomain(post));
            }

            return newWSPosts;
        }

        private List<DirtyPost> GetDirtyPosts(List<Post> allPosts, List<WSPost> wsPosts)
        {
            List<WSPost> dirtyWSPosts = wsPosts.Where(a => a.IsDirty && a.Version > 0).ToList();

            List<DirtyPost> dirtyPosts = new List<DirtyPost>();

            foreach (WSPost mobilePost in dirtyWSPosts)
            {
                DirtyPost dp = new DirtyPost();
                dp.MobilePost = mobilePost;

                Post serverPost = allPosts.SingleOrDefault(a => a.PostGuid == mobilePost.PostGuid);
                if (IsPostNotDeleted(serverPost))
                {
                    dp.ServerPost = WSPost.CopyFromDomain(serverPost);
                    dp.Differences = GetDifferences(serverPost, mobilePost);
                    dirtyPosts.Add(dp);
                }

            }
            return dirtyPosts;
        }

        private bool IsPostNotDeleted(Post serverPost)
        {
            return serverPost != null;
        }

        private List<enumPostDifference> GetDifferences(Post serverPost, WSPost mobilePost)
        {
            List<enumPostDifference> diff = new List<enumPostDifference>();

            if (!Post.NearlyEqual(serverPost.TotalCost, mobilePost.TotalCost, 0.01f))
            {
                diff.Add(enumPostDifference.TotalCost);
            }

            if (!serverPost.Description.Equals(mobilePost.Description))
            {
                diff.Add(enumPostDifference.Description);
            }


            if (!serverPost.Comment.Equals(mobilePost.Comment))
            {
                diff.Add(enumPostDifference.Comment);
            }

            if (serverPost.Group.GroupGuid != mobilePost.Group.GroupGuid)
            {
                diff.Add(enumPostDifference.GroupGuid);
            }

            if (!(serverPost.ISO4217CurrencyCode.Equals(mobilePost.ISO4217CurrencyCode)))
            {
                diff.Add(enumPostDifference.ISO4217CurrencyCode);
            }

            if (AreDifferentConsumers(serverPost.Consumers.ToList(), mobilePost.WSConsumers))
            {
                diff.Add(enumPostDifference.Consumers);
            }

            if (AreDifferentPayers(serverPost.Payers.ToList(), mobilePost.WSPayers))
            {
                diff.Add(enumPostDifference.Payers);
            }

            return diff;

        }

        private bool AreDifferentConsumers(List<Consumer> consumers, WSConsumer[] wsConsumers)
        {
            if (consumers.Count != wsConsumers.Length)
            {
                return true;
            }

            foreach (Consumer consumer in consumers)
            {
                if (wsConsumers.All(a => a.PersonGuid != consumer.Person.PersonGuid))
                {
                    return true;
                }
            }
            return false;
        }


        private bool AreDifferentPayers(List<Payer> payers, WSPayer[] wsPayers)
        {
            if (payers.Count != wsPayers.Length)
            {
                return true;
            }

            foreach (Payer payer in payers)
            {
                if (wsPayers.All(a => a.PersonGuid != payer.Person.PersonGuid))
                {
                    return true;
                }
            }
            return false;
        }


        public void SynchronizeWithServer(List<WSPost> wsPosts)
        {
            List<Post> posts = _domain.GetAllLatestPosts();

            ValidateWSPosts(wsPosts);
            List<WSPost> newWSPosts = wsPosts.Where(a => a.Version == 0).ToList();


            IEnumerable<WSPost> dirty = wsPosts.Where(a => a.IsDirty && a.Version > 0);
            foreach (WSPost wsPost in dirty)
            {
                Post updatePost = posts.SingleOrDefault(a => a.PostGuid == wsPost.PostGuid && a.Version == wsPost.Version);

                if (updatePost != null)
                {
                    //create new version on server with mobile version
                    CreateOrUpdatePost(wsPost);
                }
            }

            foreach (WSPost wsPost in newWSPosts)
            {
                CreateOrUpdatePost(wsPost);
            }
        }

        private void CreateOrUpdatePost(WSPost wsPost)
        {
            Post newPost = WSPost.MapToDomainWithoutCustomersAndGroup(wsPost);

            foreach (WSConsumer wsConsumer in wsPost.WSConsumers)
            {
                Person consumerPerson = _domain.GetPersonByGuid(wsConsumer.PersonGuid);
                Consumer consumer = new Consumer(consumerPerson);
                WSCustomer.MapToDomain(consumer, wsConsumer);
                newPost.Consumers.Add(consumer);
            }

            foreach (WSPayer wsPayer in wsPost.WSPayers)
            {
                Person payerPerson = _domain.GetPersonByGuid(wsPayer.PersonGuid);
                Payer payer = new Payer(payerPerson);
                WSCustomer.MapToDomain(payer, wsPayer);
                newPost.Payers.Add(payer);
            }

            if (wsPost.Group != null)
            {
                Group existingGroup = _domain.GetGroupByGuid(wsPost.Group.GroupGuid);
                WSGroup.MapToDomain(existingGroup, wsPost.Group);
                newPost.SetInternalGroup(existingGroup);
            }

            // this marks it for update on mobile
            wsPost.IsDirty = false;

            _domain.AddPost(newPost);
        }

        private void ValidateWSPosts(List<WSPost> wsPosts)
        {
            DomainRepository domain = new DomainRepository();
            foreach (WSPost wsPost in wsPosts)
            {
                if (wsPost.Version > 0)
                {
                    // check if the post is really there

                    List<Post> posts = domain.GetPostByGuidWithAllVersions(wsPost.PostGuid);
                    if (posts.Count == 0)
                    {
                        throw new PostNotFoundException("Error when validating mobile post. The post should have been on the server, but was not. Tried to find PostGuid: " + wsPost.PostGuid);
                    }
                    domain.GetPostVersion(wsPost.PostGuid, wsPost.Version);
                }
            }
        }
    }
}
