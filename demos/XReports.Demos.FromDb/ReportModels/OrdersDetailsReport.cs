using System;
using XReports.SchemaBuilders.Attributes;

namespace XReports.Demos.FromDb.ReportModels
{
    [ComplexHeader(1, "Product Info", 5, 9)]
    [ComplexHeader(1, "User Info", 10, 15)]
    public class OrdersDetailsReport
    {
        [ReportColumn(1, "Item #")]
        public int LineItemId { get; set; }

        [ReportColumn(2, "Order #")]
        public int OrderId { get; set; }

        [ReportColumn(3, "Ordered On")]
        [DateTimeFormat("dd MMM yyyy")]
        public DateTime CreatedOn { get; set; }

        [ReportColumn(4, "Bought at Price")]
        public decimal PriceWhenAdded { get; set; }

        [ReportColumn(5, "Product #")]
        public int ProductId { get; set; }

        [ReportColumn(6, "Title")]
        public string ProductTitle { get; set; }

        [ReportColumn(7, "Description")]
        public string ProductDescription { get; set; }

        [ReportColumn(8, "Price")]
        public decimal ProductPrice { get; set; }

        [ReportColumn(9, "Active")]
        public bool ProductIsActive { get; set; }

        [ReportColumn(10, "First Name")]
        public string UserFirstName { get; set; }

        [ReportColumn(11, "Last Name")]
        public string UserLastName { get; set; }

        [ReportColumn(12, "Email")]
        public string UserEmail { get; set; }

        [ReportColumn(13, "Date of Birth")]
        public DateTime UserDateOfBirth { get; set; }

        [ReportColumn(14, "Registered On")]
        public DateTime UserCreatedOn { get; set; }

        [ReportColumn(15, "Active")]
        public bool UserIsActive { get; set; }
    }
}
