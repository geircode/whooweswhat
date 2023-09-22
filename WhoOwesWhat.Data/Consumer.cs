using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Text;
using WhoOwesWhat.Domain;

namespace WhoOwesWhat.Domain
{
    public partial class Consumer
    {
        public Consumer()
        {
            

        }

        public Consumer(Person person)
            : this()
        {
            this.Person = person;
        }

        public static Consumer Clone(Consumer consumer)
        {
            Consumer clone = new Consumer();
            Clone(clone, consumer);
            return clone;
        }

    }
}
