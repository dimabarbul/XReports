using System.Collections.Generic;

namespace XReports.Models
{
    public class HtmlReportCell : BaseReportCell
    {
        // True if cell content should not be escaped.
        public bool IsHtml { get; set; }

        public HashSet<string> CssClasses { get; } = new HashSet<string>();

        public Dictionary<string, string> Styles { get; } = new Dictionary<string, string>();

        public Dictionary<string, string> Attributes { get; } = new Dictionary<string, string>();

        public override void Clear()
        {
            base.Clear();

            this.IsHtml = false;
            this.CssClasses.Clear();
            this.Styles.Clear();
            this.Attributes.Clear();
        }
    }
}
