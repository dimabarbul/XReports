using System.Drawing;
using XReports.ReportCellProperties;
using XReports.Table;

namespace XReports.Excel
{
    /// <summary>
    /// Report cell for Excel report.
    /// </summary>
    public class ExcelReportCell : ReportCell
    {
        /// <summary>
        /// Gets or sets cell horizontal alignment.
        /// </summary>
        public Alignment? HorizontalAlignment { get; set; }

        /// <summary>
        /// Gets or sets cell Excel number format.
        /// </summary>
        public string NumberFormat { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether cell content should be highlighted in bold.
        /// </summary>
        public bool IsBold { get; set; }

        /// <summary>
        /// Gets or sets cell background color.
        /// </summary>
        public Color? BackgroundColor { get; set; }

        /// <summary>
        /// Gets or sets cell font color.
        /// </summary>
        public Color? FontColor { get; set; }

        /// <inheritdoc />
        public override void Clear()
        {
            base.Clear();

            this.HorizontalAlignment = null;
            this.NumberFormat = null;
            this.IsBold = false;
            this.BackgroundColor = null;
            this.FontColor = null;
        }
    }
}
