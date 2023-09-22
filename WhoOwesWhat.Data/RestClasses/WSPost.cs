using System;
using System.Collections.Generic;
using System.Globalization;

namespace WhoOwesWhat.Domain.RestClasses
{
    public class WSPost
    {
        public Guid PostGuid { get; set; }
        public String Date{ get; set; }
        public String Description{ get; set; }
        public float TotalCost{ get; set; }
        public String ISO4217CurrencyCode{ get; set; }

        /// <summary>
        /// Above zero means that this Post has been synchronized before.
        /// </summary>
        public int Version{ get; set; }
        public Boolean IsDirty { get; set; }
        public String Comment{ get; set; }

        public WSGroup Group{ get; set; }

        public WSConsumer[] WSConsumers { get; set; }
        public WSPayer[] WSPayers { get; set; }

        public DateTime GetDate()
        {
            DateTime dt = DateTime.ParseExact(Date, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            return dt;
        }

        public static WSPost CopyFromDomain(Post post)
        {
            WSPost wsPost = new WSPost();
            wsPost.PostGuid = post.PostGuid;
            wsPost.Date = post.Date.ToString("yyyy-MM-dd HH:mm:ss");
            wsPost.Description = post.Description;
            wsPost.TotalCost = post.TotalCost;
            wsPost.ISO4217CurrencyCode = post.ISO4217CurrencyCode;
            wsPost.Version = post.Version;
            wsPost.Comment = post.Comment;
            if(post.Group != null)
            {
                wsPost.Group = WSGroup.MapFromDomain(post.Group);
            }

            wsPost.WSConsumers = GetWSConsumersFromPost(post).ToArray();
            wsPost.WSPayers = GetWSPayersFromPost(post).ToArray();
            return wsPost;
        }

        public static Post MapToDomainWithoutCustomersAndGroup(WSPost wsPost)
        {
            Post post = new Post();
            post.PostGuid = wsPost.PostGuid;
            post.Date = wsPost.GetDate();
            post.Description = wsPost.Description;
            post.SetTotalCost(wsPost.TotalCost);
            post.ISO4217CurrencyCode = wsPost.ISO4217CurrencyCode;
            post.SetVersion(wsPost.Version);
            post.Comment = wsPost.Comment;


            return post;
        }

        private static List<WSConsumer> GetWSConsumersFromPost(Post post)
        {
            List<WSConsumer> wsConsumers = new List<WSConsumer>();
            foreach (Consumer consumer in post.Consumers)
            {
                WSConsumer wsConsumer = new WSConsumer();
                wsConsumers.Add((WSConsumer)WSCustomer.CopyFromDomain(wsConsumer, consumer));
            }
            return wsConsumers;
        }

        private static List<WSPayer> GetWSPayersFromPost(Post post)
        {
            List<WSPayer> wsPayers = new List<WSPayer>();
            foreach (Payer payer in post.Payers)
            {
                WSPayer wsPayer = new WSPayer();
                wsPayers.Add((WSPayer)WSCustomer.CopyFromDomain(wsPayer,payer));
            }
            return wsPayers;
        }
        
    }

    public class WSPayer : WSCustomer
    {
        public static Payer MapToDomain(WSCustomer wsCustomer)
        {
            Payer payer = new Payer();
            MapToDomain(payer, wsCustomer);
            return payer;
        }
    }

    public class WSConsumer : WSCustomer
    {
        public static Consumer MapToDomain(WSCustomer wsCustomer)
        {
            Consumer consumer = new Consumer();
            MapToDomain(consumer, wsCustomer);
            return consumer;
        }
    }

    public abstract class WSCustomer
    {
        public Guid PersonGuid { get; set; }
        public float Amount { get; set; }
        public int RelativeAmountInPercentage { get; set; }
        public Boolean IsAmountSetManually { get; set; }
        
        public static WSCustomer CopyFromDomain(WSCustomer wsCustomer, Customer customer)
        {
            wsCustomer.PersonGuid = customer.Person.GetPersonGuid();
            wsCustomer.Amount = customer.Amount;
            wsCustomer.RelativeAmountInPercentage = customer.RelativeAmountInPercentage;
            wsCustomer.IsAmountSetManually = customer.AmountIsSetManually;
            return wsCustomer;
        }


        public static void MapToDomain(Customer customer, WSCustomer wsCustomer)
        {
            customer.Amount = wsCustomer.Amount;
            customer.RelativeAmountInPercentage = wsCustomer.RelativeAmountInPercentage;
            customer.AmountIsSetManually = wsCustomer.IsAmountSetManually;
        }
    }
}