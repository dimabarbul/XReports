using System;

namespace Reports.Demos.FromDb.Models
{
    public class OrderLineItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public DateTime CreatedOn { get; set; }
        public decimal PriceWhenAdded { get; set; }

        public Order Order { get; set; }
        public Product Product { get; set; }
    }
}
