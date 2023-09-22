using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Text;
using WhoOwesWhat.Domain;
using WhoOwesWhat.Domain.ApplicationServices;

namespace WhoOwesWhat.Domain
{
    public partial class Post
    {

        //public Group GetGroup
        //{
        //    get { return this.Group; }
        //}

        public Post()
        {
            this.PostGuid = Guid.NewGuid();
            this.Date = DateTime.Now;
        }

        public void SetTotalCost(float totalCost)
        {
            this.TotalCost = (float)Math.Round(totalCost, 2);
        }

        public void AddConsumer(Consumer consumer)
        {
            if (Consumers.Any(a => a == consumer))
            {
                throw new Exception("Consumer has already been added");
            }
            Consumers.Add(consumer);
            CalculateAmountBetweenCustomers(Consumers);
        }

        public void RemoveConsumer(Consumer consumer)
        {
            Consumers.Remove(consumer);
            CalculateAmountBetweenCustomers(Consumers);
        }

        public void AddPayer(Payer Payer)
        {
            if (Payers.Any(a => a == Payer))
            {
                throw new Exception("Payer has already been added");
            }
            Payers.Add(Payer);
            CalculateAmountBetweenCustomers(Payers);
        }

        public bool IsLatestPost()
        {
            DomainRepository domain = new DomainRepository();
            bool isLatestPost = domain.GetLatestPost(this.PostGuid).Version == this.Version;
            return isLatestPost;
        }


        public void RemovePayer(Payer Payer)
        {
            Payers.Remove(Payer);
            CalculateAmountBetweenCustomers(Payers);
        }

        protected void CalculateAmountBetweenCustomers(IEnumerable<Customer> customers)
        {
            List<Customer> customerList = customers.ToList();

            List<Customer> customersSetManually = customerList.Where(a => a.AmountIsSetManually).ToList();
            List<Customer> customersNotSetManually = customerList.Where(a => !a.AmountIsSetManually).ToList();

            if (customersNotSetManually.Count == 0)
            {
                return;
            }

            float customersSetManuallyAmount = customersSetManually.Sum(a => a.Amount);
            float AmountToBeDividedBetweenCustomersNotSetManually = this.TotalCost - customersSetManuallyAmount;
            if (AmountToBeDividedBetweenCustomersNotSetManually < 0)
            {
                throw new Exception("AmountToBeDividedBetweenCustomersNotSetManually cannot be negative");
            }

            if (AmountToBeDividedBetweenCustomersNotSetManually > 0)
            {
                float individualAmountPrCustomer = AmountToBeDividedBetweenCustomersNotSetManually / customersNotSetManually.Count();

                // Round off to two decimals. Totalt Amount _must_ be equal Post.TotalCost
                foreach (Customer c in customersNotSetManually)
                {
                    c.Amount = individualAmountPrCustomer;
                }
            }

            float totalAmount = customerList.Sum(a => a.Amount);

            if (!NearlyEqual(TotalCost, totalAmount, 0.01f))
            {
                float diff = TotalCost - totalAmount;
                // Last man in gets to get the rest
                customerList.Last().Amount += diff;
            }         
        }

        public static bool NearlyEqual(float a, float b, float epsilon)
        {
            float absA = Math.Abs(a);
            float absB = Math.Abs(b);
            float diff = Math.Abs(a - b);

            if (a == b)
            { // shortcut, handles infinities
                return true;
            }
            else if (a * b == 0)
            { // a or b or both are zero
                // relative error is not meaningful here
                return diff < (epsilon * epsilon);
            }
            else
            { // use relative error
                return diff / (absA + absB) < epsilon;
            }
        }

        public void SetVersion(int version)
        {
            Version = version;
        }

        //private int PostId;
        //public UUID PostGuid;
        //public Date Date;
        //public String Description;
        //public float TotalCost;
        //public String ISO4217CurrencyCode;
        //public int Version;
        //public Boolean IsDirty = false;
        //public String Comment;
        //public int GroupId;

        public static Post Clone(Post post)
        {
            Post clone = new Post();
            clone.PostGuid = post.PostGuid;
            clone.Date = post.Date;
            clone.Description = post.Description;
            clone.TotalCost = post.TotalCost;
            clone.ISO4217CurrencyCode = post.ISO4217CurrencyCode;
            clone.Version = post.Version;
            clone.Comment = post.Comment;

            clone.Group = post.Group;

            foreach (Consumer consumer in post.Consumers)
            {
                clone.AddConsumer(Consumer.Clone(consumer));
            }
            foreach (Payer payer in post.Payers)
            {
                clone.AddPayer(Payer.Clone(payer));
            }
            return clone;
        }

        private Group _internalGroup;
        public void SetInternalGroup(Group group)
        {
            _internalGroup = group;
        }
        public Group GetInternalGroup()
        {
            return _internalGroup;
        }
    }
}
