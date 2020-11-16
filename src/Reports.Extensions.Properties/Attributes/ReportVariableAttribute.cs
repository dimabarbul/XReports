using System.Drawing;
using Reports.Enums;

namespace Reports.Extensions.Properties.Attributes
{
    public class ReportVariableAttribute : Builders.Attributes.ReportVariableAttribute
    {
        public AlignmentType? Alignment { get; set; }
        public Color? FontColor { get; set; }
        public Color? BackgroundColor { get; set; }
        public Color? HeaderFontColor { get; set; }
        public Color? HeaderBackgroundColor { get; set; }
        public int? Precision { get; set; }
        public bool IsPercent { get; set; }
        public string DateTimeFormat { get; set; }

        public ReportVariableAttribute(int order, string title)
            : base(order, title)
        {
        }
    }
}
