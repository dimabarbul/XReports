using System;
using Reports.Extensions.Builders.Attributes;

namespace Reports.Demos.FromDb.ReportModels
{
    public class OrdersDetailsReport
    {
        [ReportVariable(1, "Item #")]
        public int LineItemId { get; set; }
        [ReportVariable(2, "Order #")]
        public int OrderId { get; set; }
        [ReportVariable(3, "Ordered On")]
        public DateTime CreatedOn { get; set; }
        [ReportVariable(4, "Bought at Price")]
        public decimal PriceWhenAdded { get; set; }

        [ReportVariable(5, "Product #", ComplexHeader = new[] { "Product Info" })]
        public int ProductId { get; set; }
        [ReportVariable(6, "Title", ComplexHeader = new[] { "Product Info" })]
        public string ProductTitle { get; set; }
        [ReportVariable(7, "Description", ComplexHeader = new[] { "Product Info" })]
        public string ProductDescription { get; set; }
        [ReportVariable(8, "Price", ComplexHeader = new[] { "Product Info" })]
        public decimal ProductPrice { get; set; }
        [ReportVariable(9, "Active", ComplexHeader = new[] { "Product Info" })]
        public bool ProductIsActive { get; set; }

        [ReportVariable(10, "First Name", ComplexHeader = new[] { "User Info" })]
        public string UserFirstName { get; set; }
        [ReportVariable(11, "Last Name", ComplexHeader = new[] { "User Info" })]
        public string UserLastName { get; set; }
        [ReportVariable(12, "Email", ComplexHeader = new[] { "User Info" })]
        public string UserEmail { get; set; }
        [ReportVariable(13, "Date of Birth", ComplexHeader = new[] { "User Info" })]
        public DateTime UserDateOfBirth { get; set; }
        [ReportVariable(14, "Registered On", ComplexHeader = new[] { "User Info" })]
        public DateTime UserCreatedOn { get; set; }
        [ReportVariable(15, "Active", ComplexHeader = new[] { "User Info" })]
        public bool UserIsActive { get; set; }
    }
}
