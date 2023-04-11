using System.Collections.Generic;
using XReports.Table;

namespace XReports.Html
{
    /// <summary>
    /// Report cell for HTML report.
    /// </summary>
    public class HtmlReportCell : ReportCell
    {
        /// <summary>
        /// Gets or sets a value indicating whether cell value should not be escaped (if true).
        /// </summary>
        public bool IsHtml { get; set; }

        /// <summary>
        /// Gets cell CSS classes.
        /// </summary>
        public HashSet<string> CssClasses { get; private set; } = new HashSet<string>();

        /// <summary>
        /// Gets cell styles: key is style property, value is its value.
        /// </summary>
        public Dictionary<string, string> Styles { get; private set; } = new Dictionary<string, string>();

        /// <summary>
        /// Gets cell HTML attributes.
        /// </summary>
        public Dictionary<string, string> Attributes { get; private set; } = new Dictionary<string, string>();

        /// <inheritdoc />
        public override void Clear()
        {
            base.Clear();

            this.IsHtml = false;
            this.CssClasses.Clear();
            this.Styles.Clear();
            this.Attributes.Clear();
        }

        /// <inheritdoc />
        public override ReportCell Clone()
        {
            HtmlReportCell reportCell = (HtmlReportCell)base.Clone();

            reportCell.CssClasses = new HashSet<string>(this.CssClasses);
            reportCell.Styles = new Dictionary<string, string>(this.Styles);
            reportCell.Attributes = new Dictionary<string, string>(this.Attributes);

            return reportCell;
        }
    }
}
