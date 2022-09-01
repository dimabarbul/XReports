using System;
using System.Collections.Generic;

namespace XReports.Demos.FromDb.Models
{
    public class Order
    {
        public int Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

#pragma warning disable CA2227
        public ICollection<OrderLineItem> LineItems { get; set; }
#pragma warning restore CA2227
    }
}
