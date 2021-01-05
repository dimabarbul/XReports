using System.Collections.Generic;

namespace XReports.Models
{
    public class HtmlReportCell : BaseReportCell
    {
        public string Html { get; set; }

        #region Most used or complex HTML attributes
        public HashSet<string> CssClasses { get; set; } = new HashSet<string>();
        public Dictionary<string, string> Styles { get; set; } = new Dictionary<string, string>();
        #endregion

        public Dictionary<string, string> Attributes { get; set; } = new Dictionary<string, string>();

        public override void CopyFrom(BaseReportCell reportCell)
        {
            base.CopyFrom(reportCell);

            this.Html = reportCell.InternalValue?.ToString() ?? string.Empty;
        }
    }
}
