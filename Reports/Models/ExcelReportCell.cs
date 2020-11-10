using Reports.Enums;

namespace Reports.Models
{
    public class ExcelReportCell : BaseReportCell
    {
        public AlignmentType? AlignmentType { get; set; }
        public string NumberFormat { get; set; }
    }
}
