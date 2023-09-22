using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Text;
using WhoOwesWhat.Domain;

namespace WhoOwesWhat.Domain
{
    public partial class Customer
    {


        public static void Clone(Customer clone, Customer customer)
        {
            clone.Amount = customer.Amount;
            clone.AmountIsSetManually = customer.AmountIsSetManually;
            clone.PersonId = customer.PersonId;
            clone.RelativeAmountInPercentage = customer.RelativeAmountInPercentage;
        }

    }
}
