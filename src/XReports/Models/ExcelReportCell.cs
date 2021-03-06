using System.Drawing;
using XReports.Enums;

namespace XReports.Models
{
    public class ExcelReportCell : BaseReportCell
    {
        public Alignment? HorizontalAlignment { get; set; }

        public string NumberFormat { get; set; }

        public bool IsBold { get; set; }

        public Color? BackgroundColor { get; set; }

        public Color? FontColor { get; set; }
    }
}
