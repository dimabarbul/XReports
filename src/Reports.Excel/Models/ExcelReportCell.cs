using System.Drawing;
using Reports.Enums;
using Reports.Models;

namespace Reports.Excel.Models
{
    public class ExcelReportCell : BaseReportCell
    {
        public AlignmentType? AlignmentType { get; set; }
        public string NumberFormat { get; set; }
        public bool IsBold { get; set; }

        public Color? BackgroundColor { get; set; } = Color.Aqua;
        public Color? FontColor { get; set; }
    }
}
