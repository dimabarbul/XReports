using Reports.Extensions.Builders.Attributes;

namespace Reports.Demos.FromDb.ReportModels
{
    public class ProductListReport
    {
        [ReportVariable(1, "ID")]
        public int Id { get; set; }

        [ReportVariable(2, "Title")]
        public string Title { get; set; }

        [ReportVariable(3, "Description")]
        public string Description { get; set; }

        [ReportVariable(4, "Price")]
        public decimal Price { get; set; }

        [ReportVariable(5, "Active")]
        public bool IsActive { get; set; }
    }
}
