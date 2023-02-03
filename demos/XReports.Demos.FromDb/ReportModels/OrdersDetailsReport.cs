using System;
using XReports.Attributes;

namespace XReports.Demos.FromDb.ReportModels
{
    [ComplexHeader(1, "Product Info", 5, 9)]
    [ComplexHeader(1, "User Info", 10, 15)]
    public class OrdersDetailsReport
    {
        [ReportVariable(1, "Item #")]
        public int LineItemId { get; set; }

        [ReportVariable(2, "Order #")]
        public int OrderId { get; set; }

        [ReportVariable(3, "Ordered On")]
        [DateTimeFormat("dd MMM yyyy")]
        public DateTime CreatedOn { get; set; }

        [ReportVariable(4, "Bought at Price")]
        public decimal PriceWhenAdded { get; set; }

        [ReportVariable(5, "Product #")]
        public int ProductId { get; set; }

        [ReportVariable(6, "Title")]
        public string ProductTitle { get; set; }

        [ReportVariable(7, "Description")]
        public string ProductDescription { get; set; }

        [ReportVariable(8, "Price")]
        public decimal ProductPrice { get; set; }

        [ReportVariable(9, "Active")]
        public bool ProductIsActive { get; set; }

        [ReportVariable(10, "First Name")]
        public string UserFirstName { get; set; }

        [ReportVariable(11, "Last Name")]
        public string UserLastName { get; set; }

        [ReportVariable(12, "Email")]
        public string UserEmail { get; set; }

        [ReportVariable(13, "Date of Birth")]
        public DateTime UserDateOfBirth { get; set; }

        [ReportVariable(14, "Registered On")]
        public DateTime UserCreatedOn { get; set; }

        [ReportVariable(15, "Active")]
        public bool UserIsActive { get; set; }
    }
}
