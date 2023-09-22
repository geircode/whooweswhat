using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Text;
using WhoOwesWhat.Domain;

namespace WhoOwesWhat.Domain
{
    public partial class Payer
    {
        public Payer()
        {

        }

        public Payer(Person person)
            : this()
        {
            this.Person = person;
        }

        public static Payer Clone(Payer consumer)
        {
            Payer clone = new Payer();
            Clone(clone, consumer);
            return clone;
        }
    }
}
