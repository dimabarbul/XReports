using Reports.Enums;
using Reports.Models;

namespace Reports.Excel.Models
{
    public class ExcelReportCell : BaseReportCell
    {
        public AlignmentType? AlignmentType { get; set; }
        public string NumberFormat { get; set; }
    }
}
