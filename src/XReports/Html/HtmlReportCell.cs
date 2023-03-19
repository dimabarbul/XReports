using System.Collections.Generic;
using XReports.Table;

namespace XReports.Html
{
    public class HtmlReportCell : ReportCell
    {
        // True if cell content should not be escaped.
        public bool IsHtml { get; set; }

        public HashSet<string> CssClasses { get; private set; } = new HashSet<string>();

        public Dictionary<string, string> Styles { get; private set; } = new Dictionary<string, string>();

        public Dictionary<string, string> Attributes { get; private set; } = new Dictionary<string, string>();

        public override void Clear()
        {
            base.Clear();

            this.IsHtml = false;
            this.CssClasses.Clear();
            this.Styles.Clear();
            this.Attributes.Clear();
        }

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
