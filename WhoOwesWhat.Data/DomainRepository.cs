using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Text;
using WhoOwesWhat.Domain;
using WhoOwesWhat.Domain.ApplicationServices;
using WhoOwesWhat.Domain.Exceptions;
using WhoOwesWhat.Domain.RestClasses;

namespace WhoOwesWhat.Domain
{
    public class DomainRepository
    {
        public static void SetEntityConnectionString(string entityConnectionString)
        {
            _connectionString = entityConnectionString;
        }

        private static string _connectionString = null;

        private readonly WoWModelContainer _model;
        public DomainRepository()
        {
            if (_connectionString == null)
            {
                _model = new WoWModelContainer();
            }
            else
            {
                _model = new WoWModelContainer(_connectionString);
            }
            _model.CommandTimeout = 1500;
        }


        public ObjectStateManager GetObjectStateManager()
        {
            return _model.ObjectStateManager;
        }


        protected DomainRepository(String entityConnectionString)
        {
            SetEntityConnectionString(entityConnectionString);

            _model = new WoWModelContainer(entityConnectionString);
        }

        public void ResetDatabaseAndDomain()
        {
            if (_model.DatabaseExists())
            {
                _model.DeleteDatabase();
            }

            _model.CreateDatabase();
        }


        public Person AddOrGetPerson(Person person)
        {
            if (PersonGuidExists(person.PersonGuid))
            {
                person = GetPersonByGuid(person.PersonGuid);
            }
            else
            {
                person = AddPerson(person);
            }
            return person;
        }

        public Group AddOrGetGroup(Group grp)
        {
            if (GroupGuidExists(grp.GroupGuid))
            {
                grp = GetGroupByGuid(grp.GroupGuid);
            }
            else
            {
                grp = AddGroup(grp);
            }
            return grp;
        }

        public Group GetGroupByGuid(Guid groupGuid)
        {
            Group group = _model.GroupSet.SingleOrDefault(a => a.GroupGuid == groupGuid);
            return group;
        }


        public bool PersonGuidExists(Guid personGuid)
        {
            bool guidExist = _model.PersonSet.Any(a => a.PersonGuid == personGuid);
            return guidExist;
        }


        public List<Person> GetAllPersons()
        {
            List<Person> persons = _model.PersonSet.ToList();
            return persons;
        }


        public Person AddPerson(Person person)
        {
            // Guid must be new
            // Name must be unique
            bool personNameExist = _model.PersonSet.Any(a => a.UserName == person.UserName);
            bool guidExist = PersonGuidExists(person.PersonGuid);

            if (guidExist)
            {
                throw new GuidExistException();
            }
            if (personNameExist)
            {
                throw new UserNameExistException();
            }

            _model.PersonSet.AddObject(person);
            _model.SaveChanges();
            return person;
        }

        public void DeletePerson(int personId)
        {
            Person personToDelete = _model.PersonSet.Single(a => a.PersonId == personId);
            _model.PersonSet.DeleteObject(personToDelete);
            _model.SaveChanges();
        }

        public void DeletePerson(Person person)
        {
            DeletePerson(person.PersonId);
        }

        public Post GetLatestPost(Guid postGuid)
        {
            Post latestPost = _model.PostSet.Where(a => a.PostGuid == postGuid).OrderByDescending(a => a.Version).FirstOrDefault();
            return latestPost;
        }


        public Post GetPostVersion(Guid postGuid, int version)
        {
            Post post = _model.PostSet.SingleOrDefault(a => a.PostGuid == postGuid && a.Version == version);
            if (post == null)
            {
                throw new NoSuchPostVersionException("No posts with that version was found");
            }
            return post;
        }


        public bool Authenticate(Guid personGuid, string password)
        {
            Person person = _model.PersonSet.SingleOrDefault(a => a.PersonGuid == personGuid);
            if (person == null)
            {
                throw new Exception("No such person");
            }
            var isAuthenticated = person.ConfirmPassword(password);
            return isAuthenticated;

        }

        /// <summary>
        /// Return null if not found
        /// </summary>
        /// <param name="personGuid"></param>
        /// <returns></returns>
        public Person GetPersonByGuid(Guid personGuid)
        {
            Person person = _model.PersonSet.SingleOrDefault(a => a.PersonGuid == personGuid);
            return person;
        }

        public Person GetPersonByUserName(string userName)
        {
            Person person = _model.PersonSet.SingleOrDefault(a => a.UserName == userName);
            return person;
        }

        public Group AddGroup(Group newGroup)
        {
            // Guid must be new
            // Name must be unique
            bool nameExist = _model.GroupSet.Any(a => a.Name.Equals(newGroup.Name));
            bool guidExist = GroupGuidExists(newGroup.GroupGuid);

            if (guidExist)
            {
                throw new GuidExistException();
            }
            if (nameExist)
            {
                throw new GroupNameExistException();
            }

            _model.GroupSet.AddObject(newGroup);
            _model.SaveChanges();
            return newGroup;
        }


        //public void AddPost(Post newPost)
        //{
        //    if (PostGuidAndVersionExists(newPost))
        //    {
        //        throw new PostGuidWithSameVersionExistsException("");
        //    }
        //   _model.PostSet.AddObject(newPost);
        //   _model.SaveChanges();
        //}

        public void AddPost(Post post)
        {
            if (post.IsDeleted)
            {
                throw new PostIsDeletedException("Error trying to add a Post that has been marked for deletion.");
            }

            if (post.GetInternalGroup() != null)
            {
                Group group = AddOrGetGroup(post.GetInternalGroup());
                group.Posts.Add(post);
            }

            if (post.Version == 0)
            {
                post.SetVersion(1);
            }
            else
            {
                if (!PostGuidAndVersionExists(post))
                {
                    if (!PostGuidExists(post))
                    {
                        throw new PostNotFoundException("Unable to find Post by guid");
                    }
                    throw new NoSuchPostVersionException("Unable to find version to Post");
                }

                post.SetVersion(post.Version + 1);
                if (PostGuidAndVersionExists(post))
                {
                    throw new Exception("Unable to upgrade Post to new version because another version already exist.");
                }
                _model.PostSet.AddObject(post);
            }

            _model.SaveChanges();
        }


        //public void AddNewOrAddCloneOfPost(Post post)
        //{

        //    if(post.Version == 0)
        //    {
        //        post.SetVersion(1);
        //    }
        //    else
        //    {
        //        if (!PostGuidAndVersionExists(post))
        //        {
        //            if(!PostGuidExists(post))
        //            {
        //                throw new PostNotFoundException("Unable to find Post by guid");
        //            }
        //            throw new NoSuchPostVersionException("Unable to find version to Post");
        //        }
        //        Post clone = Post.Clone(post);
        //        clone.SetVersion(clone.Version + 1);
        //        if (PostGuidAndVersionExists(clone))
        //        {
        //            throw new Exception("Unable to upgrade Post to new version because another version already exist.");
        //        }
        //        _model.PostSet.AddObject(clone);
        //    }

        //    _model.SaveChanges();
        //}

        private bool PostGuidExists(Post post)
        {
            bool guidExist = _model.PostSet.Any(a => a.PostGuid == post.PostGuid);
            return guidExist;
        }

        private bool PostGuidAndVersionExists(Post post)
        {
            bool guidExist = _model.PostSet.Any(a => a.PostGuid == post.PostGuid && a.Version == post.Version);
            return guidExist;
        }

        private bool GroupGuidExists(Guid groupGuid)
        {
            var list = _model.GroupSet.ToList();
            bool guidExist = _model.GroupSet.Any(a => a.GroupGuid == groupGuid);
            return guidExist;
        }


        public void SaveChanges()
        {
            _model.SaveChanges();
        }

        public void AcceptAllChanges()
        {
            _model.AcceptAllChanges();
        }


        public Dictionary<EntityState, List<ObjectStateEntry>> GetGetObjectStateManagerEntries()
        {
            Dictionary<EntityState, List<ObjectStateEntry>> map = new Dictionary<EntityState, List<ObjectStateEntry>>();
            var state = _model.ObjectStateManager;
            map.Add(EntityState.Added, state.GetObjectStateEntries(EntityState.Added).ToList());
            map.Add(EntityState.Deleted, state.GetObjectStateEntries(EntityState.Deleted).ToList());
            map.Add(EntityState.Modified, state.GetObjectStateEntries(EntityState.Modified).ToList());
            map.Add(EntityState.Unchanged, state.GetObjectStateEntries(EntityState.Unchanged).ToList());
            return map;
        }


        public List<Group> GetAllGroups()
        {
            return _model.GroupSet.ToList();
        }

        public List<Post> GetAllPostsWithAllVersions()
        {
            return _model.PostSet.ToList();
        }

        public List<Post> GetAllLatestPostsIncludingDeleted()
        {
            List<Post> latestPosts = new List<Post>();
            var postGuids = _model.PostSet.Select(a => a.PostGuid).Distinct();
            foreach (Guid postGuid in postGuids)
            {
                latestPosts.Add(GetLatestPost(postGuid));
            }
            return latestPosts;
        }

        public List<Post> GetAllLatestPosts()
        {
            List<Post> latestPosts = new List<Post>();
            var postGuids = GetNotDeletedPosts().Select(a => a.PostGuid);
            foreach (Guid postGuid in postGuids)
            {
                latestPosts.Add(GetLatestPost(postGuid));
            }
            return latestPosts;
        }

        public List<Post> GetNotDeletedPosts()
        {
            List<Post> notDeletedPosts = new List<Post>();
            foreach (Post post in _model.PostSet)
            {
                if (!IsPostDeleted(post))
                {
                    if (notDeletedPosts.All(a => a.PostGuid != post.PostGuid))
                    {
                        notDeletedPosts.Add(post);
                    }
                }
            }
            return notDeletedPosts;
        }

        public List<Post> GetDeletedPosts()
        {
            List<Post> deletedPosts = new List<Post>();
            foreach (Post post in _model.PostSet)
            {
                if (IsPostDeleted(post))
                {
                    if (deletedPosts.All(a => a.PostGuid != post.PostGuid))
                    {
                        deletedPosts.Add(post);
                    }
                }
            }
            return deletedPosts;
        }

        public List<Guid> GetDeletedPostsInGuidList(List<WSPost> wsPosts)
        {
            return GetDeletePostsAndFilterOnExistingPostsOnMobile(wsPosts).Select(a => a.PostGuid).Distinct().ToList();
        }

        public List<Post> GetDeletePostsAndFilterOnExistingPostsOnMobile(List<WSPost> wsPosts)
        {
            return GetDeletedPosts().Where(a => wsPosts.Any(b => a.PostGuid.Equals(b.PostGuid))).ToList();
        }

        public bool IsPostDeleted(Post post)
        {
            List<Post> latestPosts = GetAllLatestPostsIncludingDeleted();
            bool isDeleted = latestPosts.Single(a => a.PostGuid == post.PostGuid).IsDeleted;
            return isDeleted;
        }

        public List<Post> GetPostByGuidWithAllVersions(Guid postGuid)
        {
            return _model.PostSet.Where(a => a.PostGuid == postGuid).ToList();
        }


        public Post GetPostByGuidAndVersion(Guid postGuid, int version)
        {
            return _model.PostSet.SingleOrDefault(a => a.PostGuid == postGuid && a.Version == version);
        }

        public void DeletePost(Post post)
        {
            if (!PostGuidExists(post))
            {
                throw new PostNotFoundException("Error trying to delete a Post. It was not found.");
            }
            if (!PostGuidAndVersionExists(post))
            {
                throw new NoSuchPostVersionException("Error trying to delete a Post. Post was found, but not the Version.");
            }

            Post latestPost = GetLatestPost(post.PostGuid);
            if (latestPost == null)
            {
                throw new NoSuchPostVersionException("Error deleting Post. The Post's version was not the same as the latest. You can only delete the latest Post.");
            }

            latestPost.IsDeleted = true;
            _model.SaveChanges();
        }
    }
}
